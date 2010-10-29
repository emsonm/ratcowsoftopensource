#include <Application.h>
#include <Resources.h>
#include <AppFileInfo.h>
#include <Mime.h>

#include <iostream.h>
#include <Errors.h>

#include "StrokeItApp.h"
#include "StrokeItView.h"



    StrokeItApp::StrokeItApp( const char* apppath ): BApplication("application/x-vnd.StrokeIt.ME")
    {
      settings = new SettingsFile("strokeit", "strokeit");

      BString _strokefile;

      settings->AddInt32("scale", 50);
      settings->AddBool("help", false);
      settings->Load();

      bool _help;
      int32 _scale;
      int32 _hedge = 60;
      
      settings->FindInt32("scale", &_scale);
      settings->FindBool("help", &_help);

      //
      //majno, minno, buildno
      BResources * appResources = AppResources(); 
      size_t len;
      version_info* vi;
      vi = (version_info*)appResources->LoadResource('APPV', "BEOS:APP_VERSION", &len); 
      
      majno =0; minno =0; buildno = 0;
      if (vi)
      {  
        majno = vi->major;
        minno = vi->middle;
        buildno = vi->minor;
      }
      delete appResources;

      cout << "VERSION IS: " << (int)majno << "." << (int)minno << "." << (int)buildno <<endl;

      bubblehelp = new BubbleHelper();
      bubblehelp->EnableHelp(_help); 

      //_scale dictates the size of hedge...
      if (_scale==40)
        _hedge = 75;
      else if (_scale < 40)
        _hedge = 100; 

      mainwindow = new StrokeItWindow( apppath, 625, 375, (_scale * 3), (_scale *3 ) + _hedge);
      //mainwindow->Show(); 
      
      summonwindow = new StrokeItSummonWindow();
      summonwindow->Show();

      aboutwin = NULL;

      install_mime_types();
    }
    
    StrokeItApp::~StrokeItApp()
    {
       //this seems to have been the case - though don't quote me
       #ifdef __POWERPC__
	   //delete aboutwin; 
       //delete summonwindow;
       //delete mainwindow;
	   #else
       delete aboutwin; 
       delete summonwindow;
       delete mainwindow;
	   #endif
	   
       delete bubblehelp;

       delete settings;

       cout << "~StrokeItApp" << endl;
    }

    void StrokeItApp::MessageReceived( BMessage* message )
    {
      switch (message->what)
      {
        case 'pkSW':
          mainwindow->poke();
          break;

        case 'qStk':
          be_app->PostMessage(B_QUIT_REQUESTED);
          break;
      
        case 'sStk':
          mainwindow->snap();
          break;  

        case 'scsM':
          mainwindow->scale(25);
          settings->ReplaceInt32("scale", 25);
          settings->Save();
          break; 

        case 'scmM':
          mainwindow->scale(40);
          settings->ReplaceInt32("scale", 40);
          settings->Save();
          break;

        case 'scnM':
          mainwindow->scale(50);
          settings->ReplaceInt32("scale", 50);
          settings->Save();  
          break;

        case 'sclM':
          mainwindow->scale(60);
          settings->ReplaceInt32("scale", 60);
          settings->Save();
          break;

        case 'srlD':
          mainwindow->reloadviewdata();
          break;

        default:
          BApplication::MessageReceived(message);
          break;
      }
    }

    bool StrokeItApp::QuitRequested()
    {

       if ( aboutwin != 0 && aboutwin->Lock() )
       {
         aboutwin->Hide();
         aboutwin->Quit();
         //aboutwin->Unlock();
         cout << "aboutwin hide" << endl;
       }
 
       if ( summonwindow->Lock() )
       {
         summonwindow->Hide();
         summonwindow->Quit();
         //summonwindow->Unlock();
         cout << "summon hide" << endl;
       }
              
       if ( mainwindow->Lock() )
       {
         mainwindow->Hide();
         mainwindow->closeviewdata();
         mainwindow->Quit();
         //mainwindow->Unlock();
         cout << "main hide" << endl;
       }

 
       settings->ReplaceBool( "help", bubblehelp->isEnabled() );
       settings->Save();
       
       return true;
     } 

    

    void StrokeItApp::AboutRequested()
    {
       BString versionno;
       versionno << (int)majno << "." << (int)minno << "." << (int)buildno;
       BString blurb;
       blurb << "StrokeIt - 2004 Matt Emson (c)\n\nUse your input device to enter strokes...\nGet that PDA feel to your Desktop!!";
       aboutwin = new ArpAboutWindow( NULL, "StrokeIt", versionno.String(), "Dev", blurb.String());
       aboutwin->Show();
    }


    //todo....
    void StrokeItApp::install_mime_types()
    {
      BMimeType typeStroke("text/x.vnd.StrokeIt.Stroke-File");
    	typeStroke.SetShortDescription("StrokeIt Stroke file");
    	typeStroke.SetLongDescription("StrokeIt Stroke file");

      BMessage ext;
      ext.AddString("extensions", ".strokes");
      typeStroke.SetFileExtensions(&ext);
    	if (!typeStroke.IsInstalled())
	      typeStroke.Install();

      BMimeType typeStrokelet("text/x.vnd.StrokeIt.Strokelet-File");
	    typeStrokelet.SetShortDescription("StrokeIt Strokelet file");
	    typeStrokelet.SetLongDescription("StrokeIt Strokelet file");
      BMessage ext2;
      ext.AddString("extensions", ".strokelet");
      typeStrokelet.SetFileExtensions(&ext2);
	    if (!typeStrokelet.IsInstalled())
	      typeStrokelet.Install();
    }


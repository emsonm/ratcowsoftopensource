#include "SummonView.h"
#include <Menu.h>

    SummonView::SummonView(BRect rect) :
      BView(rect, "SummonView", 0, B_WILL_DRAW | B_FOLLOW_ALL)
    {
      keyboard = new BBitmap(BRect(0,0,31,31), B_CMAP8);
      keyboard->SetBits(keyboard8, 1024, 0, B_CMAP8);       
    
      popupmenu = new BPopUpMenu("SummonPopUpMenu", false, false);

      popupmenu->AddItem( new BMenuItem( "About", new BMessage(B_ABOUT_REQUESTED) ) );
      popupmenu->AddItem( new BMenuItem( "Help", new BMessage('tglH') ) );
  
      
      popupmenu->AddSeparatorItem();
      
      popupmenu->AddItem( new BMenuItem( "Snap", new BMessage('sStk') ) );
    
      BMenu* tmpscalemenu = new BMenu("Scale");
      
      tmpscalemenu->AddItem( new BMenuItem( "Small", new BMessage('scsM') ) );
      tmpscalemenu->AddItem( new BMenuItem( "Medium", new BMessage('scmM') ) );
      tmpscalemenu->AddItem( new BMenuItem( "Normal", new BMessage('scnM') ) );
      tmpscalemenu->AddItem( new BMenuItem( "Large", new BMessage('sclM') ) );

      popupmenu->AddItem( tmpscalemenu );

      popupmenu->AddSeparatorItem();

      popupmenu->AddItem( new BMenuItem( "Quit", new BMessage('qStk') ) );

      normalBtn = new BBitmap(BRect(0,0,7,7), B_CMAP8);
      normalBtn->SetBits(normalImage, 64, 0, B_CMAP8); 

      selectedBtn = new BBitmap(BRect(0,0,7,7), B_CMAP8);
      selectedBtn->SetBits(selectedImage, 64, 0, B_CMAP8); 

      //otherBtn = new BBitmap(BRect(0,0,7,7), B_CMAP8);
      //otherBtn->SetBits(otherImage, 64, 0, B_CMAP8); 

      menubutton = new MBitmapButton(BRect(31 - 8, 31 - 8, 31, 31), "mnuBtn", "", new BMessage('mnuB'), normalBtn, normalBtn, selectedBtn);
      AddChild( menubutton );
    }
 
    SummonView::~SummonView()
    {
      delete popupmenu;
      delete keyboard;
      delete normalBtn; 
      delete selectedBtn;

      app_bubblehelp->SetHelp(this, NULL);
      app_bubblehelp->SetHelp(menubutton, NULL);
      
      cout << "~SummonView" << endl;
    }

    void SummonView::pokeStrokeItWindow()
    {
      BMessenger *msgr = new BMessenger("application/x-vnd.StrokeIt.ME");
      if (msgr->IsValid())
        msgr->SendMessage( new BMessage('pkSW') );     
      delete msgr;
    }

    void  SummonView::showPopUpMenu(BPoint point)
    {
      BMenuItem *selected;

      ConvertToScreen(&point);
      selected = popupmenu->Go(point);
      
      if ( selected ) 
      {
        cout << "bip bip" << endl;
        
        //TODO - work out why this doesn't work...
        //
        //BLooper *looper;
        //BHandler *target = selected->Target(&looper);
        //looper->PostMessage(selected->Message(), target);

        if (selected->Message()->what == 'tglH')
        { 
          //change the state...
          if (selected->IsMarked()) 
          { 
            selected->SetMarked(false);
            app_bubblehelp->EnableHelp(false);
          }
          else
          { 
            selected->SetMarked(true);
            app_bubblehelp->EnableHelp(true);
          }
          return;
        }  

        BMessenger *msgr = new BMessenger("application/x-vnd.StrokeIt.ME");
        if (msgr->IsValid())
          msgr->SendMessage( selected->Message() ); 
        delete msgr;
      }
    }

    void SummonView::MessageReceived( BMessage* message )
    {
       cout << "SummonView message" << endl;

       switch (message->what) 
       {
          case 'mnuB':
            showPopUpMenu( BPoint(5, 5) );
            cout << "popup.." << endl;
            break;

          default:
            BView::MessageReceived(message);
            break;
       }
    }

    void SummonView::MouseDown(BPoint point)
    {
      int32 val;
    	Window()->CurrentMessage()->FindInt32("buttons", &val);
	    uint32 buttons = val;
	    if (buttons & B_PRIMARY_MOUSE_BUTTON)
      {
        pokeStrokeItWindow();
      }
      else if (buttons & B_SECONDARY_MOUSE_BUTTON)
      {
        showPopUpMenu( point );
      }

      cout << "bop" << endl;
    }
    
    void SummonView::AttachedToWindow()
    {
      //grab the bubble helper from the app
      app_bubblehelp = ((StrokeItApp*)be_app)->BubbleHelp();
      app_bubblehelp->SetHelp(this, "StrokeIt summon window");
      app_bubblehelp->SetHelp(menubutton, "open the menu"); 

      BMenuItem* tmphelpmenuitem = popupmenu->FindItem("Help");
      if (tmphelpmenuitem)  
        tmphelpmenuitem->SetMarked( app_bubblehelp->isEnabled() );

    } 

    void SummonView::Draw(BRect updateRect)
    {
      DrawBitmap( keyboard, BRect(0, 0, 31, 31), BRect(0, 0, 31, 31) );
      cout << "draw picture" << endl;
      Sync();
    }
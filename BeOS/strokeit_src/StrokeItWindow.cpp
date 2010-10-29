#include <Application.h>
#include <Screen.h>

#include <iostream.h>
#include <Errors.h>

#include "StrokeItWindow.h"

    StrokeItWindow::StrokeItWindow( const char* apppath, int32 left, int32 top, int32 width, int32 height): BWindow(
        BRect(left, top, left + width, top + height),
        "StrokeIt",
        B_FLOATING_WINDOW_LOOK,
        B_NORMAL_WINDOW_FEEL,
        B_WILL_ACCEPT_FIRST_CLICK | //this forces our app to never receive focus unless we select the titlbar
        B_NOT_CLOSABLE |
        B_NOT_ZOOMABLE |
        B_ASYNCHRONOUS_CONTROLS |
        B_NOT_RESIZABLE, 
        B_CURRENT_WORKSPACE 
      )
    {
      view = new StrokeItView(apppath, (width / 3), BRect(0, 0, left + width, top + height));
      AddChild(view);

      setPosition();
    } 
    
    StrokeItWindow::~StrokeItWindow()
    {
      cout << "~StrokeItWindow" << endl;
    } 

    void StrokeItWindow::MessageReceived( BMessage* message )
    {
       //cout << "message" << endl;

       switch (message->what) 
       {
          case 'retK':
            view->sendKeydown(13);
            break;

          case 'spcK':
            view->sendKeydown(32);
            break;

          case 'delK':
            view->sendKeydown(8);
            break;

          default:
            BWindow::MessageReceived(message);
            break;
       }
      
    }

    void StrokeItWindow::poke()
    {
      Lock();

      if ( IsHidden() )
        Show();
      else
        Hide();

      Unlock();
    }
    
    void StrokeItWindow::setPosition()
    {
      BScreen* tmpscreen = new BScreen();
      BRect tmpbounds = tmpscreen->Frame();
      delete tmpscreen; 
       
      BPoint tmppoint;
      BRect bounds = this->Bounds();
      tmppoint.x = tmpbounds.right - (bounds.right - bounds.left) - 5;
      tmppoint.y = tmpbounds.bottom - (bounds.bottom - bounds.top) - 45;
      MoveTo( tmppoint );
    }
    
    void StrokeItWindow::snap()
    {
      Lock();

      if ( IsHidden() )
        Show();

      setPosition();

      Unlock();
    }
    
		bool StrokeItWindow::QuitRequested()
	 	{
			be_app->PostMessage(B_QUIT_REQUESTED);
		 	return(true);
	 	}

    void StrokeItWindow::scale( int boxWidth )
    {
      int32 _hedge = 60;

      //_scale dictates the size of hedge...
      if (boxWidth==40)
        _hedge = 75;
      else if (boxWidth < 40)
        _hedge = 100;

      ResizeTo( boxWidth * 3, (boxWidth * 3) + _hedge);
      view->setSize(boxWidth);
      snap();
    }


    void StrokeItWindow::closeviewdata()
    {
      if (Lock())
      {
        view->closedata();
        Unlock();
      }
    }

    void StrokeItWindow::reloadviewdata()
    {
       if (Lock())
      {
        view->reopendata();
        Unlock();
      }
    }
   
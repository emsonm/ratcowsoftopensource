#include <Application.h>
#include <Screen.h>
#include <Messenger.h>
#include "StrokeItSummonWindow.h"

    StrokeItSummonWindow::StrokeItSummonWindow(): BWindow(
        BRect(0, 0, 31, 31),
        "SummonWindow",
        B_MODAL_WINDOW_LOOK, 
        B_FLOATING_ALL_WINDOW_FEEL,
        B_WILL_ACCEPT_FIRST_CLICK | //this forces our app to never receive focus unless we select the titlbar
        B_NOT_MOVABLE | B_NOT_ZOOMABLE | B_NOT_MINIMIZABLE | 
        B_ASYNCHRONOUS_CONTROLS,
        B_CURRENT_WORKSPACE 
      )   
    {
       //code to move window...
       setPosition();

       view = new SummonView( BRect(0, 0, 31, 31) );
       AddChild(view);
    }

    StrokeItSummonWindow::~StrokeItSummonWindow()
    {
      cout << "~StrokeItSummonWindow" << endl;
    }

    void StrokeItSummonWindow::setPosition()
    {
       BScreen* tmpscreen = new BScreen();
       BRect tmpbounds = tmpscreen->Frame();
       delete tmpscreen; 
       
       BPoint tmppoint;
       BRect bounds = this->Bounds();
       tmppoint.x = tmpbounds.right - (bounds.right - bounds.left) - 5;
       tmppoint.y = tmpbounds.bottom - (bounds.bottom - bounds.top) - 5;
       MoveTo( tmppoint );
    }

    void StrokeItSummonWindow::MessageReceived( BMessage* message )
    {
       cout << "message" << endl;

       switch (message->what) 
       {
          case 'mnuB':
            view->showPopUpMenu( BPoint(5, 5) );
            break;

          default:
            BWindow::MessageReceived(message);
            break;
       }
      
    }

    void StrokeItSummonWindow::ScreenChanged(BRect frame, color_space mode)
    {
      setPosition();
    }
    
		bool StrokeItSummonWindow::QuitRequested()
    {
      be_app->PostMessage(B_QUIT_REQUESTED);
		 	return(true);
    }
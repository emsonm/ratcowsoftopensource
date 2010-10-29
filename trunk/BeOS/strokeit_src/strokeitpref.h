#ifndef STROKEIT_PREF_H
#define STROKEIT_PREF_H

#include <Application.h>
#include <Window.h>
#include <View.h>
#include <Button.h>
#include <Message.h>
#include <Rect.h>
#include <Messenger.h>

class StrokeItPrefApp : public BApplication
{
  private:
    BWindow* _win;
    BView* _view;
    BButton* _btn;

  public:
    StrokeItPrefApp() :
      BApplication("application/x-vnd.StrokeItPref.ME")
    {
      _win = new BWindow(
        BRect(100, 100, 300, 300),
        "StrokeIt Pref",
        B_FLOATING_WINDOW_LOOK,
        B_NORMAL_WINDOW_FEEL
      );

      _view = new BView(
        BRect(100, 100, 300, 300),
        "StrokeItPrefView", 
        0, 
        B_WILL_DRAW | B_FOLLOW_ALL
      );

      _btn = new BButton(
        BRect(10, 10, 25, 25),
        "TestBtn",
        "Test",
        new BMessage('tstB'),
        0 
      );

      _view->AddChild(_btn);

      _win->AddChild(_view);
    }
    
    ~StrokeItPrefApp()
    {
      _win->Quit();
      delete _win;
    }

    bool QuitRequested()
	 	{
			be_app->PostMessage(B_QUIT_REQUESTED);
		 	return(true);
	 	}

    void MessageReceived( BMessage* message )
    {
       switch (message->what)
      {
        case 'srlD':
          BMessenger *msgr = new BMessenger("application/x-vnd.StrokeIt.ME");
          if (msgr->IsValid())
            msgr->SendMessage( selected->Message() ); 
          break;
        
        default:
          BApplication::MessageReceived(message);
          break;
      }
    }

};

int main()
{
  new StrokeItPrefApp();
  be_app->Run();
}

#endif
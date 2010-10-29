#ifndef STROKEIT_PREF_H
#define STROKEIT_PREF_H

#include <Application.h>
#include <Window.h>
#include <View.h>
#include <TabView.h>
#include <Button.h>
#include <TextControl.h>

#include <MenuField.h>
#include <MenuItem.h>
#include <MenuBar.h>
#include <PopUpMenu.h>
#include <Menu.h>

#include <Messenger.h>
#include <Message.h>

#include <Directory.h>
#include <Node.h>
#include <Entry.h>
#include <Path.h>
#include <File.h>

#include <String.h>
#include <Point.h>
#include <Rect.h>

#include <iostream.h>
#include <stdio.h>  //FOR SEEK_XXX 


#include "SettingsFile.h"

#include "PrefView.h"
#include "PrefWindow.h"



class StrokeItPrefApp : public BApplication
{
  private:
    PrefWindow* _win;
    
  public:
    StrokeItPrefApp() :
      BApplication("application/x-vnd.StrokeItPref.ME")
    {
      _win = new PrefWindow();
      _win->Show();
    }
    
    ~StrokeItPrefApp()
    {
      delete _win;
    }

    bool QuitRequested()
	 	{
      if (_win->Lock())
      { 
        _win->Hide();
        _win->Quit();
      }
			be_app->PostMessage(B_QUIT_REQUESTED);
		 	return(true);
	 	}
};

int main()
{
  new StrokeItPrefApp();
  be_app->Run();
}

#endif
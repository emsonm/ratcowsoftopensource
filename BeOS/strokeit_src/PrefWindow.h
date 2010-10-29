#ifndef STROKEIT_PREF_WINDOW_H
#define STROKEIT_PREF_WINDOW_H

#include <Application.h>
#include <Window.h>
#include <View.h>
#include <TabView.h>
#include <Button.h>
#include <TextControl.h>
#include <CheckBox.h>
#include <Slider.h>

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
#include "PrefStrokeView.h"
#include "PrefWindow.h"

////////////////////////////////////////////////////////////////////

class PrefWindow: public BWindow 
{
  private:
    PrefStrokeView* stroke_view;
    BTabView* _view;
    
    BTab *tab1;
    BTab *tab2;
    BDirectory* settings_dir;
    BMenuField* file_list;
    BMenuField* trainer_file_list; 
    BTextControl* trainer_input;
    BTextControl* trainer_output;
    BButton* trainer_button_save;
    BButton* trainer_button_saveto;
    BCheckBox* help_setting;
    BSlider* scale_setting;
    //BButton* _btn;
    BString main_save_file;
    BString trainer_save_file;
    BString traineroutput;
    bool help_value;
    int32 scale_value;
    
    int32 sliderToScale( int32 value );
    int32 scaleToSlider( int32 value );
    void  setSliderLabel();

  public:
    PrefWindow();

    virtual ~PrefWindow();

    void MessageReceived( BMessage* message );

    bool QuitRequested();

    void update_trainer_output(const char* _stroke);

    void save_stroke(const char* stroke, const char* filename); 

    void signal_app();

    void save_prefs();

};

#endif
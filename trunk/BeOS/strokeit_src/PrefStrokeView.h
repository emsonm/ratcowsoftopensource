#ifndef STROKEIT_PREFSTROKEVIEW_H
#define STROKEIT_PREFSTROKEVIEW_H

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

class PrefStrokeView: public BView
{
  private:
    bool buttonDown;
    BString bucket;
    int32 lastbucket; 
    BPoint lastpos;
    int32 boxwidth, boxheight;

  public:
    PrefStrokeView(float left, float top);

    void Draw(BRect updateRect);

    void plot(int32 x, int32 y);

    bool inBox(BPoint point);
    
    void MouseDown(BPoint point);
    
    void MouseMoved(BPoint where, uint32 code, const BMessage *a_message);

    void MouseUp(BPoint point);
};

#endif
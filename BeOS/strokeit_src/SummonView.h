#ifndef SUMMON_VIEW_H
#define SUMMON_VIEW_H

#include <View.h>
#include <Bitmap.h>
#include <PopUpMenu.h>
#include <MenuItem.h>
#include <Looper.h>
#include <Handler.h>
#include <Message.h>
#include <PictureButton.h>

#include <Rect.h>
#include <Point.h>

#include <iostream.h>

#include "StrokeItApp.h"
#include "BitmapButton.h"
#include "button_bmp.h"
#include "BubbleHelper.h"
#include "keyboard_bmp.h"

using namespace BExperimental;

class MBitmapButton : public BBitmapButton
{
  public:
    MBitmapButton(
          BRect frame, 
          const char* name,
				  const char* label,
				  BMessage* message,
				  const BBitmap* bmNormal = 0,
				  const BBitmap* bmOver = 0,
				  const BBitmap* bmPressed = 0,
				  const BBitmap* bmDisabled = 0,
				  const BBitmap* bmDisabledPressed = 0,
				  uint32 resizeMask = B_FOLLOW_LEFT | B_FOLLOW_TOP) :
            BBitmapButton(frame, name, label, message, bmNormal, 
                          bmOver, bmPressed, bmDisabled, bmDisabledPressed,
				                  resizeMask){}

   
     virtual void GetPreferredSize(float* width, float* height)
     { 
         *width = 7; *height = 7;
     }
};

class SummonView: public BView
{
  private:
    BBitmap* keyboard;
    BBitmap* normalBtn;
    BBitmap* selectedBtn;
    //BBitmap* otherBtn;
    BPopUpMenu* popupmenu;
    MBitmapButton* menubutton;
    BubbleHelper* app_bubblehelp;
    ArpAboutWindow* aboutwin;

  public:
    SummonView(BRect rect);
    
    virtual ~SummonView();

    void pokeStrokeItWindow();

    void  showPopUpMenu(BPoint point);

    void MessageReceived( BMessage* message );

    void MouseDown(BPoint point);
    
    virtual	void AttachedToWindow();

    virtual	void Draw(BRect updateRect);

};

#endif 
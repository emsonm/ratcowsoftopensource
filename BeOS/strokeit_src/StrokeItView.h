#ifndef STROKEIT_VIEW_H
#define STROKEIT_VIEW_H

#include <Rect.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <Button.h>
#include <Message.h>
#include <File.h>
#include <Entry.h>
#include <String.h>
#include <CheckBox.h>
#include <View.h>

#ifdef KEYBOARD_DEBUG
#include <Roster.h>
#endif

#include "StrokeList.h"
#include "KeyboardSupport.h"
#include "BubbleHelper.h"

class StrokeItView: public BView
{
  private:
    StrokeList* strokelist; 
    BubbleHelper* app_bubblehelp;
    bool buttonDown;
    BString bucket;
    BString _strokepath;
    int32 lastbucket; 
    
    BCheckBox* capslock;
    BCheckBox* numlock;
    BButton* returnkey;
    BButton* spacekey;
    BButton* delkey;
    BPoint lastpos;

   	port_id input_port;
   
		// keymap
		key_map* keymap_keys;
		char* keymap_chars;

    void openstrokes();
    void closestrokes();
    bool inBox(BPoint point);

    int boxwidth, boxheight;
    bool strokesloaded;

    //Obsolete....
		bool find_key_in_keymap( int32 key, int32 * map, char * chars, BMessage * msg, int32 modif );

  public:
    StrokeItView( const char* apppath, int boxSize, BRect rect);
    
    virtual ~StrokeItView();
    
    virtual	void AttachedToWindow();

    virtual	void Draw(BRect updateRect);
    
    virtual void setSize(int32 newSize);
    
    virtual void MouseDown(BPoint point);
    
    virtual void MouseMoved(BPoint where, uint32 code, const BMessage *a_message);

    virtual void MouseUp(BPoint point);
    
    void plot(int32 x, int32 y);

    virtual void sendKeydown( int32 key );

    void dokeyevent( key_event_t event );
    
    void closedata();
    void reopendata();
};

#endif //STROKEIT_VIEW_H
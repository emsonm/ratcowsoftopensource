#ifndef STROKEIT_WINDOW_H
#define STROKEIT_WINDOW_H

#include <Window.h>
#include <Rect.h>
#include <String.h>

#include "StrokeItView.h"

class StrokeItWindow: public BWindow
{
 	protected:
   	StrokeItView *view;

    void setPosition();
   	
	public:
    StrokeItWindow( const char* apppath, int32 left, int32 top, int32 width, int32 height);
    virtual ~StrokeItWindow();

    virtual void MessageReceived( BMessage* message );
    
		bool QuitRequested();  

    void poke();  
    void snap();
    void closeviewdata();
    void reloadviewdata();

    void scale( int boxWidth );
};

#endif //STROKEIT_WINDOW_H


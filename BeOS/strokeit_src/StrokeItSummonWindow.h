#ifndef STROKEIT_SUMMON_WINDOW_H
#define STROKEIT_SUMMON_WINDOW_H

#include <Window.h>
#include "SummonView.h" 

class SummonView; //forward

class StrokeItSummonWindow: public BWindow
{
 	protected:
   	SummonView *view;
   	
    void setPosition();

	public:
    StrokeItSummonWindow();
    virtual ~StrokeItSummonWindow();
    
    void ScreenChanged(BRect frame, color_space mode);
    
		bool QuitRequested();

    void MessageReceived( BMessage* message );    
};

#endif STROKEIT_SUMMON_WINDOW_H

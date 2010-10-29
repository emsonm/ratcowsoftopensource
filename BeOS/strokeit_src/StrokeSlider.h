#ifndef STROKE_SLIDER_H
#define STROKE_SLIDER_H

#include <Slider.h>
#include <String.h>

class StrokeSlider : public BSlider
{
  private:
    BString updateText;

  public:
    StrokeSlider( BRect frame, const char* name, const char* label, BMessage* message, int32 minValue, int32 maxValue, thumb_style thumbType = B_BLOCK_THUMB, uint32 resizingMode = B_FOLLOW_LEFT | B_FOLLOW_TOP, uint32 flags = B_FRAME_EVENTS | B_WILL_DRAW | B_NAVIGABLE);
    
    virtual void setUpdateText(const char* value);
    char* UpdateText(void);
};

#endif
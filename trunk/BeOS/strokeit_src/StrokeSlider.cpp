#include "StrokeSlider.h"

#include <iostream.h>

StrokeSlider::StrokeSlider( BRect frame, const char* name, const char* label, BMessage* message, int32 minValue, int32 maxValue, thumb_style thumbType, uint32 resizingMode, uint32 flags) :
  BSlider(frame, name, label, message, minValue, maxValue, thumbType, resizingMode, flags)  
{
  updateText.SetTo("");
}

void StrokeSlider::setUpdateText(const char* value)
{
  updateText << value;
  Invalidate();
}

char* StrokeSlider::UpdateText(void)
{
  cout << "wibble" << endl;
  return "wibble";
  
}


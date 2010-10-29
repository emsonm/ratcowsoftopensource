#ifndef STROKEIT_PREF_VIEW_H
#define STROKEIT_PREF_VIEW_H

#include <View.h>

class PrefView: public BView
{
  public:
    PrefView(BRect r) :
      BView(r, "", 0, B_WILL_DRAW | B_FOLLOW_ALL) 
    {
      SetViewColor(216,216,216,0);
    }
};

#endif
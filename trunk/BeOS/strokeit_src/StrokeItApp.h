#ifndef STROKEIT_APP_H
#define STROKEIT_APP_H

#include <Application.h>

#include "BubbleHelper.h"
#include "ArpAboutWindow.h"
#include "SettingsFile.h"

#include "StrokeItWindow.h"
#include "StrokeItSummonWindow.h"



class StrokeItSummonWindow; //forward;

class StrokeItApp: public BApplication 
{
  protected:
    StrokeItWindow *mainwindow;
    StrokeItSummonWindow *summonwindow;
    BubbleHelper* bubblehelp;
    ArpAboutWindow* aboutwin;
    SettingsFile* settings;
    int8 majno, minno, buildno;

    void install_mime_types();
    
  public:
    StrokeItApp( const char* apppath );
    
    virtual ~StrokeItApp();

    virtual void MessageReceived( BMessage* message );

    virtual void AboutRequested();
    virtual bool QuitRequested();

    BubbleHelper* BubbleHelp()
    {
      return bubblehelp;
    }

    SettingsFile* Settings()
    {
      return settings;
    }
};

#endif //STROKEIT_APP_H
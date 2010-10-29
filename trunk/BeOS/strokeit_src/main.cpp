#include <Application.h>

#include <unistd.h>
//#include <string.h>


#include "StrokeItApp.h"


int main(int argc, char **argv)
{
  //make sure we are in the local dir, even when run from Tracker...
  int counti;
  char datapath[256];
  for (counti = strlen(argv[0]); argv[0][counti] != '/'; counti--);
  strncpy(datapath, argv[0], (size_t)counti);
  chdir(datapath);    
  //////////////////

  new StrokeItApp( datapath );
  be_app->Run();
  delete be_app;
}
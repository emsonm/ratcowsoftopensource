#include <stdio.h>
#include "fdb.h"

DBApp::DBApp():
  BApplication("application/x-vnd.befpc-debugconsole")
{

}

DBApp::~DBApp(){

}

void DBApp::MessageReceived(BMessage *message){
  status_t err = B_OK;
  const char *text;
  
  if (message->what == 'dbug'){
    if ( (err = message->FindString("dbstring", &text)) == B_OK){
      printf("dbug : %s\n", text);
    }
    else{
      printf("error %d\n", err);
    }
  }
  else{
    BApplication::MessageReceived(message);
  }
}

int main(){
  printf("befpc debug console v0.1\nMatt Emson, April 2002\n");
  DBApp *dba = new DBApp();
  dba->Run();
}
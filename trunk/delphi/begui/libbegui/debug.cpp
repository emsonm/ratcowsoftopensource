#include <Application.h>
#include <Message.h>
#include <String.h>
#include "debug.h"

void SendMessage(BMessage *message){
  if (be_app != NULL){
    BMessenger messenger("application/x-vnd.befpc-debugconsole");
    if (messenger.IsValid()) messenger.SendMessage(message);    
  }
}

void SendText(const char *text){
  BMessage *msg = new BMessage('dbug');
  BString s;
  s << "libbegui: " << text;
  msg->AddString( "dbstring", s.String() );
  SendMessage(msg);
}
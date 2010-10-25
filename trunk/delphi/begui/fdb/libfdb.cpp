#include <Application.h>
#include <Message.h>
#include <stdio.h>
#include "libfdb.h"

void SendMessage(BMessage *message){
  if (be_app != NULL){
    printf("in sendmessage\n");
    BMessenger messenger("application/x-vnd.befpc-debugconsole");
    if (messenger.IsValid()) messenger.SendMessage(message);    
  }
}

void SendText(const char *text){
  BMessage *msg = new BMessage('dbug');
  printf("in sendtext\n");
  msg->AddString("dbstring", text);
  SendMessage(msg);
}
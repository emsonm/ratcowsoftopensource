/*
  $Header: /cvsroot/befpc/begui/begui/libbegui/MList.cpp,v 1.1.1.1 2002/03/31 10:36:25 memson Exp $
  
  $Revision: 1.1.1.1 $
  
  $Log: MList.cpp,v $
  Revision 1.1.1.1  2002/03/31 10:36:25  memson

  initial import into sourceforge

  Revision 1.7  2002/03/12 18:39:17  memson

  Revision 1.8  2002/03/11 23:22:11  memson

  Revision 1.6  2002/02/14 23:39:53  memson

  I've now added most of the events to the MButton, MCanvas, MMemo and MEdit..
  started a MPanel component. Again, the MCanvas is *not* tread safe!!!
  This means the calling thread must create it.

  NB. Before I add much more functionality, I'm going to look at other GUI
  libs to see if there are any controls worth borrowing.

  Next control will probably be a MLabel followed by a MMenuBar and MMenuItem.
  MForm will own a MMenuBar.

  Revision 1.5  2002/02/14 14:00:16  memson
  fiddled a bit.. nothing major.

  Revision 1.4  2002/02/13 23:26:25  memson
  Got the MMemo working.
  Got the MEdit working, including OnClick event signal
  Got the MButton working with a signal
  Added a few get/set accessor routined for captions/text.
  
  Revision 1.3  2002/02/11 23:26:44  memson
  Revision 1.2  2002/01/17 20:32:54  memson
    
*/

#include "MList.h"

bool MList::AddItem(MListItem* item){
  return BList::AddItem((void*)item);
}

MListItem* MList::GetItem(int32 index){
  return (MListItem*)BList::ItemAt(index);
}

MListItem::MListItem(void *item){
  fItem = item;
}

MListItem::~MListItem(){
  fItem = NULL;
}

void* MListItem::getItem(void){
  return fItem;
}

void MListItem::setItem(void *item){
  if (fItem) {
    fItem = NULL;
  }
  
  fItem = item;
}

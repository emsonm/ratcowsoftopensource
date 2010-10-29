#include <iostream.h>
#include <Errors.h>

#include "StrokeList.h"

//////////////////////////////////////////////////////////////////////////////

    StrokeListItem::StrokeListItem( const char* s )
    {
      stroke = new BString();
      key = new BString();

      /////////////////////
      stroke->Append((char*)s, strlen(s));
      key->Append((char*)s, strlen(s));
      //cout << key->String() << "  " << stroke->String() << endl;
      
      //truncate the strings
      int pos = stroke->FindFirst('=');
      cout << pos << "  ";

      key->Remove(0, pos +1);
        
      stroke->Remove(pos, stroke->Length() - pos); 

      /////////////////////
      cout << key->String() << " - " << stroke->String() << endl;
    }

    StrokeListItem::~StrokeListItem()
    { 
      delete stroke;
      delete key;
      cout << "~StrokeListItem" << endl;
    } 

    BString* StrokeListItem::getKey()
    {
      return key;
    }
     
    BString* StrokeListItem::getStroke()
    {
      return stroke;
    }


//////////////////////////////////////////////////////////////////////////////

    StrokeList::StrokeList()
    {
      list = new BList(2000);
    }

    StrokeList::~StrokeList()
    {
      delete list;
      cout << "~StrokeList" << endl;
    }

    void StrokeList::ClearList()
    {
      StrokeListItem* t;
  
      for ( int32 i = list->CountItems() ; i > 0; i-- )
      { 
        t = (StrokeListItem*)list->ItemAt(i);
        list->RemoveItem(i);
        delete t;
        cout << "Removed " << i << endl;
      }
    }

    uint32 StrokeList::Count()
    {
      return list->CountItems();
    }

    StrokeListItem* StrokeList::getItemAt(int32 index)
    {
       return (StrokeListItem*) list->ItemAt(index);
    }
    
    void StrokeList::addItem( const char* s )
    {
       list->AddItem( new StrokeListItem( s ) );
    }

    bool StrokeList::lookUp(const char* strk, char& ky)
    {
      int32 cnt = Count() -1;
      StrokeListItem* curr;

      cout <<  cnt << endl;
      
      //while (0 > 1)
      for (int i = 0; i <= cnt; i++)
      {
        curr = getItemAt(i);

        cout << i <<" > " << curr->getStroke()->String() << "  " << strk << endl;
  
        if ( curr->getStroke()->Compare(strk) == 0 ) {
          cout << curr->getStroke()->String() << "  " << strk << endl;
          cout << curr->getKey()->String() << endl;
          strncpy(&ky, curr->getKey()->String(), 1);
          return true;
        }
      }
      return false;  
    }

//////////////////////////////////////////////////////////////////////////////

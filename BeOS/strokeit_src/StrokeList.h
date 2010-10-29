#ifndef STROKE_LIST_H
#define STROKE_LIST_H

#include <String.h>
#include <List.h>

class StrokeListItem
{
  private:
    BString* stroke;
    BString* key;

  public:
    StrokeListItem( const char* s );    
    virtual ~StrokeListItem();

    BString* getKey();
    BString* getStroke();  
};

class StrokeList
{
  private:
    BList* list;

  public:
    StrokeList();
    virtual ~StrokeList();

    void ClearList();
    uint32 Count();
    
    StrokeListItem* getItemAt(int32 index);
    void addItem( const char* s );
    bool lookUp(const char* strk, char& ky);
};


#endif //STROKE_LIST_H 
#include <Application.h>


class DBApp : public BApplication{
public:
  DBApp();
  virtual ~DBApp();
  virtual void MessageReceived(BMessage *message);
};
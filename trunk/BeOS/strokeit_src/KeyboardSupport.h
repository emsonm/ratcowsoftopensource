#ifndef KEYBOARD_SUPPORT_H
#define KEYBOARD_SUPPORT_H

// Data types

typedef struct modifier_t
{
  bool active;
  bool isRight;
};


typedef struct key_event_t
{
	bool down;
	uint32 key; 
  modifier_t shift;
  modifier_t control;
  modifier_t command;
  modifier_t option;
};


#endif //KEYBOARD_SUPPORT_H
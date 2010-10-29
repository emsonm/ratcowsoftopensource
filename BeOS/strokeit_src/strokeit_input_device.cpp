/*
    This code is based upon the input server add on from the:

	BeOS VNC Server component by Mikael Eiman (mikael@eiman.tv)
	Copyright 2001
	
	The following rules apply to the use of this code:
	
	* The code may be freely distributed, as long as this comment isn't removed or edited
	* The code may not be used for commercial purposes without the explicit permission of the author
	* If you fix anything, send a copy of the corrected code to the author!
*/

#include <InputServerDevice.h>
#include <stdio.h>
#include <InterfaceDefs.h>
#include <Messenger.h>
#include <MessageQueue.h>

int32 port_thread_func( void * _data );
int32 message_shuffler( void * _data );

typedef struct data_t
{
	BMessageQueue		queue;
	BInputServerDevice	* device;
};

class StrokeITinput : public BInputServerDevice
{
	private:
		thread_id	port_thread, messenger_thread;
		data_t		data;
		
	public:
		StrokeITinput()
		:	port_thread(0),
			messenger_thread(0)
		{
			data.device = this;
		};
		
		virtual status_t InitCheck()
		{
			input_device_ref mouse = {
				"StrokeIT Mouse", B_POINTING_DEVICE, (void*)this
			};
			input_device_ref keyboard = {
				"StrokeIT Keyboard", B_KEYBOARD_DEVICE, (void*)this
			};
			
			input_device_ref * devices[3] = {
				&mouse,
				&keyboard,
				NULL
			};
			
			RegisterDevices( devices );
			
			return B_OK;
		};
		
		virtual status_t Start( const char *, void*)
		{
			if ( port_thread )
				return B_OK;
			
			messenger_thread = spawn_thread(
				message_shuffler,
				"StrokeIT Message thread",
				B_NORMAL_PRIORITY,
				&data
			);
			
			resume_thread( messenger_thread );
			
			port_thread = spawn_thread(
				port_thread_func,
				"StrokeIT port reader thread",
				B_NORMAL_PRIORITY,
				&data
			);
			
			resume_thread( port_thread );
			
			return B_OK;
		};
		
		virtual status_t Stop( const char *, void*)
		{
			return B_OK;
		};
		
		virtual ~StrokeITinput()
		{
		};
};


int32 message_shuffler( void * _data )
{
	data_t * data = (data_t*)_data;
	
	while ( true )
	{
		BMessage * msg = NULL;
		
		data->queue.Lock();
		if ( !data->queue.IsEmpty() )
		{
			msg = data->queue.NextMessage();
		}
		data->queue.Unlock();
		
		if ( msg )
			data->device->EnqueueMessage( msg );
		else
			snooze(10000);
	}
	
	return 0;
};

int32 port_thread_func( void * _data )
{
	printf("StrokeIT: Port reader running\n");
	data_t * data = (data_t*)_data;
	
	port_id		port = create_port(100, "StrokeIT Input port");
	
	while ( true )
	{
		char msgbuffer[512];
		int32 msgcode;
		
		ssize_t numread = read_port(
			port, 
			&msgcode, 
			msgbuffer, 
			sizeof(msgbuffer) 
		);
		
		if ( numread < 0 )
		{ // Error reading port
			continue;
		}
		
		BMessage * msg = new BMessage;
		status_t res = msg->Unflatten( msgbuffer );
		if ( res == B_OK )
		{
			msg->AddInt64("when", system_time() );
			msg->AddInt32("modifiers", modifiers() );
			
			data->queue.Lock();
			data->queue.AddMessage( msg );
			data->queue.Unlock();
			
			//data->device->EnqueueMessage(msg);
		} else
		{ // Some error, shouldn't happen
			delete msg;
		}
	};
	
	return 0;
}

#ifdef __POWERPC__
#pragma export on
#endif
extern "C" _EXPORT 
BInputServerDevice * 
instantiate_input_device()
{
	printf("StrokeIT: instantiate_input_device\n");
	return new StrokeITinput;
}

#include <Window.h>
#include <FindDirectory.h>
#include <Path.h>

#include <iostream.h>

#include "StrokeItView.h"
#include "StrokeItWindow.h"
#include "StrokeItApp.h"

    void StrokeItView::openstrokes()
		{
 	    BFile strokes;
      BEntry entry;
      BPath settingspath;
      off_t fileSize;
      char ch;
      uint32 check;

      //reload settingsfile..
      ((StrokeItApp*)be_app)->Settings()->Load();

      if ((check=find_directory(B_USER_SETTINGS_DIRECTORY, &settingspath))==B_OK)
      {
        //found path to settings...
        cout << "The path to the settings is... " << settingspath.Path() << endl;

        if (settingspath.Append("strokeit/strokes") == B_OK)
          _strokepath.SetTo( settingspath.Path() ); 
      }

      BString tmppath; 
      BString tmpfilename;
 
      //get the filename to use as defined in settings..
      if (((StrokeItApp*)be_app)->Settings()->FindString("strokefile", &tmpfilename) == B_OK)
      {
        tmppath << _strokepath.String() << "/" << tmpfilename.String();  
      }
      else
      {
        tmppath << _strokepath.String() << "/default.strokes";
        ((StrokeItApp*)be_app)->Settings()->AddString("strokefile", "default.strokes");
        ((StrokeItApp*)be_app)->Settings()->Save();
      }

      
      
      cout << "Looking for strokes in : "<< tmppath.String() << endl; 
       
      entry.SetTo( tmppath.String() );

			if (B_OK != strokes.SetTo(&entry, B_READ_ONLY))
      {
        cout << "no strokes" << endl; 
        return;
      }
      else cout << "strokes loaded" << endl; 

      strokes.GetSize(&fileSize);
      cout << "file sixe " << " : " << fileSize << endl;
      
      BString tmp;
      while( true ) 
      { 
        if (strokes.Read(&ch, sizeof(char)) == 0)
        {
          break;
        }

        if (ch < 32) 
        {
          tmp << '\0';

          if (tmp.Compare("0=0") == 0) break; //allow "0=0" as EOF
          if (tmp.CountChars()==0) break; //attempt to break on a null string
          strokelist->addItem(tmp.String());
          tmp.SetTo(""); //clear...
        } 
        else
        {
          tmp.Append(&ch, 1);
        }     
      }
      cout << endl;
      cout << "read file" << endl;
      strokesloaded = true;
		}

    void StrokeItView::closestrokes()
    {
      strokelist->ClearList();
      strokesloaded = false;
    } 

    void StrokeItView::closedata()
    {
      if (strokesloaded)
        closestrokes();
    }

    void StrokeItView::reopendata()
    {
      closedata();
      openstrokes();
    }

    StrokeItView::StrokeItView(const char* apppath, int boxSize, BRect rect): 
      BView(rect, "StrokeItView", 0, B_WILL_DRAW | B_FOLLOW_ALL)
    {
      strokelist = new StrokeList(); 
      buttonDown = false;
      strokesloaded = false;
      lastbucket = 0;

      boxwidth = boxSize;
      boxheight = boxwidth;

      cout << "width of " << boxwidth << endl;

      capslock = new BCheckBox(BRect(3, (boxheight * 3) + 10 /*160*/, 3+75, (boxheight * 3) + 30 /*160+20*/), "CapsLck", "Caps lock", new BMessage('cpLk'), 0);
      AddChild(capslock);  
      
      numlock = new BCheckBox(BRect(80, (boxheight * 3) + 10 /*160*/, 80+75, (boxheight * 3) + 30 /*160+20*/), "NumLck", "Num lock", new BMessage('nmLk'), 0);
      AddChild(numlock); 

      returnkey = new BButton(BRect(3, (boxheight * 3) + 35 /*185*/, 28, (boxheight * 3) + 60 /*210*/), "retBtn", "Ret", new BMessage('retK'), 0);
      AddChild(returnkey);  

      delkey = new BButton(BRect(28, (boxheight * 3) + 35 /*185*/, 56, (boxheight * 3) + 60 /*210*/), "delBtn", "DEL", new BMessage('delK'), 0);
      AddChild(delkey);

      spacekey = new BButton(BRect(28 + 28, (boxheight * 3) + 35 /*185*/, 56 + 60, (boxheight * 3) + 60 /*210*/), "spcBtn", "", new BMessage('spcK'), 0);
      AddChild(spacekey);

      get_key_map( &keymap_keys, &keymap_chars );

      _strokepath << apppath;
      cout << _strokepath.String() << endl;

		  openstrokes();
		}
    
    StrokeItView::~StrokeItView()
    {
      delete strokelist;

      app_bubblehelp->SetHelp(this, NULL);
      app_bubblehelp->SetHelp(spacekey, NULL);
      app_bubblehelp->SetHelp(delkey, NULL);
      app_bubblehelp->SetHelp(returnkey, NULL);
      app_bubblehelp->SetHelp(numlock, NULL);
      app_bubblehelp->SetHelp(capslock, NULL);

      cout << "~StrokeItWindow" << endl;
    }
    
    void StrokeItView::AttachedToWindow()
    {
      SetViewColor( 175, 175, 175, 0 );
      SetHighColor( 255, 0, 0, 0 );
      SetLowColor( 0, 255, 0, 0 );

      //grab the bubble helper from the app
      app_bubblehelp = ((StrokeItApp*)be_app)->BubbleHelp();
      app_bubblehelp->SetHelp(this, "StrokeIt entry window");
      app_bubblehelp->SetHelp(spacekey, "Space key");
      app_bubblehelp->SetHelp(delkey, "Delete key");
      app_bubblehelp->SetHelp(returnkey, "Return key");
      app_bubblehelp->SetHelp(numlock, "Numlock");
      app_bubblehelp->SetHelp(capslock, "Capslock");

      setSize(boxwidth);
    }

    void StrokeItView::Draw(BRect updateRect)
    {
      //draw grid...
      
      StrokeRect( BRect(0, 0, boxwidth * 2 /*100*/, boxwidth * 2 /*100*/) );
      StrokeRect( BRect(0, 0, boxwidth /*50*/, boxwidth * 3 /*150*/) );
      StrokeRect( BRect(0, 0, boxwidth * 3 /*150*/, boxwidth * 3 /*150*/) );
      StrokeRect( BRect(0, boxwidth * 2 /*100*/, boxwidth * 3 /*150*/, boxwidth /*50*/) );
      StrokeRect( BRect(0, boxwidth * 2 /*100*/, boxwidth * 2 /*100*/, boxwidth * 3 /*150*/) );

      //cout << "draw" << endl;
      Sync();
    } 

    bool StrokeItView::inBox(BPoint point)
    {
      if ( (point.x > 0 && point.y > 0) && (point.x < (boxheight * 3) && point.y < (boxwidth * 3)) ) 
        return true;
      else 
        return false; 
    }
    
    void StrokeItView::MouseDown(BPoint point)
    {
      //cout << "mousedown !!!\n";
      int32 val;
    	Window()->CurrentMessage()->FindInt32("buttons", &val);
	    uint32 buttons = val;
	    if ( (buttons & B_PRIMARY_MOUSE_BUTTON) && inBox(point) )
      {
        bucket.SetTo("");
        buttonDown = true; //this is used to control the mouse tracking
        plot(point.x, point.y); //decode the mouse position
        lastpos = point; //save pos for drawing the tracer
      }
    } 
    
    void StrokeItView::MouseMoved(BPoint where, uint32 code, const BMessage *a_message)
    {  
      //cout << "mousemove a!!!\n";
     
      if (buttonDown && inBox(where) ) 
      { 
        //grab old pensize and set
        float oldpensize = PenSize();
        SetPenSize(5.0);
        //grab old color and set to blue
        rgb_color oldcolour = HighColor();
        SetHighColor(0, 0, 150, 0); 
        //draw line
        StrokeLine(lastpos, where);
        //reset color and pensize
        SetPenSize(oldpensize);
        SetHighColor(oldcolour); 
        //backup the pos for next draw cycle
        lastpos = where;
 
        //cout << "mousemove b!!!\n";
        
        plot(where.x, where.y); //decode point
      }
    } 

    void StrokeItView::MouseUp(BPoint point)
    {
      //cout << "mouseup !!! " << bucket.String() << endl;
      
      buttonDown = false; //stop the process
      plot(point.x, point.y); //plot last point

      char ch;
      
      //lookup the value - if it exists...
      if (strokelist->lookUp( bucket.String(), ch))
      {
        sendKeydown((int32)ch);
      }
      
      Invalidate(); //force grid redraw..
    } 

    void StrokeItView::setSize(int32 newSize)
    {
      cout << newSize << endl;

      boxwidth = newSize;
      boxheight = boxwidth;
      
      if (LockLooper())
      {
        //reset the controls positions
        if (newSize>=50)
        {
          //capslock = new BCheckBox(BRect(3, (boxheight * 3) + 10 /*160*/, 3+75, (boxheight * 3) + 30 /*160+20*/), "CapsLck", "Caps lock", new BMessage('cpLk'), 0);
          capslock->MoveTo( 3, (boxheight * 3) + 10 );
          capslock->ResizeTo( 75,  20 );

          //numlock = new BCheckBox(BRect(80, (boxheight * 3) + 10 /*160*/, 80+75, (boxheight * 3) + 30 /*160+20*/), "NumLck", "Num lock", new BMessage('nmLk'), 0);
          numlock->MoveTo( 80, (boxheight * 3) + 10 );
          numlock->ResizeTo( 75, 20 );

          //returnkey = new BButton(BRect(3, (boxheight * 3) + 35 /*185*/, 28, (boxheight * 3) + 60 /*210*/), "retBtn", "Ret", new BMessage('retK'), 0);
          returnkey->MoveTo( 3, (boxheight * 3) + 35 );
          returnkey->ResizeTo( 25, 25);

          //delkey = new BButton(BRect(28, (boxheight * 3) + 35 /*185*/, 56, (boxheight * 3) + 60 /*210*/), "delBtn", "DEL", new BMessage('delK'), 0);
          delkey->MoveTo( 28, (boxheight * 3) + 35 );
          delkey->ResizeTo( 28, 25);

          //spacekey = new BButton(BRect(28 + 28, (boxheight * 3) + 35 /*185*/, 56 + 60, (boxheight * 3) + 60 /*210*/), "spcBtn", "", new BMessage('spcK'), 0);
          spacekey->MoveTo( 56, (boxheight * 3) + 35 );
          spacekey->ResizeTo( 60, 25);
        }
        else
        {
          //allow for a slightly different layout to get round smaller surface space... 
          capslock->MoveTo( 3, (boxheight * 3) + 10 );
          capslock->ResizeTo( 75,  20 );

          numlock->MoveTo( 3, (boxheight * 3) + 30 );
          numlock->ResizeTo( 75, 20 );

          returnkey->MoveTo( 3, (boxheight * 3) + 50 );
          returnkey->ResizeTo( 25, 25);

          delkey->MoveTo( 28, (boxheight * 3) + 50 );
          delkey->ResizeTo( 28, 25);
          
          if (newSize==40)
          {
            spacekey->MoveTo( 56, (boxheight * 3) + 50 );
            spacekey->ResizeTo( 60, 25);
          }
          else
          {
            spacekey->MoveTo( 3, (boxheight * 3) + 75 );
            spacekey->ResizeTo( 60, 25);
          }
        }

        Invalidate();
        UnlockLooper();
      } 
    }
    
    void StrokeItView::plot(int32 x, int32 y)
    {
      //"bucket" is not my term. It comes from a doc I read about this principal
      //written by the author of libstroke. Each quadrant of the grid is a logical
      //bucket...
      int32 thisbucket;
      static bool commitLast; //DANO FIX
 
      cout << "x = " << x << " y = " << y << endl; //DANO FIX 
      
      //simple code to determine the pos.
      // TODO : make this generic and use a const or global
      //        called 'unit_size'.... this mill allow for 
      //        scaling of grid/buckets.
      if      (x <  boxheight      /*50*/  && y < boxwidth       /*50*/)  thisbucket = 1;
      else if (x < (boxheight * 2) /*100*/ && y < boxwidth       /*50*/)  thisbucket = 2;
      else if (x < (boxheight * 3) /*150*/ && y < boxwidth       /*50*/)  thisbucket = 3;
      else if (x <  boxheight      /*50*/  && y < (boxwidth * 2) /*100*/) thisbucket = 4;
      else if (x < (boxheight * 2) /*100*/ && y < (boxwidth * 2) /*100*/) thisbucket = 5;
      else if (x < (boxheight * 3) /*150*/ && y < (boxwidth * 2) /*100*/) thisbucket = 6;
      else if (x <  boxheight      /*50*/  && y < (boxwidth * 3) /*150*/) thisbucket = 7;
      else if (x < (boxheight * 2) /*100*/ && y < (boxwidth * 3) /*150*/) thisbucket = 8;
      else if (x < (boxheight * 3) /*150*/ && y < (boxwidth * 3) /*150*/) thisbucket = 9;
      else return; //fix bug: fails if outside bounds

      //if (thisbucket != lastbucket) //has cursor moved to a *new* bucket?
      if (thisbucket != lastbucket && !commitLast) //DANO FIX  //has cursor moved to a *new* bucket?
      {
        BString tmp;

        commitLast = true; //DANO FIX
 
        //"bucket" is a global string that is used to track the sequence. I could
        //use a uint32 I guess, but the original Delphi code uses Delphi strings,
        //and this is also the simplest way to store sequence..
        cout << "box to add is : " << thisbucket; //DANO FIX
        tmp.SetTo("");   //DANO FIX     //hack to get around my dislike of the standatd C 
        
		tmp << thisbucket;        //hack to get around my dislike of the standatd C 
        
		cout << " our temp has '" << tmp.String() << "' in it after assign" << endl; //DANO FIX
        
		bucket.Append(tmp, 1);    //string handling routines!!!
        cout << "bucket is now " << bucket.String() << endl; //DANO FIX

        lastbucket = thisbucket; //remember this bucket next time round...
      }
      else                    //DANO FIX
        commitLast = false;   //DANO FIX
    }

    void StrokeItView::sendKeydown( int32 key )
    {
      //This is a hacked version of a routine from my SKBD app... 
      
      #ifdef KEYBOARD_DEBUG

			app_info inf;
			
			be_roster->GetActiveAppInfo(&inf);
			
			cout << "App target is : " << inf.signature << endl;

      #endif
			
		
			key_event_t event;

      event.shift.active = (bool)capslock->Value();

      //numlock overrides shiftlock
      if ( (bool)numlock->Value() )
      {
         // Translate the ASCII key values to numerals
         // when numlock is on...
         switch( key )
         {
           case 'I': 
             key = '1';
             break;
           case 'Z':
             key = '2';
             break;
           case 'B':
             key = '3';
             break;
           case 'L':
             key = '4';
             break;
           case 'S':
             key = '5';
             break;
           case 'C': //this one sucks..
           case 'G': 
             key = '6';
             break;
           case 'T':
             key = '7';
             break;
           case '8':
           case '9':
             break;
           case 'O':
             key = '0';
             break;
           default:
             return; //do nothing.. not mapped      
           
         }
      }
      else if (!event.shift.active)
      {
        //this handles the uppercase/lowercase conversion...
        //uppercase letters are $20/32 appart from uppercase
   	    if (key >= 65 && key <= (65 +25)) 
        {
          key = key + 0x20; //32
        }
      }
      
      event.key = key;
		  //cout << "0x" << hex << event.key << "\n";
     	
		  event.option.active = false;  //obsolete???
			event.control.active = false;	//obsolete???
			event.command.active = false; //obsolete???

      event.down = true; 
	    dokeyevent( event ); //send (B_KEY_DOWN) event
      
      snooze(1000); //wait for 1000th second 
      
      event.down = false;
      dokeyevent( event ); //send (B_KEY_UP) event
    }

    void StrokeItView::dokeyevent( key_event_t event )
	  {
      // this is an extremely truncated version os a routine lifted from VNC Server/SKBD   	    

      BMessage msg;
	
			int32	curr_modifiers=0;	// modifiers
		
			if ( event.down )
				msg.what = B_KEY_DOWN;
			else
				msg.what = B_KEY_UP;
			
			char string[4] = {0,0,0,0};
	    
      switch (event.key)
	    {  
        case 13:
          msg.AddInt32("key", 0x47);
				  string[0] = B_RETURN;
          break;
        
        /*case ???: /////// DELETE as in char to left, not BACKSPACE
          msg.AddInt32("key", 0x34);
					string[0] = B_DELETE;
          break;*/
        case 8:
          msg.AddInt32("key", 0x1e);
					string[0] = B_BACKSPACE;
          break;
          
        default:
          //msg.AddInt32("key", event.key); // this has an odd effect in terminal
          string[0] = event.key;        
			}
      
      msg.AddString("bytes", string);
		  for ( int i=0; string[i]; i++ )
			  msg.AddInt8("byte", string[i]);
			msg.AddInt32("modifiers", curr_modifiers);

			#ifdef KEYBOARD_DEBUG
			msg.PrintToStream();
			#endif
			
			if ( msg.FlattenedSize() > 20 )
			{ // flatten and send if 
	      
	      input_port = find_port("StrokeIT Input port");
			  if ( input_port < 0 )
				  cout << "Error connecting to input server add-on" << endl;
			  //else
				//  cout << "Input server add-on port id: " << input_port << "\n";
	
				char * buffer = new char[msg.FlattenedSize()];
				msg.Flatten( buffer, msg.FlattenedSize() );
	
	      if (  write_port( input_port, 123, buffer, msg.FlattenedSize() ) != B_OK  )
	      { 
					cout << "Error writing to input add-on port\n";
	      } 
				delete [] buffer;
			}
	   
    }
    
    //Obsolete....
		bool StrokeItView::find_key_in_keymap( int32 key, int32 * map, char * chars, BMessage * msg, int32 modif )
    { 
		  uint32 offset = map[key]; 
		  int size = chars[offset++]; 
		  char str[4] = {0,0,0,0}; 
		     
		
		  switch( size ) 
		  { 
		    case 0: 
			  return false; //unmapped key
		
				default: 
				// 2-, 3-, or 4-byte UTF-8 character 
				{ 
			  	msg->AddInt32("key", chars[offset]);
								
					strncpy( str, &(chars[offset]), size ); 
			    msg->AddString("bytes", str);
			    for ( int i=0; str[i]; i++ )
						msg->AddInt8("byte", str[i]);
					msg->AddInt32("modifiers", modif);
          
          #ifdef KEYBOARD_DEBUG
					cout << str << "\n"; 
          #endif  
				} 
				return true;  
			} 
		
		  return false;
		}

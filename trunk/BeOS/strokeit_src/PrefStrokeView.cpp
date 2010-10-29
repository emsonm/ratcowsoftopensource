#include "PrefStrokeView.h"
#include "PrefWindow.h"

    PrefStrokeView::PrefStrokeView(float left, float top) :
      BView(BRect(left, top, left+150, top+150), "pref_stroke_view", 0, B_WILL_DRAW | B_FOLLOW_ALL) 
    {
      SetViewColor( 175, 175, 175, 0 );
      boxwidth = 50;
      boxheight = 50;
      buttonDown = false;
    }

    void PrefStrokeView::Draw(BRect updateRect)
    {
      //draw grid...
      SetHighColor( 0, 255, 0, 0 );
      StrokeRect( Bounds() );
      
      SetHighColor( 255, 0, 0, 0 );
      StrokeRect( BRect(0, 0, boxwidth * 2 /*100*/, boxwidth * 2 /*100*/) );
      StrokeRect( BRect(0, 0, boxwidth /*50*/, boxwidth * 3 /*150*/) );
      StrokeRect( BRect(0, 0, boxwidth * 3 /*150*/, boxwidth * 3 /*150*/) );
      StrokeRect( BRect(0, boxwidth * 2 /*100*/, boxwidth * 3 /*150*/, boxwidth /*50*/) );
      StrokeRect( BRect(0, boxwidth * 2 /*100*/, boxwidth * 2 /*100*/, boxwidth * 3 /*150*/) );

      //cout << "draw" << endl;
      Sync();
    }

    void PrefStrokeView::plot(int32 x, int32 y)
    {
      //"bucket" is not my term. It comes from a doc I read about this principal
      //written by the author of libstroke. Each quadrant of the grid is a logical
      //bucket...
      int32 thisbucket;
      
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

      if (thisbucket != lastbucket) //has cursor moved to a *new* bucket?
      {
        BString tmp;

        //"bucket" is a global string that is used to track the sequence. I could
        //use a uint32 I guess, but the original Delphi code uses Delphi strings,
        //and this is also the simplest way to store sequence..
        tmp << thisbucket;        //hack to get around my dislike of the standatd C 
        bucket.Append(tmp, 1);    //string handling routines!!!

        lastbucket = thisbucket; //remember this bucket next time round...
      }
    }

    bool PrefStrokeView::inBox(BPoint point)
    {
      if ( (point.x > 0 && point.y > 0) && (point.x < (boxheight * 3) && point.y < (boxwidth * 3)) ) 
        return true;
      else 
        return false; 
    }
    
    void PrefStrokeView::MouseDown(BPoint point)
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
    
    void PrefStrokeView::MouseMoved(BPoint where, uint32 code, const BMessage *a_message)
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

    void PrefStrokeView::MouseUp(BPoint point)
    {
      //cout << "mouseup !!! " << bucket.String() << endl;
      
      buttonDown = false; //stop the process
      plot(point.x, point.y); //plot last point

      //set the textbox..
      ( (PrefWindow*)Window() )->update_trainer_output( bucket.String() ); 
      
      Invalidate(); //force grid redraw..
    } 

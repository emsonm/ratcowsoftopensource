//----------------------------------------------------------------------------------
// - Authors			     - Pham Minh Tri (http://www.codeproject.com/Articles/5428/Nullable-DateTimePicker)
//                       oliwan (http://www.codeproject.com/script/Membership/View.aspx?mid=3148586)
// - Last Updated      - 30/09/2010
//----------------------------------------------------------------------------------
// - Component:        - NullableDateTimePicker
// - Version:          - 1.1
// - Description:      - A datetimepicker that allows a null value.
//----------------------------------------------------------------------------------

/* This code has no explicit license attached to it. The standard RatCow Soft license 
 * applies to all modifications made.*/

/*
 * Copyright 2003 - 2012 Pham Minh Tri, oliwan, Rat Cow Software and Matt Emson. 
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this list of
 *    conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list
 *    of conditions and the following disclaimer in the documentation and/or other materials
 *    provided with the distribution.
 * 3. Neither the name of the Rat Cow Software nor the names of its contributors may be used
 *    to endorse or promote products derived from this software without specific prior written
 *    permission.
 *
 * THIS SOFTWARE IS PROVIDED BY RAT COW SOFTWARE "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied, of Rat Cow Software, Matt Emson and any other individual sited in the copyright 
 * statement above.
 *
 */

using System;
using System.Windows.Forms;

namespace RatCow.MvcFramework.Mapping.Controls
{

  /// <summary>
  /// This class wraps the basic control and enables a recognised date to be "null"
  /// </summary>
  public class NullableDateTimePicker : DateTimePicker
  {
    private DateTimePickerFormat _oldFormat = DateTimePickerFormat.Long;
    private string _oldCustomFormat;
    public bool DateIsNull { get; set; }

    private DateTime _nullDate = DateTime.FromOADate( 0 ); //DateTime.MinValue is an alternative
    public DateTime NullDate { get { return _nullDate; } set { _nullDate = value; } }


    private bool _isInternalValueChanging;
    public bool IsInternalValueChanging { get { return _isInternalValueChanging; } }


    bool _SettinUnderlyingDate = false;

    /// <summary>
    /// This method sets the initial underlying date that the popup will use... it does not change the nullable value
    /// </summary>
    /// <param name="date"></param>
    public void SetUnderlyingDate( DateTime date, bool forceNull )
    {
      try
      {
        _SettinUnderlyingDate = true;

        base.Value = date;

        if ( forceNull )
          DateIsNull = true;   //make sure the date is marked null

        Application.DoEvents();  //this will hopefully make sure pending events are all processed
      }
      finally
      {
        _SettinUnderlyingDate = false;
      }
    }

    public NullableDateTimePicker()
      : base()
    {
    }

    protected override void OnValueChanged( EventArgs eventargs )
    {
      if ( !_SettinUnderlyingDate ) base.OnValueChanged( eventargs );
    }

    public DateTime? NullableValue
    {
      get
      {
        if ( DateIsNull )
        {
          return null;
        }
        return base.Value;
      }
      set
      {
        if ( value.HasValue )
        {
          Value = value.Value;
        }
        else
        {
          Value = _nullDate;
        }
      }
    }

    public new DateTime Value
    {
      get
      {
        if ( DateIsNull )
          return _nullDate;
        else
          return base.Value;
      }
      set
      {

        if ( value == _nullDate )
        {
          if ( !DateIsNull )
          {
            _oldFormat = this.Format;
            _oldCustomFormat = this.CustomFormat;
            DateIsNull = true;
          }

          this.Format = DateTimePickerFormat.Custom;
          this.CustomFormat = this.Focused ? "|" : " ";
          base.OnValueChanged( new EventArgs() );
        }
        else
        {
          if ( DateIsNull )
          {
            this.Format = _oldFormat;
            this.CustomFormat = _oldCustomFormat;
            DateIsNull = false;
          }

          if ( value < this.MaxDate && value > this.MinDate )
          {
            base.Value = value;
          }
          else if ( value > this.MaxDate )
          {
            value = this.MaxDate;
          }
          else if ( value < this.MinDate )
          {
            value = this.MinDate;
          }
        }
      }
    }

    protected override void OnCloseUp( EventArgs eventargs )
    {
      if ( Control.MouseButtons == MouseButtons.None )
      {
        if ( DateIsNull )
        {
          _isInternalValueChanging = true;

          this.Format = _oldFormat;
          this.CustomFormat = _oldCustomFormat;
          DateIsNull = false;

          _isInternalValueChanging = false;
        }
      }

      base.OnCloseUp( eventargs ); //or the above happens too late in the process, and the calling routine gets wrong/bogus info
    }

    protected override void OnKeyDown( KeyEventArgs e )
    {
      base.OnKeyDown( e );
      if ( e.KeyCode == Keys.Delete )
      {
        this.Value = _nullDate;
        DateIsNull = true;
      }
      else if ( DateIsNull )
      {
        _isInternalValueChanging = true;
        if ( e.KeyCode == Keys.Space || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Right || e.KeyCode == Keys.Left )
        {
          this.Value = DateTime.Today;
        }
        else if ( ( Char.IsNumber( (char)e.KeyValue ) && e.KeyValue != 48 ) || ( e.KeyValue >= 97 && e.KeyValue <= 105 ) )
        { //"0" correction, NumPad1 - NumPad9 
          int typedDigit = 1;
          if ( e.KeyValue >= 97 && e.KeyValue <= 105 )
          { //<-- NumPad1 - NumPad9 must be calculated to numeric KeyValues 
            typedDigit = int.Parse( ( (char)( e.KeyValue - 48 ) ).ToString() );
          }
          else
          { //if (Char.IsNumber((char)e.KeyValue)) 
            typedDigit = int.Parse( ( (char)e.KeyValue ).ToString() );
          }
          this.Value = DateTime.Now;
          SendKeys.SendWait( "{RIGHT}" ); // Selects the day part
          SendKeys.Send( typedDigit.ToString() ); // Replaces the date part by the typed digit
        }
        _isInternalValueChanging = false;
      }
    }

    protected override void OnGotFocus( EventArgs e )
    {
      base.OnGotFocus( e );
      if ( DateIsNull )
      {
        CustomFormat = "|"; // Show the user that the NullableDateTimePicker has the Focus by simulating a cursor.
      }
    }

    protected override void OnLostFocus( EventArgs e )
    {
      if ( DateIsNull )
      {
        CustomFormat = " ";
      }
    }
  }
}

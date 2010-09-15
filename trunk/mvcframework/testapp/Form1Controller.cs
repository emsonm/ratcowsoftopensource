/*
 * Copyright 2010 Rat Cow Software and Matt Emson. All rights reserved.
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
 * or implied, of Rat Cow Software and Matt Emson.
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RatCow.MvcFramework;
using System.Windows.Forms;

namespace testapp
{
  internal partial class Form1Controller : BaseController<Form1>
  {

    /// <summary>
    /// This automatically creates an encapsulated Form1 instance
    /// </summary>
    public Form1Controller()
      : base()
    {
    }

    /// <summary>
    /// Don't strictly need this
    /// </summary>
    /// <param name="aTarget"></param>
    public Form1Controller(Form1 aTarget)
      : base(aTarget)
    {
    }

    /// <summary>
    /// This shows a simplified access; the code could
    /// pass the event info, or the directly hooked action 
    /// could be used directly.
    /// </summary>
    public void Button1Clicked()
    {
      textBox1.Text = "Hello, world!";
    }
    
    
  }

  #region UI Glue code

  /// <summary>
  /// This MVC Framework is a labour of love, rather than a perfect workd solution. That is to say,
  /// it is intended to use MVC as far as is possible, by craeting as much of an illusion of MVC as
  /// can be had. The controller will always be split in to two partial classes. The main one contains 
  /// all of code that interacts with the data. The secondary (as below) will mediate between the UI
  /// and the pure controller code. This is generally just the glue (Outlets and Actions), but
  /// along the way, it will wrap certain operations to make the code more portable between UI
  /// toolkits and presentation layers.
  /// </summary>
  partial class Form1Controller
  {

    //Outlets go here

    #region Outlets

    [Outlet("textBox1")]
    public TextBox textBox1 { get; set; } //{ get { return _textBox1.Target; } set { _textBox1.Target = value; } }

    [Outlet("button1")]
    public Button button1 { get; set; } //{ get { return _button1.Target; } set { _button1.Target = value; } }

    #endregion


    //Actions go here

    #region Actions

    /// <summary>
    /// We can hook the actions directly, but adding specific methods that we call looks prettier.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    [Action("button1", "Click")]
    public void button1_Click(object sender, EventArgs e)
    {
      Button1Clicked();
    }

    #endregion
  }

  #endregion
}

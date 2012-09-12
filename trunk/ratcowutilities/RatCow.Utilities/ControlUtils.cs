/*
 * Copyright 2007 - 2012 Rat Cow Software and Matt Emson. All rights reserved.
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

namespace RatCow.Utilities
{
  /// <summary>
  /// This call is a copy of the class in RatCow.MVCFramework.Mapping. The plan is to
  /// replace that instance with this one when I next do a release of the tools.
  /// </summary>
  public sealed class ControlUtils
  {
    /// <summary>
    /// recursive "find" for a specific control with in another control (e.g. Panel, Form, Container...) by name
    /// </summary>
    /// <param name="target"></param>
    /// <param name="controlName"></param>
    /// <param name="foundControl"></param>
    /// <returns></returns>
    public static bool FindControl( System.Windows.Forms.Control target, string controlName, ref System.Windows.Forms.Control foundControl )
    {
      //go through the controls in turn
      for ( int i = 0 ; i < target.Controls.Count ; i++ )
      {
        //is *this
        if ( target.Controls[ i ].Name == controlName )
        {
          foundControl = target.Controls[ i ];
          return true;
        }
        if ( FindControl( target.Controls[ i ], controlName, ref foundControl ) ) return true;
      }

      return false;
    }
  }
}

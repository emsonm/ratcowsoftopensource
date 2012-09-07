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

#if !CF_20

using System.Linq;

#endif

using System.Text;

namespace RatCow.MvcFramework
{
  public class UIBridge
  {
    /// <summary>
    /// This is to save referencing the underlying OS message boxes.
    /// </summary>
    /// <param name="message"></param>
    public static void Alert(string message)
    {
      Alert(message, "Warning");
    }

    /// <summary>
    /// This is to save referencing the underlying OS message boxes.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="title"></param>
    public static void Alert(string message, string title)
    {
      System.Windows.Forms.MessageBox.Show(message, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button2); //CF needs this
    }

    /// <summary>
    /// This is to save referencing the underlying OS message boxes.
    /// </summary>
    /// <param name="message"></param>
    public static void Acknowledge(string message)
    {
      Acknowledge(message, "Warning");
    }

    /// <summary>
    /// This is to save referencing the underlying OS message boxes.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="title"></param>
    public static void Acknowledge(string message, string title)
    {
      System.Windows.Forms.MessageBox.Show(message, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information, System.Windows.Forms.MessageBoxDefaultButton.Button2); //CF needs this
    }

    /// <summary>
    /// This is to save referencing the underlying OS message boxes.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool BooleanDecision(string message)
    {
      return BooleanDecision(message, "Please choose...");
    }

    /// <summary>
    /// This is to save referencing the underlying OS message boxes.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static bool BooleanDecision(string message, string title)
    {
      return (System.Windows.Forms.MessageBox.Show(message, title, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes);
    }

    public static void ProcessSystemMessages()
    {
      System.Windows.Forms.Application.DoEvents();
    }
  }

  //this is a listview helper
  public class ListViewHelper<Data> : IDisposable
  {
    System.Windows.Forms.ListView fview = null;
    List<Data> fdata = default(List<Data>);

    public ListViewHelper(System.Windows.Forms.ListView view)
    {
      fview = view;
      fview.VirtualMode = true;
      fview.MultiSelect = false; //select one and only one
      Updating = false;
    }

    public bool Updating { get; internal set; }

    public void BeginUpdate()
    {
      fview.BeginUpdate();
      fview.VirtualListSize = 0;
      Updating = true;
    }

    public void EndUpdate()
    {
      if (fdata != null)
      {
        fview.VirtualListSize = fdata.Count;
      }
      Updating = false;
      fview.EndUpdate();
    }

    public void SetData(List<Data> data)
    {
      if (!Updating)
        BeginUpdate();
      try
      {
        fdata = data;
      }
      finally
      {
        if (Updating)
          EndUpdate();
      }
    }

    /// <summary>
    /// Virtual mode notoriously screws this up badly
    /// </summary>
    public int GetSelectedIndex()
    {
      int result = -1;
      for (int i = 0; i < fview.Items.Count; i++)
      {
        if (fview.Items[i].Selected)
        {
          result = i; //could just return here, but that's messy
          break;
        }
      }
      return result;
    }

    /// <summary>
    /// Experimental
    /// </summary>
    public bool SetSelectedIndex( int index )
    {
      try
      {
        fview.Items[ index ].Selected = true; //this should work
        return true;
      }
      catch ( Exception ex )
      {
        System.Diagnostics.Debug.Write( ex.Message );
        System.Diagnostics.Debug.Write( ex.StackTrace );
        return false;
      }

    }

    /// <summary>
    /// This is a "nice to have" rather than anything really useful.
    /// </summary>
    /// <returns></returns>
    public Data GetSelectedItemOrDefault()
    {
      int itemIndex = GetSelectedIndex();

      if (itemIndex > -1 && itemIndex < fdata.Count)
      {
        return fdata[itemIndex];
      }
      else
        return default(Data);
    }

    #region fdata access

    //added to make the helper allow us to access the internal data without a reference to original instance

    public Data this[int index] { get { return (fdata == null ? default(Data) : fdata[index]); } }

    public int Count { get { return (fdata == null ? 0 : fdata.Count); } }

    #endregion fdata access

    #region IDisposable Members

    public void Dispose()
    {
      fdata = null;
      fview = null;
    }

    #endregion IDisposable Members
  }
}
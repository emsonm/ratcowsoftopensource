﻿using System;
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
  }
}

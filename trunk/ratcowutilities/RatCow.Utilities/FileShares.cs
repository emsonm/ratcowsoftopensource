using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace RatCow.Utilities
{
  public class FileShares
  {
    /// <summary>
    /// The calling process must retain a reference to the process whilst it is using the share
    /// </summary>
    public static Process CreateShare( string sharePath, string userName, string password )
    {
      var process = new Process();

      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      process.StartInfo.FileName = "net.exe";
      process.StartInfo.Arguments = String.Format( @"use {0} {1} /USER:{2}", sharePath, password, userName );
      process.StartInfo.RedirectStandardError = false;
      process.StartInfo.RedirectStandardOutput = false;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = true;

      process.Start();
      process.WaitForExit();
      return process;
    }
  }
}
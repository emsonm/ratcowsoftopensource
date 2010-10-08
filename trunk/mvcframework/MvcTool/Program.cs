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

namespace RatCow.MvcFramework.Tools  // <--- corrected namespace capitalisation and revised location
{
  /// <summary>
  /// This is an extremely basic and pretty much a "giant hack". It is meant to
  /// get us to the point that we could create the desired XxxxController initially
  /// without any donkey work. It obviuously does *not* accout for future changes
  /// to the form... this will come later on.
  /// </summary>
  class Program
  {
    static void Main(string[] args)
    {
      bool isAbstract = false;
      string className;
      //This is new - we try to interpret the compiler params
      if (args.Length == 0)
      {
        Console.WriteLine("USAGE - mvctool [options] classname");
        Console.WriteLine("\r\nOPTIONS:");
        Console.WriteLine(" --abstract / -a : prefix controllers with \"Abstract\" prefix");
        Console.WriteLine();
        return;
      }
      else if (args.Length == 12)
      {
        isAbstract = false;
        className = args[0];
      }
      else if (args.Length > 1 || args.Length == 2)
      {
        //is the param "abstract"?
        if (args[0].Contains("-a"))
        {
          isAbstract = true;
          className = args[1];
        }
        else
        {
          Console.WriteLine("Unknown parameter!");
          return;
        }
      }
      else
      {
        Console.WriteLine("Unknown parameter!");
        return;
      }


      //we currently assume thesrs is one param and that is the name of the class
      //we also assume the files will be named in a standard C# naming convention.
      //i.e. MainForm -> MainForm.Designer.cs
      if (ControllerCreationEngine.Compile(className))
      {
        //if we get here, we created the desired assembly above
        ControllerCreationEngine.Generate(className, isAbstract);
      }
      else
      {
        Console.WriteLine("Error! The file could not be generated.");
        return;
      }
    }
  }
}

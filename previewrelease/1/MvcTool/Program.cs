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

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using Microsoft.CSharp;
using System.Reflection;

namespace Ratcow.MvcFramework.MvcTool
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
      //we currently assume thesrs is one param and that is the name of the class
      //we also assume the files will be named in a standard C# naming convention.
      //i.e. MainForm -> MainForm.Designer.cs
      if (Compile(args[0]))
      {
        //if we get here, we created the desired assembly above
        Generate(args[0]);
      }
    }

    /// <summary>
    /// This loads the generated assembly and gets the class info
    /// </summary>
    /// <param name="className"></param>
    static void Generate(string className)
    {
      Assembly a = Assembly.LoadFrom("temp.dll"); //we dictate this name
      Type[] ts = a.GetTypes();
      foreach (Type t in ts)
      {
        if (t.Name.Equals(className))
        {
          System.Console.WriteLine(t.FullName);
          System.Console.WriteLine(t.Name);
          System.Console.WriteLine(t.Module);
          System.Console.WriteLine(t.Assembly.FullName);

          //essentially, we only support "Winforms" so we assume it is a form or descends from one
          //I might be able to alter this to "component" I gues...
          System.Windows.Forms.Form form = (Activator.CreateInstance(t) as System.Windows.Forms.Form);

          StringBuilder code = new StringBuilder();

          code.AppendLine("/*Auto generated*/ \r\n\t\nusing System; \r\nusing System.Windows.Forms;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\n\r\n//3rd Party\r\nusing RatCow.MvcFramework;\r\n");
          code.AppendFormat("namespace {0}\r\n", t.Namespace);
          code.AppendLine("{");

          StringBuilder code_s1 = new StringBuilder();

          code_s1.AppendFormat("\tinternal partial class {0}Controller: BaseController<{0}>\r\n", t.Name);
          code_s1.AppendLine("\t{");
          
          //constructor
          code_s1.AppendFormat("\t\tpublic {0}Controller() : base()\r\n", t.Name);
          code_s1.AppendLine("\t\t{\r\n\t\t}\r\n");


          StringBuilder code_s2 = new StringBuilder();
          code_s2.AppendLine("\r\n#region GUI glue code\r\n");
          code_s2.AppendFormat("\tpartial class {0}Controller\r\n", t.Name);
          code_s2.AppendLine("\t{");

          //we now have access to the controls
          foreach (System.Windows.Forms.Control control in form.Controls)
          {
            //System.Console.WriteLine(" var {0} : {1} ", control.Name, control.GetType().Name);
            //add the declaration to code_s2
            code_s2.AppendFormat("\t\t[Outlet(\"{1}\")]\r\n\t\tpublic {0} {1} ", control.GetType().Name, control.Name); //add var
            code_s2.AppendLine("{ get; set; }"); 

            //add in the click handlers for buttons, coz I'm lazy like that
            if (control is System.Windows.Forms.Button)
            {
              code_s2.AppendFormat("\t\t[Action(\"{0}\", \"Click\")]\r\n\t\tpublic void F{0}_Click(object sender, EventArgs e)\r\n", control.Name);
              code_s2.AppendLine("\t\t{\r\n\t\t\t//Auto generated call");

              code_s1.AppendFormat("\t\tvoid {0}Click()\r\n", control.Name);
              code_s1.AppendLine("\t\t{\r\n");
              code_s1.AppendLine("\t\t}\r\n");

              code_s2.AppendFormat("\t\t\t{0}Click();\r\n", control.Name);

              code_s2.AppendLine("\t\t}\r\n");
            }
          }

          code_s1.AppendLine("\t}"); //end of class declaration
          code.AppendLine(code_s1.ToString()); 

          code_s2.AppendLine("\t}"); //end of class declaration  
          code_s2.AppendLine("#endregion /*GUI glue code*/");       
          code.AppendLine(code_s2.ToString());

          code.AppendLine("}");

          TextWriter writer = new StreamWriter(File.OpenWrite(String.Format("{0}Controller.cs", className)));
          try
          {
            writer.WriteLine(code.ToString());
          }
          finally
          {
            writer.Close();
          }

          System.Console.WriteLine(code.ToString());

        }
      }
      return;
    }

    /// <summary>
    /// Hacked mini compiler - it does enough to compile *basic* designer classes
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    static bool Compile(string className)
    {
      //we attempt to compile the file provided and then read info from it
      CSharpCodeProvider compiler = new CSharpCodeProvider();
      CompilerParameters compParams = new CompilerParameters();
      compParams.ReferencedAssemblies.Add("System.dll");
      compParams.ReferencedAssemblies.Add("System.XML.dll");
      compParams.ReferencedAssemblies.Add("System.Data.dll");
      compParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");
      compParams.ReferencedAssemblies.Add("System.Drawing.dll");

      //we can add more in here from command line in future revision

      compParams.GenerateExecutable = false;
      compParams.CompilerOptions = "/t:library";
      compParams.OutputAssembly = "./temp.dll";

      StringBuilder s = new StringBuilder();


      string namespaceName = "";
      TextReader tr = new StreamReader(File.OpenRead(String.Format("{0}.Designer.cs", className)));
      try
      {
        //read till we find the namespace

        while (true)
        {
          string t = tr.ReadLine();
          s.Append(t);
          if (t.Contains("namespace"))
          {
            namespaceName = t.Substring(10).Trim();
            break;
          }
        }

        s.Append(tr.ReadToEnd());
      }
      finally
      {
        tr.Close();
      }

      //This is a bit of a hack... we need the designer to be a "Form" so we can compile it and
      //then use the Activator to access the contents later on. We *have* to call "InitializeComponents()"
      //otherwise, the form is in an uninitialized state and we will not have access to the parts we
      //actually *want*.
      string code = "{ public " + className + "(): base() { InitializeComponent();} } /*class*/ } /*namespace*/";
      string dummy = String.Format("namespace {2} {3} partial class {0} : System.Windows.Forms.Form {1}", className, code, namespaceName, "{");

      //The files to compile
      string[] files = { dummy, s.ToString() };

      CompilerResults res = null;
      try
      {
        res = compiler.CompileAssemblyFromSource(compParams, files);
      }
      catch (BadImageFormatException ex)
      {
        System.Console.WriteLine(ex.Message);
        return false;
      }
      catch (Exception ex)
      {
        System.Console.WriteLine(ex.Message);
        return false;
      }
      if (res.Errors.HasErrors)
      {
        StringBuilder sb = new StringBuilder();
        sb.Append("\nIllegal C# source code generated: ");
        sb.Append(res.Errors.Count.ToString());
        sb.Append(" Errors:\n");
        foreach (CompilerError error in res.Errors)
        {
          sb.Append("Line: ");
          sb.Append(error.Line.ToString());
          sb.Append(" - ");
          sb.Append(error.ErrorText);
          sb.Append('\n');
        }
        System.Console.WriteLine(sb.ToString());
        return false;
      }

      return true;
    }
  }
}

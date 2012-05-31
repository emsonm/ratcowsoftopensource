﻿/*
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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace RatCow.MvcFramework.Tools
{
  /// <summary>
  /// Moved this in to a class library so that it can be reused
  /// </summary>
  public class ControllerCreationEngine
  {
    const string ABSTRACT_PREFIX = "Abstract";

    /// <summary>
    /// Split this up in to sections so I can more easliy re-use it in the future.
    /// </summary>
    /// <param name="className"></param>
    public static void Generate(string className, string outputAssemblyName, CompilerFlags flags)
    {
      string prefix = (flags.IsAbstract ? ABSTRACT_PREFIX : String.Empty);

      //this *can* generate more than one entry
      ControlTree[] trees = GenerateTree(className, outputAssemblyName);
      if (trees != null && trees.Length == 1)
      {
        ControlTree tree = trees[0];
        string s = ClassGenerator(tree, flags);

        string fileName = String.Format("{1}{0}Controller.cs", tree.ClassName, prefix);

        //added a check to see if file exists, otherwise we might get weird streaming issues
        //if it does, I delete it for now.
        if (File.Exists(fileName))
          File.Delete(fileName);

        TextWriter writer = new StreamWriter(File.OpenWrite(fileName));
        try
        {
          writer.WriteLine(s);
        }
        finally
        {
          writer.Close();
        }
      }
    }

    /// <summary>
    /// Hacked mini compiler - it does enough to compile *basic* designer classes
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static bool Compile(string className, string outputAssemblyName)
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
      compParams.OutputAssembly = outputAssemblyName; // "./temp.dll";

      StringBuilder s = new StringBuilder();

      string namespaceName = "";
      string fileToCompile = String.Format("{0}.Designer.cs", className);

      if (!File.Exists(fileToCompile)) return false;

      TextReader tr = new StreamReader(File.OpenRead(fileToCompile));
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

    /// <summary>
    /// Builds a tree of control info
    /// </summary>
    /// <param name="className"></param>
    public static ControlTree[] GenerateTree(string className, string outputAssemblyName)
    {
      List<ControlTree> trees = new List<ControlTree>();

      Assembly a = Assembly.LoadFrom(outputAssemblyName /*"temp.dll"*/); //we dictate this name
      Type[] ts = a.GetTypes();
      foreach (Type t in ts)
      {
        if (t.Name.Equals(className))
        {
          ControlTree tree = new ControlTree();
          trees.Add(tree); //just to get it sorted..

          //essentially, we only support "Winforms" so we assume it is a form or descends from one
          //I might be able to alter this to "component" I gues...
          System.Windows.Forms.Form form = (Activator.CreateInstance(t) as System.Windows.Forms.Form);

          tree.ClassName = t.Name;
          tree.NamespaceName = t.Namespace;

          ////we now have access to the controls
          //foreach (System.Windows.Forms.Control control in form.Controls)
          //{
          //  tree.AddControl(control.Name, control.GetType());
          //}

          IterateTree(form, tree, className);
        }
      }
      return trees.ToArray();
    }

    /// <summary>
    /// This fixes the subcontrol issues
    /// </summary>
    private static void IterateTree(System.Windows.Forms.Control targetControl, ControlTree tree, string path)
    {
      Console.WriteLine(path);

      foreach (System.Windows.Forms.Control control in targetControl.Controls)
      {
        tree.AddControl(control.Name, control.GetType());
        if (control.Controls.Count > 0)
        {
          IterateTree(control, tree, String.Format("{0}_{1}", path, control.Name)); //recurse....
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tree"></param>
    /// <param name="isAbstract"></param>
    /// <returns></returns>
    public static string ClassGenerator(ControlTree tree, CompilerFlags flags)
    {
      string prefix = (flags.IsAbstract ? ABSTRACT_PREFIX : String.Empty);

      StringBuilder code = new StringBuilder();

      code.AppendLine("/*Auto generated - this code was generated by the MvcFramework compiler, created by RatCow Soft - \r\n See http://code.google.com/p/ratcowsoftopensource/ */ \r\n\r\nusing System; \r\nusing System.Windows.Forms;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Reflection;\r\n\r\n//3rd Party\r\nusing RatCow.MvcFramework;\r\n");
      code.AppendFormat("namespace {0}\r\n", tree.NamespaceName);
      code.AppendLine("{");

      StringBuilder code_s1 = new StringBuilder();

      code_s1.AppendFormat("\tinternal partial class {1}{0}Controller: BaseController<{0}>\r\n", tree.ClassName, prefix);
      code_s1.AppendLine("\t{");

      //constructor
      code_s1.AppendFormat("\t\tpublic {1}{0}Controller() : base()\r\n", tree.ClassName, prefix);
      code_s1.AppendLine("\t\t{\r\n\t\t}\r\n");

      StringBuilder code_s2 = new StringBuilder();
      code_s2.AppendLine("\r\n#region GUI glue code\r\n");
      code_s2.AppendFormat("\tpartial class {1}{0}Controller\r\n", tree.ClassName, prefix);
      code_s2.AppendLine("\t{");

      if (flags.RestrictActions)
      {
        //load the actions file
        ViewActionMap map = (flags.UseDefaultActionsFile ? GetDefaultViewActionMap() : GetNamedViewActionMap(tree.ClassName, true));

        //we now have access to the controls
        foreach (var control in tree.Controls)
        {
          //System.Console.WriteLine(" var {0} : {1} ", control.Name, control.GetType().Name);
          //add the declaration to code_s2
          code_s2.AppendFormat("\t\t[Outlet(\"{1}\")]\r\n\t\tpublic {0} {1} ", control.Value.Name, control.Key); //add var
          code_s2.AppendLine("{ get; set; }");

          //this should find all known event types
          AddMappedActions(tree.ClassName, control, code_s2, code_s1, flags, map);

          //specific listView hooks
          if (control.Value == typeof(System.Windows.Forms.ListView))
          {
            AddListViewHandler(control.Key, code_s2, code_s1, flags);
          }
        }
      }
      else
      {
        //we now have access to the controls
        foreach (var control in tree.Controls)
        {
          //System.Console.WriteLine(" var {0} : {1} ", control.Name, control.GetType().Name);
          //add the declaration to code_s2
          code_s2.AppendFormat("\t\t[Outlet(\"{1}\")]\r\n\t\tpublic {0} {1} ", control.Value.Name, control.Key); //add var
          code_s2.AppendLine("{ get; set; }");

          //this should find all known event types
          AddKnownActions(tree.ClassName, control, code_s2, code_s1, flags);

          //specific listView hooks
          if (control.Value == typeof(System.Windows.Forms.ListView))
          {
            AddListViewHandler(control.Key, code_s2, code_s1, flags);
          }
        }
      }

      //some boiler plate code which helps set the data for a LisViewHelper

      code_s2.AppendLine("\t\tprotected void SetData<T>(ListViewHelper<T> helper, List<T> data) where T : class");
      code_s2.AppendLine("\t\t{\r\n\t\t\t//Auto generated call");

      code_s2.AppendLine("\t\t\tType t = helper.GetType();");
      code_s2.AppendLine("\t\t\tt.InvokeMember(\"SetData\", BindingFlags.Default | BindingFlags.InvokeMethod, null, helper, new object[] { data });");

      code_s2.AppendLine("\t\t}\r\n");

      code_s1.AppendLine("\t}"); //end of class declaration
      code.AppendLine(code_s1.ToString());

      code_s2.AppendLine("\t}"); //end of class declaration
      code_s2.AppendLine("#endregion /*GUI glue code*/");
      code.AppendLine(code_s2.ToString());

      code.AppendLine("}");

      return code.ToString();
    }

    /// <summary>
    /// Loads a specific file
    /// </summary>
    private static ViewActionMap GetNamedViewActionMap(string className, bool useDefaltIfNotFound)
    {
      var path = Path.Combine(System.Environment.CurrentDirectory, String.Format("{0}.mvcmap", className));
      if (File.Exists(path))
      {
        ViewActionMap result = ViewActionMap.Load(path);
        return result;
      }
      else if (useDefaltIfNotFound)
        return GetDefaultViewActionMap();
      else
        throw new Exception("The mvcmap file specified was not found");
    }

    /// <summary>
    /// Loads a generic file for all classes in project
    /// </summary>
    private static ViewActionMap GetDefaultViewActionMap()
    {
      var path = Path.Combine(System.Environment.CurrentDirectory, "default.mvcmap");
      if (File.Exists(path))
      {
        ViewActionMap result = ViewActionMap.Load(path);
        return result;
      }
      else
        return new ViewActionMap(true); //fallback
    }

    /// <summary>
    /// Start to implement generic event handling
    /// </summary>
    private static void AddKnownActions(string viewName, KeyValuePair<string, Type> control, StringBuilder hook, StringBuilder action, CompilerFlags flags)
    {
      //test - get a list of all the events for this control
      EventInfo[] eva = control.Value.GetEvents();
      foreach (var ev in eva)
      {
        //generally, the event is XXXXEvent and the args are XXXXArgs..
        //TODO: look at making htis more generic.. can we get at the param info for the delegate?
        if (ev.EventHandlerType == typeof(System.EventHandler))
        {
          AddStandardAction(viewName, control.Key, ev.Name, hook, action, flags);
        }
        else if (ev.EventHandlerType == typeof(System.Windows.Forms.MouseEventHandler))
        {
          AddMouseAction(viewName, control.Key, ev.Name, hook, action, flags);
        }
        else if (ev.EventHandlerType == typeof(System.Windows.Forms.KeyEventHandler))
        {
          AddKeyAction(viewName, control.Key, ev.Name, hook, action, flags);
        }
        else if (ev.EventHandlerType == typeof(System.Windows.Forms.DragEventHandler))
        {
          AddDragAction(viewName, control.Key, ev.Name, hook, action, flags);
        }
        //could add an else clause to create a theoretical event arg from the handler name here...
      }
    }

    /// <summary>
    /// This is going to initially be fairly slow
    /// </summary>
    private static void AddMappedActions(string viewName, KeyValuePair<string, Type> control, StringBuilder hook, StringBuilder action, CompilerFlags flags, ViewActionMap map)
    {
      var added = new List<String>(); //tally, to avoid double adds
      //we first add the generic stuff
      foreach (var ev in map.GlobalMap)
      {
        if (!added.Contains(ev.EventName))
        {
          //does the event exist?
          var evi = control.Value.GetEvent(ev.EventName);

          if (evi == null) continue;
          else
          {
            AddAction(viewName, control.Key, ev.EventName, ev.EventArgsName, hook, action, flags);
            added.Add(ev.EventName);
          }
        }
      }

      //IEnumerable<ViewControlAction> controlMapItems = map.ControlActionMap.F(ev => ev.ControlType == control.Value.Name);
      var controlMapItems = from item in map.ControlActionMap
                            where (item.ControlType == control.Value.Name)
                            select item;

      if (controlMapItems == null) return;
      else
      {
        foreach (var controlMap in controlMapItems)
        {
          if (controlMap != null)
          {
            foreach (var ev in controlMap.ControlActions)
            {
              //this has to be here
              if (!added.Contains(ev.EventName))
              {
                AddAction(viewName, control.Key, ev.EventName, ev.EventArgsName, hook, action, flags);
                added.Add(ev.EventName);
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// EventArgs stub
    /// </summary>
    private static void AddStandardAction(string viewName, string controlName, string actionName, StringBuilder hook, StringBuilder action, CompilerFlags flags)
    {
      AddAction(viewName, controlName, actionName, "EventArgs", hook, action, flags);
    }

    /// <summary>
    /// MouseEventArgs stub.
    /// </summary>
    private static void AddMouseAction(string viewName, string controlName, string actionName, StringBuilder hook, StringBuilder action, CompilerFlags flags)
    {
      AddAction(viewName, controlName, actionName, "MouseEventArgs", hook, action, flags);
    }

    /// <summary>
    /// DragEventHandler stub
    /// </summary>
    private static void AddDragAction(string viewName, string controlName, string actionName, StringBuilder hook, StringBuilder action, CompilerFlags flags)
    {
      AddAction(viewName, controlName, actionName, "DragEventArgs", hook, action, flags);
    }

    /// <summary>
    /// KeyEventHandler stub.
    /// </summary>
    private static void AddKeyAction(string viewName, string controlName, string actionName, StringBuilder hook, StringBuilder action, CompilerFlags flags)
    {
      AddAction(viewName, controlName, actionName, "KeyEventArgs", hook, action, flags);
    }

    /// <summary>
    /// Create generic event hooks
    /// </summary>
    private static void AddAction(string viewName, string controlName, string actionName, string eventArgsType, StringBuilder hook, StringBuilder action, CompilerFlags flags)
    {
      //DEBUG - Console.WriteLine("{0}Controller :: {1} :: {2} - -a : {3} -p : {4} -e : {5} -v : {6}", viewName, controlName, actionName, flags.IsAbstract, flags.UsePartialMethods, flags.PassControllerToEvents, flags.ProtectListViews);

      hook.AppendFormat("\t\t[Action(\"{0}\", \"{1}\")]\r\n\t\tpublic void F{0}_{1}(object sender, {2} e)\r\n", controlName, actionName, eventArgsType);
      hook.AppendLine("\t\t{\r\n\t\t\t//Auto generated call");

      action.AppendFormat(
        "\t\t{2} void {0}{1}({4}{5} e){3}\r\n",
        controlName,
        actionName,
        (flags.UsePartialMethods ? "partial" : "protected virtual"),
        (flags.UsePartialMethods ? ";" : String.Empty),
        (flags.PassControllerToEvents ? String.Format("{0}Controller controller,", viewName) : String.Empty),
        eventArgsType
       );
      //added "protected virtual" so that I can descend and not have to alter this class at all.
      //20120529 - added partial method option for 3.5+

      if (!flags.UsePartialMethods)
      {
        action.AppendLine("\t\t{\r\n");
        action.AppendLine("\t\t}\r\n");
      }//20120529 - added partial method option for 3.5+

      hook.AppendFormat("\t\t\t{0}{1}({2}e);\r\n", controlName, actionName, (flags.PassControllerToEvents ? "this, " : String.Empty));

      hook.AppendLine("\t\t}\r\n");
    }

    /// <summary>
    /// This adds is a standard hook for the ListView and creates a ListViewHelper attached to it.
    /// </summary>
    /// <param name="controlName"></param>
    /// <param name="hook"></param>
    /// <param name="action"></param>
    private static void AddListViewHandler(string controlName, StringBuilder hook, StringBuilder action, CompilerFlags flags)
    {
      hook.AppendFormat("\t\tprotected ListViewHelper<T> Get{0}Helper<T>() where T : class\r\n", controlName);
      hook.AppendLine("\t\t{\r\n\t\t\t//Auto generated call");

      hook.AppendFormat("\t\t\tvar lvh = new ListViewHelper<T>({0});\r\n", controlName);
      hook.AppendLine("\t\t\treturn lvh;");

      hook.AppendLine("\t\t}\r\n");

      //add in the handler for the virtual list item

      hook.AppendFormat("\t\t[Action(\"{0}\", \"RetrieveVirtualItem\")]\r\n\t\tpublic void F{0}_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)\r\n", controlName);
      hook.AppendLine("\t\t{\r\n\t\t\t//Auto generated call");

      action.AppendFormat("\t\tprotected virtual void {0}RetrieveVirtualItem(RetrieveVirtualItemEventArgs e)\r\n", controlName);    //added "protected virtual" so that I can descend and not have to alter this class at all.
      action.AppendLine("\t\t{");

      action.AppendLine("\t\t\t/*we will first try to get an item from the partial method*/");
      action.AppendLine("\t\t\tListViewItem item = null; //set to a known value");

      if (flags.UsePartialMethods)
      {
        action.AppendFormat("\t\t\t{0}GetItem(ref item, e); //try to get the value from partial implementation\r\n", controlName);
      }

      if (flags.ProtectListViews)
      {
        action.AppendLine("\t\t\tif (item == null) //if, null, save ourselves from crashing \r\n\t\t\t{");
        action.AppendLine("\t\t\t/*default placeholder to avoid crashes*/\r\n\t\t\titem = new ListViewItem();\r\n");
        action.AppendLine("\t\t\t/*we need to provide a value for each column*/");
        action.AppendFormat("\t\t\tint count = ({0}.Columns.Count);\r\n", controlName);
        action.AppendLine("\t\t\tif (count > 1)\r\n\t\t\t{");
        action.AppendLine("\t\t\t\titem.Text = \"Temp value\";");
        action.AppendLine("\t\t\t\tfor (int i = 1; i < count; i++)\r\n\t\t\t\t{");
        action.AppendLine("\t\t\t\t\titem.SubItems.Add(\"Temp Subitem\");\r\n\t\t\t\t}\t\r\n\t\t\t}\r\n\t\t}");
      }

      action.AppendLine("\r\n\t\t\te.Item = item;");

      action.AppendLine("\t\t}\r\n");

      hook.AppendFormat("\t\t\t{0}RetrieveVirtualItem(e);\r\n", controlName);

      hook.AppendLine("\t\t}\r\n");

      if (flags.UsePartialMethods)
      {
        action.AppendFormat(
        "\t\tpartial void {0}GetItem(ref ListViewItem item, RetrieveVirtualItemEventArgs e);\r\n",
        controlName
       );
      }
    }
  }
}
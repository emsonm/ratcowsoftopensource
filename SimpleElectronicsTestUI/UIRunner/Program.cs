using RatCow.Sketch;
using RatCow.Sketch.UI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace UIRunner
{
    /// <summary>
    /// This app is a small UI runner. 
    /// 
    /// Run a specific Sketch by passing the path to the built assembly 
    /// and the sketch class name as the params (in that order)
    /// 
    /// As this is configured, make sure the assembly required for the LCDTest is built
    /// otherwise this will fail as it stands!
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {            
            if(args.Length < 2)
            {
                MessageBox.Show("You must supply the assembly and class name of a sketch!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //there should be more validation here...
            var assemblyPath = args[0];
            var className = args[1];

            //load the sketch
            var sketch = LoadSketchFromAssembly(assemblyPath, className);

            RunApp(sketch);
        }

        /// <summary>
        /// A little assembly/class loader.
        /// 
        /// This is not perfect as it relies on the data being "perfect"
        /// </summary>
        static Sketch LoadSketchFromAssembly(string assemblyPath, string className)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);

            if (assembly != null)
            {
                var sketchType = assembly.GetType(className);
                if(sketchType != null)
                {
                    return (Sketch)Activator.CreateInstance(sketchType);
                }
            }

            return null;
        }

        static void RunApp(Sketch sketch)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new SketchUIForm();
            if (mainForm.LoadSketch(sketch)) //this could do with being a little more generic
            {
                Application.Run(mainForm);
            }
            else
            {
                MessageBox.Show("The specified sketch was unable to be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

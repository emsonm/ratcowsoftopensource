using RatCow.Sketch.Tests;
using RatCow.Sketch.UI;
using System;
using System.Windows.Forms;

namespace Testapp
{
    static class Program
    {
        /// <summary>
        /// Testbed app.
        /// 
        /// NB. this is the old testbed app and as such it statically links to the sketches.
        ///     To use dynamic loading, please use instead the UIRunner.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new SketchUIForm();
            mainForm.LoadSketchByType(typeof(LCDTest)); //this could do with being a little more generic

            Application.Run(mainForm);
        }
    }
}

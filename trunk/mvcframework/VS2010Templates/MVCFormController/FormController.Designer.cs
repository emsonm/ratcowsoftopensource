/*Auto generated - this code was generated by the MvcFramework compiler, created by RatCow Soft - 
 See http://code.google.com/p/ratcowsoftopensource/ */ 

using System; 
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

//3rd Party
using RatCow.MvcFramework;

namespace $rootnamespace$
{
	internal partial class $fileinputname$Controller
	{
		public $fileinputname$Controller() : base()
		{
		}

	}


#region GUI glue code

	partial class Form1Controller
	{
		protected void SetData<T>(ListViewHelper<T> helper, List<T> data) where T : class
		{
			//Auto generated call
			Type t = helper.GetType();
			t.InvokeMember("SetData", BindingFlags.Default | BindingFlags.InvokeMethod, null, helper, new object[] { data });
		}

	}
#endregion /*GUI glue code*/

}


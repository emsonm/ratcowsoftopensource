/*Auto generated*/ 

	using System; 
using System.Windows.Forms;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

//3rd Party
using RatCow.MvcFramework;

namespace CFTest2003
{
	internal partial class AbstractMainFormController: BaseController<MainForm>
	{
		public AbstractMainFormController() : base()
		{
		}

		protected virtual void button1Click()
		{

		}

	}


#region GUI glue code

	partial class AbstractMainFormController
	{
		[Outlet("textBox1")]
		public TextBox textBox1 { get; set; }
		[Outlet("button1")]
		public Button button1 { get; set; }
		[Action("button1", "Click")]
		public void Fbutton1_Click(object sender, EventArgs e)
		{
			//Auto generated call
			button1Click();
		}

	}
#endregion /*GUI glue code*/

}


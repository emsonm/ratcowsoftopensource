/*Auto generated*/ 
	
using System; 
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//3rd Party
using RatCow.MvcFramework;

namespace testapp2
{
	internal partial class Form1Controller: BaseController<Form1>
	{
		public Form1Controller() : base()
		{
		}

		void button1Click()
		{
      MessageBox.Show(textBox1.Text);
      if (checkBox1.Checked)
        label1.Text = textBox1.Text;

		}

	}


#region GUI glue code

	partial class Form1Controller
	{
		[Outlet("textBox1")]
		public TextBox textBox1 { get; set; }
		[Outlet("label1")]
		public Label label1 { get; set; }
		[Outlet("button1")]
		public Button button1 { get; set; }
		[Action("button1", "Click")]
		public void Fbutton1_Click(object sender, EventArgs e)
		{
			//Auto generated call
			button1Click();
		}

		[Outlet("checkBox1")]
		public CheckBox checkBox1 { get; set; }
	}
#endregion /*GUI glue code*/

}


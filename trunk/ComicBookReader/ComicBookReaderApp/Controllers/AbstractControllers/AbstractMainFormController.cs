/*Auto generated*/ 

	using System; 
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//3rd Party
using RatCow.MvcFramework;

namespace cbr
{
	internal partial class AbstractMainFormController: BaseController<MainForm>
	{
		public AbstractMainFormController() : base()
		{
		}

		protected virtual void infoButtonClick()
		{

		}

		protected virtual void openButtonClick()
		{

		}

	}


#region GUI glue code

	partial class AbstractMainFormController
	{
		[Outlet("infoButton")]
		public Button infoButton { get; set; }
		[Action("infoButton", "Click")]
		public void FinfoButton_Click(object sender, EventArgs e)
		{
			//Auto generated call
			infoButtonClick();
		}

		[Outlet("openButton")]
		public Button openButton { get; set; }
		[Action("openButton", "Click")]
		public void FopenButton_Click(object sender, EventArgs e)
		{
			//Auto generated call
			openButtonClick();
		}

		[Outlet("pageList")]
		public ListBox pageList { get; set; }
		[Outlet("pageImage")]
		public PictureBox pageImage { get; set; }
	}
#endregion /*GUI glue code*/

}


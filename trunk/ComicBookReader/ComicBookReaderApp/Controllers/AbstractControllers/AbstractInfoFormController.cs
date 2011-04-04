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
	internal partial class AbstractInfoFormController: BaseController<InfoForm>
	{
		public AbstractInfoFormController() : base()
		{
		}

		protected virtual void cancelButtonClick()
		{

		}

		protected virtual void okButtonClick()
		{

		}

	}


#region GUI glue code

	partial class AbstractInfoFormController
	{
		[Outlet("numOfIssuesEdit")]
		public TextBox numOfIssuesEdit { get; set; }
		[Outlet("label4")]
		public Label label4 { get; set; }
		[Outlet("issueNumEdit")]
		public TextBox issueNumEdit { get; set; }
		[Outlet("label3")]
		public Label label3 { get; set; }
		[Outlet("seriesEdit")]
		public TextBox seriesEdit { get; set; }
		[Outlet("label2")]
		public Label label2 { get; set; }
		[Outlet("titleEdit")]
		public TextBox titleEdit { get; set; }
		[Outlet("label1")]
		public Label label1 { get; set; }
		[Outlet("cancelButton")]
		public Button cancelButton { get; set; }
		[Action("cancelButton", "Click")]
		public void FcancelButton_Click(object sender, EventArgs e)
		{
			//Auto generated call
			cancelButtonClick();
		}

		[Outlet("okButton")]
		public Button okButton { get; set; }
		[Action("okButton", "Click")]
		public void FokButton_Click(object sender, EventArgs e)
		{
			//Auto generated call
			okButtonClick();
		}

	}
#endregion /*GUI glue code*/

}


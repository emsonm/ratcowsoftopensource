using System;
using System.Xml;

namespace Ingenium.WF2XAML.Templates
{
	public class WindowsPhonePage : BaseTemplate
	{
		public WindowsPhonePage()
		{
		}

		public override XmlElement RenderToWPF(XmlDocument document)
		{
			XmlElement xmlElement = document.CreateElement("phoneNavigation", "PhoneApplicationPage", "clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone.Controls.Navigation");
			if (xmlElement != null)
			{
				xmlElement.SetAttribute("xmlns", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
				xmlElement.SetAttribute("xmlns:x", "http://schemas.microsoft.com/winfx/2006/xaml");
				xmlElement.SetAttribute("xmlns:phoneNavigation", "clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone.Controls.Navigation");
				xmlElement.SetAttribute("xmlns:mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
				xmlElement.SetAttribute("Title", base.Control.Text);
				int width = base.Control.Width;
				xmlElement.SetAttribute("Width", width.ToString());
				int height = base.Control.Height;
				xmlElement.SetAttribute("Height", height.ToString());
				document.AppendChild(xmlElement);
				XmlElement xmlElement1 = document.CreateElement("Canvas");
				xmlElement1.SetAttribute("Name", "http://schemas.microsoft.com/winfx/2006/xaml", string.Concat("cvs", base.Control.Name));
				xmlElement.AppendChild(xmlElement1);
				base.RenderChilds(xmlElement1);
			}
			return xmlElement;
		}
	}
}
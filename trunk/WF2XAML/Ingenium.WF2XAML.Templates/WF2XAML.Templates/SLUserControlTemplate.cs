using System;
using System.Xml;

namespace Ingenium.WF2XAML.Templates
{
	public class SLUserControlTemplate : BaseTemplate
	{
		public SLUserControlTemplate()
		{
		}

		public override XmlElement RenderToWPF(XmlDocument document)
		{
			XmlElement xmlElement = document.CreateElement("UserControl");
			if (xmlElement != null)
			{
				xmlElement.SetAttribute("xmlns", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
				xmlElement.SetAttribute("xmlns:x", "http://schemas.microsoft.com/winfx/2006/xaml");
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
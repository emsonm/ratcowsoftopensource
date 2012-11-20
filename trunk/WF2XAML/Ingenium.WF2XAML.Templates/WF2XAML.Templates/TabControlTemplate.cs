using System;
using System.Xml;

namespace Ingenium.WF2XAML.Templates
{
	public class TabControlTemplate : BaseTemplate
	{
		public TabControlTemplate()
		{
		}

		public override XmlElement RenderToWPF(XmlDocument document)
		{
			XmlElement xmlElement = document.CreateElement("TabControl");
			xmlElement.SetAttribute("Name", "http://schemas.microsoft.com/winfx/2006/xaml", base.Control.Name);
			int width = base.Control.Width;
			xmlElement.SetAttribute("Width", width.ToString());
			int height = base.Control.Height;
			xmlElement.SetAttribute("Height", height.ToString());
			int top = base.Control.Top;
			xmlElement.SetAttribute("Canvas.Top", top.ToString());
			int left = base.Control.Left;
			xmlElement.SetAttribute("Canvas.Left", left.ToString());
			base.RenderChilds(xmlElement);
			return xmlElement;
		}
	}
}
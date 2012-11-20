using System;
using System.Xml;

namespace Ingenium.WF2XAML.Templates
{
	public class TabItemTemplate : BaseTemplate
	{
		public TabItemTemplate()
		{
		}

		public override XmlElement RenderToWPF(XmlDocument document)
		{
			XmlElement xmlElement = document.CreateElement("TabItem");
			xmlElement.SetAttribute("Name", "http://schemas.microsoft.com/winfx/2006/xaml", base.Control.Name);
			xmlElement.SetAttribute("Header", base.Control.Text);
			XmlElement xmlElement1 = document.CreateElement("Canvas");
			xmlElement1.SetAttribute("Name", "http://schemas.microsoft.com/winfx/2006/xaml", string.Concat("cvs", base.Control.Name));
			xmlElement.AppendChild(xmlElement1);
			base.RenderChilds(xmlElement1);
			return xmlElement;
		}
	}
}
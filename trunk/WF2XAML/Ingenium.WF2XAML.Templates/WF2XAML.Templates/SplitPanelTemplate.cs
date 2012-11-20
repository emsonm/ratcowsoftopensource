using System;
using System.Xml;

namespace Ingenium.WF2XAML.Templates
{
	public class SplitPanelTemplate : BaseTemplate
	{
		public SplitPanelTemplate()
		{
		}

		public override XmlElement RenderToWPF(XmlDocument document)
		{
			XmlElement xmlElement = document.CreateElement("Canvas");
			if (!string.IsNullOrEmpty(base.Control.Name))
			{
				xmlElement.SetAttribute("Name", "http://schemas.microsoft.com/winfx/2006/xaml", base.Control.Name);
			}
			xmlElement.SetAttribute("HorizontalAlignment", "Stretch");
			xmlElement.SetAttribute("VerticalAlignment", "Stretch");
			base.RenderChilds(xmlElement);
			return xmlElement;
		}
	}
}
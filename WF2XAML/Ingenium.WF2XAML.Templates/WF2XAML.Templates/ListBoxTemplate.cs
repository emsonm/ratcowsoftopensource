using System;
using System.Windows.Forms;
using System.Xml;

namespace Ingenium.WF2XAML.Templates
{
	internal class ListBoxTemplate : BaseTemplate
	{
		public ListBoxTemplate()
		{
		}

		public override XmlElement RenderToWPF(XmlDocument document)
		{
			XmlElement xmlElement = document.CreateElement("ListBox");
			xmlElement.SetAttribute("Name", "http://schemas.microsoft.com/winfx/2006/xaml", base.Control.Name);
			int width = base.Control.Width;
			xmlElement.SetAttribute("Width", width.ToString());
			int height = base.Control.Height;
			xmlElement.SetAttribute("Height", height.ToString());
			int top = base.Control.Top;
			xmlElement.SetAttribute("Canvas.Top", top.ToString());
			int left = base.Control.Left;
			xmlElement.SetAttribute("Canvas.Left", left.ToString());
			ListBox control = (ListBox)base.Control;
			foreach (object item in control.Items)
			{
				XmlElement xmlElement1 = document.CreateElement("ListBoxItem");
				xmlElement1.SetAttribute("Content", item.ToString());
				xmlElement.AppendChild(xmlElement1);
			}
			return xmlElement;
		}
	}
}
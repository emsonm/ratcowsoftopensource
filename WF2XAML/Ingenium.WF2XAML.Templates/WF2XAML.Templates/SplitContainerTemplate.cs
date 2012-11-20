using System;
using System.Windows.Forms;
using System.Xml;

namespace Ingenium.WF2XAML.Templates
{
	public class SplitContainerTemplate : BaseTemplate
	{
		public SplitContainerTemplate()
		{
		}

		public override XmlElement RenderToWPF(XmlDocument document)
		{
			SplitContainer control = (SplitContainer)base.Control;
			XmlElement xmlElement = document.CreateElement("Grid");
			int width = base.Control.Width;
			xmlElement.SetAttribute("Width", width.ToString());
			int height = base.Control.Height;
			xmlElement.SetAttribute("Height", height.ToString());
			int top = base.Control.Top;
			xmlElement.SetAttribute("Canvas.Top", top.ToString());
			int left = base.Control.Left;
			xmlElement.SetAttribute("Canvas.Left", left.ToString());
			XmlElement xmlElement1 = document.CreateElement("Grid.RowDefinitions");
			XmlElement xmlElement2 = document.CreateElement("Grid.ColumnDefinitions");
			xmlElement.AppendChild(xmlElement1);
			xmlElement.AppendChild(xmlElement2);
			XmlElement wPF = base.Templates[0].RenderToWPF(document);
			XmlElement wPF1 = base.Templates[1].RenderToWPF(document);
			xmlElement.AppendChild(wPF);
			xmlElement.AppendChild(wPF1);
			if (control.Orientation != Orientation.Vertical)
			{
				XmlElement xmlElement3 = document.CreateElement("ColumnDefinition");
				xmlElement2.AppendChild(xmlElement3);
				XmlElement xmlElement4 = document.CreateElement("RowDefinition");
				xmlElement1.AppendChild(xmlElement4);
				XmlElement xmlElement5 = document.CreateElement("RowDefinition");
				xmlElement5.SetAttribute("Height", "Auto");
				xmlElement1.AppendChild(xmlElement5);
				XmlElement xmlElement6 = document.CreateElement("RowDefinition");
				xmlElement1.AppendChild(xmlElement6);
				XmlElement xmlElement7 = document.CreateElement("GridSplitter");
				xmlElement7.SetAttribute("Grid.Row", "1");
				xmlElement7.SetAttribute("Grid.Column", "0");
				xmlElement7.SetAttribute("Height", "3");
				xmlElement7.SetAttribute("VerticalAlignment", "Center");
				xmlElement7.SetAttribute("HorizontalAlignment", "Stretch");
				xmlElement.AppendChild(xmlElement7);
				wPF.SetAttribute("Grid.Column", "0");
				wPF.SetAttribute("Grid.Row", "0");
				wPF1.SetAttribute("Grid.Column", "0");
				wPF1.SetAttribute("Grid.Row", "2");
			}
			else
			{
				XmlElement xmlElement8 = document.CreateElement("RowDefinition");
				xmlElement1.AppendChild(xmlElement8);
				XmlElement xmlElement9 = document.CreateElement("ColumnDefinition");
				xmlElement2.AppendChild(xmlElement9);
				XmlElement xmlElement10 = document.CreateElement("ColumnDefinition");
				xmlElement10.SetAttribute("Width", "Auto");
				xmlElement2.AppendChild(xmlElement10);
				XmlElement xmlElement11 = document.CreateElement("ColumnDefinition");
				xmlElement2.AppendChild(xmlElement11);
				XmlElement xmlElement12 = document.CreateElement("GridSplitter");
				xmlElement12.SetAttribute("Grid.Row", "0");
				xmlElement12.SetAttribute("Grid.Column", "1");
				xmlElement12.SetAttribute("Width", "3");
				xmlElement12.SetAttribute("VerticalAlignment", "Stretch");
				xmlElement12.SetAttribute("HorizontalAlignment", "Center");
				xmlElement.AppendChild(xmlElement12);
				wPF.SetAttribute("Grid.Row", "0");
				wPF.SetAttribute("Grid.Column", "0");
				wPF1.SetAttribute("Grid.Row", "0");
				wPF1.SetAttribute("Grid.Column", "2");
			}
			xmlElement.SetAttribute("Name", "http://schemas.microsoft.com/winfx/2006/xaml", base.Control.Name);
			return xmlElement;
		}
	}
}
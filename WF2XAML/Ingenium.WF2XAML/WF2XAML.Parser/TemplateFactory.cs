using Ingenium.WF2XAML.Configuration;
using Ingenium.WF2XAML.Templates;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Ingenium.WF2XAML.Parser
{
	public class TemplateFactory
	{
		public TemplateFactory()
		{
		}

		public static BaseTemplate GetTemplate(Control control, WinFormConverter parser, BaseTemplate parent)
		{
			WF2XAMLSection section = (WF2XAMLSection)ConfigurationManager.GetSection("WF2XAMLSection");
			TemplateElement item = section.Templates[control.GetType().FullName];
			if (item == null)
			{
				string fullName = control.GetType().BaseType.FullName;
				if (fullName == "System.Windows.Forms.Form")
				{
					WinFormConverter.RootContainerTypes rootContainerType = parser.RootContainerType;
					switch (rootContainerType)
					{
						case WinFormConverter.RootContainerTypes.WindowContainer:
						{
							fullName = "WPFWindow";
							break;
						}
						case WinFormConverter.RootContainerTypes.PageContainer:
						{
							fullName = "WPFPage";
							break;
						}
						case WinFormConverter.RootContainerTypes.UserControlContainer:
						{
							fullName = "WPFUserControl";
							break;
						}
						case WinFormConverter.RootContainerTypes.SilverlightUserControlContainer:
						{
							fullName = "SLUserControl";
							break;
						}
						case WinFormConverter.RootContainerTypes.WindowsPhoneContainer:
						{
							fullName = "WindowsPhonePage";
							break;
						}
						default:
						{
							fullName = "WPFWindow";
							break;
						}
					}
				}
				item = section.Templates[fullName];
				if (item == null)
				{
					item = section.Templates["System.Windows.Forms.Panel"];
					parser.ParserErrors.Add(new ParserError(0, string.Format("Missing control template: {0}", control.GetType().FullName)));
				}
			}
			if (item == null)
			{
				return null;
			}
			else
			{
				BaseTemplate baseTemplate = (BaseTemplate)Activator.CreateInstance(Type.GetType(item.ClassName));
				baseTemplate.Control = control;
				baseTemplate.Parser = parser;
				baseTemplate.Parent = parent;
				return baseTemplate;
			}
		}
	}
}
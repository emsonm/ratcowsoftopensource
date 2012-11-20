using Ingenium.WF2XAML.Templates;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Ingenium.WF2XAML.Parser
{
	public class WinFormConverter
	{
		private string _sourceFilePath;

		private string _destFilePath;

		private string _result;

		private Dictionary<string, string> _properties;

		private List<ParserError> _parserErrors;

		private WinFormConverter.RootContainerTypes _rootContainerType;

		public string DestFilePath
		{
			get
			{
				return this._destFilePath;
			}
			set
			{
				this._destFilePath = value;
			}
		}

		public List<ParserError> ParserErrors
		{
			get
			{
				return this._parserErrors;
			}
			set
			{
				this._parserErrors = value;
			}
		}

		public Dictionary<string, string> Properties
		{
			get
			{
				return this._properties;
			}
		}

		public string Result
		{
			get
			{
				return this._result;
			}
		}

		public WinFormConverter.RootContainerTypes RootContainerType
		{
			get
			{
				return this._rootContainerType;
			}
			set
			{
				this._rootContainerType = value;
			}
		}

		public string SourceFilePath
		{
			get
			{
				return this._sourceFilePath;
			}
			set
			{
				this._sourceFilePath = value;
			}
		}

		public WinFormConverter()
		{
			this._result = string.Empty;
			this._properties = new Dictionary<string, string>();
			this._parserErrors = new List<ParserError>();
		}

		public BaseTemplate BuildControlTemplateTree(Control instance, BaseTemplate parent)
		{
			BaseTemplate baseTemplate;
			try
			{
				BaseTemplate template = TemplateFactory.GetTemplate(instance, this, parent);
				if (template == null)
				{
					this.ParserErrors.Add(new ParserError(0, string.Format("Template not found for control: {0}", instance.GetType().FullName)));
				}
				else
				{
					foreach (Control control in instance.Controls)
					{
						template.Templates.Add(this.BuildControlTemplateTree(control, template));
					}
				}
				baseTemplate = template;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				this.ParserErrors.Add(new ParserError(0, string.Format("Error while building control template tree: {0}", exception.Message)));
				baseTemplate = null;
			}
			return baseTemplate;
		}

		public bool Convert(Control instance)
		{
			bool flag;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				BaseTemplate baseTemplate = this.BuildControlTemplateTree(instance, null);
				if (baseTemplate == null)
				{
					this._result = string.Empty;
					flag = false;
				}
				else
				{
					baseTemplate.RenderToWPF(xmlDocument);
					this._result = xmlDocument.OuterXml;
					flag = true;
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				this.ParserErrors.Add(new ParserError(0, string.Format("Error converting form: {0}", exception.Message)));
				flag = false;
			}
			return flag;
		}

		public XmlDocument RenderToWPF(Control instance)
		{
			XmlDocument xmlDocument = new XmlDocument();
			BaseTemplate baseTemplate = this.BuildControlTemplateTree(instance, null);
			if (baseTemplate != null)
			{
				baseTemplate.RenderToWPF(xmlDocument);
			}
			return xmlDocument;
		}

		public enum RootContainerTypes
		{
			WindowContainer,
			PageContainer,
			UserControlContainer,
			SilverlightUserControlContainer,
			WindowsPhoneContainer
		}
	}
}
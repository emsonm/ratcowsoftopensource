using System;
using System.Configuration;

namespace Ingenium.WF2XAML.Configuration
{
	public sealed class TemplateElement : ConfigurationElement
	{
		[ConfigurationProperty("ClassName", IsRequired=true)]
		public string ClassName
		{
			get
			{
				return (string)base["ClassName"];
			}
			set
			{
				base["ClassName"] = value;
			}
		}

		[ConfigurationProperty("ControlName", IsKey=true, IsRequired=true)]
		public string ControlName
		{
			get
			{
				return (string)base["ControlName"];
			}
			set
			{
				base["ControlName"] = value;
			}
		}

		public TemplateElement()
		{
		}
	}
}
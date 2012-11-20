using System.Configuration;

namespace Ingenium.WF2XAML.Configuration
{
	public class WF2XAMLSection : ConfigurationSection
	{
		[ConfigurationProperty("Templates", IsDefaultCollection=true)]
		public TemplateCollection Templates
		{
			get
			{
				return (TemplateCollection)base["Templates"];
			}
		}

		public WF2XAMLSection()
		{
		}
	}
}
using System;
using System.Configuration;

namespace Ingenium.WF2XAML.Configuration
{
	public sealed class TemplateCollection : ConfigurationElementCollection
	{
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		protected override string ElementName
		{
			get
			{
				return "Template";
			}
		}

		public TemplateElement this[int index]
		{
			get
			{
				return (TemplateElement)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		public TemplateElement this[string controlName]
		{
			get
			{
				return (TemplateElement)base.BaseGet(controlName);
			}
		}

		public TemplateCollection()
		{
		}

		public bool ContainsKey(string key)
		{
			bool flag = false;
			object[] objArray = base.BaseGetAllKeys();
			object[] objArray1 = objArray;
			int num = 0;
			while (num < (int)objArray1.Length)
			{
				object obj = objArray1[num];
				if ((string)obj != key)
				{
					num++;
				}
				else
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new TemplateElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((TemplateElement)element).ControlName;
		}
	}
}
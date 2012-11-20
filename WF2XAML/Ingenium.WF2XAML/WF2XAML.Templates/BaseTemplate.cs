using Ingenium.WF2XAML.Parser;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Ingenium.WF2XAML.Templates
{
	public class BaseTemplate
	{
		private List<BaseTemplate> _templates;

		private BaseTemplate _parent;

		private Control _control;

		private WinFormConverter _parser;

		public Control Control
		{
			get
			{
				return this._control;
			}
			set
			{
				this._control = value;
			}
		}

		public BaseTemplate Parent
		{
			get
			{
				return this._parent;
			}
			set
			{
				this._parent = value;
			}
		}

		public WinFormConverter Parser
		{
			get
			{
				return this._parser;
			}
			set
			{
				this._parser = value;
			}
		}

		public List<BaseTemplate> Templates
		{
			get
			{
				return this._templates;
			}
		}

		public BaseTemplate()
		{
			this._templates = new List<BaseTemplate>();
		}

		protected void RenderChilds(XmlElement container)
		{
			foreach (BaseTemplate template in this.Templates)
			{
				container.AppendChild(template.RenderToWPF(container.OwnerDocument));
			}
		}

		public virtual XmlElement RenderToWPF(XmlDocument document)
		{
			return null;
		}
	}
}
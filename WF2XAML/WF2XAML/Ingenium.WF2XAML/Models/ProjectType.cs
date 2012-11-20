using System;

namespace Ingenium.WF2XAML.Models
{
	public class ProjectType
	{
		public int Code
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public ProjectType(int code, string description)
		{
			this.Code = code;
			this.Description = description;
		}
	}
}
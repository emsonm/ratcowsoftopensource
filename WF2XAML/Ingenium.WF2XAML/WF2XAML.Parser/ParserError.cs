using System;

namespace Ingenium.WF2XAML.Parser
{
	public class ParserError
	{
		public string ErrorText
		{
			get;
			set;
		}

		public int ErrorType
		{
			get;
			set;
		}

		public ParserError(int errorType, string errorText)
		{
			this.ErrorType = errorType;
			this.ErrorText = errorText;
		}
	}
}
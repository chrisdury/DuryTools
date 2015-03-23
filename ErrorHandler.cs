using System;

namespace DuryTools
{
	public class ErrorHandler : Exception
	{

		public ErrorHandler(string message)
		{
			//System.Web.HttpContext.Current.Response.Redirect("error.aspx?m=" + message);
			throw new Exception(message);
		}

		public ErrorHandler(string message, Exception innerException) : base(message, innerException)
		{
			throw new Exception(message,innerException);
		}
	}
}

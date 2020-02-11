
using System;
using System.Net;

namespace Strogg.Core.Web
{
    public enum WebRequestMethod
	{
		GET     = 0,
		POST    = 1
	}

	public static class HeaderKeys
	{
		public static readonly string USER_AGENT        = "user-agent";
		public static readonly string ACCEPT            = "accept";
		public static readonly string ACCEPT_ENCODING   = "accept-encoding";
	}


	public interface IWebInfo : ICloneable
	{
		long ContentLength          { get; set; }

		string ContentType          { get; set; }

		WebRequestMethod Method     { get; set; }

		WebHeaderCollection Headers { get; set; }
		int Timeout                 { get; set; }
	}
}
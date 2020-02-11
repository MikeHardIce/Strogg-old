
using System.Net;

namespace Strogg.Core.Web
{
    public class WebInfo : IWebInfo
	{
		public long ContentLength               { get; set; }

		public string ContentType               { get; set; }

		public WebRequestMethod Method          { get; set; }

		public WebHeaderCollection Headers      { get; set; }

		public int Timeout                      { get; set; }

		public WebInfo ()
		{
			this.Headers = new WebHeaderCollection();

			// Default values

			this.ContentLength      = 0;

			this.ContentType        = null;

			this.Timeout            = 9000;

			this.Headers.Add(HeaderKeys.USER_AGENT,         "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:42.0) Gecko/20100101 Firefox/42.0");
			this.Headers.Add(HeaderKeys.ACCEPT,             "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
			//this.Headers.Add(HeaderKeys.ACCEPT_ENCODING,    "gzip, deflate");

			this.Method             = WebRequestMethod.GET;
		}

		public object Clone ( )
		{
			IWebInfo webInfo = new WebInfo();

			webInfo.ContentLength           = this.ContentLength;
			webInfo.ContentType             = this.ContentType;
			webInfo.Timeout                 = this.Timeout;

			webInfo.Method                  = this.Method;

			foreach(string key in this.Headers.AllKeys)
			{
				webInfo.Headers.Add(key, this.Headers[key]);
			}

			return webInfo;
		}
	}
}
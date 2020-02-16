
using System;
using System.Linq;
using System.Net;
using Strogg.Network.Proxies;

namespace Strogg.Network.Web
{
    public class SimpleWebRequest : IWebRequest
	{
		private HttpWebRequest  webRequest;

        private IProxy proxy;


		public string Url
            => this.webRequest.RequestUri.AbsoluteUri;

		public IProxy Proxy 
            => this.proxy;

		public SimpleWebRequest (string url, IProxy proxy)
		{
			if(url != null && url.Length > 0)
			{
				this.webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
			}

			this.webRequest.AllowAutoRedirect               = true;
			this.webRequest.AuthenticationLevel             = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
			this.webRequest.MaximumAutomaticRedirections    = 10;
			
			this.webRequest.Proxy   = proxy.WebProxy;
			this.proxy              = proxy;
		}

		public IWebResponse Execute ( )
		{
			HttpWebResponse webResponse;
			SimpleWebResponse simpleWebResponse;

			try
			{
				webResponse = (HttpWebResponse) this.webRequest.GetResponse();
			}
			catch (WebException ex)
			{
				if(ex.Response != null)
				{
					webResponse = (HttpWebResponse) ex.Response;

				}
				else
				{
					throw new Exception("Proxy[" + this.proxy.IpAddress + ":" + this.proxy.Port + "] timed out. Try to redirect the request.");
				}
			}

			simpleWebResponse = new SimpleWebResponse(webResponse, this.proxy.IpAddress, Convert.ToInt32(this.proxy.Port));

			if(simpleWebResponse.Content.Contains("Redirecting") || simpleWebResponse.Content.Contains("redirecting"))
			{
				throw new Exception("Proxy[" + this.proxy.IpAddress + ":" + this.proxy.Port + "] tries to redirect the request.");
			}

			return simpleWebResponse;
		}

		public void SetRequestInformation (IWebInfo webInfo)
		{
			this.webRequest.ContentLength   = webInfo.ContentLength;
			this.webRequest.ContentType     = webInfo.ContentType;

			this.webRequest.Method          = webInfo.Method.ToString();

			this.webRequest.Accept          = CopyHeader(HeaderKeys.ACCEPT, webInfo, this.webRequest.Accept);
			this.webRequest.UserAgent       = CopyHeader(HeaderKeys.USER_AGENT, webInfo, this.webRequest.UserAgent);

			this.webRequest.Timeout             = webInfo.Timeout;
			this.webRequest.ReadWriteTimeout    = webInfo.Timeout;
		}

		public string CopyHeader (string key, IWebInfo webInfo, string defaultValue)
			=> webInfo.Headers.AllKeys.Any(m => m.Equals(key, StringComparison.InvariantCultureIgnoreCase)) ? 
							webInfo.Headers[key] : defaultValue;
	}
}
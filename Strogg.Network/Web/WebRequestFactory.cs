
using Strogg.Network.Proxies;

namespace Strogg.Network.Web
{
    public class WebRequestFactory : IWebRequestFactory
	{
		private readonly IProxyManager  proxyManager;

		public WebRequestFactory (IProxyManager proxyManager)
            => this.proxyManager   = proxyManager;
		
		public IWebRequest CreateWebRequest (string url, IWebInfo webInfo)
		{
			var webRequest = new SimpleWebRequest(url, this.proxyManager.GetNext());

			webRequest.SetRequestInformation(webInfo);
			// use data from _webInfo

			return webRequest;
		}
	}
}
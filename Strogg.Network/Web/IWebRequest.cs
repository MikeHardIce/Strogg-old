
using Strogg.Network.Proxies;

namespace Strogg.Network.Web
{
    public interface IWebRequest
	{
		string Url    { get; }

		IProxy Proxy  { get; }
		IWebResponse Execute ();

		void SetRequestInformation (IWebInfo WebInfo);
	}
}
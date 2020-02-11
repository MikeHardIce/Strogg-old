
using Strogg.Core.Proxies;

namespace Strogg.Core.Web
{
    public interface IWebRequest
	{
		string Url    { get; }

		IProxy Proxy  { get; }
		IWebResponse Execute ();

		void SetRequestInformation (IWebInfo WebInfo);
	}
}
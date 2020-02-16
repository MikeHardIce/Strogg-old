
using Strogg.Network.Web;

namespace Strogg.Network.Proxies
{
    public interface IProxyManager
	{
		IProxy GetNext();

		void Punish (IWebResponse webResponse);

		void Commend (string ipAddress, int port);
	}
}

using Strogg.Core.Web;

namespace Strogg.Core.Proxies
{
    public interface IProxyManager
	{
		IProxy GetNext();

		void Punish (IWebResponse webResponse);

		void Commend (string ipAddress, int port);
	}
}
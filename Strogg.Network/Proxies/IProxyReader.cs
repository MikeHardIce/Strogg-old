
using System.Collections.Generic;

namespace Strogg.Network.Proxies
{
    public interface IProxyReader
	{
		void AddToProxyList (IList<IProxy> list);
	}
}
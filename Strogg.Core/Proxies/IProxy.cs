
using System.Net;

namespace Strogg.Core.Proxies
{
	public interface IProxy
	{
		ProxyRanking Ranking    { get; }
		uint Timeouts            { get; }
		IWebProxy WebProxy      { get; }

		int Port                { get; }

		string IpAddress        { get; }

		void Punish(uint amount = 1);

		void Commend(uint amount = 1);
	}
}
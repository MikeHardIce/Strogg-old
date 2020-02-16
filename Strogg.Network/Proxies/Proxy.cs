
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Strogg.Network.Proxies
{
    public class Proxy : IProxy
    {
        private object lock1 = new object();

        private static List<(int threshold, ProxyRanking ranking)> Rankings = new List<(int threshold, ProxyRanking ranking)>
        {
            (50, ProxyRanking.Bad)
            , (20, ProxyRanking.Average)
            , (5, ProxyRanking.Good)
            , (0, ProxyRanking.VeryGood)
        };

        private ProxyRanking      ranking;
        private uint               timeouts;
        private IWebProxy         webProxy;
        private string            ipAddress;
        private int               port;
        
        public uint Timeouts
            => this.timeouts;

        public IWebProxy WebProxy
            => this.webProxy;

        public int Port
            => this.port;

        public string IpAddress
            => this.ipAddress;

        public ProxyRanking Ranking
            => Rankings.FirstOrDefault(rnk => this.timeouts >= rnk.threshold).ranking;

		public Proxy (IWebProxy webProxy)
        {
            this.webProxy   = webProxy;

            this.timeouts 	 = 0;

            this.ipAddress  = ((WebProxy) webProxy).Address.Host;
            this.port       = ((WebProxy) webProxy).Address.Port;

            this.ranking    = ProxyRanking.None;
        }

        public void Punish ( uint amount = 1 )
        {
            lock(lock1)
            {
                this.timeouts += amount;
            } 
        }

        public void Commend ( uint amount = 1 )
        {
            lock(this.lock1)
            {
                if(this.timeouts > amount)
                {
                    this.timeouts -= amount;
                }
            }
        }
    }
}
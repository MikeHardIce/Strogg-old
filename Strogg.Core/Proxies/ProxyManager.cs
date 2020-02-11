
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Logging;
using Strogg.Core.Web;

namespace Strogg.Core.Proxies
{
    public class ProxyManager : IProxyManager
    {
        private readonly ILogger    logger;
        private object              lock1           = new object();
        private object              lock2           = new object();

        private IList<IProxy>       proxies;
        private bool                firstTime;
        private int                 lookups;
        
        public ProxyManager (ILogger logger)
            => this.logger      = logger;
        

        public ProxyManager (ILogger logger, IProxyReader reader)
        {
            this.logger         = logger;

            this.firstTime      = true;
            this.proxies        = new List<IProxy>();

            this.logger.LogInformation( "Initializing proxies ...");

            reader.AddToProxyList(this.proxies);

            this.logger.LogInformation(GetProxiesAsString(this.proxies));

            this.logger.LogInformation( $"Loaded {this.proxies.Count} proxies.");           
        }

        public IProxy GetNext()
        {
            lock(this.lock1)
            {
                ProxyRanking useRank             = UseRanking();

                IProxy proxy                     = GetRandomProxyWithRanking(useRank);

                IncreaseLookUpCounter();

                return proxy;
            }
            
        }

        private IProxy GetRandomProxyWithRanking (ProxyRanking ranking)
        {
            var proxyList         = this.proxies.Where(m => m.Ranking == ranking).ToList();
            var rand              = new Random();
            int count             = proxyList.Count;

            if(count > 0)
            {
                return proxyList.ElementAt(rand.Next(count));
            }

            return proxies.ElementAt(rand.Next(this.proxies.Count));

        }
        
        private ProxyRanking UseRanking ()
        {
            if(this.proxies.Count(m => m.Ranking == ProxyRanking.None) > 0)
            {
                return ProxyRanking.None;
            }
            else if(this.lookups % 53 == 0 && this.proxies.Count(m => m.Ranking == ProxyRanking.Bad) > 0 )
            {
                return ProxyRanking.Bad;
            }
            else if(this.lookups % 19 == 0 && this.proxies.Count(m => m.Ranking == ProxyRanking.Average) > 0)
            {
                return ProxyRanking.Average;
            }
            else if (this.lookups % 3 == 0 && this.proxies.Count(m => m.Ranking == ProxyRanking.Good) > 0)
            {
                return ProxyRanking.Good;
            }
            else if (this.proxies.Count(m => m.Ranking == ProxyRanking.VeryGood) > 0)
            {
                return ProxyRanking.VeryGood;
            }

            return ProxyRanking.None;
        }

        private void IncreaseLookUpCounter ()
            => this.lookups = this.lookups switch {
                int b when b >= 55 => 0,
                _ => this.lookups + 1
            }; 

        public void Commend ( string ipAddress, int port )
        {
            lock(this.lock2)
            {
                if(this.proxies.Any(m => m.IpAddress == ipAddress && m.Port == port))
                {
                    Commend(this.proxies, ipAddress, port);
                }
            }
            
        }

        private void Punish (IList<IProxy> proxies, string ipAddress, int port )
        {
            
            IProxy proxy = proxies.Where(m => m.IpAddress == ipAddress && m.Port == port).FirstOrDefault();

            if(proxy != null)
            {
                 proxy.Punish();
            }
        }

        public void Punish ( IWebResponse webResponse )
        {
            lock(this.lock1)
            {
                IProxy proxy = this.proxies.Where(m => m.IpAddress == webResponse.Proxy.IpAddress && m.Port == webResponse.Proxy.Port).FirstOrDefault();

                if(proxy != null)
                {
                    // punish regarding to the status code
                    if(webResponse.HttpStatusCode == HttpStatusCode.Forbidden
                        // Bad Request can also emerge because the proxy cannot handle the url formatting
                        || webResponse.HttpStatusCode == HttpStatusCode.BadRequest
                        || webResponse.HttpStatusCode == HttpStatusCode.BadGateway
                        || webResponse.HttpStatusCode == HttpStatusCode.RequestTimeout)
                    {
                        proxy.Punish(10);
                    }
                    else
                    {
                        proxy.Punish();
                    }
                     
                }
            }
            
        }

        private void Commend (IList<IProxy> proxies, string ipAddress, int port )
        {
            
            IProxy proxy = proxies.Where(m => m.IpAddress == ipAddress && m.Port == port).FirstOrDefault();

            if(proxy != null)
            {
                 proxy.Commend();
            }
        }

        public void AddProxy (string ipAddress, int port)
        {
            var wproxy    = new WebProxy(ipAddress, port);
            var proxy     = new Proxy(wproxy);

            this.proxies.Add(proxy);

            this.logger.LogInformation($"{proxy.IpAddress}: {proxy.Port} added");
                
        }

        public string GetProxiesAsString ()
            => GetProxiesAsString(this.proxies);

        public static string GetProxiesAsString (IList<IProxy> proxies)
            => string.Join("\n", proxies.Select(proxy => $"{proxy.Timeouts}  {proxy.IpAddress} {proxy.Port}"));
        
    }
}
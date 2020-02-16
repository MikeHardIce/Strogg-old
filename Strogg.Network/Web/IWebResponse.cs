
using System.IO;
using System.Net;
using Strogg.Network.Proxies;

namespace Strogg.Network.Web
{
    public interface IWebResponse
	{
		string IpAddress                { get; }

		int Port 						{ get; }
		Stream ResponseStream           { get; }
		HttpStatusCode HttpStatusCode   { get; } 
		string ContentType              { get; }
		string Content                  { get; }

		string Url                      { get; }

		bool IsValid 					{ get; }
	}
}
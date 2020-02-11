
using System.IO;
using System.Net;
using Strogg.Core.Proxies;

namespace Strogg.Core.Web
{
    public interface IWebResponse
	{
		IProxy Proxy                    { get; }
		Stream ResponseStream           { get; }
		HttpStatusCode HttpStatusCode   { get; } 
		string ContentType              { get; }
		string Content                  { get; }

		string Url                      { get; }

		bool IsValid 					{ get; }
	}
}
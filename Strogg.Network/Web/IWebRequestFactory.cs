
namespace Strogg.Network.Web
{
    public interface IWebRequestFactory
	{
		IWebRequest CreateWebRequest (string url, IWebInfo webInfo);
	}
}
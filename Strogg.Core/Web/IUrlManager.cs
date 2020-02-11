
namespace Strogg.Core.Web
{
    public interface IUrlManager
	{
		bool CanLoad (string url);

		int Transform (string url);

		void Add (string url);

		void Remove (string url);

		void ClearAll();
	}
}
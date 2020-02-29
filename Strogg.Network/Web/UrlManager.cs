
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Strogg.Network.Web
{
    public class UrlManager : IUrlManager
    {
        private object lock1 = new object();

		private HashSet<int> hashedUrls;

		public UrlManager ()
            => this.hashedUrls = new HashSet<int>();

		public void Add ( string url )
		{
			lock(this.lock1)
			{
				this.hashedUrls.Add(Transform(url));
			}

		}

		public void Remove (string url)
		{
			lock(this.lock1)
			{
				this.hashedUrls.Remove(Transform(url));
			}
		}
		public bool CanLoad ( string url )
		{
			lock(this.lock1)
			{
				int hashedUrl = Transform(url);
				return !this.hashedUrls.Any(m => m == hashedUrl);
			}
		}

		public void ClearAll ( )
		{
			lock(this.lock1)
			{
				this.hashedUrls.Clear();
			}

		}

		public string GetAllAsString ()
            =>	string.Join("\n", this.hashedUrls.OrderBy(m => m));

		public int Transform ( string url )
		{
			string transformedUrl   = url;
			int hashedUrl       	= 0;

			string numbers  		= Regex.Replace(transformedUrl, "[^\\d]", "");

			transformedUrl         	= Regex.Replace(transformedUrl, "[\\d#\\.\\,]", "");             

			transformedUrl         	= transformedUrl.Replace(" ", "");

			transformedUrl         	= string.Concat(transformedUrl.OrderBy(m => m));

			transformedUrl         	= transformedUrl + numbers;

			hashedUrl       		= transformedUrl.GetHashCode();

			return hashedUrl;
		}
    }
}
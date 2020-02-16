
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Strogg.Network.Proxies;

namespace Strogg.Network.Web
{
    public class SimpleWebResponse : IWebResponse
	{
		private HttpWebResponse     webResponse;
		private string              content;
		private MemoryStream        stream;
		private string              ipAddress;
		private int                 port;

		public string IpAddress
            => this.ipAddress;

		public int Port
            => this.port;        
		public HttpStatusCode HttpStatusCode
            => this.webResponse.StatusCode;

		public bool IsValid 
            => HttpStatusCode == HttpStatusCode.Accepted || HttpStatusCode == HttpStatusCode.OK;

		public string Url
            => this.webResponse.ResponseUri.AbsoluteUri;

		public string ContentType
            => this.webResponse.ContentType;

		public string Content
            => this.content;

		public Stream ResponseStream
		{
			get
			{
				using Stream newStream = new MemoryStream();

				this.stream.Seek(0, SeekOrigin.Begin);

				this.stream.CopyTo(newStream);

				newStream.Seek(0,SeekOrigin.Begin);

				return newStream;
			}
		}

        public SimpleWebResponse (HttpWebResponse webResponse, string ipAddress = "", int port = 0)
        {
            this.webResponse    = webResponse;
			this.stream         = new MemoryStream();

			this.ipAddress      = ipAddress;
			this.port           = port;

			this.content        = "";

			ReadContent();
        }

		private void ReadContent ()
		{
			if(this.webResponse != null)
			{
				Encoding encoding           = this.webResponse.CharacterSet == "" ? Encoding.UTF8 : Encoding.GetEncoding(this.webResponse.CharacterSet);

				// its a bit slow to copy the stream

				if(this.webResponse.GetResponseStream().CanRead)
				{
					this.webResponse.GetResponseStream().CopyTo(this.stream);

					this.stream.Seek(0, SeekOrigin.Begin);

					StringBuilder builder      = new StringBuilder();

					using StreamReader reader  = new StreamReader(this.stream, encoding);

					builder.Append(reader.ReadToEnd());

					builder                 = builder.Replace("\t","").Replace("\r","").Replace("\n","");

					string responseString   = builder.ToString();

					this.content            = Regex.Replace(responseString, "<!--.*?-->","");

					this.content            = Regex.Replace(this.content, "<script.*?</script>", "");

					this.content            = Regex.Replace(this.content, "<link.*?/>", "");
				}
			}
		}
	}

	public class DummyResponse : WebResponse
	{

	}
}
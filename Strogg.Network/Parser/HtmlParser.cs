
using System.Collections.Generic;
using System.Linq;
using CsQuery;

namespace Strogg.Core.Parser
{
    public class HtmlParser
	{ 
		private readonly CQ cqParser;

		public HtmlParser (string html)
            => this.cqParser = CQ.Create(html);

		public bool TryGetChildOf (IDomObject parent, string path, string parameterName, string parameterValue, out IDomObject element)
			=> TryGetElement(parent.Cq(), path, parameterName, parameterValue,out element);


		public bool TryGetElement (string path, string parameterName, string parameterValue, out IDomObject element)
            => TryGetElement(this.cqParser, path, parameterName, parameterValue, out element);
		
		public static bool TryGetElement (CQ parser, string path, string parameterName, string parameterValue, out IDomObject element)
		{
			element = parser.FirstElement();

			var elements = GetCollection(parser, path, parameterName, parameterValue);

            if(elements.Count > 0)
            {
                element = elements.FirstOrDefault();
                return true;
            }
                
            return false;            
		}

		public ICollection<IDomObject> GetCollection (string path, string parameterName, string parameterValue)
            => GetCollection(this.cqParser, path, parameterName, parameterValue);
		
		public static ICollection<IDomObject> GetCollection (CQ parser, string path, string parameterName, string parameterValue)
		{
			var list  		                = new List<IDomObject>();

			string cleanedPath             	= CleanUpString(path);
			string cleanedParameterName    	= CleanUpString(parameterName);
			string cleanedParameterValue   	= CleanUpString(parameterValue);

			CQ elements             		= parser.Find(cleanedPath);

			foreach(IDomObject element in elements)
			{
				if(cleanedParameterName.Length < 1 
                    || (element.HasAttribute(cleanedParameterName) && element.GetAttribute(cleanedParameterName).Contains(cleanedParameterValue)))
				{
					list.Add(element);
				}
			}

			return list;
		}

		private static string CleanUpString (string value)
            => string.IsNullOrWhiteSpace(value) ? "" : value.ToLower();
	}
}
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Finaltec.Hart.Analyzer.Core
{
    public class VersionService
    {
        public Version GetCurrentVersion()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://hartprotocollite.codeplex.com/releases/");

                WebResponse webResponse = request.GetResponse();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                string html = sr.ReadToEnd();

                Regex regex = new Regex("<h1 class=\"page_title\">HART Analyzer (\\d.\\d)</h1>", RegexOptions.Singleline);

                Match match = regex.Match(html);
                if(match.Success)
                    return new Version(match.Groups[1].Value);
            }
            catch
            {
                return new Version();
            }

            return new Version();
        }
    }
}
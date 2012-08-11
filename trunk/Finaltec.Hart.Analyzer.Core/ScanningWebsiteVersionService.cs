using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using MEFedMVVM.ViewModelLocator;

namespace Finaltec.Hart.Analyzer.Core
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [ExportService(ServiceType.Runtime, typeof(IVersionService))]
    public class ScanningWebsiteVersionService : IVersionService
    {
        private Version _cachedVersion;
        public string DefaultWebsite { get; set; }

        public ScanningWebsiteVersionService()
        {
            DefaultWebsite = "http://hartanalyzer.codeplex.com/releases/";
        }

        public Version GetCurrentVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        public Version GetOnlineVersion()
        {
            if (_cachedVersion != null)
                return _cachedVersion;

            Version version = new Version();
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(DefaultWebsite);

                WebResponse webResponse = request.GetResponse();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                string html = sr.ReadToEnd();

                Regex regex = new Regex("<h1 class=\"page_title\">HART Analyzer (\\d.\\d)</h1>", RegexOptions.Singleline);

                Match match = regex.Match(html);
                if(match.Success)
                    version = new Version(match.Groups[1].Value);
            }
            finally
            {
                _cachedVersion = version;
            }

            return _cachedVersion;
        }

        public void GetOnlineVersionAsync()
        {
            Action action = () => GetOnlineVersion();
            action.BeginInvoke(GetOnlineVersionCallback, null);
        }

        private void GetOnlineVersionCallback(IAsyncResult ar)
        {
            if(GetOnlineVersionResult != null)
                GetOnlineVersionResult.Invoke(this, _cachedVersion);
        }

        public event Action<object, Version> GetOnlineVersionResult;
    }
}
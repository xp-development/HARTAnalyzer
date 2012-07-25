using System;
using System.Reflection;
using Cinch;
using MEFedMVVM.ViewModelLocator;

namespace Finaltec.Hart.Analyzer.ViewModel
{
    [ExportViewModel("AboutViewModel")]
    public class AboutViewModel : ViewModelBase
    {
        private string _productVersion;

        public string ProductVersion
        {
            get { return _productVersion; }
            set
            { 
                _productVersion = string.Format("Version: {0}", value);
                NotifyPropertyChanged("ProductVersion");
            }
        }

        public AboutViewModel()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            ProductVersion = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }
    }
}
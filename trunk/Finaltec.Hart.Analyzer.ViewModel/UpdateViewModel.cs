using System;
using System.ComponentModel.Composition;
using Cinch;
using Finaltec.Hart.Analyzer.Core;
using MEFedMVVM.ViewModelLocator;

namespace Finaltec.Hart.Analyzer.ViewModel
{
    [ExportViewModel("UpdateViewModel")]
    public class UpdateViewModel : ViewModelBase
    {
        private string _currentVersion;
        private string _onlineVersion;
        private Version _currentVersionFromService;

        public string CurrentVersion
        {
            get { return _currentVersion; }
            set
            {
                _currentVersion = value;
                NotifyPropertyChanged("CurrentVersion");
            }
        }

        public string OnlineVersion
        {
            get { return _onlineVersion; }
            set
            {
                _onlineVersion = value;
                NotifyPropertyChanged("OnlineVersion");
            }
        }

        public SimpleCommand<object, object> CloseCommand { get; private set; }

        [ImportingConstructor]
        public UpdateViewModel(IVersionService versionService)
        {
            CloseCommand = new SimpleCommand<object, object>(obj => RaiseCloseRequest(true));

            _currentVersionFromService = versionService.GetCurrentVersion();

            versionService.GetOnlineVersionResult += (o, version) =>
                                                         {
                                                             if (version == null || version == new Version())
                                                                 OnlineVersion = "Cannot check for updates.";
                                                             else
                                                             {
                                                                 if (version != _currentVersionFromService)
                                                                     OnlineVersion = version.ToString();
                                                                 else
                                                                     OnlineVersion = "The newest version is already installed.";
                                                             }
                                                         };

            OnlineVersion = "Loading..";
            versionService.GetOnlineVersionAsync();
            CurrentVersion = _currentVersionFromService.ToString();
        }
    }
}
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Navigation;
using Cinch;

namespace Finaltec.Hart.Analyzer.View
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("AboutViewModel", typeof(AboutDialog))]
    public partial class AboutDialog
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            string navigateUri = ((Hyperlink) sender).NavigateUri.ToString();
            Process.Start(new ProcessStartInfo(navigateUri));
            e.Handled = true;
        }
    }
}

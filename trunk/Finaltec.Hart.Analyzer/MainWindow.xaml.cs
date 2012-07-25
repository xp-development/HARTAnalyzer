using System.Collections.Specialized;
using Finaltec.Hart.Analyzer.ViewModel;
using Finaltec.Hart.Analyzer.ViewModel.DataModels;

namespace Finaltec.Hart.Analyzer.View
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Width = ViewModel.Properties.Settings.Default.Width;
            Height = ViewModel.Properties.Settings.Default.Height;
            Top = ViewModel.Properties.Settings.Default.WindowTop;
            Left = ViewModel.Properties.Settings.Default.WindowLeft;
            WindowState = ViewModel.Properties.Settings.Default.WindowState;
        }

        /// <summary>
        /// Window is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void WindowLoaded(object sender, System.EventArgs e)
        {
            DataTransferModel.GetInstance().Output.CollectionChanged += LogUpdated;
        }

        /// <summary>
        /// Window is unloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void WindowUnloaded(object sender, System.EventArgs e)
        {
            DataTransferModel.GetInstance().Output.CollectionChanged -= LogUpdated;
        }

        /// <summary>
        /// Log was updated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void LogUpdated(object sender, NotifyCollectionChangedEventArgs e)
        {
            lvOutput.ScrollIntoView(lvOutput.Items[lvOutput.Items.Count -1]);
        }

        /// <summary>
        /// ExtendedTextBox selected text changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="newValue">The new value.</param>
        private void ExtendedTextBoxSelectedTextChanged(object sender, string newValue)
        {
            ((MainViewModel) DataContext).SelectedOutput = newValue;
        }

        private void Window_StateChanged(object sender, System.EventArgs e)
        {
            ViewModel.Properties.Settings.Default.WindowState = WindowState;

            ViewModel.Properties.Settings.Default.Save();
        }

        private void Window_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            ViewModel.Properties.Settings.Default.Width = e.NewSize.Width;
            ViewModel.Properties.Settings.Default.Height = e.NewSize.Height;

            ViewModel.Properties.Settings.Default.Save();
        }

        private void Window_LocationChanged(object sender, System.EventArgs e)
        {
            ViewModel.Properties.Settings.Default.WindowTop = Top;
            ViewModel.Properties.Settings.Default.WindowLeft = Left;

            ViewModel.Properties.Settings.Default.Save();
        }
    }
}

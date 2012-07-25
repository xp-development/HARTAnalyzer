using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;
using Cinch;
using Finaltec.Hart.Analyzer.ViewModel.DataModels;
using Finaltec.Hart.Analyzer.ViewModel.Properties;
using MEFedMVVM.ViewModelLocator;

namespace Finaltec.Hart.Analyzer.ViewModel
{
    /// <summary>
    /// SettingsDialogModel class.
    /// Implements base class ViewModelBase.
    /// </summary>
    [ExportViewModel("SettingsViewModel")]
    public class SettingsViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the COM ports.
        /// </summary>
        /// <value>The COM ports.</value>
        public List<string> ComPorts { get; set; }
        /// <summary>
        /// Gets or sets the preamble counts.
        /// </summary>
        /// <value>The preamble counts.</value>
        public List<string> PreambleCounts { get; set; }

        /// <summary>
        /// Gets or sets the settings data model.
        /// </summary>
        /// <value>The settings data model.</value>
        public SettingsDataModel SettingsDataModel { get; set; }

        /// <summary>
        /// Gets or sets the confirm command.
        /// </summary>
        /// <value>The confirm command.</value>
        public SimpleCommand<object, object> ConfirmCommand { get; private set; }
        /// <summary>
        /// Gets or sets the cancel command.
        /// </summary>
        /// <value>The cancel command.</value>
        public SimpleCommand<object, object> CancelCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel()
        {
            SettingsDataModel = new SettingsDataModel();

            InitComPorts();
            InitPreambles();
            InitCommands();
        }

        /// <summary>
        /// Inits the commands.
        /// </summary>
        private void InitCommands()
        {
            ConfirmCommand = new SimpleCommand<object, object>(CanExecuteConfirmCommand, ConfirmCommandExecute);
            CancelCommand = new SimpleCommand<object, object>(CancelCommandExecute);
        }

        /// <summary>
        /// Inits the preambles.
        /// </summary>
        private void InitPreambles()
        {
            PreambleCounts = new List<string>();
            for (int i = 5; i < 20; i++)
            {
                PreambleCounts.Add(i.ToString());
            }
        }

        /// <summary>
        /// Inits the COM ports.
        /// </summary>
        private void InitComPorts()
        {
            ComPorts = new List<string>();
            foreach (string portName in SerialPort.GetPortNames())
            {
                ComPorts.Add(portName);
            }
        }

        /// <summary>
        /// Cancel command execute.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private void CancelCommandExecute(object obj)
        {
            if (Application.Current.MainWindow == null)
                Application.Current.Shutdown(0);

            CloseActivePopUpCommand.Execute(false);
        }

        /// <summary>
        /// Confirms the command execute.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private void ConfirmCommandExecute(object obj)
        {
            Settings.Default.COM_Port = SettingsDataModel.ComPort;
            Settings.Default.Preambles = SettingsDataModel.Preamble;
            Settings.Default.ShowOnStartup = SettingsDataModel.ShowSettingsOnStart;
            Settings.Default.Save();

            CloseActivePopUpCommand.Execute(true);
        }

        /// <summary>
        /// Determines whether this instance [can execute confirm command] the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can execute confirm command] the specified obj; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteConfirmCommand(object obj)
        {
            return !string.IsNullOrEmpty(SettingsDataModel.ComPort) && !string.IsNullOrEmpty(SettingsDataModel.Preamble);
        }
    }
}
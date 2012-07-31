using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using Cinch;
using Finaltec.Communication.HartLite;
using Finaltec.Hart.Analyzer.ViewModel.Common;
using Finaltec.Hart.Analyzer.ViewModel.DataModels;
using Finaltec.Hart.Analyzer.ViewModel.DataTemplate;
using Finaltec.Hart.Analyzer.ViewModel.Model;
using Finaltec.Hart.Analyzer.ViewModel.Properties;
using MEFedMVVM.ViewModelLocator;

namespace Finaltec.Hart.Analyzer.ViewModel
{
    /// <summary>
    /// MainViewModel class.
    /// Implements base class ViewModelBase.
    /// </summary>
    /// 
    [ExportViewModel("MainViewModel")]
    public class MainViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _visualizerService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly IHartCommunicationLiteEx _hartCommunication;
        private string _value;
        private readonly SynchronizationContext _synchronizationContext;

        private string _selectedOutput = "";
        private bool _isConnected;
        private readonly SettingsViewModel _settingsViewModel;

        /// <summary>
        /// Gets or sets the data transfer model.
        /// </summary>
        /// <value>The data transfer model.</value>
        public DataTransferModel DataTransferModel { get; set; }
        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public Filter Filter { get; set; }

        /// <summary>
        /// Gets or sets the selected output.
        /// </summary>
        /// <value>The selected output.</value>
        public string SelectedOutput
        {
            get { return _selectedOutput; }
            set
            {
                _selectedOutput = value; 
                SelectedOutputChanged();
            }
        }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return _value; }
            set { _value = value; NotifyPropertyChanged("Value"); }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; NotifyPropertyChanged("IsConnected"); }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [swap bits].
        /// </summary>
        /// <value><c>true</c> if [swap bits]; otherwise, <c>false</c>.</value>
        public bool SwapBytes
        {
            get { return Settings.Default.BitSwapping; }
            set
            {
                Settings.Default.BitSwapping = value;
                NotifyPropertyChanged("SwapBytes");

                SelectedOutputChanged();
            }
        }

        public SimpleCommand<object, object> ConnectDisconnectCommand { get; private set; }
        public SimpleCommand<object, object> SendCommand { get; private set; }
        public SimpleCommand<object, object> DisplayConnectionSettingsCommand { get; private set; }
        public SimpleCommand<object, object> AboutCommand { get; private set; }
        public SimpleCommand<object, object> HelpCommand { get; private set; }
        public SimpleCommand<object, object> GiveFeedbackCommand { get; private set; }

        [ImportingConstructor]
        public MainViewModel(IUIVisualizerService visualizerService, IViewAwareStatusWindow window, IMessageBoxService messageBoxService, IHartCommunicationLiteEx hartCommunication)
        {
            _settingsViewModel = new SettingsViewModel();
            _visualizerService = visualizerService;
            _messageBoxService = messageBoxService;
            _hartCommunication = hartCommunication;
            window.ViewLoaded += () =>
                                     {
                                         if (Settings.Default.ShowOnStartup)
                                             _visualizerService.ShowDialog("SettingsViewModel", _settingsViewModel);
                                     };
            window.ViewWindowClosed += () =>
                                           {
                                               Settings.Default.Save();
                                               Application.Current.Shutdown();
                                           };
            ReadSettings();
            DataTransferModel = DataTransferModel.GetInstance();
            _synchronizationContext = SynchronizationContext.Current;

            InitCommands();
        }

        /// <summary>
        /// Inits the commands.
        /// </summary>
        private void InitCommands()
        {
            ConnectDisconnectCommand = new SimpleCommand<object, object>(ConnectDisconnectCommandExecute);
            SendCommand = new SimpleCommand<object, object>(obj => IsConnected, SendCommandExecute);
            DisplayConnectionSettingsCommand = new SimpleCommand<object, object>(obj => !IsConnected, DisplayConnectionSettingsCommandExecute);
            AboutCommand = new SimpleCommand<object, object>(AboutCommandExecute);
            HelpCommand = new SimpleCommand<object, object>(obj => NavigateTo("http://hartanalyzer.codeplex.com/documentation/"));
            GiveFeedbackCommand = new SimpleCommand<object, object>(obj => NavigateTo("http://hartanalyzer.codeplex.com/discussions/"));
        }

        private static void NavigateTo(string httpUrl)
        {
            Process.Start(new ProcessStartInfo(httpUrl));
        }

        /// <summary>
        /// Reads the settings.
        /// </summary>
        private void ReadSettings()
        {
            SettingsDataModel settings = _settingsViewModel.SettingsDataModel;
            settings.ComPort = Settings.Default.COM_Port;
            settings.Preamble = Settings.Default.Preambles;
            settings.ShowSettingsOnStart = Settings.Default.ShowOnStartup;

            SwapBytes = Settings.Default.BitSwapping;
            Filter = new Filter();
        }

        /// <summary>
        /// Send command execute.
        /// </summary>
        /// <param name="o">The o.</param>
        private void SendCommandExecute(object o)
        {
            SendCommandViewModel sendCommandViewModel = new SendCommandViewModel(_visualizerService);
            bool? dialog = _visualizerService.ShowDialog("SendCommandViewModel", sendCommandViewModel);
            

            if (dialog.HasValue && dialog.Value)
            {
                List<byte> databytes = new List<byte>();

                foreach (Input input in sendCommandViewModel.RawValues)
                {
                    if (string.IsNullOrEmpty(input.RawValue))
                        continue;

                    if (input.DataBytes != null)
                    {
                        databytes.AddRange(input.DataBytes);
                        continue;
                    }

                    return;
                }

                _hartCommunication.SendAsync(sendCommandViewModel.Command, (databytes.Count > 0) ? databytes.ToArray() : new byte[0]);
            }
        }

        /// <summary>
        /// Connect and disconnect command execute.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private void ConnectDisconnectCommandExecute(object obj)
        {
            if (!IsConnected)
            {
                try
                {
                    _hartCommunication.Initialize(_settingsViewModel.SettingsDataModel.ComPort);
                    OpenResult result = _hartCommunication.Open();

                    switch (result)
                    {
                        case OpenResult.UnknownComPortError:
                            _messageBoxService.ShowError("Unknown COM port error while connecting to device.", "Error");
                            return;
                        case OpenResult.ComPortNotExisting:
                            _messageBoxService.ShowError("Selected COM port does not exists.", "Error");
                            return;
                        case OpenResult.ComPortIsOpenAlreadyOpen:
                            _messageBoxService.ShowError("COM port is already opened.", "Error");
                            return;
                    }

                    _hartCommunication.PreambleLength = Convert.ToInt32(_settingsViewModel.SettingsDataModel.Preamble);
                    _hartCommunication.Receive += ReceiveValueHandle;
                    _hartCommunication.SendingCommand += SendingValueHandle;

                    IsConnected = true;
                }
                catch (Exception e)
                {
                    _messageBoxService.ShowError("Error on connecting to device.\n\n" + e, "Error");
                }
            }
            else
            {
                try
                {
                    CloseResult result = _hartCommunication.Close();

                    switch (result)
                    {
                        case CloseResult.PortIsNotOpen:

                            _messageBoxService.ShowError("The port is not open.", "Error");
                            return;
                    }

                    IsConnected = false;

                    _hartCommunication.Receive -= ReceiveValueHandle;
                    _hartCommunication.SendingCommand -= SendingValueHandle;
                }
                catch (Exception e)
                {
                    _messageBoxService.ShowError("Error on disconnecting from device.\n\n" + e, "Error");
                }
            }
        }

        /// <summary>
        /// Sendings the value handle.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        private void SendingValueHandle(object sender, CommandRequest args)
        {
            _synchronizationContext.Send(delegate
            {
                DataTransferModel.Output.Add(new CommandData(InformationType.Send, args.PreambleLength, args.Delimiter, BitConverter.ToString(args.Address.ToByteArray()), args.CommandNumber, args.Data,
                    args.Checksum));
            }, null);
        }

        /// <summary>
        /// Receives the value handle.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        private void ReceiveValueHandle(object sender, CommandResult args)
        {
            _synchronizationContext.Send(delegate
            {
                DataTransferModel.Output.Add(new CommandData(InformationType.Receive, args.PreambleLength, args.Delimiter, BitConverter.ToString(args.Address.ToByteArray()), args.CommandNumber, args.Data,
                   BitConverter.ToString(new[] { args.ResponseCode.FirstByte, args.ResponseCode.SecondByte }), args.Checksum));
            }, null);
        }

        /// <summary>
        /// Selecteds the output changed.
        /// </summary>
        private void SelectedOutputChanged()
        {
            string[] outputParts = SelectedOutput.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (outputParts.Any(outputPart => outputPart.Trim().Length > 2) || outputParts.Any(outputPart => string.IsNullOrEmpty(outputPart) || outputPart == " "))
            {
                return;
            }

            List<byte> selectedBytes = outputParts.Select(outputPart => byte.Parse(outputPart, NumberStyles.HexNumber)).ToList();

            string byteValue = string.Empty;
            string shortValue = string.Empty;
            string uShortValue = string.Empty;
            string int24Value = string.Empty;
            string uInt24Value = string.Empty;
            string intValue = string.Empty;
            string uIntValue = string.Empty;
            string floatValue = string.Empty;
            string stringValue = string.Empty;

            if (selectedBytes.Count == 1)
                byteValue = string.Format("Byte Value: '{0}' - ", selectedBytes[0]);

            if(selectedBytes.Count == 2)
                shortValue = string.Format("Short Value: '{0}' - ", (SwapBytes) ? BitConverter.ToInt16(selectedBytes.ToArray().Reverse().ToArray(), 0) : BitConverter.ToInt16(selectedBytes.ToArray(), 0));

            if(selectedBytes.Count == 2)
                uShortValue = string.Format("UShort Value: '{0}' - ", (SwapBytes) ? BitConverter.ToUInt16(selectedBytes.ToArray().Reverse().ToArray(), 0) : BitConverter.ToUInt16(selectedBytes.ToArray(), 0));

            if (selectedBytes.Count == 3) 
                int24Value = string.Format("Int24 Value: '{0}' - ", (SwapBytes) ? new Int24(BitConverter.ToInt32(new byte[] { selectedBytes[2], selectedBytes[1], selectedBytes[0], 0 }, 0)).Value : new Int24(BitConverter.ToInt32(new byte[] { selectedBytes[0], selectedBytes[1], selectedBytes[2], 0 }, 0)).Value);
            
            if (selectedBytes.Count == 3)
                uInt24Value = string.Format("UInt24 Value: '{0}' - ", (SwapBytes) ? BitConverter.ToUInt32(new byte[] { selectedBytes[0], selectedBytes[1], selectedBytes[2], 0 }, 0) : BitConverter.ToUInt32(new byte[] { selectedBytes[2], selectedBytes[1], selectedBytes[0], 0 }, 0));
            
            if (selectedBytes.Count == 4)
                intValue = string.Format("Int32 Value: '{0}' - ", (SwapBytes) ? BitConverter.ToInt32(selectedBytes.ToArray().Reverse().ToArray(), 0) : BitConverter.ToInt32(selectedBytes.ToArray(), 0));

            if (selectedBytes.Count == 4)
                uIntValue = string.Format("UInt32 Value: '{0}' - ", (SwapBytes) ? BitConverter.ToUInt32(selectedBytes.ToArray().Reverse().ToArray(), 0) : BitConverter.ToUInt32(selectedBytes.ToArray(), 0));

            if (selectedBytes.Count == 4)
                floatValue = string.Format("Float Value: '{0}' - ", (SwapBytes) ? BitConverter.ToSingle(selectedBytes.ToArray().Reverse().ToArray(), 0) : BitConverter.ToSingle(selectedBytes.ToArray(), 0));

            if(selectedBytes.Count > 0)
                stringValue = string.Format("String Value: '{0}'", Encoding.ASCII.GetString(selectedBytes.ToArray()));

            Value = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", (!string.IsNullOrEmpty(byteValue)) ? byteValue : "",
                                    (!string.IsNullOrEmpty(shortValue)) ? shortValue : "",
                                    (!string.IsNullOrEmpty(uShortValue)) ? uShortValue : "",
                                    (!string.IsNullOrEmpty(int24Value)) ? int24Value : "",
                                    (!string.IsNullOrEmpty(uInt24Value)) ? uInt24Value : "",
                                    (!string.IsNullOrEmpty(intValue)) ? intValue : "",
                                    (!string.IsNullOrEmpty(uIntValue)) ? uIntValue : "",
                                    (!string.IsNullOrEmpty(floatValue)) ? floatValue : "",
                                    (!string.IsNullOrEmpty(stringValue)) ? stringValue : "");
        }

        /// <summary>
        /// Displays the connection settings command execute.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private void DisplayConnectionSettingsCommandExecute(object obj)
        {
            _visualizerService.ShowDialog("SettingsViewModel", _settingsViewModel);
        }

        private void AboutCommandExecute(object obj)
        {
            _visualizerService.ShowDialog("AboutViewModel", new AboutViewModel());
        }
    }
}
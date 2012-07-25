using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Cinch;
using Finaltec.Hart.Analyzer.ViewModel.DataModels;
using MEFedMVVM.ViewModelLocator;

namespace Finaltec.Hart.Analyzer.ViewModel
{
    /// <summary>
    /// SendCommandModel class.
    /// Implements base class ViewModelBase.
    /// </summary>
    [ExportViewModel("SendCommandViewModel")]
    public class SendCommandViewModel : ViewModelBase
    {
        private byte _command;
        private bool _enableData;
        private string _errorMessage;
        private InputDataType _type;
        private Input _selectedRawValue;
        private bool _typeChangeable;

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        public byte Command
        {
            get { return _command; }
            set
            {
                _command = value;
                EnableData = value > 0;
            }
        }

        public InputDataType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged("Type");
            }
        }


        public bool TypeChangeable
        {
            get { return _typeChangeable; }
            set
            {
                _typeChangeable = value;
                NotifyPropertyChanged("TypeChangeable");
            }
        }

        public Input SelectedRawValue
        {
            get { return _selectedRawValue; }
            set
            {
                if (value == null)
                    return;

                _selectedRawValue = value;
                NotifyPropertyChanged("SelectedRawValue");
                Type = _selectedRawValue.Type;
            }
        }


        /// <summary>
        /// Gets or sets the raw values.
        /// </summary>
        /// <value>The raw values.</value>
        public ObservableCollection<Input> RawValues { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether [enable data].
        /// </summary>
        /// <value><c>true</c> if [enable data]; otherwise, <c>false</c>.</value>
        public bool EnableData
        {
            get { return _enableData; }
            set
            {
                _enableData = value;
                NotifyPropertyChanged("EnableData");
            }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
                NotifyPropertyChanged("ShowError");
            }
        }
        /// <summary>
        /// Gets the show error.
        /// </summary>
        /// <value>The show error.</value>
        public Visibility ShowError { get { return (string.IsNullOrEmpty(ErrorMessage)) ? Visibility.Collapsed : Visibility.Visible; }}

        /// <summary>
        /// Gets or sets a value indicating whether [command valid].
        /// </summary>
        /// <value><c>true</c> if [command valid]; otherwise, <c>false</c>.</value>
        public bool CommandValid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SendCommandViewModel"/> is response.
        /// </summary>
        /// <value><c>true</c> if response; otherwise, <c>false</c>.</value>
        public bool Response { get; set; }

        /// <summary>
        /// Occurs when raw data was cleared.
        /// </summary>
        public event Action RawDataCleared;

        /// <summary>
        /// Gets or sets the send command.
        /// </summary>
        /// <value>The send command.</value>
        public SimpleCommand<object, object> SendCommand { get; private set; }
        /// <summary>
        /// Gets or sets the cancel command.
        /// </summary>
        /// <value>The cancel command.</value>
        public SimpleCommand<object, object> CancelCommand { get; private set; }
        /// <summary>
        /// Gets or sets the clear data command.
        /// </summary>
        /// <value>The clear data command.</value>
        public SimpleCommand<object, object> ClearDataCommand { get; private set; }

        [ImportingConstructor]
        public SendCommandViewModel(IUIVisualizerService visualizerService)
        {
            InitCommands();

            RawValues = new ObservableCollection<Input>();
            RawValues.Add(new Input(RawValues, InputType.Byte));

            Response = false;
        }

        /// <summary>
        /// Inits the commands.
        /// </summary>
        private void InitCommands()
        {
            SendCommand = new SimpleCommand<object, object>(CanExecuteSendCommand, SendCommandExecute);
            CancelCommand = new SimpleCommand<object, object>(obj => CloseActivePopUpCommand.Execute(false));
            ClearDataCommand = new SimpleCommand<object, object>(ClearRawDataCommand);
        }

        public void GotFocusAction(object sender, RoutedEventArgs args)
        {
            _selectedRawValue = null;
            NotifyPropertyChanged("SelectedRawValue");
            TypeChangeable = false;
        }

        public void TextBoxGotFocusAction(object sender, RoutedEventArgs args)
        {
            TypeChangeable = true;
        }

        /// <summary>
        /// Send command execute.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private void SendCommandExecute(object obj)
        {
            Response = true;
            CloseActivePopUpCommand.Execute(Response);
        }

        /// <summary>
        /// Determines whether this instance [can execute send command] the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can execute send command] the specified obj; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecuteSendCommand(object obj)
        {
            if (!CheckData()) 
                return false;

            ErrorMessage = string.Empty;
            return CommandValid;
        }

        /// <summary>
        /// Clears the raw data.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private void ClearRawDataCommand(object obj)
        {
            RawValues.Clear();
            RawValues.Add(new Input(RawValues, InputType.Byte));

            if(RawDataCleared != null)
                RawDataCleared.Invoke();
        }

        /// <summary>
        /// Checks the data.
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            foreach (Input input in RawValues.Where(input => !string.IsNullOrEmpty(input.RawValue)).Where(input => !input.IsValid))
            {
                ErrorMessage = input.ValidateErrorMessage;
                return false;
            }

            return true;
        }
    }
}
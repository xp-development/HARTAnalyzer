using System.ComponentModel;
using Finaltec.Hart.Analyzer.ViewModel.Properties;

namespace Finaltec.Hart.Analyzer.ViewModel.DataTemplate
{
    /// <summary>
    /// Filter class.
    /// Implements interface INotifyPropertyChanged.
    /// </summary>
    public class Filter : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a value indicating whether [display time].
        /// </summary>
        /// <value><c>true</c> if [display time]; otherwise, <c>false</c>.</value>
        public bool DisplayTime
        {
            get { return Settings.Default.Display_Time; }
            set
            {
                Settings.Default.Display_Time = value;
                InvokePropertyChanged("DisplayTime");
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [display send or recived].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [display send or recived]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplaySendOrRecived
        {
            get { return Settings.Default.Display_SendOrRecived; }
            set
            {
                Settings.Default.Display_SendOrRecived = value;
                InvokePropertyChanged("DisplaySendOrRecived");
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [display preamble].
        /// </summary>
        /// <value><c>true</c> if [display preamble]; otherwise, <c>false</c>.</value>
        public bool DisplayPreamble
        {
            get { return Settings.Default.Display_Preamble; }
            set
            {
                Settings.Default.Display_Preamble = value;
                InvokePropertyChanged("DisplayPreamble");
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [display delimiter].
        /// </summary>
        /// <value><c>true</c> if [display delimiter]; otherwise, <c>false</c>.</value>
        public bool DisplayDelimiter
        {
            get { return Settings.Default.Display_Delimiter; }
            set
            {
                Settings.Default.Display_Delimiter = value;
                InvokePropertyChanged("DisplayDelimiter");
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [display address].
        /// </summary>
        /// <value><c>true</c> if [display address]; otherwise, <c>false</c>.</value>
        public bool DisplayAddress
        {
            get { return Settings.Default.Display_Address; }
            set
            {
                Settings.Default.Display_Address = value;
                InvokePropertyChanged("DisplayAddress");
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [display command].
        /// </summary>
        /// <value><c>true</c> if [display command]; otherwise, <c>false</c>.</value>
        public bool DisplayCommand
        {
            get { return Settings.Default.Display_Command; }
            set
            {
                Settings.Default.Display_Command = value;
                InvokePropertyChanged("DisplayCommand");
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [display byte count].
        /// </summary>
        /// <value><c>true</c> if [display byte count]; otherwise, <c>false</c>.</value>
        public bool DisplayByteCount
        {
            get { return Settings.Default.Display_ByteCount; }
            set
            {
                Settings.Default.Display_ByteCount = value;
                InvokePropertyChanged("DisplayByteCount");
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [display data].
        /// </summary>
        /// <value><c>true</c> if [display data]; otherwise, <c>false</c>.</value>
        public bool DisplayData
        {
            get { return Settings.Default.Display_Data; }
            set
            {
                Settings.Default.Display_Data = value;
                InvokePropertyChanged("DisplayData");
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [display response].
        /// </summary>
        /// <value><c>true</c> if [display response]; otherwise, <c>false</c>.</value>
        public bool DisplayResponse
        {
            get { return Settings.Default.Display_Response; }
            set
            {
                Settings.Default.Display_Response = value;
                InvokePropertyChanged("DisplayResponse");
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [display checksum].
        /// </summary>
        /// <value><c>true</c> if [display checksum]; otherwise, <c>false</c>.</value>
        public bool DisplayChecksum
        {
            get { return Settings.Default.Display_Checksum; }
            set
            {
                Settings.Default.Display_Checksum = value;
                InvokePropertyChanged("DisplayChecksum");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        public Filter()
        {
            DisplayTime = Settings.Default.Display_Time;
            DisplaySendOrRecived = Settings.Default.Display_SendOrRecived;
            DisplayPreamble = Settings.Default.Display_Preamble;
            DisplayDelimiter = Settings.Default.Display_Delimiter;
            DisplayAddress = Settings.Default.Display_Address;
            DisplayCommand = Settings.Default.Display_Command;
            DisplayByteCount = Settings.Default.Display_ByteCount;
            DisplayData = Settings.Default.Display_Data;
            DisplayResponse = Settings.Default.Display_Response;
            DisplayChecksum = Settings.Default.Display_Checksum;
        }

        /// <summary>
        /// Invokes the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void InvokePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Occurs on changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
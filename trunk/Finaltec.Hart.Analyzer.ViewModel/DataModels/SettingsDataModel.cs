namespace Finaltec.Hart.Analyzer.ViewModel.DataModels
{
    /// <summary>
    /// SettingsDataModel class.
    /// </summary>
    public class SettingsDataModel
    {
        /// <summary>
        /// Gets or sets the COM port.
        /// </summary>
        /// <value>The COM port.</value>
        public string ComPort { get; set; }
        /// <summary>
        /// Gets or sets the preamble count.
        /// </summary>
        /// <value>The preamble.</value>
        public string Preamble { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [show settings on start].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [show settings on start]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowSettingsOnStart { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows;
using Cinch;
using log4net;

namespace Finaltec.Hart.Analyzer.View
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));

        /// <summary>
        /// Called on application startup.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.StartupEventArgs"/> instance containing the event data.</param>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            InitLogging();
            CinchBootStrapper.Initialise(new List<Assembly> { typeof(App).Assembly });
        }

        /// <summary>
        /// Inits the logging.
        /// </summary>
        private static void InitLogging()
        {
            Log.Info(string.Format("{0}{0}## - Start application at {1}.{0}", Environment.NewLine, DateTime.Now.ToString(CultureInfo.InvariantCulture)));
            Log.Info("Log4net initialized successfully from app config file.");
        }
    }
}

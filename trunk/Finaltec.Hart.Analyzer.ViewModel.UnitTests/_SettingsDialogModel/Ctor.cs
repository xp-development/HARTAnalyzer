using NUnit.Framework;

namespace Finaltec.Hart.Analyzer.ViewModel.UnitTests._SettingsDialogModel
{
    [TestFixture]
    public class Ctor
    {
        [Test]
        public void Usage()
        {
            SettingsViewModel settingsViewModel = new SettingsViewModel();
            Assert.That(settingsViewModel, Is.Not.Null);
        }
    }
}
using System;
using System.Threading;
using Cinch;
using Finaltec.Hart.Analyzer.ViewModel.Model;
using Finaltec.Hart.Analyzer.ViewModel.Properties;
using FluentAssertions;
using NUnit.Framework;

namespace Finaltec.Hart.Analyzer.ViewModel.UnitTests._MainViewModel
{
    [TestFixture]
    public class ViewLoaded
    {
        private TestViewAwareStatusWindow _window;
        private TestUIVisualizerService _visualizerService;
        private bool _settingWasCalled;
        private bool _updateWasCalled;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        [SetUp]
        public void SetUp()
        {
            _window = new TestViewAwareStatusWindow();
            _visualizerService = new TestUIVisualizerService();
            _settingWasCalled = false;
            _updateWasCalled = false;
        }

        [TearDown]
        public void TearDown()
        {
            Settings.Default.Reset();
        }

        [Test]
        public void DontShowSettingsAndNewVersion()
        {
            Settings.Default.ShowOnStartup = false;
            TestVersionService versionService = new TestVersionService(new Version(0, 5), new Version(0, 5));
            new MainViewModel(_visualizerService, _window, new TestMessageBoxService(), new RealHartCommunicationLite(), versionService);

            _window.SimulateViewIsLoadedEvent();
            versionService.ReleaseOnlineVersion();

            _settingWasCalled.Should().BeFalse();
            _updateWasCalled.Should().BeFalse();
        }

        [Test]
        public void ShowSettingsButDontShowNewVersion()
        {
            _visualizerService.ShowDialogResultResponders.Enqueue(() => _settingWasCalled = true);

            Settings.Default.ShowOnStartup = true;
            TestVersionService versionService = new TestVersionService(new Version(0, 5), new Version(0, 5));
            new MainViewModel(_visualizerService, _window, new TestMessageBoxService(), new RealHartCommunicationLite(), versionService);

            _window.SimulateViewIsLoadedEvent();
            versionService.ReleaseOnlineVersion();

            _settingWasCalled.Should().BeTrue();
            _updateWasCalled.Should().BeFalse();
        }

        [Test]
        public void DontShowSettingsButShowNewVersionIfNewOnlineVersionIsAvailable()
        {
            _visualizerService.ShowDialogResultResponders.Enqueue(() => _updateWasCalled = true);

            Settings.Default.ShowOnStartup = false;
            Settings.Default.LastUpdateCheck = DateTime.Now.AddDays(-9);
            TestVersionService versionService = new TestVersionService(new Version(0, 5), new Version(0, 6));
            new MainViewModel(_visualizerService, _window, new TestMessageBoxService(), new RealHartCommunicationLite(), versionService);

            _window.SimulateViewIsLoadedEvent();
            versionService.ReleaseOnlineVersion();

            _settingWasCalled.Should().BeFalse();
            _updateWasCalled.Should().BeTrue();
        }

        [Test]
        public void DontShowSettingsButDontShowNewVersionIfLastUpdateCheckIsNotLongerSevenDays()
        {
            _visualizerService.ShowDialogResultResponders.Enqueue(() => _updateWasCalled = true);

            Settings.Default.ShowOnStartup = false;
            Settings.Default.LastUpdateCheck = DateTime.Now.AddDays(-5);
            new MainViewModel(_visualizerService, _window, new TestMessageBoxService(), new RealHartCommunicationLite(), new TestVersionService(new Version(0, 5), new Version(0, 6)));

            _window.SimulateViewIsLoadedEvent();

            _settingWasCalled.Should().BeFalse();
            _updateWasCalled.Should().BeFalse();
        }
    }
}
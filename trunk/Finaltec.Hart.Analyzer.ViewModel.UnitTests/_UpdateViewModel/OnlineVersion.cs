using System;
using FluentAssertions;
using NUnit.Framework;

namespace Finaltec.Hart.Analyzer.ViewModel.UnitTests._UpdateViewModel
{
    [TestFixture]
    public class OnlineVersion
    {
        [Test]
        public void Usage()
        {
            var versionService = new TestVersionService(new Version("5.0"), new Version("6.1"));
            UpdateViewModel viewModel = new UpdateViewModel(versionService);

            viewModel.OnlineVersion.Should().Be("Loading..");

            versionService.ReleaseOnlineVersion();

            viewModel.OnlineVersion.Should().Be("6.1");
        }

        [Test]
        public void ShouldSetInformationForNewestVersion()
        {
            var versionService = new TestVersionService(new Version("6.1"), new Version("6.1"));
            UpdateViewModel viewModel = new UpdateViewModel(versionService);

            viewModel.OnlineVersion.Should().Be("Loading..");

            versionService.ReleaseOnlineVersion();

            viewModel.OnlineVersion.Should().Be("The newest version is already installed.");
        }

        [Test]
        public void ShouldSetErrorMessageIfOnlineVersionIsNotReadable_VersionIsNull()
        {
            ShouldSetErrorMessageIfOnlineVersionIsNotReadable(null);
        }

        [Test]
        public void ShouldSetErrorMessageIfOnlineVersionIsNotReadable_VersionIsDefault()
        {
            ShouldSetErrorMessageIfOnlineVersionIsNotReadable(new Version());
        }

        private static void ShouldSetErrorMessageIfOnlineVersionIsNotReadable(Version version)
        {
            var versionService = new TestVersionService(new Version("5.0"), version);
            UpdateViewModel viewModel = new UpdateViewModel(versionService);

            viewModel.OnlineVersion.Should().Be("Loading..");

            versionService.ReleaseOnlineVersion();

            viewModel.OnlineVersion.Should().Be("Cannot check for updates.");
        }
    }
}
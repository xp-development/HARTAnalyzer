using System;
using FluentAssertions;
using NUnit.Framework;

namespace Finaltec.Hart.Analyzer.Core.UnitTest._ScanningWebsiteVersionService
{
    [TestFixture]
    [Ignore("Manual test")]
    public class GetOnlineVersion
    {
        [Test]
        public void Usage()
        {
            ScanningWebsiteVersionService service = new ScanningWebsiteVersionService
                                                        {
                                                            DefaultWebsite = "http://hartprotocollite.codeplex.com/releases/"
                                                        };

            Version version = service.GetOnlineVersion();

            version.Major.Should().Be(0);
            version.Minor.Should().Be(6);
        }

        [Test]
        public void FailOnNotAvailable()
        {
            ScanningWebsiteVersionService service = new ScanningWebsiteVersionService
                                                        {
                                                            DefaultWebsite = "http://www.google.de"
                                                        };

            Version version = service.GetOnlineVersion();

            version.Major.Should().Be(0);
            version.Minor.Should().Be(0);
            version.Revision.Should().Be(-1);
            version.Build.Should().Be(-1);
        }
    }
}
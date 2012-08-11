using System;
using FluentAssertions;
using NUnit.Framework;

namespace Finaltec.Hart.Analyzer.Core.UnitTest._ScanningWebsiteVersionService
{
    [TestFixture]
    public class GetCurrentVersion
    {
        [Test]
        public void ShouldReturnCurrentAssemblyVersion()
        {
            ScanningWebsiteVersionService service = new ScanningWebsiteVersionService();

            Version version = service.GetCurrentVersion();

            version.Major.Should().Be(0);
            version.Minor.Should().Be(7);
            version.Revision.Should().Be(0);
            version.Build.Should().Be(0);
        }
    }
}
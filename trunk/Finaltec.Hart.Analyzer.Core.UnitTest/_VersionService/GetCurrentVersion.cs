using System;
using FluentAssertions;
using NUnit.Framework;

namespace Finaltec.Hart.Analyzer.Core.UnitTest._VersionService
{
    [TestFixture]
    [Ignore("Manual test")]
    public class GetCurrentVersion
    {
        [Test]
        public void Usage()
        {
            VersionService service = new VersionService();

            Version version = service.GetCurrentVersion();

            version.Major.Should().Be(0);
            version.Minor.Should().Be(6);
        }

        [Test]
        public void FailOnNotAvailable()
        {
            Assert.Fail();
        }
    }
}
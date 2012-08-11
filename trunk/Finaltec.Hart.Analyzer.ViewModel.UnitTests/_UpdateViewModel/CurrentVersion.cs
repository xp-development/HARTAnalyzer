using System;
using FluentAssertions;
using NUnit.Framework;

namespace Finaltec.Hart.Analyzer.ViewModel.UnitTests._UpdateViewModel
{
    [TestFixture]
    public class CurrentVersion
    {
        [Test]
        public void Usage()
        {
            UpdateViewModel viewModel = new UpdateViewModel(new TestVersionService(new Version("5.0"), new Version("6.1")));

            viewModel.CurrentVersion.Should().Be("5.0");
        }
    }
}
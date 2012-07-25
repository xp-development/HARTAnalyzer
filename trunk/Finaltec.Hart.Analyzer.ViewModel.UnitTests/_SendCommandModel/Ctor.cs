using NUnit.Framework;

namespace Finaltec.Hart.Analyzer.ViewModel.UnitTests._SendCommandModel
{
    [TestFixture]
    public class Ctor
    {
        [Test]
        public void Usage()
        {
            SendCommandViewModel sendCommandViewModel = new SendCommandViewModel(new Cinch.TestUIVisualizerService());
            Assert.That(sendCommandViewModel, Is.Not.Null);
        }
    }
}
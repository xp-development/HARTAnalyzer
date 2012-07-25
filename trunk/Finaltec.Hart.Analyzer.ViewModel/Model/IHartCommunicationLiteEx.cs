using Finaltec.Communication.HartLite;

namespace Finaltec.Hart.Analyzer.ViewModel.Model
{
    public interface IHartCommunicationLiteEx : IHartCommunicationLite
    {
        void Initialize(string comPort);
    }
}
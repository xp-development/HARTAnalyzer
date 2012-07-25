using System;
using System.ComponentModel.Composition;
using Finaltec.Communication.HartLite;
using MEFedMVVM.ViewModelLocator;

namespace Finaltec.Hart.Analyzer.ViewModel.Model
{
#if !Isolated
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [ExportService(ServiceType.Runtime, typeof(IHartCommunicationLiteEx))]
#endif
    public class RealHartCommunicationLite : IHartCommunicationLiteEx
    {
        private HartCommunicationLite _hartCommunicationLite;

        public void Initialize(string comPort)
        {
            _hartCommunicationLite = new HartCommunicationLite(comPort);
        }

        public event ReceiveHandler Receive
        {
            add { _hartCommunicationLite.Receive += value; }
            remove { _hartCommunicationLite.Receive -= value; }
        }

        public event SendingCommandHandler SendingCommand
        {
            add { _hartCommunicationLite.SendingCommand += value; }
            remove { _hartCommunicationLite.SendingCommand -= value; }
        }

        public int PreambleLength
        {
            get { return _hartCommunicationLite.PreambleLength; }
            set { _hartCommunicationLite.PreambleLength = value; }
        }

        public int MaxNumberOfRetries
        {
            get { return _hartCommunicationLite.MaxNumberOfRetries; }
            set { _hartCommunicationLite.MaxNumberOfRetries = value; }
        }

        public TimeSpan Timeout
        {
            get { return _hartCommunicationLite.Timeout; }
            set { _hartCommunicationLite.Timeout = value; }
        }

        public bool AutomaticZeroCommand
        {
            get { return _hartCommunicationLite.AutomaticZeroCommand; }
            set { _hartCommunicationLite.AutomaticZeroCommand = value; }
        }

        public IAddress Address
        {
            get { return _hartCommunicationLite.Address; }
        }

        public OpenResult Open()
        {
            return _hartCommunicationLite.Open();
        }

        public CloseResult Close()
        {
            return _hartCommunicationLite.Close();
        }

        public CommandResult Send(byte command)
        {
            return _hartCommunicationLite.Send(command);
        }

        public CommandResult Send(byte command, byte[] data)
        {
            return _hartCommunicationLite.Send(command, data);
        }

        public CommandResult SendZeroCommand()
        {
            return _hartCommunicationLite.SendZeroCommand();
        }

        public void SendAsync(byte command)
        {
            _hartCommunicationLite.SendAsync(command);
        }

        public void SendAsync(byte command, byte[] data)
        {
            _hartCommunicationLite.SendAsync(command, data);
        }

        public void SendZeroCommandAsync()
        {
            _hartCommunicationLite.SendZeroCommandAsync();
        }

        public void SwitchAddressTo(IAddress address)
        {
            _hartCommunicationLite.SwitchAddressTo(address);
        }
    }
}
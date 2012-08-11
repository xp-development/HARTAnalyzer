using System;
using System.Threading;
using Finaltec.Hart.Analyzer.Core;

namespace Finaltec.Hart.Analyzer.ViewModel.UnitTests
{
    public class TestVersionService : IVersionService
    {
        public event Action<object, Version> GetOnlineVersionResult;

        private readonly Version _current;
        private readonly Version _online;
        private readonly AutoResetEvent _resetEvent;

        public TestVersionService(Version current, Version online)
        {
            _current = current;
            _online = online;
            _resetEvent = new AutoResetEvent(false);
        }

        public Version GetCurrentVersion()
        {
            return _current;
        }

        public Version GetOnlineVersion()
        {
            return _online;
        }

        public void ReleaseOnlineVersion()
        {
            if (GetOnlineVersionResult != null)
                GetOnlineVersionResult.Invoke(this, _online);
        }

        public void GetOnlineVersionAsync()
        {
            Action action = () => _resetEvent.WaitOne(10000);
            action.BeginInvoke(null, null);
        }
    }
}
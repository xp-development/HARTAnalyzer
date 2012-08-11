using System;

namespace Finaltec.Hart.Analyzer.Core
{
    public interface IVersionService
    {
        Version GetCurrentVersion();
        Version GetOnlineVersion();
        void GetOnlineVersionAsync();
        event Action<object, Version> GetOnlineVersionResult;
    }
}
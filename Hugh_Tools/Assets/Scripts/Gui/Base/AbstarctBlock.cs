using System;

namespace HughCommon.HGui.NBase
{
    public abstract class AbstractBlock
        : IDisposable
    {
        protected abstract void Dispose();

        void IDisposable.Dispose()
        {
            Dispose();
        }
    }
}

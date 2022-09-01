using System;
using System.Linq;
using System.Threading;

namespace Echorium.Utils
{
    public class CancellationHelper : IDisposable
    {
        private readonly CancellationTokenSource _cancellation;
        private bool _isDisposed;

        public CancellationToken Token { get; }

        public static CancellationHelper Create(params CancellationToken[] tokens)
        {
            return new CancellationHelper(tokens);
        }

        private CancellationHelper(params CancellationToken[] tokens)
        {
            _cancellation = tokens?.Any() ?? false
                ? CancellationTokenSource.CreateLinkedTokenSource(tokens)
                : new CancellationTokenSource();

            Token = _cancellation.Token;
        }

        public void Cancel()
        {
            if (_isDisposed)
                return;

            lock (_cancellation)
            {
                if (_isDisposed)
                    return;

                try
                {
                    _cancellation.Cancel();
                }
                catch
                {
                    // ignored
                }
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            lock (_cancellation)
            {
                if (_isDisposed)
                    return;

                _cancellation.Cancel();
                _cancellation.Dispose();
                _isDisposed = true;
            }
        }
    }



    public class CancellationStorage
    {
        private CancellationHelper? _cancellationHelper;

        public CancellationHelper? CancellationHelper => _cancellationHelper;

        public CancellationHelper Refresh(params CancellationToken[] tokens)
        {
            var cancellation = CancellationHelper.Create(tokens);
            Interlocked.Exchange(ref _cancellationHelper, cancellation)?.Cancel();
            return cancellation;
        }
    }
}

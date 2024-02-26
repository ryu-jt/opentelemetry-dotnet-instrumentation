using System;
using System.Collections.Generic;
using WhaTap.Trace.Utils;

namespace WhaTap.Trace
{
    internal class Transaction : IDisposable
    {
        public long TraceId { get; }
        public Step TransactionStep { get; set; }
        public Step CurrentStep { get; set; }
        public int WebServiceDepth { get; set; }
        public int MethodHookDepth { get; set; }

        public Transaction(Step step)
        {
            TraceId = ThreadLocalRandom.GetTraceId();
            step.TraceId = TraceId;
            TransactionStep = step;
            CurrentStep = step;
            WebServiceDepth = 0;
            MethodHookDepth = 0;
        }

        ~Transaction()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    class ActiveTransaction
    {
        private static readonly Lazy<ActiveTransaction> lazy = new Lazy<ActiveTransaction>(() => new ActiveTransaction());

        public static ActiveTransaction Instance => lazy.Value;

        private ActiveTransaction()
        {
        }

        public void CreateTransaction(Step step)
        {
            _activeTransaction.Set(new Transaction(step));
        }

        public void ReleaseTransaction()
        {
            _activeTransaction.Set(null);
        }

        public Transaction Transaction => _activeTransaction.Get();
        private AsyncLocalCompat<Transaction> _activeTransaction = new AsyncLocalCompat<Transaction>();
    }
}

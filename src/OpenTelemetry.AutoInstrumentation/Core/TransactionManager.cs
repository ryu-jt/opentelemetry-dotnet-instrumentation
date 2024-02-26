using System;
using System.Collections.Generic;
using WhaTap.Trace.Utils;

namespace WhaTap.Trace
{
    public class TransactionManager
    {
        public void StartTransaction(Step step)
        {
            if (step != null) ActiveTransaction.Instance?.CreateTransaction(step);
        }

        public void EndTransaction()
        {
            ActiveTransaction.Instance?.ReleaseTransaction();
        }

        public void StartSection(Step step)
        {
            if (step == null) return;

            var transaction = ActiveTransaction.Instance.Transaction;
            if (transaction == null) return;

            step.TraceId = transaction.TraceId;
            transaction.CurrentStep = step;
        }

        public void EndSection(Step step)
        {
            step?.Finish(DateTimeOffset.UtcNow);

            var transaction = ActiveTransaction.Instance.Transaction;
            if (transaction != null) transaction.CurrentStep = step?.Parent;
        }
    }
}

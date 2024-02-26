using System;
using System.Reflection;
using WhaTap.Trace.GoAgent;
using WhaTap.Trace.Utils;

namespace WhaTap.Trace
{
    class TraceControl
    {
        private static readonly Lazy<TraceControl> lazy = new Lazy<TraceControl>(() => new TraceControl());

        public static TraceControl Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public void StartTransaction(Step? step)
        {
            if (step == null) return;

            Logger.Instance.Debug($"* {MethodBase.GetCurrentMethod().DeclaringType.Name}.{MethodBase.GetCurrentMethod().Name} - {step.StepId}");

            try
            {
                _transactionManager.StartTransaction(step);
                UDPSender.SendTransactionStart(step);

                sendHttpHeaderStep(step);

                // TODO: Ryu
                // ActiveStack.Instance.AddTransaction(step);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Tracer.StartTransction: {ex}");
            }
        }

        public void EndTransaction()
        {
            Step step = ActiveTransaction.Instance.Transaction?.TransactionStep;
            if (step == null) return;
            if (step.IsFinished) return;

            Logger.Instance.Debug($"* {MethodBase.GetCurrentMethod().DeclaringType.Name}.{MethodBase.GetCurrentMethod().Name} - {step.StepId}");

            try
            {
                step.Finish(DateTimeOffset.UtcNow);

                _transactionManager.EndTransaction();

                if (step.Error) UDPSender.SendTransactionError(step);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Tracer.EndTransaction: {ex}");
            }

            UDPSender.SendTransactionEnd(step);
        }

        public Step? StartSection(Step? step)
        {
            if (step == null) return null;

            Logger.Instance.Debug($"* {MethodBase.GetCurrentMethod().DeclaringType.Name}.{MethodBase.GetCurrentMethod().Name} - {step.StepId}");

            try
            {
                _transactionManager.StartSection(step);
                return step;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Tracer.StartSection: {ex}");
                return null;
            }
        }

        public void EndSection(Step? step)
        {
            if (step == null) return;
            if (step.IsFinished) return;

            Logger.Instance.Debug($"* {MethodBase.GetCurrentMethod().DeclaringType.Name}.{MethodBase.GetCurrentMethod().Name} - {step.StepId}");

            try
            {
                _transactionManager.EndSection(step);

                UDPSender.SendStep(step);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Tracer.EndSection: {ex}");
            }
        }

        // 텍스트 기반의 step처럼 시작/종료가 없는 단순한 step을 처리합니다.
        public void AddSection(Step? step)
        {
            if (step == null) return;
            if (step.IsFinished) return;

            Logger.Instance.Debug($"* {MethodBase.GetCurrentMethod().DeclaringType.Name}.{MethodBase.GetCurrentMethod().Name} - {step.StepId}");

            try
            {
                _transactionManager.StartSection(step);
                _transactionManager.EndSection(step);

                UDPSender.SendStep(step);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Tracer.AddSection: {ex}");
            }
        }

        public void SetException(Step? step, Exception exception)
        {
            if (step == null) return;
            if (step.IsFinished) return;

            Logger.Instance.Debug($"* {MethodBase.GetCurrentMethod().DeclaringType.Name}.{MethodBase.GetCurrentMethod().Name} - {step.StepId}");

            try
            {
                step.SetException(exception);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Tracer.SetException: {ex}");
            }
        }

        private void sendHttpHeaderStep(Step step)
        {
            // TODO: Ryu
            //if (!Settings.Instance.ProfileHttpHeaderEnabled || (step.Type != PacketType.Httpc)) return;
            //UDPSender.SendData(PacketType.Msg, step);
        }

        private TransactionManager _transactionManager = new TransactionManager();
    }
}

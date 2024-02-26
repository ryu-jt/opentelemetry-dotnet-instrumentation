using System;
using System.Globalization;
using System.Text;
using WhaTap.Trace.Utils;

namespace WhaTap.Trace
{
    public class Step : IDisposable
    {
        private bool _disposed = false;

        public Step(Step? parent, bool isFinished = false)
        {
            if (parent != null)
            {
                TraceId = parent.TraceId;
            }

            Parent = parent;
            StepId = ThreadLocalRandom.GetStepId();
            StartTime = DateTime.UtcNow;
            IsFinished = isFinished;
        }

        ~Step()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            if (disposing)
            {
                OnFinished = null;
            }
        }

        public void SetException(Exception exception)
        {
            if (IsFinished) return;

            Error = true;

            if (exception != null)
            {
                if (exception is AggregateException aggregateException && aggregateException.InnerExceptions.Count > 0)
                {
                    exception = aggregateException.InnerExceptions[0];
                }

                ErrorType = exception.GetType().ToString();
                ErrorMsg = exception.Message;
                ErrorStack = exception.StackTrace ?? string.Empty;
            }
        }

        public void Finish(DateTimeOffset finishTimestamp)
        {
            if (IsFinished) return;
            IsFinished = true;

            Duration = finishTimestamp - StartTime;
            if (Duration < TimeSpan.Zero) Duration = TimeSpan.Zero;

            try
            {
                OnFinished?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Step.OnFinished: {ex}");
            }
        }

        public event Action<Step> OnFinished;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"TraceId: {TraceId}");
            sb.AppendLine($"ParentId: {ParentId}");
            sb.AppendLine($"StepId: {StepId}");
            sb.AppendLine($"Resource: {ResourceName}");
            sb.AppendLine($"Type: {Type}");
            sb.AppendLine($"Start: {StartTime}");
            sb.AppendLine($"Duration: {Duration}");
            sb.AppendLine($"Error: {Error}");
            return sb.ToString();
        }

        public string ToJsonString()
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append($"\"TraceId\":{TraceId},");
            sb.Append($"\"ParentId\":{ParentId},");
            sb.Append($"\"StepId\":{StepId},");
            sb.Append($"\"Resource\":\"{ResourceName}\",");
            sb.Append($"\"Type\":\"{Type}\",");
            sb.Append($"\"Start\":\"{StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz", CultureInfo.InvariantCulture)}\",");
            sb.Append($"\"Duration\":{Duration.TotalMilliseconds},");
            sb.Append($"\"Error\":{Error.ToString().ToLower()}");
            sb.Append("}");
            return sb.ToString();
        }

        public bool IsFinished { get; private set; } = false;

        public PacketType Type { get; set; }
        public ulong StepId { get; }
        public Step? Parent { get; }
        public ulong ParentId => Parent?.StepId ?? 0;
        public long TraceId { get; set; }
        public DateTimeOffset StartTime { get; }
        public TimeSpan Duration { get; set; }
        public string ResourceName { get; set; }

        public bool Error { get; set; }
        public string ErrorMsg { get; set; }
        public string ErrorType { get; set; }
        public string ErrorStack { get; set; }

        public string ActiveStack { get; set; }

        public string MessageHash { get; set; }
        public string MessageDesc { get; set; }
        public string MessageValue { get; set; }

        public string OutHost { get; set; }
        public string OutPort { get; set; }
        public string UserId { get; set; }

        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public string HttpRequestHeaders { get; set; }
        public string HttpRequestRef { get; set; }
        public string Host { get; set; }
        public string HttpRequestIpAddr { get; set; }
        public string HttpRequestUagent { get; set; }

        public string DbType { get; set; }
        public string DbConn { get; set; }
        public string DbUser { get; set; }
        public string DbName { get; set; }
        public string SqlQuery { get; set; }
        public string SqlRows { get; set; }
        public string AspNetRoute { get; set; }
        public string AspNetController { get; set; }
        public string AspNetAction { get; set; }
    }
}

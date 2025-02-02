namespace WhaTap.Trace.Utils
{
#if NET452
    using System;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Messaging;

    public class AsyncLocalCompat<T>
    {
        private readonly string _name = "__WhaTap_Scope_Current__" + Guid.NewGuid();

        public T Get()
        {
            var handle = CallContext.LogicalGetData(_name) as ObjectHandle;

            return handle == null
                       ? default(T)
                       : (T)handle.Unwrap();
        }

        public void Set(T value)
        {
            CallContext.LogicalSetData(_name, new ObjectHandle(value));
        }
    }
#else
    using System.Threading;

    public class AsyncLocalCompat<T>
    {
        private readonly AsyncLocal<T> _asyncLocal = new AsyncLocal<T>();

        public T Get()
        {
            return _asyncLocal.Value;
        }

        public void Set(T value)
        {
            _asyncLocal.Value = value;
        }
    }
#endif
}

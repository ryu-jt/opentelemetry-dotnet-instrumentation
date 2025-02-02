// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace OpenTelemetry.AutoInstrumentation.CallTarget.Handlers;

internal static class BeginMethodSlowHandler<TIntegration, TTarget>
{
    private static readonly InvokeDelegate _invokeDelegate;

    static BeginMethodSlowHandler()
    {
        Logger.Instance.Debug("BeginMethodSlowHandler<TIntegration, TTarget>.cctor()");

        try
        {
            DynamicMethod? dynMethod = IntegrationMapper.CreateSlowBeginMethodDelegate(typeof(TIntegration), typeof(TTarget));
            if (dynMethod != null)
            {
                _invokeDelegate = (InvokeDelegate)dynMethod.CreateDelegate(typeof(InvokeDelegate));
            }
        }
        catch (Exception ex)
        {
            throw new CallTargetInvokerException(ex);
        }
        finally
        {
            if (_invokeDelegate is null)
            {
                _invokeDelegate = (instance, arguments) => CallTargetState.GetDefault();
            }
        }
    }

    internal delegate CallTargetState InvokeDelegate(TTarget instance, object[] arguments);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static CallTargetState Invoke(TTarget instance, object[] arguments)
    {
        Logger.Instance.Debug("BeginMethodSlowHandler<TIntegration, TTarget>.Invoke()");

        // TODO:
        //return new CallTargetState(Activity.Current, _invokeDelegate(instance, arguments));

        return new CallTargetState(null, _invokeDelegate(instance, arguments));
    }
}

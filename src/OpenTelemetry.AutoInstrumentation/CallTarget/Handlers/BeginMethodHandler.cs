// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

#pragma warning disable SA1649 // File name must match first type name

namespace OpenTelemetry.AutoInstrumentation.CallTarget.Handlers;

internal static class BeginMethodHandler<TIntegration, TTarget>
{
    private static readonly InvokeDelegate _invokeDelegate;

    static BeginMethodHandler()
    {
        Logger.Instance.Debug("BeginMethodHandler<TIntegration, TTarget>.cctor()");

        try
        {
            DynamicMethod? dynMethod = IntegrationMapper.CreateBeginMethodDelegate(typeof(TIntegration), typeof(TTarget), Array.Empty<Type>());
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
                _invokeDelegate = instance => CallTargetState.GetDefault();
            }
        }
    }

    internal delegate CallTargetState InvokeDelegate(TTarget instance);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static CallTargetState Invoke(TTarget instance)
    {
        Logger.Instance.Debug("BeginMethodHandler<TIntegration, TTarget>.Invoke()");

        // TODO:
        //return new CallTargetState(Activity.Current, _invokeDelegate(instance));

        return new CallTargetState(null, _invokeDelegate(instance));
    }
}

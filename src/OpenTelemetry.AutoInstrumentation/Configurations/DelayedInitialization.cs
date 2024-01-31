// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.CompilerServices;
using OpenTelemetry.AutoInstrumentation.Loading;
using OpenTelemetry.AutoInstrumentation.Loading.Initializers;
using OpenTelemetry.AutoInstrumentation.Plugins;

namespace OpenTelemetry.AutoInstrumentation.Configurations;

internal static class DelayedInitialization
{
    internal static class Traces
    {
#if NET6_0_OR_GREATER
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddAspNetCore(LazyInstrumentationLoader lazyInstrumentationLoader, PluginManager pluginManager)
        {
            lazyInstrumentationLoader.Add(new AspNetCoreInitializer(pluginManager));
        }
#endif

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddHttpClient(LazyInstrumentationLoader lazyInstrumentationLoader, PluginManager pluginManager)
        {
            new HttpClientInitializer(lazyInstrumentationLoader, pluginManager);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddGrpcClient(LazyInstrumentationLoader lazyInstrumentationLoader, PluginManager pluginManager)
        {
            lazyInstrumentationLoader.Add(new GrpcClientInitializer(pluginManager));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddSqlClient(LazyInstrumentationLoader lazyInstrumentationLoader, PluginManager pluginManager)
        {
            new SqlClientInitializer(lazyInstrumentationLoader, pluginManager);
        }

#if NET6_0_OR_GREATER

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddEntityFrameworkCore(LazyInstrumentationLoader lazyInstrumentationLoader, PluginManager pluginManager)
        {
            lazyInstrumentationLoader.Add(new EntityFrameworkCoreInitializer(pluginManager));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddGraphQL(LazyInstrumentationLoader lazyInstrumentationLoader, PluginManager pluginManager, TracerSettings tracerSettings)
        {
            lazyInstrumentationLoader.Add(new GraphQLInitializer(pluginManager, tracerSettings));
        }
#endif

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddQuartz(LazyInstrumentationLoader lazyInstrumentationLoader, PluginManager pluginManager)
        {
            lazyInstrumentationLoader.Add(new QuartzInitializer(pluginManager));
        }
    }

    internal static class Metrics
    {
#if NET6_0_OR_GREATER
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddAspNetCore(LazyInstrumentationLoader lazyInstrumentationLoader)
        {
            lazyInstrumentationLoader.Add(new AspNetCoreMetricsInitializer());
        }
#endif

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddHttpClient(LazyInstrumentationLoader lazyInstrumentationLoader)
        {
            new HttpClientMetricsInitializer(lazyInstrumentationLoader);
        }
    }
}

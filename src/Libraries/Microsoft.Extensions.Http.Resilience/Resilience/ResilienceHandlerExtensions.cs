// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static Microsoft.Extensions.DependencyInjection.ResilienceHttpClientBuilderExtensions;

namespace Microsoft.Extensions.Http.Resilience.Resilience;
internal class ResilienceHandlerExtensions
{
    public static IHttpStandardResiliencePipelineBuilder AddStandardResiliencePipeline(
        IServiceCollection services,
        string name,
        Action<HttpStandardResilienceOptions>? postAction = null)
    {
        services.TryAddSingleton<IResilienceHandlerFactory, ResilienceHandlerFactory>();
        string optionsName = services.ConfigureStandardResiliencePipelineOptions(name);
        string pipelineName = optionsName;

        if (postAction != null)
        {
            _ = services.Configure<HttpStandardResilienceOptions>(optionsName, postAction);
        }

        services.AddHttpResiliencePipeline(pipelineName, (builder, context) =>
        {
            builder.ConfigureStandardResiliencePipeline(context, optionsName);
        });

        return new HttpStandardResiliencePipelineBuilder(pipelineName, services);
    }
}

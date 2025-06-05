// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static Microsoft.Extensions.DependencyInjection.ResilienceHttpClientBuilderExtensions;

namespace Microsoft.Extensions.Http.Resilience.Resilience;

/// <summary>
/// Extension method to register a standard resilience pipeline handler in the service collection with name.
/// </summary>
public static class ResilienceHandlerExtensions
{
    /// <summary>
    /// Adds a standard resilience pipeline to service collection, with a specified name and optional post-configuration action.
    /// </summary>
    /// <param name="services">IServiceCollection instance.</param>
    /// <param name="name">Name used to identify the resilience pipeline, could be a HttpClient name. It will be used as part of the generated resilience pipeline name.</param>
    /// <param name="postAction">A delegate action to config the options of standard resilience pipeline.</param>
    /// <returns>IHttpStandardResiliencePipelineBuilder instance, which holds the IServiceCollection and the registered resilience pipeline name, usually it's `{name}-standard`.</returns>
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

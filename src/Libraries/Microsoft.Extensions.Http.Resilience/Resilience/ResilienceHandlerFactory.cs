// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;

namespace Microsoft.Extensions.Http.Resilience.Resilience;

/// <summary>
/// Factory for creating resilience handlers.
/// </summary>
internal class ResilienceHandlerFactory : IResilienceHandlerFactory
{
    private IServiceProvider ServiceProvider { get; }

    public ResilienceHandlerFactory(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    /// Creates a resilience handler based on certain resilience pipeline.
    /// </summary>
    /// <param name="pipelineName">Name of HTTP client.</param>
    /// <returns>An instance of ResilienceHandler.</returns>
    public ResilienceHandler CreateResilienceHandler(string pipelineName)
    {
        return CreateResilienceHandlerInternal(CreateResiliencePipelineProvider(ServiceProvider, pipelineName));
    }

    /// <summary>
    /// Internal method to create a resilience pipeline provider.
    /// </summary>
    /// <param name="serviceProvider">IServiceProvider instance.</param>
    /// <param name="pipelineName">The pipeline name.</param>
    /// <returns>Resilience pipeline provider used to create ResilienceHandler.</returns>
    private static Func<HttpRequestMessage, ResiliencePipeline<HttpResponseMessage>> CreateResiliencePipelineProvider(IServiceProvider serviceProvider, string pipelineName)
    {
        ResiliencePipelineProvider<string> resilienceProvider = serviceProvider.GetRequiredService<ResiliencePipelineProvider<string>>();
        ResiliencePipeline<HttpResponseMessage> pipeline = resilienceProvider.GetPipeline<HttpResponseMessage>(pipelineName);
        return (request) => pipeline;
    }

    /// <summary>
    /// Internal method to create a resilience handler given a Polly resilience pipeline provider.
    /// </summary>
    /// <param name="pipelineProvider">Polly resilience pipeline provider.</param>
    /// <returns>ResilienceHandler instance.</returns>
    private static ResilienceHandler CreateResilienceHandlerInternal(Func<HttpRequestMessage, ResiliencePipeline<HttpResponseMessage>> pipelineProvider) => new ResilienceHandler(pipelineProvider);
}

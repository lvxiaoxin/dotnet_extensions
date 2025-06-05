// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.Extensions.Http.Resilience.Resilience;

/// <summary>
/// The interface for ResilienceHandlerFactory.
/// </summary>
public interface IResilienceHandlerFactory
{
    /// <summary>
    /// Creates a resilience handler based on certain resilience pipeline.
    /// </summary>
    /// <param name="pipelineName">Name of HTTP client.</param>
    /// <returns>An instance of ResilienceHandler.</returns>
    ResilienceHandler CreateResilienceHandler(string pipelineName);
}

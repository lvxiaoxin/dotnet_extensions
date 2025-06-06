﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ClientModel;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using OpenAI;

namespace Microsoft.Extensions.AI;

/// <summary>Shared utility methods for integration tests.</summary>
internal static class IntegrationTestHelpers
{
    /// <summary>Gets an <see cref="OpenAIClient"/> to use for testing, or <see langword="null"/> if the associated tests should be disabled.</summary>
    public static OpenAIClient? GetOpenAIClient()
    {
        var configuration = TestRunnerConfiguration.Instance;

        string? apiKey = configuration["OpenAI:Key"];
        string? mode = configuration["OpenAI:Mode"];

        if (string.Equals(mode, "AzureOpenAI", StringComparison.OrdinalIgnoreCase))
        {
            var endpoint = configuration["OpenAI:Endpoint"]
                ?? throw new InvalidOperationException("To use AzureOpenAI, set a value for OpenAI:Endpoint");

            if (apiKey is not null)
            {
                return new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(apiKey));
            }
            else
            {
                return new AzureOpenAIClient(new Uri(endpoint), new DefaultAzureCredential());
            }
        }
        else if (apiKey is not null)
        {
            return new OpenAIClient(apiKey);
        }

        return null;
    }
}

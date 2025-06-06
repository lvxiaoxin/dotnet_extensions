﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace Microsoft.Extensions.AI;

public class TextReasoningContentTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("text")]
    public void Constructor_String_PropsDefault(string? text)
    {
        TextReasoningContent c = new(text);
        Assert.Null(c.RawRepresentation);
        Assert.Null(c.AdditionalProperties);
        Assert.Equal(text ?? string.Empty, c.Text);
    }

    [Fact]
    public void Constructor_PropsRoundtrip()
    {
        TextReasoningContent c = new(null);

        Assert.Null(c.RawRepresentation);
        object raw = new();
        c.RawRepresentation = raw;
        Assert.Same(raw, c.RawRepresentation);

        Assert.Null(c.AdditionalProperties);
        AdditionalPropertiesDictionary props = new() { { "key", "value" } };
        c.AdditionalProperties = props;
        Assert.Same(props, c.AdditionalProperties);

        Assert.Equal(string.Empty, c.Text);
        c.Text = "text";
        Assert.Equal("text", c.Text);
        Assert.Equal("text", c.ToString());

        c.Text = null;
        Assert.Equal(string.Empty, c.Text);
        Assert.Equal(string.Empty, c.ToString());

        c.Text = string.Empty;
        Assert.Equal(string.Empty, c.Text);
        Assert.Equal(string.Empty, c.ToString());
    }
}

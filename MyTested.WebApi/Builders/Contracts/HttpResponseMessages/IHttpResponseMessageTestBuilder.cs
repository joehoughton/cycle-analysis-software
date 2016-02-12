﻿// MyTested.WebApi - ASP.NET Web API Fluent Testing Framework
// Copyright (C) 2015 Ivaylo Kenov.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
namespace MyTested.WebApi.Builders.Contracts.HttpResponseMessages
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using Base;
    using Models;

    /// <summary>
    /// Used for testing HTTP response message results.
    /// </summary>
    public interface IHttpResponseMessageTestBuilder : IBaseTestBuilderWithCaughtException
    {
        /// <summary>
        /// Tests whether certain type of response model is returned from the HTTP response message object content.
        /// </summary>
        /// <typeparam name="TResponseModel">Type of the response model.</typeparam>
        /// <returns>Builder for testing the response model errors.</returns>
        IModelDetailsTestBuilder<TResponseModel> WithResponseModelOfType<TResponseModel>();

        /// <summary>
        /// Tests whether a deeply equal object to the provided one is returned from the HTTP response message object content.
        /// </summary>
        /// <typeparam name="TResponseModel">Type of the response model.</typeparam>
        /// <param name="expectedModel">Expected model to be returned.</param>
        /// <returns>Builder for testing the response model errors.</returns>
        IModelDetailsTestBuilder<TResponseModel> WithResponseModel<TResponseModel>(TResponseModel expectedModel);

        /// <summary>
        /// Tests whether the content of the HTTP response message is of certain type.
        /// </summary>
        /// <typeparam name="TContentType">Type of expected HTTP content.</typeparam>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder WithContentOfType<TContentType>()
            where TContentType : HttpContent;

        /// <summary>
        /// Tests whether the content of the HTTP response message is the provided string.
        /// </summary>
        /// <param name="content">Expected string content.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder WithStringContent(string content);

        /// <summary>
        /// Tests whether the HTTP response message has the provided media type formatter.
        /// </summary>
        /// <param name="mediaTypeFormatter">Expected media type formatter.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder WithMediaTypeFormatter(MediaTypeFormatter mediaTypeFormatter);

        /// <summary>
        /// Tests whether the HTTP response message has the provided type of media type formatter.
        /// </summary>
        /// <typeparam name="TMediaTypeFormatter">Type of MediaTypeFormatter.</typeparam>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder WithMediaTypeFormatterOfType<TMediaTypeFormatter>()
            where TMediaTypeFormatter : MediaTypeFormatter, new();

        /// <summary>
        /// Tests whether the HTTP response message contains the default media type formatter provided by the framework.
        /// </summary>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder WithDefaultMediaTypeFormatter();

        /// <summary>
        /// Tests whether the HTTP response message contains response header with certain name.
        /// </summary>
        /// <param name="name">Name of expected response header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder ContainingHeader(string name);

        /// <summary>
        /// Tests whether the HTTP response message contains response header with certain name and value.
        /// </summary>
        /// <param name="name">Name of expected response header.</param>
        /// <param name="value">Value of expected response header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder ContainingHeader(string name, string value);

        /// <summary>
        /// Tests whether the HTTP response message contains response header with certain name and collection of value.
        /// </summary>
        /// <param name="name">Name of expected response header.</param>
        /// <param name="values">Collection of values in the expected response header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder ContainingHeader(string name, IEnumerable<string> values);

        /// <summary>
        /// Tests whether the HTTP response message contains response headers provided by dictionary.
        /// </summary>
        /// <param name="headers">Dictionary containing response headers.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder ContainingHeaders(IDictionary<string, IEnumerable<string>> headers);

        /// <summary>
        /// Tests whether the HTTP response message contains content header with certain name.
        /// </summary>
        /// <param name="name">Name of expected content header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder ContainingContentHeader(string name);

        /// <summary>
        /// Tests whether the HTTP response message contains content header with certain name and value.
        /// </summary>
        /// <param name="name">Name of expected content header.</param>
        /// <param name="value">Value of expected content header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder ContainingContentHeader(string name, string value);

        /// <summary>
        /// Tests whether the HTTP response message contains content header with certain name and collection of value.
        /// </summary>
        /// <param name="name">Name of expected content header.</param>
        /// <param name="values">Collection of values in the expected content header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder ContainingContentHeader(string name, IEnumerable<string> values);

        /// <summary>
        /// Tests whether the HTTP response message contains content headers provided by dictionary.
        /// </summary>
        /// <param name="headers">Dictionary containing content headers.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder ContainingContentHeaders(IDictionary<string, IEnumerable<string>> headers);

        /// <summary>
        /// Tests whether HTTP response message status code is the same as the provided HttpStatusCode.
        /// </summary>
        /// <param name="statusCode">Expected status code.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder WithStatusCode(HttpStatusCode statusCode);

        /// <summary>
        /// Tests whether HTTP response message version is the same as the provided version as string.
        /// </summary>
        /// <param name="version">Expected version as string.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder WithVersion(string version);

        /// <summary>
        /// Tests whether HTTP response message version is the same as the provided version.
        /// </summary>
        /// <param name="major">Major number in the expected version.</param>
        /// <param name="minor">Minor number in the expected version.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder WithVersion(int major, int minor);

        /// <summary>
        /// Tests whether HTTP response message version is the same as the provided version.
        /// </summary>
        /// <param name="version">Expected version.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder WithVersion(Version version);

        /// <summary>
        /// Tests whether HTTP response message reason phrase is the same as the provided reason phrase as string.
        /// </summary>
        /// <param name="reasonPhrase">Expected reason phrase as string.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder WithReasonPhrase(string reasonPhrase);

        /// <summary>
        /// Tests whether HTTP response message returns success status code between 200 and 299.
        /// </summary>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpResponseMessageTestBuilder WithSuccessStatusCode();

        /// <summary>
        /// Gets the HTTP response message used in the testing.
        /// </summary>
        /// <returns>Instance of HttpResponseMessage.</returns>
        HttpResponseMessage AndProvideTheHttpResponseMessage();
    }
}

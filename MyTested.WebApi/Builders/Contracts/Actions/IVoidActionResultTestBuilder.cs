﻿// MyTested.WebApi - ASP.NET Web API Fluent Testing Framework
// Copyright (C) 2015 Ivaylo Kenov.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
namespace MyTested.WebApi.Builders.Contracts.Actions
{
    using Base;
    using Common;

    /// <summary>
    /// Used for testing void actions.
    /// </summary>
    public interface IVoidActionResultTestBuilder : IBaseTestBuilderWithCaughtException
    {
        /// <summary>
        /// Tests whether action result is void.
        /// </summary>
        /// <returns>Base test builder.</returns>
        IBaseTestBuilderWithCaughtException ShouldReturnEmpty();

        /// <summary>
        /// Used for testing action attributes and model state.
        /// </summary>
        /// <returns>Should have test builder.</returns>
        IShouldHaveTestBuilder<VoidActionResult> ShouldHave();

        /// <summary>
        /// Used for testing whether action throws exception.
        /// </summary>
        /// <returns>Should throw test builder.</returns>
        IShouldThrowTestBuilder ShouldThrow();
    }
}

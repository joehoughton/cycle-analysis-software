﻿// MyTested.WebApi - ASP.NET Web API Fluent Testing Framework
// Copyright (C) 2015 Ivaylo Kenov.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
namespace MyTested.WebApi.Builders.And
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Actions;
    using Base;
    using Contracts.Actions;
    using Contracts.And;

    /// <summary>
    /// Class containing AndAlso() method allowing additional assertions after model state tests.
    /// </summary>
    /// <typeparam name="TActionResult">Result from invoked action in ASP.NET Web API controller.</typeparam>
    public class AndTestBuilder<TActionResult> : BaseTestBuilderWithActionResult<TActionResult>,
        IAndTestBuilder<TActionResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndTestBuilder{TActionResult}" /> class.
        /// </summary>
        /// <param name="controller">Controller on which the action will be tested.</param>
        /// <param name="actionName">Name of the tested action.</param>
        /// <param name="caughtException">Caught exception during the action execution.</param>
        /// <param name="actionResult">Result from the tested action.</param>
        /// <param name="actionAttributes">Collected action attributes from the method call.</param>
        public AndTestBuilder(
            ApiController controller,
            string actionName,
            Exception caughtException,
            TActionResult actionResult,
            IEnumerable<object> actionAttributes = null)
            : base(controller, actionName, caughtException, actionResult, actionAttributes)
        {
        }

        /// <summary>
        /// Method allowing additional assertions after the model state tests.
        /// </summary>
        /// <returns>Builder for testing the action result.</returns>
        public IActionResultTestBuilder<TActionResult> AndAlso()
        {
            return new ActionResultTestBuilder<TActionResult>(
                this.Controller,
                this.ActionName,
                this.CaughtException,
                this.ActionResult,
                this.ActionLevelAttributes);
        }
    }
}

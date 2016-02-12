﻿// MyTested.WebApi - ASP.NET Web API Fluent Testing Framework
// Copyright (C) 2015 Ivaylo Kenov.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
namespace MyTested.WebApi.Builders.Actions.ShouldHave
{
    using System.Linq;
    using Common.Extensions;
    using Contracts.And;
    using Contracts.Models;
    using Exceptions;
    using Models;

    /// <summary>
    /// Class containing methods for testing model state.
    /// </summary>
    /// <typeparam name="TActionResult">Result from invoked action in ASP.NET Web API controller.</typeparam>
    public partial class ShouldHaveTestBuilder<TActionResult>
    {
        /// <summary>
        /// Provides way to continue test case with specific model state errors.
        /// </summary>
        /// <typeparam name="TRequestModel">Request model type to be tested for errors.</typeparam>
        /// <returns>Response model test builder.</returns>
        public IModelErrorTestBuilder<TRequestModel> ModelStateFor<TRequestModel>()
        {
            return new ModelErrorTestBuilder<TRequestModel>(this.Controller, this.ActionName, this.CaughtException);
        }

        /// <summary>
        /// Checks whether the tested action's provided model state is valid.
        /// </summary>
        /// <returns>Test builder with AndAlso method.</returns>
        public IAndTestBuilder<TActionResult> ValidModelState()
        {
            this.CheckValidModelState();
            return this.NewAndTestBuilder();
        }

        /// <summary>
        /// Checks whether the tested action's provided model state is not valid.
        /// </summary>
        /// <param name="withNumberOfErrors">Expected number of errors. If default null is provided, the test builder checks only if any errors are found.</param>
        /// <returns>Test builder with AndAlso method.</returns>
        public IAndTestBuilder<TActionResult> InvalidModelState(int? withNumberOfErrors = null)
        {
            var actualModelStateErrors = this.Controller.ModelState.Values.SelectMany(c => c.Errors).Count();
            if (actualModelStateErrors == 0
                || (withNumberOfErrors != null && actualModelStateErrors != withNumberOfErrors))
            {
                throw new ModelErrorAssertionException(string.Format(
                    "When calling {0} action in {1} expected to have invalid model state{2}, {3}.",
                    this.ActionName,
                    this.Controller.GetName(),
                    withNumberOfErrors == null ? string.Empty : string.Format(" with {0} errors", withNumberOfErrors),
                    withNumberOfErrors == null ? "but was in fact valid" : string.Format("but in fact contained {0}", actualModelStateErrors)));
            }

            return this.NewAndTestBuilder();
        }
    }
}

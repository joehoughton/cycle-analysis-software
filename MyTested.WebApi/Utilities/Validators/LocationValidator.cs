﻿// MyTested.WebApi - ASP.NET Web API Fluent Testing Framework
// Copyright (C) 2015 Ivaylo Kenov.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
namespace MyTested.WebApi.Utilities.Validators
{
    using System;
    using System.Linq;
    using Builders.Uris;

    /// <summary>
    /// Validator class containing URI validation logic.
    /// </summary>
    public static class LocationValidator
    {
        /// <summary>
        /// Validates an URI provided as string.
        /// </summary>
        /// <param name="location">Expected URI as string.</param>
        /// <param name="failedValidationAction">Action to call in case of failed validation.</param>
        /// <returns>Valid Uri created from the provided string.</returns>
        public static Uri ValidateAndGetWellFormedUriString(
            string location,
            Action<string, string, string> failedValidationAction)
        {
            if (!Uri.IsWellFormedUriString(location, UriKind.RelativeOrAbsolute))
            {
                failedValidationAction(
                    "location",
                    "to be URI valid",
                    string.Format("instead received {0}", location));
            }

            return new Uri(location, UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Validates the Uri from action result containing one.
        /// </summary>
        /// <param name="actionResult">Action result with Uri.</param>
        /// <param name="location">Expected Uri.</param>
        /// <param name="failedValidationAction">Action to call in case of failed validation.</param>
        public static void ValidateUri(
            dynamic actionResult,
            Uri location,
            Action<string, string, string> failedValidationAction)
        {
            RuntimeBinderValidator.ValidateBinding(() =>
            {
                var actualLocation = (Uri)actionResult.Location;
                if (location != actualLocation)
                {
                    failedValidationAction(
                        "location",
                        string.Format("to be {0}", location.OriginalString),
                        string.Format("instead received {0}", actualLocation.OriginalString));
                }
            });
        }

        /// <summary>
        /// Validates URI by using UriTestBuilder.
        /// </summary>
        /// <param name="actionResult">Dynamic representation of action result.</param>
        /// <param name="uriTestBuilder">UriTestBuilder for validation.</param>
        /// <param name="failedValidationAction">Action to execute, if the validation fails.</param>
        public static void ValidateLocation(
            dynamic actionResult,
            Action<MockedUriTestBuilder> uriTestBuilder,
            Action<string, string, string> failedValidationAction)
        {
            RuntimeBinderValidator.ValidateBinding(() =>
            {
                var actualUri = actionResult.Location as Uri;

                var newUriTestBuilder = new MockedUriTestBuilder();
                uriTestBuilder(newUriTestBuilder);
                var expectedUri = newUriTestBuilder.GetMockedUri();

                var validations = newUriTestBuilder.GetMockedUriValidations();
                if (validations.Any(v => !v(expectedUri, actualUri)))
                {
                    failedValidationAction(
                        "URI",
                        "to equal the provided one",
                        "was in fact different");
                }
            });
        }
    }
}

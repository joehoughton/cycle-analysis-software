﻿// MyTested.WebApi - ASP.NET Web API Fluent Testing Framework
// Copyright (C) 2015 Ivaylo Kenov.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
namespace MyTested.WebApi.Builders.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Base;
    using Common.Extensions;
    using Utilities;

    /// <summary>
    /// Base class for all attribute test builders.
    /// </summary>
    public abstract class BaseAttributesTestBuilder : BaseTestBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAttributesTestBuilder" /> class.
        /// </summary>
        /// <param name="controller">Controller which will be tested.</param>
        protected BaseAttributesTestBuilder(ApiController controller)
            : base(controller)
        {
            this.Validations = new List<Action<IEnumerable<object>>>();
        }

        /// <summary>
        /// Gets the validation actions for the tested attributes.
        /// </summary>
        /// <value>Collection of validation actions for the attributes.</value>
        protected ICollection<Action<IEnumerable<object>>> Validations { get; private set; }

        internal ICollection<Action<IEnumerable<object>>> GetAttributeValidations()
        {
            return this.Validations;
        }

        /// <summary>
        /// Checks whether the collected attributes contain the provided attribute type.
        /// </summary>
        /// <typeparam name="TAttribute">Type of expected attribute.</typeparam>
        /// <param name="failedValidationAction">Action to execute, if the validation fails.</param>
        protected void ContainingAttributeOfType<TAttribute>(Action<string, string> failedValidationAction)
            where TAttribute : Attribute
        {
            var expectedAttributeType = typeof(TAttribute);
            this.Validations.Add(attrs =>
            {
                if (attrs.All(a => a.GetType() != expectedAttributeType))
                {
                    failedValidationAction(
                        expectedAttributeType.ToFriendlyTypeName(),
                        "in fact such was not found");
                }
            });
        }

        /// <summary>
        /// Checks whether the collected attributes contain RouteAttribute.
        /// </summary>
        /// <param name="template">Expected overridden route template of the action.</param>
        /// <param name="failedValidationAction">Action to execute, if the validation fails.</param>
        /// <param name="withName">Optional expected route name.</param>
        /// <param name="withOrder">Optional expected route order.</param>
        protected void ChangingRouteTo(
            string template,
            Action<string, string> failedValidationAction,
            string withName = null,
            int? withOrder = null)
        {
            this.ContainingAttributeOfType<RouteAttribute>(failedValidationAction);
            this.Validations.Add(attrs =>
            {
                var routeAttribute = this.TryGetAttributeOfType<RouteAttribute>(attrs);
                var actualTemplate = routeAttribute.Template;
                if (template.ToLower() != actualTemplate.ToLower())
                {
                    failedValidationAction(
                                string.Format("{0} with '{1}' template", routeAttribute.GetName(), template),
                                string.Format("in fact found '{0}'", actualTemplate));
                }

                var actualName = routeAttribute.Name;
                if (!string.IsNullOrEmpty(withName) && withName != actualName)
                {
                    failedValidationAction(
                                string.Format("{0} with '{1}' name", routeAttribute.GetName(), withName),
                                string.Format("in fact found '{0}'", actualName));
                }

                var actualOrder = routeAttribute.Order;
                if (withOrder.HasValue && withOrder != actualOrder)
                {
                    failedValidationAction(
                                string.Format("{0} with order of {1}", routeAttribute.GetName(), withOrder),
                                string.Format("in fact found {0}", actualOrder));
                }
            });
        }

        /// <summary>
        /// Checks whether the collected attributes contain AuthorizeAttribute.
        /// </summary>
        /// <param name="failedValidationAction">Action to execute, if the validation fails.</param>
        /// <param name="withAllowedRoles">Optional expected authorized roles.</param>
        /// <param name="withAllowedUsers">Optional expected authorized users.</param>
        protected void RestrictingForAuthorizedRequests(
            Action<string, string> failedValidationAction,
            string withAllowedRoles = null,
            string withAllowedUsers = null)
        {
            this.ContainingAttributeOfType<AuthorizeAttribute>(failedValidationAction);
            var testAllowedUsers = !string.IsNullOrEmpty(withAllowedUsers);
            var testAllowedRoles = !string.IsNullOrEmpty(withAllowedRoles);
            if (testAllowedUsers || testAllowedRoles)
            {
                if (testAllowedRoles)
                {
                    this.Validations.Add(attrs =>
                    {
                        var authorizeAttribute = this.GetAttributeOfType<AuthorizeAttribute>(attrs);
                        var actualRoles = authorizeAttribute.Roles;
                        if (withAllowedRoles != actualRoles)
                        {
                            failedValidationAction(
                                string.Format("{0} with allowed '{1}' roles", authorizeAttribute.GetName(), withAllowedRoles),
                                string.Format("in fact found '{0}'", actualRoles));
                        }
                    });
                }

                if (testAllowedUsers)
                {
                    this.Validations.Add(attrs =>
                    {
                        var authorizeAttribute = this.GetAttributeOfType<AuthorizeAttribute>(attrs);
                        var actualUsers = authorizeAttribute.Users;
                        if (withAllowedUsers != actualUsers)
                        {
                            failedValidationAction(
                                string.Format("{0} with allowed '{1}' users", authorizeAttribute.GetName(), withAllowedUsers),
                                string.Format("in fact found '{0}'", actualUsers));
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Gets an attribute of the given type from the provided collection of objects and throws exception if such is not found.
        /// </summary>
        /// <typeparam name="TAttribute">Type of expected attribute.</typeparam>
        /// <param name="attributes">Collection of attributes.</param>
        /// <returns>The found attribute of the given type.</returns>
        protected TAttribute GetAttributeOfType<TAttribute>(IEnumerable<object> attributes)
            where TAttribute : Attribute
        {
            return (TAttribute)attributes.First(a => a.GetType() == typeof(TAttribute));
        }

        /// <summary>
        /// Gets an attribute of the given type from the provided collection of objects.
        /// </summary>
        /// <typeparam name="TAttribute">Type of expected attribute.</typeparam>
        /// <param name="attributes">Collection of attributes.</param>
        /// <returns>The found attribute of the given type or null, if such attribute is not found.</returns>
        protected TAttribute TryGetAttributeOfType<TAttribute>(IEnumerable<object> attributes)
            where TAttribute : Attribute
        {
            return attributes.FirstOrDefault(a => a.GetType() == typeof(TAttribute)) as TAttribute;
        }
    }
}

﻿namespace People.SelfHostedApi.Tests.ActionSelection.PeopleController
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;
    using NUnit.Framework;
    using SelfHostedApi.Controllers;

    [TestFixture]
    public class PeopleControllerActionSelectionTests
    {
        [Test]
        [TestCase("http://localhost:3001/api/people/", "GET", typeof(PeopleController), "Get")]
        [TestCase("http://localhost:3001/api/people/1", "GET", typeof(PeopleController), "Get")]
        public void CorrectControllerAndActionAreSelected_Test(string url, string method, Type controller, string action)
        {
            // Arrange
            IHttpRouteData routeData;
            var config = new HttpConfiguration();
            WebApiRouteConfig.Register(config);
            config.EnsureInitialized();
            var actionSelector = config.Services.GetActionSelector();
            var controllerSelector = config.Services.GetHttpControllerSelector();
            var request = SetupRequest(url, method, config, out routeData);

            // Act
            var controllerDescriptor = controllerSelector.SelectController(request);
            var context = new HttpControllerContext(config, routeData, request)
            {
                ControllerDescriptor = controllerDescriptor
            };
            var actionDescriptor = actionSelector.SelectAction(context);

            // Assert
            Assert.AreEqual(controller, controllerDescriptor.ControllerType);
            Assert.AreEqual(action, actionDescriptor.ActionName);
        }

        private static HttpRequestMessage SetupRequest(string url, string method, HttpConfiguration config,
            out IHttpRouteData routeData)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), url);
            routeData = config.Routes.GetRouteData(request);
            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            return request;
        }
    }
}
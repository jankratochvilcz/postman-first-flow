using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace Postman.Jan.FirstFlow
{
    public class GetPostmanEmailResponse
    {
        public string Email { get; set; }
        public string ChartHopSearchQuery { get; set; }

        /// <summary>
        /// Hardcoded for testing purposes as I don't have access to make integrations in our production Slack account.
        /// </summary>
        public string SlackEmail { get; set; }
    }

    public class GetPostmanEmailResponseExample : OpenApiExample<GetPostmanEmailResponse>
    {
        public override IOpenApiExample<GetPostmanEmailResponse> Build(
            NamingStrategy namingStrategy = null
        )
        {
            Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "Success Response",
                    new GetPostmanEmailResponse
                    {
                        Email = "jan.kratochvil@postman.com",
                        ChartHopSearchQuery = "jan+kratochvil",
                        SlackEmail = "kratochvil.jan@outlook.com"
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }

    public class GetPostmanEmail
    {
        private readonly ILogger<GetPostmanEmail> _logger;

        public GetPostmanEmail(ILogger<GetPostmanEmail> log)
        {
            _logger = log;
        }

        [FunctionName("GetPostmanEmail")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity(
            "function_key",
            SecuritySchemeType.ApiKey,
            Name = "code",
            In = OpenApiSecurityLocationType.Query
        )]
        [OpenApiParameter(
            name: "firstName",
            In = ParameterLocation.Query,
            Required = true,
            Type = typeof(string),
            Description = "First name"
        )]
        [OpenApiParameter(
            name: "secondName",
            In = ParameterLocation.Query,
            Required = true,
            Type = typeof(string),
            Description = "Second (surname) name"
        )]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(string),
            Description = "The OK response",
            Example = typeof(GetPostmanEmailResponseExample)
        )]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req
        )
        {
            string firstName = req.Query["firstName"];
            string secondName = req.Query["secondName"];

            var response = new GetPostmanEmailResponse
            {
                Email = $"{firstName}.{secondName}@postman.com",
                ChartHopSearchQuery = $"{firstName}+{secondName}",
                SlackEmail = "kratochvil.jan@outlook.com"
            };

            return new OkObjectResult(response);
        }
    }
}

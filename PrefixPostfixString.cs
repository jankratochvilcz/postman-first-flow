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
    public class PrefixPostfixStringResponseExample : OpenApiExample<GetPostmanEmailResponse>
    {
        public override IOpenApiExample<GetPostmanEmailResponse> Build(
            NamingStrategy namingStrategy = null
        )
        {
            Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "Response for prefix: a, string: b, postfix: c",
                    "abc",
                    namingStrategy
                )
            );

            return this;
        }
    }

    public class PrefixPostfixString
    {
        private readonly ILogger<GetPostmanEmail> _logger;

        public PrefixPostfixString(ILogger<GetPostmanEmail> log)
        {
            _logger = log;
        }

        [FunctionName("PrefixPostfixString")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity(
            "function_key",
            SecuritySchemeType.ApiKey,
            Name = "code",
            In = OpenApiSecurityLocationType.Query
        )]
        [OpenApiParameter(
            name: "string",
            In = ParameterLocation.Query,
            Required = true,
            Type = typeof(string),
            Description = "The target string."
        )]
        [OpenApiParameter(
            name: "prefix",
            In = ParameterLocation.Query,
            Required = true,
            Type = typeof(string),
            Description = "The prefix for the target string."
        )]
        [OpenApiParameter(
            name: "postfix",
            In = ParameterLocation.Query,
            Required = true,
            Type = typeof(string),
            Description = "The postfix for the target string"
        )]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "text/plain",
            bodyType: typeof(string),
            Description = "The OK response",
            Example = typeof(GetPostmanEmailResponseExample)
        )]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req
        )
        {
            string prefix = req.Query["prefix"];
            string postfix = req.Query["postfix"];
            string content = req.Query["string"];

            string response = $"{prefix}{content}{postfix}";

            return new OkObjectResult(response);
        }
    }
}

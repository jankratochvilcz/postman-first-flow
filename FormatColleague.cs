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
    public class FormatColleagueResponseExample : OpenApiExample<GetPostmanEmailResponse>
    {
        public override IOpenApiExample<GetPostmanEmailResponse> Build(NamingStrategy namingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Success Response", "xxx", namingStrategy));

        return this;
        }
    }

    public class FormatColleague
    {
        private readonly ILogger<GetPostmanEmail> _logger;

        public FormatColleague(ILogger<GetPostmanEmail> log)
        {
            _logger = log;
        }

        [FunctionName("FormatColleague")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "First name")]
        [OpenApiParameter(name: "surname", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Second (surname) name")]
        [OpenApiParameter(name: "startDate", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Start date. Formatted as string.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response", Example = typeof(GetPostmanEmailResponseExample))]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            string name = req.Query["name"];
            string surname = req.Query["surname"];
            string startDate = req.Query["startDate"];

            string response = $"@{name}.{surname} and you report to the same manager. They joined Postman on {startDate}.";
            return new OkObjectResult(response);
        }
    }
}


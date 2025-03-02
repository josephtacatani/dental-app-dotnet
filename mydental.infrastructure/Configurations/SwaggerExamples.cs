using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace mydental.infrastructure.Configurations
{
    public static class SwaggerExamples
    {
        public static OpenApiSchema PatientSuccessExample()
        {
            return new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["statusCode"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(200) },
                    ["message"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Patients retrieved successfully.") },
                    ["data"] = new OpenApiSchema
                    {
                        Type = "array",
                        Items = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["id"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(1) },
                                ["fullName"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("John Doe") },
                                ["email"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("john@example.com") },
                                ["contactNumber"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("+1234567890") }
                                //birthdate
                                //sex
                                //photo

                            }
                        }
                    }
                }
            };
        }

        public static OpenApiSchema PatientNotFoundExample()
        {
            return new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["statusCode"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(404) },
                    ["message"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("No patients found.") },
                    ["errorMessages"] = new OpenApiSchema
                    {
                        Type = "array",
                        Items = new OpenApiSchema { Type = "string" },
                        Example = new OpenApiArray { new OpenApiString("No patients found.") }
                    },
                    ["data"] = new OpenApiSchema { Type = "object", Nullable = true, Example = null }
                }
            };
        }
        public static OpenApiSchema RefreshTokenExample()
        {
            return new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["refreshToken"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("yourRefreshTokenHere") }
                }
            };
        }

        public static OpenApiSchema LogoutExample()
        {
            return new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["refreshToken"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("yourRefreshTokenHere") }
                }
            };
        }

    }
}

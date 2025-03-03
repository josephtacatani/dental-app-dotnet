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

        /// <summary>
        /// Example response when a service is successfully retrieved.
        /// </summary>
        public static OpenApiSchema ServiceSuccessExample()
        {
            return new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["statusCode"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(200) },
                    ["message"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Service retrieved successfully.") },
                    ["errorMessages"] = new OpenApiSchema
                    {
                        Type = "array",
                        Items = new OpenApiSchema { Type = "string" },
                        Example = new OpenApiArray()
                    },
                    ["data"] = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["id"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(1) },
                            ["serviceName"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Teeth Cleaning") },
                            ["title"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Professional Teeth Cleaning") },
                            ["content"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("A comprehensive cleaning service for your teeth.") },
                            ["photo"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("https://example.com/images/cleaning.jpg") }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Example response when a service is not found.
        /// </summary>
        public static OpenApiSchema ServiceNotFoundExample()
        {
            return new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["statusCode"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(404) },
                    ["message"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("No services found.") },
                    ["errorMessages"] = new OpenApiSchema
                    {
                        Type = "array",
                        Items = new OpenApiSchema { Type = "string" },
                        Example = new OpenApiArray { new OpenApiString("No services found.") }
                    },
                    ["data"] = new OpenApiSchema { Type = "object", Nullable = true, Example = null }
                }
            };
        }

    }
}

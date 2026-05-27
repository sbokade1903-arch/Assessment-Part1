using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using AssessmentApi.Endpoints;
using AssessmentApi.Errors;
using AssessmentApi.OpenApi;
using AssessmentApi.Serialization;
using AssessmentApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.TypeInfoResolverChain.Insert(0, ApiJsonContext.Default);
    });

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, ApiJsonContext.Default);
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                x => x.Key,
                x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

        var response = ApiErrorResponse.Create(
            context.HttpContext,
            StatusCodes.Status400BadRequest,
            "One or more validation errors occurred.",
            errors);

        return new BadRequestObjectResult(response);
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Assessment Part 1 API",
        Version = "v1",
        Description = "Orders, customers, and products API for Part 1 of the assessment."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile), includeControllerXmlComments: true);
    options.CustomSchemaIds(GetSchemaId);
    options.OperationFilter<StandardErrorResponsesOperationFilter>();
    options.DocumentFilter<ExcludeFromClientGenerationDocumentFilter>();
});

builder.Services.AddSingleton<OrderStore>();
builder.Services.AddSingleton<CustomerStore>();
builder.Services.AddSingleton<ProductStore>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var error = ApiErrorResponse.Create(
            context,
            StatusCodes.Status500InternalServerError,
            "An unexpected error occurred.");

        context.Response.StatusCode = error.StatusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(error, ApiJsonContext.Default.ApiErrorResponse);
    });
});

app.UseStatusCodePages(async context =>
{
    var httpContext = context.HttpContext;

    if (httpContext.Response.HasStarted || httpContext.Response.StatusCode < 400)
    {
        return;
    }

    var message = httpContext.Response.StatusCode == StatusCodes.Status404NotFound
        ? "The requested resource was not found."
        : "The request could not be completed.";

    var error = ApiErrorResponse.Create(httpContext, httpContext.Response.StatusCode, message);
    httpContext.Response.ContentType = "application/json";
    await httpContext.Response.WriteAsJsonAsync(error, ApiJsonContext.Default.ApiErrorResponse);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Assessment API v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Assessment API Swagger";
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapProductEndpoints();

app.Run();

static string GetSchemaId(Type type)
{
    if (!type.IsGenericType)
    {
        return type.Name;
    }

    var genericName = type.Name[..type.Name.IndexOf('`')];
    var argumentNames = string.Join("And", type.GetGenericArguments().Select(GetSchemaId));
    return $"{genericName}Of{argumentNames}";
}

/// <summary>
/// Application entry point for the assessment API.
/// </summary>
public partial class Program;

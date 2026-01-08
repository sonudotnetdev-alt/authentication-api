using Authentication.Core.Interface;
using Authentication.Infrastructure;
using Authentication.Infrastructure.Repository;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", pol =>
    {
        pol.AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:4200");
    });
});

builder.Services.AddControllers();
//builder.Services.AddMvc().AddXmlSerializerFormatters();
//builder.Services.AddControllers()
//                .AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Jwt Auth Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme,
        }
    };
    options.AddSecurityDefinition("Bearer",securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
    options.AddSecurityRequirement(securityRequirement);

    //options.SwaggerDoc("1.0", new OpenApiInfo { Title = "My API", Version = "1.0" });

    //// Define a Swagger document for API version 2.0
    //options.SwaggerDoc("2.0", new OpenApiInfo { Title = "My API", Version = "2.0" });
    //// Resolving Conflicts
    //// This resolves conflicts when multiple actions match the same route and HTTP method
    //// by selecting the first action encountered
    //options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    //// Doc Inclusion Predicate
    //// Define a predicate function to determine which API Endpoints should be included
    //options.DocInclusionPredicate((version, apiDesc) =>
    //{

    //    // Attempt to get the MethodInfo for the API Endpoint
    //    if (!apiDesc.TryGetMethodInfo(out MethodInfo method))
    //        return false; //If the MethodInfo for the API description cannot be retrieved, the endpoint is excluded.
    //                      // Extracts the versions specified by [ApiVersion] attributes at the method level.
    //                      // Ensures that method-level versioning is considered when deciding which endpoints to include in the Swagger documentation
    //    var methodVersions = method.GetCustomAttributes(true)
    //        .OfType<ApiVersionAttribute>()
    //        .SelectMany(attr => attr.Versions);
    //    // Extracts the versions specified by [ApiVersion] attributes at the controller level.
    //    // Ensures that controller-level versioning is considered, allowing for endpoints that might be versioned at the controller level.
    //    var controllerVersions = method.DeclaringType?
    //        .GetCustomAttributes(true)
    //        .OfType<ApiVersionAttribute>()
    //        .SelectMany(attr => attr.Versions);
    //    // Combining Versions
    //    // Combines the versions extracted from both the method and controller levels to create a unified set of versions.
    //    // Ensures that all relevant versions are considered, avoiding duplication by using Distinct()
    //    var allVersions = methodVersions.Union(controllerVersions).Distinct();
    //    // Checks if any of the combined versions match the version specified in the Swagger document
    //    // This determines if the API Endpoint should be included in the current Swagger document based on the version.
    //    return allVersions.Any(v => v.ToString() == version);
    //});
});

builder.Services.InfrastructureConfiguration(builder.Configuration);
builder.Services.AddScoped<IOrderService,OrderServices>();
builder.Services.AddMemoryCache();
builder.Services.AddLazyCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    //This property is set to specify the connection string for Redis
    //The value is fetched from the application's configuration system, i.e., appsettings.json file
    options.Configuration = builder.Configuration["RedisCacheOptions:Configuration"];
    //This property helps in setting a logical name for the Redis cache instance. 
    //The value is also fetched from the appsettings.json file
    options.InstanceName = builder.Configuration["RedisCacheOptions:InstanceName"];
});
//Setting Api version
builder.Services.AddApiVersioning(options =>
{
    // Specify the default API version
    options.DefaultApiVersion = new ApiVersion(1, 0);

    // If the client does not specify a version, use the default version
    options.AssumeDefaultVersionWhenUnspecified = true;

    // Advertise the API versions supported for the particular endpoint
    options.ReportApiVersions = true;

    // Read the version number from the query string parameter "version"
    options.ApiVersionReader = new QueryStringApiVersionReader("version");

    // Read the version number from the headers
    options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");

    // Read version info from URL segment
    options.ApiVersionReader = new UrlSegmentApiVersionReader();

    // Read the version number from the Media Type Header
    options.ApiVersionReader = new MediaTypeApiVersionReader();
});

//Setting api rate limiter
builder.Services.AddRateLimiter(rateLimiterOption =>
{
    rateLimiterOption.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    rateLimiterOption.AddFixedWindowLimiter("fixed", option =>
    {
        option.AutoReplenishment = true;
        option.PermitLimit = 3;
        option.Window = TimeSpan.FromSeconds(20);
        //option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        option.QueueLimit = 0;
    });

    rateLimiterOption.AddSlidingWindowLimiter("sliding", option =>
    {
        option.PermitLimit = 10;
        option.Window =TimeSpan.FromSeconds(10);
        option.SegmentsPerWindow = 2;
        option.QueueProcessingOrder= QueueProcessingOrder.OldestFirst;
        option.QueueLimit = 5;
    });

    rateLimiterOption.AddTokenBucketLimiter("token", option =>
    {
        option.TokenLimit = 100;
        option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        option.QueueLimit = 5;
        option.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        option.QueueLimit = 1;
        option.TokensPerPeriod = 20;
        option.AutoReplenishment = true;
    });

    rateLimiterOption.AddConcurrencyLimiter("concurrency", option =>
    {
        option.PermitLimit = 10;
        option.QueueLimit= 5;
        option.QueueProcessingOrder=QueueProcessingOrder.OldestFirst;

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(options =>
    //{
    //    // Define the Swagger endpoints for each version
    //    options.SwaggerEndpoint("/swagger/1.0/swagger.json", "My API V1");
    //    options.SwaggerEndpoint("/swagger/2.0/swagger.json", "My API V2");
    //});
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseRateLimiter();
InfrastructureRegistration.infrastructureConfigMiddleware(app);
app.Run();

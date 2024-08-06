using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using FluentValidation;
using MediatR;
using ESLA.Microservice.Quiz.Core.Request.Validations;
using ESLA.Microservice.Quiz.Core.Request.Pipelines;

namespace ESLA.Microservice.Quiz
{
    public static class Startup
    {
        public class EnumSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema model, SchemaFilterContext context)
            {
                if (context.Type.IsEnum)
                {
                    model.Enum.Clear();
                    foreach (string enumName in Enum.GetNames(context.Type))
                    {
                        var memberInfo = context.Type.GetMember(enumName).FirstOrDefault(m => m.DeclaringType == context.Type);
                        EnumMemberAttribute? enumMemberAttribute = memberInfo?.GetCustomAttributes(typeof(EnumMemberAttribute), false).OfType<EnumMemberAttribute>().FirstOrDefault();
                        string label = enumMemberAttribute == null || string.IsNullOrWhiteSpace(enumMemberAttribute.Value)
                         ? enumName
                         : enumMemberAttribute.Value;
                        model.Enum.Add(new OpenApiString(label));
                    }
                }
            }
        }
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            return services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
             {
                 c.SchemaFilter<EnumSchemaFilter>();
                 c.MapType<DateOnly>(() => new OpenApiSchema
                 {
                     Type = "string",
                     Format = "date",
                     Example = new OpenApiString("2023-01-01")
                 });
                 c.MapType<Ulid>(() => new OpenApiSchema
                 {
                     Type = "string",
                     Format = "string",
                     Example = new OpenApiString("01HEXY08C8AADWQA6ZCE3YT3ZW")
                 });
                 c.MapType<TimeOnly>(() => new OpenApiSchema
                 {
                     Type = "string",
                     Format = "TimeOnlyFormat",
                     Example = new OpenApiString("09:00:00")
                 });
                 c.CustomSchemaIds(x =>
                  {
                      var modelName = x.GetCustomAttributes<DisplayNameAttribute>()
                      .SingleOrDefault()?
                      .DisplayName;

                      return string.IsNullOrEmpty(modelName) ? x.Name : modelName;
                  });

                 c.SwaggerDoc("v1", new OpenApiInfo { Title = "ESLA.Microservice.Quiz", Version = "v1" });
             }

             );
        }
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
            services.AddMediatR(cfg =>
                       {
                           cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly);
                       });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));


            return services;
        }
    }
}
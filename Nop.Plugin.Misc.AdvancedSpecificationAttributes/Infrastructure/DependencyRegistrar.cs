using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Factories;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services;

namespace Nop.Plugin.Misc.CategorySpecAttribute.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="appSettings">App settings</param>
        public virtual void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            services.AddScoped<ICategorySpecificationAttributeService, CategorySpecificationAttributeService>();
            services.AddScoped<ICustomSpecificationAttributeService, CustomSpecificationAttributeService>();
            services.AddScoped<ICategorySpecificationAttributeGroupModelFactory, CategorySpecificationAttributeGroupModelFactory>();
            services.AddScoped<ICustomSpecificationAttributeModelFactory, CustomSpecificationAttributeModelFactory>();
            services.AddScoped<ICustomSpecificationAttributeParser, CustomSpecificationAttributeParser>();
        }

        public int Order => 1;
    }
}

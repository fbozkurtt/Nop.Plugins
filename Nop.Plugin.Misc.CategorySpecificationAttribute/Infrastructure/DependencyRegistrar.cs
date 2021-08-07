using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Misc.CategorySpecAttribute.Services;
using Nop.Plugin.Misc.CategorySpecificationAttribute.Factories;

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
            services.AddScoped<ISpecificationAttributeGroupCategoryModelFactory, SpecificationAttributeGroupCategoryModelFactory>();
        }

        public int Order => 1;
    }
}

using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Infrastructure.Mapper
{
    class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor

        public MapperConfiguration()
        {
            CreateMap<CustomSpecificationAttribute, CustomSpecificationAttributeModel>()
               .ForMember(model => model.AttributeControlTypeName, options => options.Ignore())
               .ForMember(model => model.AttributeFilterTypeName, options => options.Ignore())
               .ForMember(model => model.ConditionAllowed, options => options.Ignore())
               .ForMember(model => model.ConditionModel, options => options.Ignore());
            CreateMap<CustomSpecificationAttributeModel, CustomSpecificationAttribute>()
                .ForMember(entity => entity.AttributeControlType, options => options.Ignore())
                .ForMember(entity => entity.AttributeFilterType, options => options.Ignore())
                .ForMember(entity => entity.ConditionAttributeXml, options => options.Ignore());
        }

        #endregion

        #region Properties

        public int Order => 1;

        #endregion
    }
}

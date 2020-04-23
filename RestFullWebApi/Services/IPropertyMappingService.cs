using System.Collections.Generic;

namespace RestFullWebApi.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();

        bool ValidMappingExistFor<TSource, TDestination>(string fields);
    }
}
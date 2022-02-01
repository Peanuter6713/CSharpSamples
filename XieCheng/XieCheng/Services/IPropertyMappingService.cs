using System.Collections.Generic;

namespace XieCheng.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool IsMappingExists<TSource, TDestination>(string fileds);
        bool IsPropertiesExists<T>(string fields);
    }
}
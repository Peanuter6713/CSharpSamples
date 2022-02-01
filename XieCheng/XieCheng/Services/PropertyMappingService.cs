using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XieCheng.DtoS;
using XieCheng.Models;

namespace XieCheng.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _touristRoutePropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id", new PropertyMappingValue(new List<string>() { "Id"}) },
            {"Title", new PropertyMappingValue(new List<string>() { "Title"}) },
            {"Rating", new PropertyMappingValue(new List<string>() { "Rating"}) },
            {"OriginalPrice", new PropertyMappingValue(new List<string>() { "OriginalPrice"}) }
        };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<TouristRouteDto, TouristRoute>(_touristRoutePropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            // 获得匹配映射的对象
            var mathMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (mathMapping.Count() == 1)
            {
                return mathMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool IsMappingExists<TSource, TDestination>(string fileds)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fileds))
            {
                return true;
            }

            // 逗号来分割字段字符串
            var fieldsAfterSplit = fileds.Split(",");

            foreach (var field in fieldsAfterSplit)
            {
                // 去掉多余的空格
                var trimmedField = field.Trim();
                // 获得属性名称字符串
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsPropertiesExists<T>(string fields)
        {
            if (string.IsNullOrEmpty(fields))
            {
                return true;
            }

            // 使用逗号来分隔字符串
            var fieldsAfterSplit = fields.Split(',');
            foreach(var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // 如果在T中没有找到对应属性
                if (propertyInfo == null)
                {
                    return false;
                }
            }

            return true;
        }

    }
}

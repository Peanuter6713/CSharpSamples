using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using XieCheng.Services;

namespace XieCheng.Helper
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// ApplySort 是对IQueryable的拓展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> source,
            string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException("mappingDictionary");
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByString = string.Empty;
            var orderByAfterSplit = orderBy.Split(',');

            foreach (var order in orderByAfterSplit)
            {
                var trimOrder = order.Trim();

                // 通过字符串判断升序还是降序
                var orderDescending = trimOrder.EndsWith("desc");

                // 删除升序或降序字符串 "asc" or "des" 来获得属性的名称
                var indexOfFirstSpace = trimOrder.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1
                    ? trimOrder : trimOrder.Remove(indexOfFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentNullException($"Key mapping for {propertyName} is missing");
                }

                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    // 给 IQueryable 添加排序字符串
                    orderByString = orderByString + (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ",")
                        + destinationProperty
                        + (orderDescending ? " descending" : " ascending"); 
                }
            }

            return source.OrderBy(orderByString);
        }
    }
}

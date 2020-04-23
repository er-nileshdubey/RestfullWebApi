using RestFullWebApi.Entities;
using RestFullWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFullWebApi.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> authorPropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id", new PropertyMappingValue(new List<string>() { "Id" })},
            {"MainCategory", new PropertyMappingValue(new List<string>() { "MainCategory" })},
            {"Age", new PropertyMappingValue(new List<string>() { "DateOfBirth" }, true)},
            {"Name", new PropertyMappingValue(new List<string>() { "FirstName", "LastName" })}
        };

        public IList<IPropertyMapping> _propertyMapping = new List
            <IPropertyMapping>();

        public bool ValidMappingExistFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
                return true;

            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                var trimmedFiled = field.Trim();
                var indexOfFirstSpace = trimmedFiled.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedFiled : trimmedFiled.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                    return false;
            }
            return true;
        }
        public PropertyMappingService()
        {
            _propertyMapping.Add(new PropertyMapping<AuthorsDto, Author>(authorPropertyMapping));
        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMapping.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
                return matchingMapping.First()._mappingDictionary;

            throw new Exception($"Cannot find exact property mapping instance " + $"for <{typeof(TSource)}, {typeof(TDestination)}>");
        }
    }
}

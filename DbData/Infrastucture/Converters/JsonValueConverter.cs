using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Hiroshima.DbData.Infrastucture.Converters
{
    class JsonValueConverter<TEntity> : ValueConverter<TEntity, string>
    {
        public JsonValueConverter(JsonSerializerSettings serializerSettings = null,
            ConverterMappingHints mappingHints = null)
            : base(model => JsonConvert.SerializeObject(model, serializerSettings),
                value => JsonConvert.DeserializeObject<TEntity>(value, serializerSettings),
                mappingHints)
        {}

        public static ValueConverter Default { get; } =
            new JsonValueConverter<TEntity>(null, null);

        public static ValueConverterInfo DefaultInfo { get; } =
            new ValueConverterInfo(typeof(TEntity),
                typeof(string),
                i => new JsonValueConverter<TEntity>(null, i.MappingHints));
    }
}

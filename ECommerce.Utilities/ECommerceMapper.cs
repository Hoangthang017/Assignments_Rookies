using AutoMapper;

namespace ECommerce.Utilities
{
    public static class ECommerceMapper
    {
        public static T Map<T>(IMapper mapper, params object[] sources) where T : class
        {
            if (!sources.Any())
            {
                return default(T);
            }

            var initialSource = sources[0];

            var mappingResult = mapper.Map<T>(initialSource);

            for (int i = 1; i < sources.Length; i++)
            {
                mappingResult = mapper.Map(sources[i], mappingResult);
            }
            return mappingResult;
        }
    }
}
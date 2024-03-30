namespace Otus_Delegates
{
    public static class EnumerableExtensions
    {
        public static T GetMax<T>(this IEnumerable<T> collection, Func<T, float> convertToNumber)
        {
            if (collection == null || !collection.Any())
                throw new ArgumentException("Collection must not be null or empty");

            T maxElement = default(T)!;
            float maxValue = float.MinValue;

            foreach (var item in collection)
            {
                float convertedValue = convertToNumber(item);
                if (convertedValue > maxValue)
                {
                    maxValue = convertedValue;
                    maxElement = item;
                }
            }

            return maxElement;
        }
    }
}
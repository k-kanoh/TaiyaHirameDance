namespace TaiyaHirameDance.ToolBox
{
    public static class Extension
    {
        public static void Add<TKey, TValue>(this IList<KeyValuePair<TKey, TValue>> list, TKey key, TValue value)
        {
            list.Add(new KeyValuePair<TKey, TValue>(key, value));
        }
    }
}

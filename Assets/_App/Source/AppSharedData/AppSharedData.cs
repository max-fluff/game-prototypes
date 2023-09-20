using System.Collections.Generic;

namespace MaxFluff.Prototypes
{
    public sealed class AppSharedData
    {
        private readonly Dictionary<string, ISharedData> _data = new Dictionary<string, ISharedData>();

        public void Add(ISharedData sharedData)
        {
            var key = sharedData.GetType().ToString();

            _data[key] = sharedData;
        }

        public void Add(params ISharedData[] sharedData)
        {
            foreach (var data in sharedData)
            {
                var key = data.GetType().ToString();

                _data[key] = data;
            }
        }

        public bool TryGet<TData>(out TData data, bool shouldRemove = false) where TData : ISharedData
        {
            data = default;
            var key = typeof(TData).ToString();

            if (!_data.TryGetValue(key, out var sharedData))
                return false;

            data = (TData)sharedData;
            if (shouldRemove)
                _data.Remove(key);

            return true;
        }

        public TData Get<TData>(bool shouldRemove = false) where TData : ISharedData
        {
            TryGet(out TData data, shouldRemove);
            return data;
        }

        public void Remove<TData>() where TData : ISharedData
        {
            var key = typeof(TData).ToString();
            _data.Remove(key);
        }
    }
}
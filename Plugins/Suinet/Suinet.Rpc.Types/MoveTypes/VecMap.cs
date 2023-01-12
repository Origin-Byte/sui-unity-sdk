using System.Collections.Generic;

namespace Suinet.Rpc.Types.MoveTypes
{
    public class VecMap<TKey, TValue>
    {
        public string Type { get; set; }

        public VecMapFields<TKey, TValue> Fields { get; set; }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            var result = new Dictionary<TKey, TValue>();
            foreach(var entry in Fields.Contents)
            {
                result.Add(entry.Fields.Key, entry.Fields.Value);
            }
            return result;
        }
    }

    public class VecMapFields<TKey, TValue>
    {
        public List<VecMapEntry<TKey, TValue>> Contents { get; set; }
    }

    public class VecMapEntry<TKey, TValue>
    {
        public string Type { get; set; }

        public VecMapEntryFields<TKey, TValue> Fields { get; set; }
    }

    public class VecMapEntryFields<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

    }
}

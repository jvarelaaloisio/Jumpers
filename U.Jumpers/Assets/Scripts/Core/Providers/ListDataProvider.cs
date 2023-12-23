using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Providers
{
    public abstract class ListDataProvider<T> : DataProvider<List<T>>
    {
        public override List<T> Value { get; set; } = new();

        public override string GetLogData()
            => $"elements: [{Value.Aggregate(string.Empty, (accum, current) => $"{accum}, {current}[{current.GetType().Name}]")}]";
    }
}
using System;
using System.Collections;
using System.Collections.Generic;

// TODO:

namespace WhaTap.Trace.Utils
{
    public static class DictionaryUtils
    {
        public static object GetValueOrDefault(IDictionary dictionary, object key)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            return TryGetValue(dictionary, key, out var value)
                       ? value
                       : default;
        }

        private static bool TryGetValue(IDictionary dictionary, object key, out object value)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            value = null;
            object valueObj;

            // per its contract, IDictionary.Item[] should return null instead of throwing an exception
            // if a key is not found, but let's use try/catch to be defensive against misbehaving implementations
            try
            {
                valueObj = dictionary[key];
            }
            catch
            {
                return false;
            }

            if (valueObj is IConvertible convertible)
            {
                value = convertible.ToType(valueObj.GetType(), provider: null);
                return true;
            }
            else if (valueObj != null)
            {
                value = valueObj;
                return true;
            }

            return false;

            // switch (valueObj)
            // {
            //     case TValue valueTyped:
            //         value = valueTyped;
            //         return true;
            //     case IConvertible convertible:
            //         value = (TValue)convertible.ToType(typeof(TValue), provider: null);
            //         return true;
            //     default:
            //         value = default;
            //         return false;
            // }
        }
    }
}

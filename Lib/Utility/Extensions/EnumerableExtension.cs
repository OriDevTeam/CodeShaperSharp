// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces


// Library Namespaces


namespace Lib.Utility.Extensions
{
    public static class EnumerableExtension
    {
        public static T Next<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            bool flag = false;

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (flag) return enumerator.Current;

                    if (predicate(enumerator.Current))
                    {
                        flag = true;
                    }
                }
            }
            return default(T);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    static partial class Permutation
    {
        public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements)
        {
            int c = elements.Count();
            for (int i = 0; i < c; i++)
            {
                foreach (IEnumerable<T> r in elements.DifferentCombinations(i)) {
                    yield return r;
                }
            }
            yield return elements;
        }
    }
}

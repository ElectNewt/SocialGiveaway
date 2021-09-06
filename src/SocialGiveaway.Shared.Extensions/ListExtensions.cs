using System.Collections.Generic;
using System.Linq;

namespace SocialGiveaway.Shared.Extensions
{
    public static class ListExtensions
    {
        public static List<T> GetCommonItems<T>(this List<List<T>> lists)
        {
            HashSet<T> hs = new HashSet<T>(lists.First());
            for (int i = 1; i < lists.Count; i++)
                hs.IntersectWith(lists[i]);
            return hs.ToList();
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidnightTests
{
    static class CollectionAssertHelper
    {
        public static bool AreEqualContent<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            var benum = b.GetEnumerator();

            foreach (var item in a)
            {
                if (benum.MoveNext())
                {
                    Assert.AreEqual(item, benum.Current); 
                }
                else
                {
                    Assert.Fail("unequal size");
                    return false;
                }
            }

            return true;

        }
    }
}

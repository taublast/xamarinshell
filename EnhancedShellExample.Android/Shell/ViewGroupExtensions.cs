using Android.Views;
using System.Collections.Generic;
using System.Linq;

namespace AppoMobi.Framework.Droid.Renderers
{
    public static class ViewGroupExtensions
    {

        /// <summary>
        /// The GetChildrenOfType.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="self">The self<see cref="ViewGroup"/>.</param>
        /// <returns>The <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<T> GetChildrenOfType<T>(this ViewGroup self) where T : View
        {
            for (var i = 0; i < self.ChildCount; i++)
            {
                View child = self.GetChildAt(i);
                var typedChild = child as T;
                if (typedChild != null)
                    yield return typedChild;

                if (child is ViewGroup)
                {
                    IEnumerable<T> myChildren = (child as ViewGroup).GetChildrenOfType<T>();
                    foreach (T nextChild in myChildren.ToList())
                        yield return nextChild;
                }
            }
        }


    }
}
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    internal static partial class CustomExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> source, IEnumerable<string> navigationPropertyPaths)
            where T : class
        {
            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }

        public static IEnumerable<string> GetIncludePaths(this DbContext context, Type clrEntityType)
        {
            IEntityType entityType = context.Model.FindEntityType(clrEntityType);
            HashSet<INavigation> includedNavigations = new HashSet<INavigation>();
            Stack<IEnumerator<INavigation>> stack = new Stack<IEnumerator<INavigation>>();
            while (true)
            {
                List<INavigation> entityNavigations = new List<INavigation>();
                foreach (INavigation navigation in entityType.GetNavigations())
                {
                    if (includedNavigations.Add(navigation))
                    {
                        entityNavigations.Add(navigation);
                    }
                }
                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                    {
                        yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
                    }
                }
                else
                {
                    foreach (INavigation navigation in entityNavigations)
                    {
                        INavigation inverseNavigation = navigation.Inverse;
                        if (inverseNavigation != null)
                        {
                            includedNavigations.Add(inverseNavigation);
                        }
                    }
                    stack.Push(entityNavigations.GetEnumerator());
                }
                while (stack.Count > 0 && !stack.Peek().MoveNext())
                {
                    stack.Pop();
                }

                if (stack.Count == 0)
                {
                    break;
                }

                entityType = stack.Peek().Current.TargetEntityType;
            }
        }
    }
}
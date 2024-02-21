using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public static class HtmlElementExtensions
    {
        public static IEnumerable<HtmlElement> FindElementsBySelector(this HtmlElement element, Selector selector)
        {
            var results = new HashSet<HtmlElement>();
            FindElementsRecursive(element, selector, results);
            return results;
        }




        private static void FindElementsRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
        {
            var descendants = element.Descendants();

            foreach (var descendant in descendants)
            {

                if (selector.Match(descendant))
                {
                   
                    if (selector.Child != null)
                    {
                        
                        FindElementsRecursive(descendant, selector.Child, result);

                    }
                    else
                    {

                       
                        var Ancestors = descendant.Ancestors();
                        foreach (var ancestor in Ancestors)
                        {
                            if (selector.Parent == null)
                                break;
                            if (selector.Match(ancestor))
                            {
                                selector = selector.Parent;
                            }
                        }
                        //only in case that the selector (with all his roots )was ok
                        if (selector.Parent== null)
                        {
                            result.Add(descendant);
                        }


                    }
                }
            }
        }


    }
}
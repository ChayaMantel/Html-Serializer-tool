using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }

        public List<string> Classes = new List<string>();
        public Selector Child { get; set; }
        public Selector Parent { get; set; }

        public bool Match(HtmlElement element)
        {
            // Check if tag name matches
            if (!string.IsNullOrEmpty(TagName) && element.Name != TagName)
            {
                return false;
            }

            // Check if ID matches
            if (!string.IsNullOrEmpty(Id) && element.Id != Id)
            {
                return false;
            }

            // Check if all classes match
            if (Classes.Any() && !Classes.All(c => element.Classes.Contains(c)))
            {
                return false;
            }

           // All conditions met, return true
            return true;
        }

        public static Selector FromQueryString(string queryString)
        {
            // Split the query string into parts based on spaces

            string[] queryParts = Regex.Split(queryString, " ");

            Selector rootSelector = null;
            Selector currentSelector = new Selector();

            foreach (var item in queryParts)
            {

                var newSelector = new Selector();
                // Add it as a child of the current selector
                currentSelector.Child = (newSelector);
                newSelector.Parent=(currentSelector);
                // Update the current selector to point to the new one
                currentSelector = newSelector;
                if(rootSelector == null)
                {
                    rootSelector=newSelector;
                    rootSelector.Parent = null;
                }
               
                string[] parts = Regex.Split(item, "(?=[#.])").Where(S=>S.Length>0).ToArray();
                foreach(var part in parts)
                {
                    if (part.StartsWith("#"))
                    {
                       
                       
                       currentSelector.Id = part.Substring(1, part.Length - 1);
                       
                    }
                    else if (part.StartsWith("."))
                    {
                        // Part starts with ., update Classes property
                        currentSelector.Classes.Add(part.Substring(1));
                    }
                    else
                    {
                        // Part doesn't start with # or ., check if it's a valid HTML tag name
                        if (HtmlHelper.Instance.AllTags.Contains(part)|| HtmlHelper.Instance.SelfClosingTags.Contains(part.Substring(1, part.Length - 1)))
                        {
                            currentSelector.TagName = part;
                        }
                    }
                }
                


            }

            return rootSelector;
        }


    }
}

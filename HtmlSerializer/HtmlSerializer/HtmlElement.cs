using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            List<HtmlElement> result = new List<HtmlElement>(); 
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
           
            while (queue.Count > 0)
            {
             
                HtmlElement current = queue.Dequeue();
                yield return current;

                foreach (HtmlElement child in current.Children)
                {   
                    queue.Enqueue(child);
                }
            }
          
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;
            while (current.Parent != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
        public IEnumerable<HtmlElement> Query(string selector)
        {
            Selector parsedSelector = Selector.FromQueryString(selector);
            if (parsedSelector == null)
            {
                throw new ArgumentException("Invalid selector");
            }

            // Search for elements based on the parsed selector
            return QueryElement(this, parsedSelector);
        }

        private IEnumerable<HtmlElement> QueryElement(HtmlElement element, Selector selector)
        {

            return HtmlElementExtensions.FindElementsBySelector(element, selector);
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Id: {Id}, ");
            sb.Append($"Name: {Name}, ");
            sb.Append("Attributes: [");
            sb.Append(string.Join(", ", Attributes.Select(a => $"'{a}'")));
            sb.Append("], ");
            sb.Append("Classes: [");
            sb.Append(string.Join(", ", Classes.Select(c => $"'{c}'")));
            sb.Append("], ");
           
            
            return sb.ToString();
        }
    }
}

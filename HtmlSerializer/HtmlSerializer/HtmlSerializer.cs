using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class Serializer
    {
        public Serializer()
        {

        }
        public async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

        public HtmlElement Serialize(string html)
        {
            var cleanHtml = Regex.Replace(html, @"<!--[\s\S]*?-->", ""); // Remove comments, including multiline comments
            cleanHtml = Regex.Replace(cleanHtml, @"\s*\n\s*|\s{2,}", " "); // Replace multiple spaces with a single space

            var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(x => x.Length > 0);
          
            HtmlElement rootElement = null;

            HtmlElement currentElement = new HtmlElement();

            foreach (var tag in htmlLines)
            {
                string[] words = tag.Split(' ');


                if (tag == "html/")
                {
                    // End of HTML reached
                    break;
                }
                else if (tag.StartsWith("/"))
                {

                    if (currentElement != null && HtmlHelper.Instance.AllTags.Contains(words[0].Substring(1)))
                    {
                        currentElement = currentElement.Parent;
                        continue;

                    }

                }

                else if (HtmlHelper.Instance.AllTags.Contains(words[0], StringComparer.OrdinalIgnoreCase) || HtmlHelper.Instance.SelfClosingTags.Contains(words[0]))
                {



                    if (words.Length > 1 && words[1].StartsWith('{'))
                    {
                        currentElement!.InnerHtml = tag;
                    }
                    else
                    {
                        var newElement = new HtmlElement { Name = words[0] };
                        currentElement.Children.Add(newElement);
                        newElement.Parent = currentElement;
                        //יוצר אובייקט Regex (ביטוי רגולרי) שמטרתו לחלץ תכונות (attributes) מאלמנט ה-HTML. הביטוי הרגולרי מזהה זוגות של שם תכונה וערך תכונה בתוך התגית.
                        var attributesRegex = new Regex(@"(\w+)(?:=""([^""]*)""|$)");
                        var attributesMatch = attributesRegex.Matches(tag);
                        foreach (Match attributeMatch in attributesMatch)
                        {
                            var attributeName = attributeMatch.Groups[1].Value;
                            var attributeValue = attributeMatch.Groups[2].Value;

                            if (attributeName.ToLower() == "class")
                            {
                                // Split the class attribute into parts
                                newElement.Classes.AddRange(attributeValue.Split(' '));
                            }
                            else if (attributeName.ToLower() == "id")
                            {
                                newElement.Id = attributeValue;
                            }
                            else
                            {
                                newElement.Attributes.Add(attributeValue);
                            }


                        }

                        bool isSelfClosing = tag.EndsWith("/>") || (HtmlHelper.Instance.SelfClosingTags.Contains(words[0]));
                        if (rootElement == null)
                        {

                            rootElement = newElement;
                        }

                        if (!isSelfClosing)
                        {
                            currentElement = newElement;
                        }
                    }

                }

                else
                {
                   if (currentElement!=null)
                        currentElement.InnerHtml = tag;
                }
            }
            return rootElement;
        }


    }

}


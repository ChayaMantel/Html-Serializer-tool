using HtmlSerializer;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

Serializer serializer = new Serializer();

var html = await serializer.Load("https://netfree.link/");

HtmlElement dom = serializer.Serialize(html);

var result = dom.Query("p");

Console.WriteLine(result.ToList().Count());
result.ToList().ForEach(item => Console.WriteLine(item.ToString()));


  
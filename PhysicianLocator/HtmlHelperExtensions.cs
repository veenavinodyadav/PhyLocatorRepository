using PhysicianLocator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicianLocator

{
    public static class HtmlHelperExtensions
    {
        public static string CategoryTree(this HtmlHelper html, List<commentview> categories)
        {
            string htmlOutput = string.Empty;

            if (categories.Count() > 0)
            {
                htmlOutput += "<ul>";
                foreach (var category in categories)
                {
                    htmlOutput += "<li>";
                    htmlOutput += category.comm;
                    htmlOutput += html.CategoryTree(categories);
                    htmlOutput += "</li>";
                }
                htmlOutput += "</ul>";
            }

            return htmlOutput;
        }
    }
}
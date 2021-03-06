﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Politiq.Helpers
{
    public static class Formatting
    {
        private static readonly IList<string> Unpluralizables = new List<string>
        { "equipment", "information", "rice", "money", "species", "series", "fish", "sheep", "deer" };
        private static readonly IDictionary<string, string> Pluralizations = new Dictionary<string, string>
        {
            // Start with the rarest cases, and move to the most common
            { "person", "people" },
            { "ox", "oxen" },
            { "child", "children" },
            { "foot", "feet" },
            { "tooth", "teeth" },
            { "goose", "geese" },
            // And now the more standard rules.
            { "(.*)fe?", "$1ves" },         // ie, wolf, wife
            { "(.*)man$", "$1men" },
            { "(.+[aeiou]y)$", "$1s" },
            { "(.+[^aeiou])y$", "$1ies" },
            { "(.+z)$", "$1zes" },
            { "([m|l])ouse$", "$1ice" },
            { "(.+)(e|i)x$", @"$1ices"},    // ie, Matrix, Index
            { "(octop|vir)us$", "$1i"},
            { "(.+(s|x|sh|ch))$", @"$1es"},
            { "(.+)", @"$1s" }
        };

        public static string Pluralize(int count, string singular)
        {
            if (count == 1)
                return singular;

            if (Unpluralizables.Contains(singular))
                return singular;

            var plural = "";

            foreach (var pluralization in Pluralizations)
            {
                if (Regex.IsMatch(singular, pluralization.Key))
                {
                    plural = Regex.Replace(singular, pluralization.Key, pluralization.Value);
                    break;
                }
            }

            return plural;
        }

        public static string ConvertScribToHtml(string str)
        {
            Regex exp;
            exp = new Regex(@"#(.+?)#");
            str = exp.Replace(str, "<strong>$1</strong>");

            exp = new Regex(@"\*(.+?)\*");
            str = exp.Replace(str, "<em>$1</em>");

            exp = new Regex(@"_(.+?)_");
            str = exp.Replace(str, "<u>$1</u>");

            exp = new Regex(@"\^(.+?)\^");
            str = exp.Replace(str, "<del>$1</del>");

            exp = new Regex(@"\|(.+?)\|");
            str = exp.Replace(str, "<blockquote>$1</blockquote>");
            
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789".ToCharArray();
            int i = 0;
            exp = new Regex(@"\[(.+?)\]");
            str = exp.Replace(str, delegate(Match m){
                string result = "<p class='para'>(" + chars.GetValue(i).ToString() + ") " + m.Groups[1].Value + "</p>";
                i++;
                return result;
                });

            return str;
        }
    }


}


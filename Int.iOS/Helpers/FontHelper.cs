//
// FontHelper.cs
//
// Author:
//       Sogurov Fiodor <f.songurov@software-dep.net>
//
// Copyright (c) 2016 Songurov
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Int.Core.Extensions;
using Int.Core.Network.Singleton;
using Int.Core.Tools.Font;
using UIKit;

namespace Int.iOS.Tools.Font
{
    public class FontManager : Singleton<FontManager>, IFont<UIFont>
    {
        private readonly IDictionary<string, Lazy<UIFont>> _fonts = new Dictionary<string, Lazy<UIFont>>();

        public string Extensions { get; set; }

        public void Add(IList<string> nameFonts, string root = "")
        {
            if (nameFonts.IsNull() || nameFonts.Count == 0) return;

            foreach (var item in nameFonts.Where(x => !_fonts.ContainsKey(x)))
                _fonts.Add(item, new Lazy<UIFont>(() =>
                {
                    if (!TryCreate(root + item, out var font))
                        throw new Exception($"Font with {item} not found.");
                    return font;
                }));
        }

        public UIFont GetFont(string nameFont, float sizeFont = 16, string root = "")
        {
            return Get(nameFont, sizeFont);
        }

        public UIFont Get(string type)
        {
            return Get(type, 16f);
        }

        public UIFont Get(string type, float size)
        {
            try
            {
                return _fonts.ContainsKey(type)
                ? _fonts[type]?.Value?.WithSize(size)
                : UIFont.FromName("HelveticaNeueInterface-Regular", size);
            }
            catch (Exception ex)
            {
                //please correctly path resources
                return UIFont.FromName("HelveticaNeueInterface-Regular", size);
            }
        }

        public IList<UIFont> GetFontByFamilyName(string name)
        {
            return (from fam in UIFont.FamilyNames.Where(_ => _.Contains(name)).ToArray()
                    from subFam in UIFont.FontNamesForFamilyName(fam)
                    select UIFont.FromName(subFam, 16f)).ToList();
        }


        public bool Exists(string name)
        {
            return UIFont.FromName(name, 16f) != null;
        }

        public void PrintAllFonts()
        {
            foreach (var font in UIFont.FamilyNames)
            {
                Console.WriteLine(font + ":");
                var subFamilty = UIFont.FontNamesForFamilyName(font);
                foreach (var subfont in subFamilty)
                    Console.WriteLine("   -{0}", subfont);
            }
        }

        private static bool TryCreate(string name, out UIFont font)
        {
            name = Regex.Replace(name, "\\.ttf$", string.Empty);
            name = Regex.Replace(name, "\\.otf$", string.Empty);
            font = UIFont.FromName(name, 16f);
            return font != null;
        }
    }
}
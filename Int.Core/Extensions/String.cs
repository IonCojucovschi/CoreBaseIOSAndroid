//
// String.cs
//
// Author:
//       Songurov <f.songurov@software-dep.net>
//
// Copyright (c) 2016 
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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Int.Core.Extensions
{
    /// <summary>
    ///     Extensions.
    /// </summary>
    public static partial class Extensions
    {
        //private static readonly Random _random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        ///     First char Upper
        /// </summary>
        /// <returns>The upper first.</returns>
        /// <param name="value">Value.</param>
        public static string ToUpperFirst(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var sb = new StringBuilder(value);
            sb[0] = char.ToUpper(sb[0]);
            return sb.ToString();
        }

        public static string ToHttps(this string @this)
        {
            return @this.Replace("http", "https");
        }

        public static bool IsEmpty(this string str, IEnumerable<string> strLocal)
        {
            return strLocal.Any(x => x.IsNullOrWhiteSpace());
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        //public static string RandomImage(object obj, int width = 200, int height = 300) => $"https://picsum.photos/{width}/{height}/?image={_random.Next()}";

        public static string ValueOrDefault(this string inputString, string defaultString)
        {
            var result = defaultString;

            if (inputString != null)
            {
                var valueNoSpaces = inputString.Trim();
                if (valueNoSpaces.Length > 0)
                {
                    result = inputString;
                }
            }

            return result;
        }

        /// <summary>
        ///     Last char Upper
        /// </summary>
        /// <returns>The upper last.</returns>
        /// <param name="value">Value.</param>
        public static string ToUpperLast(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var sb = new StringBuilder(value);
            sb[value.Length - 1] = char.ToUpper(sb[value.Length - 1]);
            return sb.ToString();
        }

        /// <summary>
        ///     Upper Case Word
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UpperCaseWords(this string text)
        {
            var sb = new StringBuilder(text.Length);
            var capitalize = true;

            foreach (var c in text.Trim())
            {
                sb.Append(capitalize ? char.ToUpper(c) : char.ToLower(c));
                capitalize = !char.IsLetter(c);
            }

            return Regex.Replace(sb.ToString().Replace(" ", string.Empty), "([A-Z])", " $1").Trim();
        }

        public static DateTime UtsToDateTime(this string unixTimeStamp)
        {
            if (string.IsNullOrWhiteSpace(unixTimeStamp))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(unixTimeStamp));

            return
                new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(double.Parse(unixTimeStamp))
                    .ToLocalTime();
        }

        public static Stream ToStream(this string str)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static async Task<byte[]> GetUrl(this string url)
        {
            return await new HttpClient().GetByteArrayAsync(url);
        }

        /// <summary>
        ///     Valid email.
        /// </summary>
        /// <returns><c>true</c>, if valid email was ised, <c>false</c> otherwise.</returns>
        /// <param name="str">String.</param>
        public static bool IsEmail(this string str)
        {
            return Regex.IsMatch(str,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);
        }

        public static T FromJson<T>(this string json) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            catch
            {
                return default(T);
            }
        }

        public static string ToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
//
// Object.cs
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Int.Core.Extensions
{
    /// <summary>
    ///     Extensions.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        ///     Ises the number.
        /// </summary>
        /// <returns><c>true</c>, if number was ised, <c>false</c> otherwise.</returns>
        /// <param name="value">Value.</param>
        public static bool IsNumber(this object value)
        {
            return Regex.IsMatch(value as string, @"^\d+$");
        }

        /// <summary>
        ///     Tos the xml.
        /// </summary>
        /// <returns>The xml.</returns>
        /// <param name="obj">Object.</param>
        public static string ToXml(this object obj)
        {
            var x = new XmlSerializer(obj.GetType());
            var stringwriter = new StringWriter();
            x.Serialize(stringwriter, obj);
            return stringwriter.ToString();
        }

        public static void TimerReponse(this object datetime, Action action, int timer = 3000)
        {
            Task.Delay(timer)
                .ContinueWith(obj =>
                {
                    action?.Invoke();
                });
        }

        public static byte[] Serialize<T>(T obj)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var stream = new MemoryStream();
            using (var writer =
                XmlDictionaryWriter.CreateBinaryWriter(stream))
            {
                serializer.WriteObject(writer, obj);
            }
            return stream.ToArray();
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static T Deserialize<T>(byte[] data)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new MemoryStream(data))
            using (var reader =
                XmlDictionaryReader.CreateBinaryReader(
                    stream, XmlDictionaryReaderQuotas.Max))
            {
                return (T)serializer.ReadObject(reader);
            }
        }

        public static void UpdateObject(this object org, object update)
        {
            var currentEntityProperties = org.GetType().GetRuntimeProperties();
            var newEntityProperties = update.GetType().GetRuntimeProperties();

            var entityProperties = newEntityProperties as PropertyInfo[] ?? newEntityProperties.ToArray();
            foreach (var currentEntityProperty in currentEntityProperties)
                foreach (var newEntityProperty in entityProperties)
                {
                    if (newEntityProperty.Name != currentEntityProperty.Name) continue;
                    if (!newEntityProperty.CanRead) continue;

                    try
                    {
                        var value = newEntityProperty.GetValue(update);
                        var valueOld = newEntityProperty.GetValue(org);

                        if (value is Enum)
                        {
                            var typesOld = Convert.ToUInt32(valueOld as Enum);
                            var typesNew = Convert.ToUInt32(value as Enum);

                            if (typesNew != typesOld && currentEntityProperty.CanWrite)
                                currentEntityProperty.SetValue(org, value);
                        }
                        else if (value is string)
                        {
                            if (!(value as string).IsNullOrWhiteSpace() && currentEntityProperty.CanWrite)
                                currentEntityProperty.SetValue(org, value);
                        }
                        else
                        {
                            if (!value.IsNull() && currentEntityProperty.CanWrite)
                                currentEntityProperty.SetValue(org, value);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
        }

        public static T GetPropValue<T>(this object source, string propertyName)
        {
            var property = source.GetType().GetRuntimeProperties().FirstOrDefault(p =>
                string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
            return property != null
                ? property.GetValue(source) is T
                    ? (T)property.GetValue(source)
                    : default(T)
                : default(T);
        }

        public static void MainThread(this object obj, Action action)
        {
            var context = SynchronizationContext.Current;

            context.Post(o => { action?.Invoke(); }, null);
        }
    }
}
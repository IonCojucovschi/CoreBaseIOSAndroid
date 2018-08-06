//
// ObjectTest.cs
//
// Author:
//       Songurov Fiodor <f.songurov@software-dep.net>
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
using Int.Core.Extensions;
using NUnit.Framework;

namespace NUnit.Int.Core.Extensions
{
    [TestFixture]
    public class ObjectTest
    {
        [Serializable]
        public class Example
        {
            public double Employees1 { get; set; }
            public int Employees2 { get; set; }
            public int Employees3 { get; set; }
        }

        [Test]
        public void IsNumber_12_true()
        {
            //arr
            object value = 12;

            //act
            var result = value.IsNumber();
            const bool exp = true;

            //assert
            Assert.AreEqual(exp, result);
        }

        [Test]
        public void ToXml_Type_Xml()
        {
            //arr
            object value = new Example();

            //act
            var result = value.ToXml();
            const string exp =
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>\n<Example xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\n  " +
                "<employees1>0</employees1>\n " +
                " <employees2>0</employees2>\n  " +
                "<employees3>0</employees3>\n" +
                "</Example>";

            //assert
            Assert.AreEqual(exp, result);
        }
    }
}
using Int.Core.Extensions;
using NUnit.Framework;

namespace NUnit.Int.Core.Extensions
{
    [TestFixture]
    public class StringTest
    {
        [Test]
        public void StringExtension_ToUpper_First()
        {
            //arr
            const string values = "test";

            //act
            var result = values.ToUpperFirst();
            const string exp = "Test";

            //assert
            Assert.AreEqual(exp, result);
        }

        [Test]
        public void StringExtension_ToUpper_Last()
        {
            //arr
            const string values = "test";
            //act
            var result = values.ToUpperLast();
            const string exp = "tesT";
            //assert
            Assert.AreEqual(exp, result);
        }

        [Test]
        public void StringExtension_Upper_CaseWords()
        {
            //arr
            const string value = " something  in the    way ";
            //act
            var result = value.UpperCaseWords();
            const string exp = "Something In The Way";
            //assert
            Assert.AreEqual(exp, result);
        }

        //[Test]
        //public void StringExtension_ValidEmail_Regex()
        //{
        //    //arr
        //    const string values = "test@asd.com";
        //    //act
        //    var result = values.IsValidEmail();
        //    const bool exp = true;
        //    //assert
        //    Assert.AreEqual(exp, result);
        //}
    }
}
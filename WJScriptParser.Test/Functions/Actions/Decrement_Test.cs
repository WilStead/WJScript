using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class Decrement_Test
    {
        [TestMethod]
        public void Decrement_Null()
        {
            var source =
@"
a;
return a--;
";
            var result = Parser.Execute(source);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Decrement_Array_Postfix()
        {
            var source =
@"
return [0,1,2,3]--;
";
            var expected = new List<object> { 0d, 1d, 2d, 3d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Decrement_Array_Prefix()
        {
            var source =
@"
return --[0,1,2,3];
";
            var expected = new List<object> { 0d, 1d, 2d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Decrement_Boolean_T_Postfix()
        {
            var source =
@"
return true--;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Decrement_Boolean_T_Prefix()
        {
            var source =
@"
return --true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Decrement_Boolean_F_Postfix()
        {
            var source =
@"
return false--;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Decrement_Boolean_F_Prefix()
        {
            var source =
@"
return --false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Decrement_Number_Postfix()
        {
            var source =
@"
return 1--;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void Decrement_Number_Prefix()
        {
            var source =
@"
return --1;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(0, (double)result);
        }

        [TestMethod]
        public void Decrement_Number_String_Numeric_Postfix()
        {
            var source =
@"
return ""1""--;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("1", (string)result);
        }

        [TestMethod]
        public void Decrement_Number_String_Numeric_Prefix()
        {
            var source =
@"
return --""1"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(0, (double)result);
        }

        [TestMethod]
        public void Decrement_Number_String_NonNumeric_Postfix()
        {
            var source =
@"
return ""foo""--;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("foo", (string)result);
        }

        [TestMethod]
        public void Decrement_Number_String_NonNumeric_Prefix()
        {
            var source =
@"
return --""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("fo", (string)result);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class Subset_Test
    {
        [TestMethod]
        public void Subset_Null()
        {
            var source =
@"
a;
return a.Subset();
";
            var result = Parser.Execute(source);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Subset_NoArgs()
        {
            var source =
@"
a = ""hello"";
a = a.Subset();
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("hello", (string)result);
        }

        [TestMethod]
        public void Subset_Array_NoArgs()
        {
            var source =
@"
a = [0,1,2,3];
a = a.Subset();
return a;
";
            var expected = new List<object> { 0d, 1d, 2d, 3d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Subset_OneArg_0()
        {
            var source =
@"
a = ""hello"";
a = a.Subset(0);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("hello", (string)result);
        }

        [TestMethod]
        public void Subset_Array_OneArg_0()
        {
            var source =
@"
a = [0,1,2,3];
a = a.Subset(0);
return a;
";
            var expected = new List<object> { 0d, 1d, 2d, 3d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Subset_OneArg_NonZero()
        {
            var source =
@"
a = ""hello"";
a = a.Subset(1);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("ello", (string)result);
        }

        [TestMethod]
        public void Subset_Array_OneArg_NonZero()
        {
            var source =
@"
a = [0,1,2,3];
a = a.Subset(1);
return a;
";
            var expected = new List<object> { 1d, 2d, 3d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Subset_TwoArgs()
        {
            var source =
@"
a = ""hello"";
a = a.Subset(1, 2);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("el", (string)result);
        }

        [TestMethod]
        public void Subset_Array_TwoArgs()
        {
            var source =
@"
a = [0,1,2,3];
a = a.Subset(1, 2);
return a;
";
            var expected = new List<object> { 1d, 2d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Subset_ThreeArgs()
        {
            var source =
@"
a = ""hello"";
a = a.Subset(1, 2, 3);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("el", (string)result);
        }

        [TestMethod]
        public void Subset_Array_ThreeArgs()
        {
            var source =
@"
a = [0,1,2,3];
a = a.Subset(1, 2, 3);
return a;
";
            var expected = new List<object> { 1d, 2d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Subset_Boolean()
        {
            var source =
@"
a = true;
a = a.Subset();
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.AreEqual(true, (bool)result);
        }

        [TestMethod]
        public void Subset_Number()
        {
            var source =
@"
a = 1;
a = a.Subset();
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }
    }
}

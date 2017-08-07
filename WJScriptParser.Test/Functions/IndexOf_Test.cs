using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class IndexOf_Test
    {
        [TestMethod]
        public void IndexOf_Null()
        {
            var source =
@"
a;
return a.IndexOf();
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(-1, (double)result);
        }

        [TestMethod]
        public void IndexOf_NoArgs()
        {
            var source =
@"
a = [0,1,2,3];
a = a.IndexOf();
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(-1, (double)result);
        }

        [TestMethod]
        public void IndexOf_Array_OneArg_Match()
        {
            var source =
@"
a = [0,1,2,3];
a = a.IndexOf(1);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void IndexOf_Array_OneArg_NoMatch()
        {
            var source =
@"
a = [0,1,2,3];
a = a.IndexOf(4);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(-1, (double)result);
        }

        [TestMethod]
        public void IndexOf_Array_TwoArgs_Match()
        {
            var source =
@"
a = [0,1,2,3];
a = a.IndexOf(4, 1);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void IndexOf_Array_TwoArgs_NoMatch()
        {
            var source =
@"
a = [0,1,2,3];
a = a.IndexOf(4, 5);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(-1, (double)result);
        }

        [TestMethod]
        public void IndexOf_Other_Args()
        {
            var source =
@"
a = true;
a = a.IndexOf(1);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(-1, (double)result);
        }

        [TestMethod]
        public void IndexOf_String_OneArg_Match()
        {
            var source =
@"
a = ""hello"";
a = a.IndexOf(""e"");
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void IndexOf_String_OneArg_NoMatch()
        {
            var source =
@"
a = ""hello"";
a = a.IndexOf(""a"");
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(-1, (double)result);
        }

        [TestMethod]
        public void IndexOf_String_TwoArgs_Match()
        {
            var source =
@"
a = ""hello"";
a = a.IndexOf(""a"", ""e"");
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void IndexOf_String_TwoArgs_NoMatch()
        {
            var source =
@"
a = ""hello"";
a = a.IndexOf(""a"", ""b"");
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(-1, (double)result);
        }
    }
}

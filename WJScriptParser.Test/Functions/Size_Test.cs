using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class Size_Test
    {
        [TestMethod]
        public void Size_Null()
        {
            var source =
@"
a;
return a.Size();
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(0, (double)result);
        }

        [TestMethod]
        public void Size_Array_NoArgs()
        {
            var source =
@"
a = [0,1,2,3];
a = a.Size();
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(4, (double)result);
        }

        [TestMethod]
        public void Size_Array_OneArg_Match()
        {
            var source =
@"
a = [0,1,2,3];
a = a.Size(4);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Size_Array_OneArg_NoMatch()
        {
            var source =
@"
a = [0,1,2,3];
a = a.Size(1);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Size_Array_TwoArgs_Match()
        {
            var source =
@"
a = [0,1,2,3];
a = a.Size(1, 4);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Size_Array_TwoArgs_NoMatch()
        {
            var source =
@"
a = [0,1,2,3];
a = a.Size(1, 2);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Size_Boolean_NoArgs_True()
        {
            var source =
@"
a = true;
a = a.Size();
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void Size_Boolean_NoArgs_False()
        {
            var source =
@"
a = false;
a = a.Size();
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(0, (double)result);
        }

        [TestMethod]
        public void Size_Boolean_OneArg_True_Match()
        {
            var source =
@"
a = true;
a = a.Size(1);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Size_Boolean_OneArg_True_NoMatch()
        {
            var source =
@"
a = true;
a = a.Size(0);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Size_Boolean_TwoArgs_True_Match()
        {
            var source =
@"
a = true;
a = a.Size(2, 1);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Size_Boolean_TwoArgs_True_NoMatch()
        {
            var source =
@"
a = true;
a = a.Size(0, 2);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Size_Boolean_TwoArgs_False_Match()
        {
            var source =
@"
a = false;
a = a.Size(2, 0);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Size_Boolean_TwoArgs_False_NoMatch()
        {
            var source =
@"
a = false;
a = a.Size(1, 2);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Size_Number_NoArgs()
        {
            var source =
@"
a = 1;
a = a.Size();
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void Size_Number_OneArg_Match()
        {
            var source =
@"
a = 1;
a = a.Size(1);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Size_Number_OneArg_NoMatch()
        {
            var source =
@"
a = 1;
a = a.Size(2);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Size_Number_TwoArgs_Match()
        {
            var source =
@"
a = 1;
a = a.Size(0, 1);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Size_Number_TwoArgs_NoMatch()
        {
            var source =
@"
a = 1;
a = a.Size(0, 2);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Size_String_NoArgs()
        {
            var source =
@"
a = ""hello"";
a = a.Size();
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(5, (double)result);
        }

        [TestMethod]
        public void Size_String_OneArg_Match()
        {
            var source =
@"
a = ""hello"";
a = a.Size(5);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Size_String_OneArg_NoMatch()
        {
            var source =
@"
a = ""hello"";
a = a.Size(1);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Size_String_TwoArgs_Match()
        {
            var source =
@"
a = ""hello"";
a = a.Size(1, 5);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Size_String_TwoArgs_NoMatch()
        {
            var source =
@"
a = ""hello"";
a = a.Size(1, 2);
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }
    }
}

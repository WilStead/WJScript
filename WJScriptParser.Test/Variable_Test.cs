using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test
{
    [TestClass]
    public class Variable_Test
    {
        [TestMethod]
        public void Array_ToBoolean_NonEmpty()
        {
            var source =
@"
return [0,1,2,3] || false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Array_ToBoolean_Empty()
        {
            var source =
@"
return [] || false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Number_ToBoolean_0()
        {
            var source =
@"
return 0 || false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Number_ToBoolean_NonZero()
        {
            var source =
@"
return 1 || false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void String_ToBoolean_True()
        {
            var source =
@"
return ""true"" || false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void String_ToBoolean_False()
        {
            var source =
@"
return ""false"" || false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void String_ToBoolean_0()
        {
            var source =
@"
return ""0"" || false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void String_ToBoolean_NonZero_Numeric()
        {
            var source =
@"
return ""1"" || false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void String_ToBoolean_Empty()
        {
            var source =
@"
return """" || false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void String_ToBoolean_Other()
        {
            var source =
@"
return ""foo"" || false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }
    }
}

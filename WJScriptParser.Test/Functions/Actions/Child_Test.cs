using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class Child_Test
    {
        [TestMethod]
        public void Child_Null_Parent()
        {
            var source =
@"
a;
return a.b;
";
            var result = Parser.Execute(source);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Child_Null_Parent_Add()
        {
            var source =
@"
a;
a.foo = 4;
return a.foo;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(4, (double)result);
        }

        [TestMethod]
        public void Child_Null_Child()
        {
            var source =
@"
a = [0,1,2,3];
b;
return a.b;
";
            var result = Parser.Execute(source);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Child_NotPresent()
        {
            var source =
@"
a = [0,1,2,3];
return a.foo;
";
            var result = Parser.Execute(source);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Child_Array_Present()
        {
            var source =
@"
a = [0,1,2,3];
a.foo = 4;
return a.foo;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(4, (double)result);
        }

        [TestMethod]
        public void Child_Boolean_Present()
        {
            var source =
@"
a = true;
a.foo = 4;
return a.foo;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(4, (double)result);
        }

        [TestMethod]
        public void Child_Number_Present()
        {
            var source =
@"
a = 5.2;
a.foo = 4;
return a.foo;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(4, (double)result);
        }

        [TestMethod]
        public void Child_String_Present()
        {
            var source =
@"
a = ""bar"";
a.foo = 4;
return a.foo;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(4, (double)result);
        }

        [TestMethod]
        public void Child_Present_PostTypeChange()
        {
            var source =
@"
a = [0,1,2,3];
a.foo = 4;
a = true;
return a.foo;
";
            var result = Parser.Execute(source);

            Assert.IsNull(result);
        }
    }
}

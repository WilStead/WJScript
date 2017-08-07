using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace WScriptParser.Test.Functions.Actions
{
    [TestClass]
    public class Assign_Test
    {
        [TestMethod]
        public void CanAssignNumber()
        {
            var source =
@"
a = 1;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void CanAssignString()
        {
            var source =
@"
a = ""hello"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("hello", (string)result);
        }

        [TestMethod]
        public void CanAssignBoolean()
        {
            var source =
@"
a = true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.AreEqual(true, (bool)result);
        }

        [TestMethod]
        public void CanAssignArray()
        {
            var list = new List<object>
            {
                0d,
                true,
                "hello"
            };

            var source =
@"
a = [0, true, ""hello""];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(list, (List<object>)result);
        }

        [TestMethod]
        public void CanAssignVariable()
        {
            var source =
@"
a = 1;
b = 2;
a = b;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(2, (double)result);
        }

        [TestMethod]
        public void Variables_CanGetValue()
        {
            var source =
@"
a = 1;
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void CanReassign()
        {
            var source =
@"
a = 1;
a = 2;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(2, (double)result);
        }
    }
}

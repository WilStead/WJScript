using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WJScriptParser.Test
{
    [TestClass]
    public class Math_Test
    {
        [TestMethod]
        public void E()
        {
            var source =
@"
return Math.E;
";
            var result = Parser.Evaluate(source);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(double));
            Assert.AreEqual(Math.E, (double)result);
        }

        [TestMethod]
        public void Truncate_Null()
        {
            var source =
@"
a;
return Math.Truncate(a);
";
            var result = Parser.Evaluate(source);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Truncate_Array()
        {
            var source =
@"
return Math.Truncate(new double[] { 0, 1.5, 2.3, 3.2 });
";
            var result = Parser.Evaluate(source);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int[]));
            Assert.AreEqual(0, ((int[])result)[0]);
            Assert.AreEqual(1, ((int[])result)[1]);
            Assert.AreEqual(2, ((int[])result)[2]);
            Assert.AreEqual(3, ((int[])result)[3]);
        }

        [TestMethod]
        public void Truncate_Boolean()
        {
            var source =
@"
return Math.Truncate(true);
";
            var result = Parser.Evaluate(source);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(1, (int)result);
        }

        [TestMethod]
        public void Truncate_Number()
        {
            var source =
@"
return Math.Truncate(5.4);
";
            var result = Parser.Evaluate(source);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(5, (int)result);
        }

        [TestMethod]
        public void Truncate_String_Numeric()
        {
            var source =
@"
return Math.Truncate(""5.4"");
";
            var result = Parser.Evaluate(source);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(5, (int)result);
        }

        [TestMethod]
        public void Truncate_String_NonNumeric()
        {
            var source =
@"
return Math.Truncate(""foo"");
";
            var result = Parser.Evaluate(source);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, (int)result);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions.Math
{
    [TestClass]
    public class Math_Test
    {
        [TestMethod]
        public void E()
        {
            var source =
@"
return E();
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(System.Math.E, (double)result);
        }

        [TestMethod]
        public void E_Null()
        {
            var source =
@"
a;
return E(a);
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(System.Math.E, (double)result);
        }

        [TestMethod]
        public void E_NonNull()
        {
            var source =
@"
return E(5.4);
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(System.Math.E, (double)result);
        }

        [TestMethod]
        public void Truncate_Null()
        {
            var source =
@"
a;
return Truncate(a);
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(0, (double)result);
        }

        [TestMethod]
        public void Truncate_Array_Single()
        {
            var source =
@"
return Truncate([0,1,2,3]);
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(4, (double)result);
        }

        [TestMethod]
        public void Truncate_Array_Multiple()
        {
            var source =
@"
return Truncate([0,1,2,3], [0,1,2]);
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(7, (double)result);
        }

        [TestMethod]
        public void Truncate_Boolean_Single()
        {
            var source =
@"
return Truncate(true);
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void Truncate_Boolean_Multiple()
        {
            var source =
@"
return Truncate(true, false);
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void Truncate_Number_Single()
        {
            var source =
@"
return Truncate(5.4);
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(5, (double)result);
        }

        [TestMethod]
        public void Truncate_Number_Multiple()
        {
            var source =
@"
return Truncate(5.4, 3.8);
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(8, (double)result);
        }

        [TestMethod]
        public void Truncate_String_Numeric_Single()
        {
            var source =
@"
return Truncate(""5.4"");
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(5, (double)result);
        }

        [TestMethod]
        public void Truncate_String_NonNumeric_Single()
        {
            var source =
@"
return Truncate(""foo"");
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(3, (double)result);
        }

        [TestMethod]
        public void Truncate_String_Numeric_Multiple()
        {
            var source =
@"
return Truncate(""5.4"", ""3.8"");
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(8, (double)result);
        }

        [TestMethod]
        public void Truncate_String_NonNumeric_Multiple()
        {
            var source =
@"
return Truncate(""foo"", ""bar"");
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(6, (double)result);
        }

        [TestMethod]
        public void Truncate_Assortment()
        {
            var source =
@"
return Truncate([0,1,2,3], true, 5.4, ""5.4"", ""foo"");
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(18, (double)result);
        }
    }
}

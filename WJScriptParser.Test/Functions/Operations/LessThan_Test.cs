using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions.Operations
{
    [TestClass]
    public class LessThan_Test
    {
        [TestMethod]
        public void LessThan_Array_Array_False()
        {
            var source =
@"
return [0,1,2,3] < [0,1,2];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Array_Array_True()
        {
            var source =
@"
return [0,1,2] < [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Array_Boolean_NonEmpty_T()
        {
            var source =
@"
return [0,1,2,3] < true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Array_Boolean_Empty_T()
        {
            var source =
@"
return [] < true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Array_Boolean_NonEmpty_F()
        {
            var source =
@"
return [0,1,2,3] < false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Array_Boolean_Empty_F()
        {
            var source =
@"
return [] < false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Array_Number_True()
        {
            var source =
@"
return [0,1,2,3] < 5;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Array_Number_False()
        {
            var source =
@"
return [0,1,2,3] < 3;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Array_String_Numeric_True()
        {
            var source =
@"
return [0,1,2,3] < ""5"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Array_String_Numeric_NotEqual()
        {
            var source =
@"
return [0,1,2,3] < ""3"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Array_String_NonNumeric_True()
        {
            var source =
@"
return [0,1,2,3] < ""foooo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Array_String_NonNumeric_False()
        {
            var source =
@"
return [0,1,2,3] < ""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Boolean_Array_NonEmpty_T()
        {
            var source =
@"
return true < [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Boolean_Array_Empty_T()
        {
            var source =
@"
return true < [];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Boolean_Array_NonEmpty_F()
        {
            var source =
@"
return false < [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Boolean_Array_Empty_F()
        {
            var source =
@"
return false < [];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Boolean_Number_True()
        {
            var source =
@"
return false < 1;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Boolean_Number_False()
        {
            var source =
@"
return true < 0;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Boolean_String_True()
        {
            var source =
@"
return false < ""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Boolean_String_False()
        {
            var source =
@"
return true < ""false"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Number_Array_True()
        {
            var source =
@"
return 3 < [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Number_Array_False()
        {
            var source =
@"
return 5 < [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Number_Boolean_True()
        {
            var source =
@"
return 0 < true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Number_Boolean_False()
        {
            var source =
@"
return 2 < false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Number_Number_False()
        {
            var source =
@"
return 2 < 1;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Number_Number_True()
        {
            var source =
@"
return 1 < 2;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Number_String_Numeric_True()
        {
            var source =
@"
return 2 < ""3"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Number_String_Numeric_False()
        {
            var source =
@"
return 3 < ""2"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_Number_String_NonNumeric_True()
        {
            var source =
@"
return 1 < ""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_Number_String_NonNumeric_False()
        {
            var source =
@"
return 4 < ""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_String_Array_Numeric_True()
        {
            var source =
@"
return ""3"" < [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_String_Array_Numeric_False()
        {
            var source =
@"
return ""5"" < [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_String_Array_NonNumeric_True()
        {
            var source =
@"
return ""foo"" < [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_String_Array_NonNumeric_False()
        {
            var source =
@"
return ""foooo"" < [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_String_Boolean_True()
        {
            var source =
@"
return ""false"" < true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_String_Boolean_False()
        {
            var source =
@"
return ""foo"" < false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_String_Number_Numeric_True()
        {
            var source =
@"
return ""1"" < 2;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_String_Number_Numeric_False()
        {
            var source =
@"
return ""2"" < 1;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_String_Number_NonNumeric_True()
        {
            var source =
@"
return ""foo"" < 4;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_String_Number_NonNumeric_False()
        {
            var source =
@"
return ""foo"" < 2;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_String_String_Numeric_True()
        {
            var source =
@"
return ""1"" < ""2"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_String_String_Numeric_False()
        {
            var source =
@"
return ""2"" < ""1"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void LessThan_String_String__NonNumeric_True()
        {
            var source =
@"
return ""foo"" < ""fooo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void LessThan_String_String_NonNumeric_False()
        {
            var source =
@"
return ""fooo"" < ""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }
    }
}

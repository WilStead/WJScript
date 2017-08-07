using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions.Operations
{
    [TestClass]
    public class NotEqual_Test
    {
        [TestMethod]
        public void NotEqual_Array_Array_Equal()
        {
            var source =
@"
return [0,1,2,3] != [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Array_Array_NotEqual()
        {
            var source =
@"
return [0,1,2,3] != [0,1,2];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Array_Boolean_NonEmpty_T()
        {
            var source =
@"
return [0,1,2,3] != true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Array_Boolean_Empty_T()
        {
            var source =
@"
return [] != true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Array_Boolean_NonEmpty_F()
        {
            var source =
@"
return [0,1,2,3] != false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Array_Boolean_Empty_F()
        {
            var source =
@"
return [] != false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Array_Number_Equal()
        {
            var source =
@"
return [0,1,2,3] != 4;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Array_Number_NotEqual()
        {
            var source =
@"
return [0,1,2,3] != 3;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Array_String_Numeric_Equal()
        {
            var source =
@"
return [0,1,2,3] != ""4"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Array_String_Numeric_NotEqual()
        {
            var source =
@"
return [0,1,2,3] != ""3"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Array_String_NonNumeric()
        {
            var source =
@"
return [0,1,2,3] != ""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Boolean_Array_NonEmpty_T()
        {
            var source =
@"
return true != [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Boolean_Array_Empty_T()
        {
            var source =
@"
return true != [];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Boolean_Array_NonEmpty_F()
        {
            var source =
@"
return false != [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Boolean_Array_Empty_F()
        {
            var source =
@"
return false != [];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Boolean_Number_Equal()
        {
            var source =
@"
return false != 0;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Boolean_Number_NotEqual()
        {
            var source =
@"
return false != 3;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Boolean_String_Equal()
        {
            var source =
@"
return false != ""false"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Boolean_String_NotEqual()
        {
            var source =
@"
return false != ""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Number_Array_NotEqual()
        {
            var source =
@"
return 3 != [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Number_Array_Equal()
        {
            var source =
@"
return 4 != [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Number_Boolean_Equal()
        {
            var source =
@"
return 1 != true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Number_Boolean_NotEqual()
        {
            var source =
@"
return 2 != false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Number_Number_NotEqual()
        {
            var source =
@"
return 2 != 1;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Number_Number_Equal()
        {
            var source =
@"
return 1 != 1;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Number_String_Numeric_Equal()
        {
            var source =
@"
return 1 != ""1"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_Number_String_Numeric_NotEqual()
        {
            var source =
@"
return 1 != ""2"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Number_String_NonNumeric_NotEqual()
        {
            var source =
@"
return 1 != ""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_Number_String_NonNumeric_Equal()
        {
            var source =
@"
return 3 != ""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_String_Array_Numeric_Equal()
        {
            var source =
@"
return ""4"" != [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_String_Array_Numeric_NotEqual()
        {
            var source =
@"
return ""3"" != [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_String_Array_NonNumeric()
        {
            var source =
@"
return ""foo"" != [0,1,2,3];
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_String_Boolean_Equal()
        {
            var source =
@"
return ""false"" != false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_String_Boolean_NotEqual()
        {
            var source =
@"
return ""foo"" != false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_String_Number_Numeric_Equal()
        {
            var source =
@"
return ""1"" != 1;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_String_Number_Numeric_NotEqual()
        {
            var source =
@"
return ""1"" != 2;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_String_Number_NonNumeric_NotEqual()
        {
            var source =
@"
return ""foo"" != 1;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void NotEqual_String_Number_NonNumeric_Equal()
        {
            var source =
@"
return ""foo"" != 3;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_String_String_Equal()
        {
            var source =
@"
return ""foo"" != ""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void NotEqual_String_String_NotEqual()
        {
            var source =
@"
return ""foo"" != ""bar"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }
    }
}

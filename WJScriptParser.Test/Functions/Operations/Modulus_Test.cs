﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class Modulus_Test
    {
        [TestMethod]
        public void Modulus_Null()
        {
            var source =
@"
a;
return a % 1;
";
            var result = Parser.Execute(source);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Modulus_Array_Array()
        {
            var source =
@"
return [0,1,2,3] % [1,2,4];
";
            var expected = new List<object> { 0d, 3d, 4d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Modulus_Array_Other()
        {
            var source =
@"
return [0,1,2,3] % 1;
";
            var expected = new List<object> { 0d, 2d, 3d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Modulus_Array_Null()
        {
            var source =
@"
a;
return [0,1,2,3] % a;
";
            var expected = new List<object> { 0d, 1d, 2d, 3d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Modulus_Boolean_Array_Match()
        {
            var source =
@"
return true % [0,1,2,3,true];
";
            var expected = new List<object> { 0d, 1d, 2d, 3d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Xor_Boolean_Boolean_TT()
        {
            var source =
@"
return true % true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Xor_Boolean_Boolean_TF()
        {
            var source =
@"
return true % false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Xor_Boolean_Boolean_FT()
        {
            var source =
@"
return false % true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Xor_Boolean_Boolean_FF()
        {
            var source =
@"
return false % false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Xor_Boolean_Number_T0()
        {
            var source =
@"
return true % 0;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Xor_Boolean_Number_T_NonZero()
        {
            var source =
@"
return true % 1;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Xor_Boolean_Number_F0()
        {
            var source =
@"
return false % 0;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Xor_Boolean_Number_F_NonZero()
        {
            var source =
@"
return false % 1;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Xor_Boolean_String_T_True()
        {
            var source =
@"
return true % ""true"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Xor_Boolean_String_T_False()
        {
            var source =
@"
return true % ""false"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Xor_Boolean_String_F_True()
        {
            var source =
@"
return false % ""true"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Xor_Boolean_String_F_False()
        {
            var source =
@"
return false % ""false"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Modulus_Number_Array()
        {
            var source =
@"
return 1 % [0,1,2,3];
";
            var expected = new List<object> { 0d, 2d, 3d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Xor_Number_Boolean_0_T()
        {
            var source =
@"
return 0 % true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Xor_Number_Boolean_0_F()
        {
            var source =
@"
return 0 % false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Xor_Number_Boolean_1_T()
        {
            var source =
@"
return 1 % true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Xor_Number_Boolean_1_F()
        {
            var source =
@"
return 1 % false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Modulus_Number_Number()
        {
            var source =
@"
return 3 % 2;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void Modulus_Number_String_Numeric()
        {
            var source =
@"
return 3 % ""2"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void Modulus_Number_String_NonNumeric()
        {
            var source =
@"
return 3 % ""foo"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("3%foo", (string)result);
        }

        [TestMethod]
        public void Modulus_String_Array_Match()
        {
            var source =
@"
return ""foo"" % [0,1,2,""foo""];
";
            var expected = new List<object> { 0d, 1d, 2d };

            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(List<object>));
            CollectionAssert.AreEqual(expected, (List<object>)result);
        }

        [TestMethod]
        public void Xor_String_Boolean_False_T()
        {
            var source =
@"
return ""false"" % true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Xor_String_Boolean_False_F()
        {
            var source =
@"
return ""false"" % false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Xor_String_Boolean_True_T()
        {
            var source =
@"
return ""true"" % true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void Xor_String_Boolean_True_F()
        {
            var source =
@"
return ""true"" % false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Modulus_String_Number_Numeric()
        {
            var source =
@"
return ""3"" % 2;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void Modulus_String_Number_NonNumeric()
        {
            var source =
@"
return ""foo"" % 2;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("foo%2", (string)result);
        }

        [TestMethod]
        public void Modulus_String_String_Numeric()
        {
            var source =
@"
return ""3"" % ""2"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }

        [TestMethod]
        public void Modulus_String_String_NonNumeric()
        {
            var source =
@"
return ""foo"" % ""bar"";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("foo%bar", (string)result);
        }
    }
}
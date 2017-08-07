﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class AndAssign_Test
    {
        [TestMethod]
        public void AndAssign_True_First()
        {
            var source =
@"
return true &= false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void AndAssign_True_Second()
        {
            var source =
@"
return false &= true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void AndAssign_True()
        {
            var source =
@"
return true &= true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class OrAssign_Test
    {
        [TestMethod]
        public void OrAssign_True_First()
        {
            var source =
@"
return true |= false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void OrAssign_True_Second()
        {
            var source =
@"
return false |= true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void OrAssign_False()
        {
            var source =
@"
return false |= false;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(bool));
            Assert.IsFalse((bool)result);
        }
    }
}

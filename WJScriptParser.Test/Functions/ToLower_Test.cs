using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class ToLower_Test
    {
        [TestMethod]
        public void ToLower()
        {
            var source =
@"
a = ""HELLO"";
a = a.ToLower();
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("hello", (string)result);
        }

        [TestMethod]
        public void ToLower_Null()
        {
            var source =
@"
a;
return a.ToLower();
";
            var result = Parser.Execute(source);

            Assert.IsNull(result);
        }
    }
}

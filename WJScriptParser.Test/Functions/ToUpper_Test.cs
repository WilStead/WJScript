using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class ToUpper_Test
    {
        [TestMethod]
        public void ToUpper()
        {
            var source =
@"
a = ""hello"";
a = a.ToUpper();
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(string));
            Assert.AreEqual("HELLO", (string)result);
        }

        [TestMethod]
        public void ToUpper_Null()
        {
            var source =
@"
a;
return a.ToUpper();
";
            var result = Parser.Execute(source);

            Assert.IsNull(result);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class Get_Test
    {
        [TestMethod]
        public void CanGetValue()
        {
            var source =
@"
a = 1;
a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }
    }
}

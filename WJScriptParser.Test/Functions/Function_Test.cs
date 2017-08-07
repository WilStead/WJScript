using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class Function_Test
    {
        [TestMethod]
        public void Function()
        {
            var source =
@"
a = 1;
function b(a)
{
    a++;
    return a;
}
return b(a);
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(2, (double)result);
        }
    }
}

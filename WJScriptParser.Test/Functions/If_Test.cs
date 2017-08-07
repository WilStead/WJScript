using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class If_Test
    {
        [TestMethod]
        public void If_True()
        {
            var source =
@"
a = 1;
if(a == 1)
{
    a++;
}
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(2, (double)result);
        }

        [TestMethod]
        public void If_False()
        {
            var source =
@"
a = 1;
if(a == 2)
{
    a++;
}
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(1, (double)result);
        }
    }
}

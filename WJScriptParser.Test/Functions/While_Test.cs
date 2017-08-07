using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test.Functions
{
    [TestClass]
    public class While_Test
    {
        [TestMethod]
        public void While()
        {
            var source =
@"
a = 1;
while(a < 5)
{
    a++;
}
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(5, (double)result);
        }

        [TestMethod]
        public void While_Continue()
        {
            var source =
@"
a = 1;
while(a < 5)
{
    if (a == 2)
    {
        a = 6;
        continue;
    }
    a++;
}
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(6, (double)result);
        }

        [TestMethod]
        public void While_Break()
        {
            var source =
@"
a = 1;
while(a < 5)
{
    if (a == 2)
    {
        break;
    }
    a++;
}
return a;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetType() == typeof(double));
            Assert.AreEqual(2, (double)result);
        }
    }
}

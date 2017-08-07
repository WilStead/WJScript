using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WScriptParser.Test
{
    [TestClass]
    public class Parser_Test
    {
        [TestMethod]
        public void Compile_CanCompile()
        {
            var source =
@"
a = 1;
";
            var expected_script = "a=1;";

            var script = Parser.Compile(source);

            Assert.AreEqual(expected_script, script);
        }

        [TestMethod]
        public void Compile_CommentsAreStripped()
        {
            var source =
@"
// comment
a = 1; // comment
";
            var expected_script = "a=1;";

            var script = Parser.Compile(source);

            Assert.AreEqual(expected_script, script);
        }

        [TestMethod]
        public void Compile_MismatchedEnclosuresCompileEmpty()
        {
            var source =
@"
{
    a = 1;
";
            var expected_script = "";

            var script = Parser.Compile(source);

            Assert.AreEqual(expected_script, script);
        }

        [TestMethod]
        public void Execute_EmptyResultIsNull()
        {
            var source = "";

            var result = Parser.Execute(source);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void OOP_Test()
        {
            var source =
@"
a.b = 3;
return !(1 + 2 * 3 - 4 / 2 + 7 % a.b - 2 ^ (1 + 2) + 2) || true && !true;
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.AreEqual(true, (bool)result);
        }

        [TestMethod]
        public void Smoke_Test()
        {
            var source =
@"
a = 1;
b.c = 2;
d = [0,1,2,3,4,5,6,7,8,9];
while (a < 10) {
    if (a == 1) {
        b.c = b.c ^ 2;
    } else if (a % 2 == 0) {
        b.c *= d[a];
    } else {
        b.c -= d[a] * d[a - 2];
    }
    a++;
}
return ""The result is "" + b.c + ""."";
";
            var result = Parser.Execute(source);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("The result is -103.", (string)result);
        }
    }
}

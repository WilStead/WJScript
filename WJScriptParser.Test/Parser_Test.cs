using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace WJScriptParser.Test
{
    public class TestChildArgClass
    {
        public bool B { get; set; }
    }

    public class TestArgClass
    {
        public int X;
        public int Y { get; set; }
        public ICollection<TestChildArgClass> Children { get; set; }
    }

    [TestClass]
    public class Parser_Test
    {
        [TestMethod]
        public void Empty()
        {
            var result = Parser.Evaluate(string.Empty);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Expression()
        {
            var script =
@"1 + 2";
            var result = Parser.Evaluate(script);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Script()
        {
            var script =
@"
let x = 1;
let y = 2;
return x + y;
";
            var result = Parser.Evaluate(script);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Value_Params()
        {
            var result = Parser.Evaluate(string.Empty, 1, 2);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Value_Params_Script()
        {
            var script =
@"
let x = (int)args[0];
let y = (int)args[1];
return x + y;
";
            var result = Parser.Evaluate(script, 1, 2);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Object_Params()
        {
            var result = Parser.Evaluate(string.Empty, new TestArgClass { X = 1, Y = 2 });
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Object_Params_Script()
        {
            var script =
@"
let x = (args[0] as TestArgClass).X;
let y = (args[0] as TestArgClass).Y;
return x + y;
";
            var result = Parser.Evaluate(script, new TestArgClass { X = 1, Y = 2 });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Object_Params_Script_Update()
        {
            var globals = new TestArgClass { X = 1, Y = 2 };
            var script =
@"
let x = (args[0] as TestArgClass).X;
let y = (args[0] as TestArgClass).Y;
(args[0] as TestArgClass).X = x + y;
";
            var result = Parser.Evaluate(script, globals);

            Assert.IsNull(result);
            Assert.AreEqual(3, globals.X);
        }

        [TestMethod]
        public void Object_Child_Params_Script_Update()
        {
            var globals = new TestArgClass
            {
                Children = new List<TestChildArgClass> { new TestChildArgClass { B = true }, new TestChildArgClass { B = true }, new TestChildArgClass { B = false } }
            };
            var script =
@"
(args[0] as TestArgClass).Children.ToList().ForEach(c => c.B = false);
";
            var result = Parser.Evaluate(script, globals);

            Assert.IsNull(result);
            Assert.IsTrue(globals.Children.All(c => !c.B));
        }

        [TestMethod]
        public void OOP_Test()
        {
            var script =
@"
let a = 3;
return !(1 + 2 * 3 - 4 / 2 + 7 % a - 2 ^ (1 + 2) + 2) || true && !true;
";
            var result = Parser.Evaluate(script);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Math_Test()
        {
            var script =
@"
let a = 1;
let b = 0;
b[""c""] = 2;
let d = new int[]{0,1,2,3,4,5,6,7,8,9};
while (a < 10) {
    if (a == 1) {
        b[""c""] = Math.Pow(b[""c""], 2);
    } else if (a % 2 == 0) {
        b[""c""] *= d[a];
    } else {
        b[""c""] -= d[a] * d[a - 2];
    }
    a++;
}
return $""The result is {b[""c""]}."";
";
            var result = Parser.Evaluate(script);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("The result is -103.", (string)result);
        }
    }
}

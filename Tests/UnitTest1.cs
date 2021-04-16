using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CautionReadWrite()
        {
            var docs = SVGToManim.SVGToManim.ReadSVG("https://dev.w3.org/SVG/tools/svgweb/samples/svg-files/caution.svg");
            Assert.AreEqual(4, docs.Count);
            string output = docs.ToManim();
            Assert.IsTrue(output.Length > 0);
            Console.WriteLine(output);
            Assert.IsFalse(output.Contains("NaN"));
        }


        [TestMethod]
        public void CartmanReadWrite()
        {
            var docs = SVGToManim.SVGToManim.ReadSVG("https://dev.w3.org/SVG/tools/svgweb/samples/svg-files/cartman.svg");
            Assert.AreEqual(9, docs.Count);
            string output = docs.ToManim();
            Assert.IsTrue(output.Length > 0);
            Console.WriteLine(output);
            Assert.IsFalse(output.Contains("NaN"));
        }

        [TestMethod]
        public void HeartReadWrite()
        {
            var docs = SVGToManim.SVGToManim.ReadSVG("https://dev.w3.org/SVG/tools/svgweb/samples/svg-files/heart.svg");
            Assert.AreEqual(1, docs.Count);
            string output = docs.ToManim();
            Assert.IsTrue(output.Length > 0);
            Console.WriteLine(output);
            Assert.IsFalse(output.Contains("NaN"));
        }

        [TestMethod]
        public void Linear2GradientReadWrite()
        {
            var docs = SVGToManim.SVGToManim.ReadSVG("https://dev.w3.org/SVG/tools/svgweb/samples/svg-files/lineargradient2.svg");
            Assert.AreEqual(4, docs.Count);
            string output = docs.ToManim();
            Assert.IsTrue(output.Length > 0);
            Console.WriteLine(output);
            Assert.IsFalse(output.Contains("NaN"));
        }

        [TestMethod]
        public void Linear4GradientReadWrite()
        {
            var docs = SVGToManim.SVGToManim.ReadSVG("https://dev.w3.org/SVG/tools/svgweb/samples/svg-files/lineargradient4.svg");
            Assert.AreEqual(2, docs.Count);
            string output = docs.ToManim();
            Assert.IsTrue(output.Length > 0);
            Console.WriteLine(output);
            Assert.IsFalse(output.Contains("NaN"));
        }

        [TestMethod]
        public void WirelessReadWrite()
        {
            var docs = SVGToManim.SVGToManim.ReadSVG("https://dev.w3.org/SVG/tools/svgweb/samples/svg-files/wireless.svg");
            Assert.AreEqual(2, docs.Count);
            string output = docs.ToManim();
            Assert.IsTrue(output.Length > 0);
            Console.WriteLine(output);
            Assert.IsFalse(output.Contains("NaN"));
        }

        [TestMethod]
        public void TomruenReadWrite()
        {
            var docs = SVGToManim.SVGToManim.ReadSVG("https://upload.wikimedia.org/wikipedia/en/f/f1/Tomruen_test.svg");
            Assert.AreEqual(8194, docs.Count);
            string output = docs.ToManim();
            Assert.IsTrue(output.Length > 0);
            Console.WriteLine(output);
            Assert.IsFalse(output.Contains("NaN"));
        }

        [TestMethod]
        public void TestPatternReadWrite()
        {
            var docs = SVGToManim.SVGToManim.ReadSVG("https://upload.wikimedia.org/wikipedia/commons/b/bd/Test.svg");
            Assert.AreEqual(4, docs.Count);
            string output = docs.ToManim();
            Assert.IsTrue(output.Length > 0);
            Console.WriteLine(output);
            Assert.IsFalse(output.Contains("NaN"));
        }
    }
}


using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using StackUsageAnalyzer;

namespace StackUsageAnalyzerTest
{
    using System.Linq;

    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void TestParserEmpty()
        {
            var result = SuFileParser.Parse(new List<string>());

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TestParserSingleLineStatic()
        {
            var result = SuFileParser.Parse(new List<string>() { "usart_serial.h:106:20:usart_serial_getchar 24  static" }).ToList();

            Assert.AreEqual(1, result.Count());

            var res = result.First();

            Assert.AreEqual("usart_serial.h", res.File);
            Assert.AreEqual<uint>(106, res.Line);
            Assert.AreEqual<uint>(20, res.Column);
            Assert.AreEqual("usart_serial_getchar", res.FunctionName);
            Assert.AreEqual<uint>(24, res.Bytes);
            Assert.AreEqual(FunctionStackInfo.Qualifier.Static, res.Qualifiers);
        }

        [TestMethod]
        public void TestParserSingleLineDynamic()
        {
            var result = SuFileParser.Parse(new List<string>() { "usart_serial.h:73:42:usart_serial_getchar 9854  dynamic" }).ToList();

            Assert.AreEqual(1, result.Count());

            var res = result.First();

            Assert.AreEqual("usart_serial.h", res.File);
            Assert.AreEqual<uint>(73, res.Line);
            Assert.AreEqual<uint>(42, res.Column);
            Assert.AreEqual("usart_serial_getchar", res.FunctionName);
            Assert.AreEqual<uint>(9854, res.Bytes);
            Assert.AreEqual(FunctionStackInfo.Qualifier.Dynamic, res.Qualifiers);
        }

        [TestMethod]
        public void TestParserSingleLineDynamicBounded()
        {
            var result = SuFileParser.Parse(new List<string>() { "usart_serial.h:106:20:usart_serial_getchar 24  dynamic,bounded" }).ToList();

            Assert.AreEqual(1, result.Count());

            var res = result.First();

            Assert.AreEqual("usart_serial.h", res.File);
            Assert.AreEqual<uint>(106, res.Line);
            Assert.AreEqual<uint>(20, res.Column);
            Assert.AreEqual("usart_serial_getchar", res.FunctionName);
            Assert.AreEqual<uint>(24, res.Bytes);
            Assert.AreEqual(FunctionStackInfo.Qualifier.Dynamic | FunctionStackInfo.Qualifier.Bounded, res.Qualifiers);
        }

        [TestMethod]
        public void TestParserManyLines()
        {
            var data = new List<string>();
            data.AddRange(TestData.board_init.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
            data.AddRange(TestData.events.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
            data.AddRange(TestData.interrupt_sam_nvic.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
            data.AddRange(TestData.osc8_calib.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));

            var result = SuFileParser.Parse(data).ToList();

            Assert.AreEqual(21, result.Count());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParserSingleLineMalformed()
        {
            // Need ToList for this to throw
            var res = SuFileParser.Parse(new List<string>() { "usart_serial.h:10a:20:usart_serial_getchar 24  dynamic,bounded" }).ToList();
            
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void TestParserFieldOutOfRange()
        {
            // Need ToList for this to throw
            var res = SuFileParser.Parse(new List<string>() { "usart_serial.h:99999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999:20:usart_serial_getchar 24  dynamic,bounded" }).ToList();

        }
    }


}

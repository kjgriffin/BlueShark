using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midnight.PreProcessor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.PreProcessor.Tests
{
    [TestClass()]
    public class DeCommenterTests
    {
        [TestMethod()]
        public void StripSingleLineCommentsTest()
        {
            const string commentseq = "//";

            const string testseq = "This is a test of stuff.\r\n//This too may be a great test.\r\nOn this//line no // more.";
            const string golden = "This is a test of stuff.\r\n\r\nOn this\r\n";

            Assert.AreEqual(golden, testseq.StripSingleLineComments(commentseq));
        }

        [TestMethod()]
        public void StripMultiLineCommentsTest()
        {
            const string startseq = "/*";
            const string endseq = "*/";

            const string testseq = "This is /*\r\n a great // big */\r\n /* test sew \r\n stuff \r\n/*/Here";
            const string golden = "This is \r\n Here";
            Assert.AreEqual(golden, testseq.StripMultiLineComments(startseq, endseq));
        }
    }
}
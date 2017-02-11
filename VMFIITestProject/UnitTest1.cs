using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VMFInstanceInserter;

namespace VMFInstanceInserterTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Initialize()
        {
            if (File.Exists("../../../testmap/instancetest.temp.vmf"))
                File.Delete("../../../testmap/instancetest.temp.vmf");
            if (File.Exists("../../../testmap/vmmtest.vmf"))
                File.Delete("../../../testmap/vmmtest.vmf");
        }
        [TestMethod]
        public void TestFilesExists()
        {
            Assert.IsTrue(File.Exists("../../../testmap/instancetest.vmf"));

        }
        [TestMethod]
        public void TestMain()
        {
            string[] args = new string[] { "../../../testmap/instancetest.vmf", "../../../testmap/instancetest.temp.vmf" };
            VMFInstanceInserter.Program.Main(args);
            Assert.IsTrue(File.Exists("../../../testmap/instancetest.temp.vmf"));
        }
        [TestMethod]
        public void TestMainVmm()
        {
            string[] args = new string[] { "../../../testmap/vmmtest.vmm", "../../../testmap/vmmtest.vmf" };
            VMFInstanceInserter.Program.Main(args);
            Assert.IsTrue(File.Exists("../../../testmap/vmmtest.vmf"));
        }
    }
}
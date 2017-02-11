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
            if (File.Exists("../../../testmap/vmmtest.temp.vmf"))
                File.Delete("../../../testmap/vmmtest.temp.vmf");
        }
        [TestMethod]
        public void TestFilesExists()
        {
            Assert.IsTrue(File.Exists("../../../testmap/instancetest.vmf"));

        }
        [TestMethod]
        public void TestMain()
        {
            string[] args = new string[] { "../../../testmap/instancetest.vmf"};
            VMFInstanceInserter.Program.Main(args);
            Assert.IsTrue(File.Exists("../../../testmap/instancetest.temp.vmf"));
        }
        [TestMethod]
        public void TestMainVmm()
        {
            string[] args = new string[] { "../../../testmap/vmmtest.vmm"};
            VMFInstanceInserter.Program.Main(args);
            Assert.IsTrue(File.Exists("../../../testmap/vmmtest.temp.vmf"));
        }
        [TestMethod]
        public void TestVmmFile()
        {
            VMMFile vmmfile = new VMMFile("../../../testmap/vmmtest.vmm");
            Assert.IsTrue(vmmfile.vmfs.Count == 2);
            Assert.IsTrue(vmmfile.vmfs[0]["File"].Equals("main.vmf"));
            Assert.IsTrue(vmmfile.vmfs[0]["TopLevel"].Equals("1"));
            Assert.IsTrue(vmmfile.vmfs[1]["File"].Equals("second.vmf"));
            Assert.IsTrue(vmmfile.vmfs[1]["Name"].Equals("second"));
        }
    }
}
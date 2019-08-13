using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoardGamePaint;
using System.IO;

namespace BoardGamePaintTest
{
    [TestClass]
    public class UnitTest1
    {
        public string getFilePath(string baseFileName)
        {
            string directory = Directory.GetCurrentDirectory();
            directory = Directory.GetParent(directory).Parent.Parent.ToString();
            directory += "\\Assets";
            return directory + "\\" + baseFileName;
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(3, (1 + 3));
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(3, (1 + 2));
        }

        [TestMethod]
        public void TestMethod4()
        {
            Assert.AreEqual(3, (1 + 2));
        }

        [TestMethod]
        public void FileTest1()
        {
            Assert.IsTrue(File.Exists(getFilePath("exit.png")));
        }
        [TestMethod]
        public void FileTest2()
        {
            FileStream file = File.Create("testfile.tst");
            Assert.IsNotNull(file);
            file.Close();
        }
        [TestMethod]
        public void FileTest3()
        {
            string directory = Directory.GetCurrentDirectory();
            directory = Directory.GetParent(directory).Parent.Parent.ToString();
            directory += "\\Assets";
            Assert.AreEqual("Debug", directory);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System;
using static System.Net.Mime.MediaTypeNames;

[TestClass]
public class ManagersTest
{
    public ManagersTest()
    {
    }

    public void initPrep()
    {
        Managers.init(null);
    }

    [TestMethod]
    public void initTest1()
    {
        //Managers.init();
        //Assert.AreEqual(1, Managers.initNumber);
        Assert.IsNotNull(new Managers(null));
    }

    //[TestMethod]
    //public void initTest2()
    //{
    //    initPrep();
    //    initPrep();
    //    Assert.AreEqual(1, Managers.initNumber);
    //}
}

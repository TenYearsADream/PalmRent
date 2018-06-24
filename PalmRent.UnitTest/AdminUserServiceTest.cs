using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PalmRent.Service;

namespace PalmRent.UnitTest
{
    /// <summary>
    /// AdminUserServiceTest 的摘要说明
    /// </summary>
    [TestClass]
    public class AdminUserServiceTest
    {
        private AdminUserService userService = new AdminUserService();
        public AdminUserServiceTest()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        [TestMethod]
        public void TestAddAdminUser()
        {
            long uid =
                userService.AddAdminUser("abc", "189181", "123", "123@qq.com", null);
            var user = userService.GetById(uid);
            Assert.AreEqual(user.Name, "abc");
            Assert.AreEqual(user.PhoneNum, "189181");
            Assert.AreEqual(user.Email, "123@qq.com");
            Assert.IsNull(user.CityId);
            Assert.IsTrue(userService.CheckLogin("189181", "123"));
            Assert.IsFalse(userService.CheckLogin("189181", "abc"));
            userService.GetAll();
            Assert.IsNotNull(userService.GetByPhoneNum("189181"));
            //userService.MarkDeleted(uid);//为了保证TestCase可以重复执行，那么把创建的数据干掉
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO:  在此处添加测试逻辑
            //
        }
    }
}

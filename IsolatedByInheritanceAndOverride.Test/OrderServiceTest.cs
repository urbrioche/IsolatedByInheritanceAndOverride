﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace IsolatedByInheritanceAndOverride.Test
{
    /// <summary>
    /// OrderServiceTest 的摘要描述
    /// </summary>
    [TestClass]
    public class OrderServiceTest
    {
        public OrderServiceTest()
        {
            //
            // TODO:  在此加入建構函式的程式碼
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///取得或設定提供目前測試回合
        ///的相關資訊與功能的測試內容。
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region 其他測試屬性

        //
        // 您可以使用下列其他屬性撰寫您的測試:
        //
        // 執行該類別中第一項測試前，使用 ClassInitialize 執行程式碼
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在類別中的所有測試執行後，使用 ClassCleanup 執行程式碼
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在執行每一項測試之前，先使用 TestInitialize 執行程式碼
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在執行每一項測試之後，使用 TestCleanup 執行程式碼
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion 其他測試屬性

        [TestMethod]
        public void Test_SyncBookOrders_3_Orders_Only_2_book_order()
        {
            // hard to isolate dependency to unit test

            //var target = new OrderService();
            //target.SyncBookOrders();
            var target = new OrderServiceForTest();
            target.SetOrder(new List<Order>()
            {
                new Order() {Type = "Book"},
                new Order() {Type = "CD"},
                new Order() {Type = "Book"},
            });
            var mockBookDao = Substitute.For<IBookDao>();
            target.SetBookDao(mockBookDao);
            target.SyncBookOrders();
            //mockBookDao.ReceivedWithAnyArgs(2).Insert(new Order());
            mockBookDao.Received(2).Insert(Arg.Is<Order>(o => o.Type == "Book"));
            //target
        }
    }

    public class OrderServiceForTest : OrderService
    {
        private List<Order> _orders;
        private IBookDao _bookDao;

        public void SetBookDao(IBookDao bookDao)
        {
            _bookDao = bookDao;
        }

        public void SetOrder(List<Order> orders)
        {
            _orders = orders;
        }

        protected override IBookDao GetBookDao()
        {
            return _bookDao;
        }

        protected override List<Order> GetOrders()
        {
            return _orders;
        }
    }
}
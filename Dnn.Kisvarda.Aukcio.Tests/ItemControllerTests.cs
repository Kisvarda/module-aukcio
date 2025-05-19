using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Controllers;
using Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Models;
using Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Components;
using Moq;
using NUnit.Framework;

namespace Dnn.Kisvarda.Aukcio.Tests
{
    [TestFixture]
    public class ItemControllerTests
    {
        private Mock<IItemManager> _mockItemManager;
        private Mock<IBidManager> _mockBidManager;
        private ItemController _controller;

        [SetUp]
        public void Setup()
        {
            _mockItemManager = new Mock<IItemManager>();
            _mockBidManager = new Mock<IBidManager>();

            ItemManager.Instance = _mockItemManager.Object;
            BidManager.Instance = _mockBidManager.Object;

            _controller = new ItemController();
            _controller.ControllerContext = new ControllerContext();
            _controller.TempData = new TempDataDictionary();
        }

        [Test]
        public void Auctions_Get_ReturnsViewWithItemList_WhenItemExists()
        {
            var item = new Item { ItemId = 2, Title = "Laptop", HighestBid = 1000, MinimumBidIncrement = 100 };
            _mockItemManager.Setup(m => m.GetItem(2, It.IsAny<int>())).Returns(item);

            var result = _controller.Auctions() as ViewResult;
            var model = result.Model as List<Item>;

            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(1));
            Assert.That(model[0].ItemId, Is.EqualTo(2));
        }

        [Test]
        public void Auctions_Get_ReturnsHttpNotFound_WhenItemIsNull()
        {
            _mockItemManager.Setup(m => m.GetItem(2, It.IsAny<int>())).Returns((Item)null);

            var result = _controller.Auctions();

            Assert.That(result, Is.InstanceOf<HttpNotFoundResult>());
        }

        [Test]
        public void Auctions_Post_ReturnsError_WhenBidTooLow()
        {
            var item = new Item { ItemId = 1, HighestBid = 1000, MinimumBidIncrement = 200 };
            _mockItemManager.Setup(m => m.GetItem(1, It.IsAny<int>())).Returns(item);

            var result = _controller.Auctions(1, 1, 1100);

            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(result, Is.InstanceOf<RedirectToRouteResult>());
        }

        [Test]
        public void Auctions_Post_SubmitsBid_WhenValid()
        {
            var item = new Item { ItemId = 1, HighestBid = 1000, MinimumBidIncrement = 200 };
            _mockItemManager.Setup(m => m.GetItem(1, It.IsAny<int>())).Returns(item);

            var result = _controller.Auctions(1, 5, 1300) as RedirectToRouteResult;

            _mockBidManager.Verify(m => m.CreateBid(It.Is<Bid>(b => b.ItemId == 1 && b.UserId == 5 && b.Amount == 1300)), Times.Once);
            _mockItemManager.Verify(m => m.UpdateItem(It.Is<Item>(i => i.HighestBid == 1300)), Times.Once);
            Assert.That(_controller.TempData["SuccessMessage"], Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void Auctions_Post_ReturnsNotFound_WhenItemNull()
        {
            _mockItemManager.Setup(m => m.GetItem(1, It.IsAny<int>())).Returns((Item)null);

            var result = _controller.Auctions(1, 1, 1500);

            Assert.That(result, Is.InstanceOf<HttpNotFoundResult>());
        }
    }

    // Interface mocks for injection
    public interface IItemManager
    {
        Item GetItem(int itemId, int moduleId);
        void UpdateItem(Item item);
    }

    public interface IBidManager
    {
        void CreateBid(Bid bid);
    }
}
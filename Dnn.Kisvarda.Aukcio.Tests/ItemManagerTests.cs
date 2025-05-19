using Moq;
using NUnit.Framework;
using Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Models;
using Dnn.Kisvarda.Aukcio.Tests;

namespace Dnn.Kisvarda.Aukcio.Tests
{
    public class ItemManagerForTest
    {
        private readonly IDataContext _context;

        public ItemManagerForTest(IDataContext context)
        {
            _context = context;
        }

        public void CreateItem(Item t)
        {
            var rep = _context.GetRepository<Item>();
            rep.Insert(t);
        }

        public Item GetItem(int itemId, int moduleId)
        {
            var rep = _context.GetRepository<Item>();
            return rep.GetById(itemId, moduleId);
        }

        public void UpdateItem(Item t)
        {
            var rep = _context.GetRepository<Item>();
            rep.Update(t);
        }

        public void DeleteItem(Item t)
        {
            var rep = _context.GetRepository<Item>();
            rep.Delete(t);
        }
    }

    [TestFixture]
    public class ItemManagerTests
    {
        [Test]
        public void CreateItem_ShouldInsertItemToRepository()
        {
            var mockRepository = new Mock<IRepository<Item>>();
            var mockContext = new Mock<IDataContext>();

            var testItem = new Item
            {
                ItemId = 1,
                Title = "Teszt tétel",
                Description = "Leírás",
                HighestBid = 0
            };

            mockContext.Setup(ctx => ctx.GetRepository<Item>())
                       .Returns(mockRepository.Object);

            var itemManager = new ItemManagerForTest(mockContext.Object);

            itemManager.CreateItem(testItem);

            mockRepository.Verify(r => r.Insert(It.Is<Item>(i => i == testItem)), Times.Once);
        }

        [Test]
        public void GetItem_ShouldReturnCorrectItemFromRepository()
        {
            var mockRepository = new Mock<IRepository<Item>>();
            var mockContext = new Mock<IDataContext>();

            var expectedItem = new Item
            {
                ItemId = 2,
                Title = "Második tétel",
                Description = "Második leírás",
                HighestBid = 1000
            };

            mockRepository.Setup(r => r.GetById(2, 1)).Returns(expectedItem);
            mockContext.Setup(ctx => ctx.GetRepository<Item>()).Returns(mockRepository.Object);

            var itemManager = new ItemManagerForTest(mockContext.Object);

            var result = itemManager.GetItem(2, 1);

            Assert.That(result.ItemId, Is.EqualTo(expectedItem.ItemId));
            Assert.That(result.Title, Is.EqualTo(expectedItem.Title));
            Assert.That(result.Description, Is.EqualTo(expectedItem.Description));
            Assert.That(result.HighestBid, Is.EqualTo(expectedItem.HighestBid));
        }

        [Test]
        public void UpdateItem_ShouldCallUpdateOnRepository()
        {
            var mockRepository = new Mock<IRepository<Item>>();
            var mockContext = new Mock<IDataContext>();

            var updatedItem = new Item
            {
                ItemId = 3,
                Title = "Frissített tétel",
                Description = "Új leírás",
                HighestBid = 2500
            };

            mockContext.Setup(ctx => ctx.GetRepository<Item>())
                       .Returns(mockRepository.Object);

            var itemManager = new ItemManagerForTest(mockContext.Object);

            itemManager.UpdateItem(updatedItem);

            mockRepository.Verify(r => r.Update(It.Is<Item>(i => i == updatedItem)), Times.Once);
        }

        [Test]
        public void DeleteItem_ShouldCallDeleteOnRepository()
        {
            var mockRepository = new Mock<IRepository<Item>>();
            var mockContext = new Mock<IDataContext>();

            var itemToDelete = new Item
            {
                ItemId = 4,
                Title = "Törlendő tétel",
                Description = "Rég nem elérhető",
                HighestBid = 500
            };

            mockContext.Setup(ctx => ctx.GetRepository<Item>())
                       .Returns(mockRepository.Object);

            var itemManager = new ItemManagerForTest(mockContext.Object);

            itemManager.DeleteItem(itemToDelete);

            mockRepository.Verify(r => r.Delete(It.Is<Item>(i => i == itemToDelete)), Times.Once);
        }
    }
}
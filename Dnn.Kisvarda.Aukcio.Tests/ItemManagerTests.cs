using Moq;
using NUnit.Framework;
using Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Models;

namespace Dnn.Kisvarda.Aukcio.Tests
{
    // Saját interfész a repository-hoz (DNN nélkül)
    public interface IRepository<T>
    {
        void Insert(T entity);
    }

    // Saját interfész a DataContext-hez
    public interface IDataContext
    {
        IRepository<T> GetRepository<T>();
    }

    // Tesztelhető ItemManager-változat, ami konstruktoron keresztül kapja a függőségeket
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
    }

    [TestFixture]
    public class ItemManagerTests
    {
        [Test]
        public void CreateItem_ShouldInsertItemToRepository()
        {
            // Arrange
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

            // Act
            itemManager.CreateItem(testItem);

            // Assert
            mockRepository.Verify(r => r.Insert(It.Is<Item>(i => i == testItem)), Times.Once);
        }
    }
}

using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Dnn.Kisvarda.Aukcio.Tests
{
    public class AuctionLogicForTest
    {
        private readonly IDataContext _context;

        public AuctionLogicForTest(IDataContext context)
        {
            _context = context;
        }

        public bool TryPlaceBid(int itemId, decimal bidAmount, out string errorMessage)
        {
            var itemRepo = _context.GetRepository<TestItem>();
            var bidRepo = _context.GetRepository<TestBid>();

            var item = itemRepo.GetById(itemId, 0);
            if (item == null)
            {
                errorMessage = "Tétel nem található.";
                return false;
            }

            if (bidAmount <= item.HighestBid)
            {
                errorMessage = "A licit nem lehet alacsonyabb vagy egyenlő a jelenlegi legmagasabbnál.";
                return false;
            }

            bidRepo.Insert(new TestBid { ItemId = itemId, BidAmount = bidAmount });
            item.HighestBid = bidAmount;
            itemRepo.Update(item);

            errorMessage = null;
            return true;
        }
    }

    [TestFixture]
    public class AuctionLogicTests
    {
        [Test]
        public void TryPlaceBid_ShouldRejectTooLowBid()
        {
            var mockItemRepo = new Mock<IRepository<TestItem>>();
            var mockBidRepo = new Mock<IRepository<TestBid>>();
            var mockContext = new Mock<IDataContext>();

            var item = new TestItem { ItemId = 1, HighestBid = 1000 };
            mockItemRepo.Setup(r => r.GetById(1, 0)).Returns(item);

            mockContext.Setup(c => c.GetRepository<TestItem>()).Returns(mockItemRepo.Object);
            mockContext.Setup(c => c.GetRepository<TestBid>()).Returns(mockBidRepo.Object);

            var logic = new AuctionLogicForTest(mockContext.Object);

            var success = logic.TryPlaceBid(1, 1000, out string error);

            Assert.That(success, Is.False);
            Assert.That(error, Is.EqualTo("A licit nem lehet alacsonyabb vagy egyenlő a jelenlegi legmagasabbnál."));
            mockBidRepo.Verify(r => r.Insert(It.IsAny<TestBid>()), Times.Never);
            mockItemRepo.Verify(r => r.Update(It.IsAny<TestItem>()), Times.Never);
        }

        [Test]
        public void TryPlaceBid_ShouldAcceptValidBid()
        {
            var mockItemRepo = new Mock<IRepository<TestItem>>();
            var mockBidRepo = new Mock<IRepository<TestBid>>();
            var mockContext = new Mock<IDataContext>();

            var item = new TestItem { ItemId = 1, HighestBid = 1000 };
            mockItemRepo.Setup(r => r.GetById(1, 0)).Returns(item);

            mockContext.Setup(c => c.GetRepository<TestItem>()).Returns(mockItemRepo.Object);
            mockContext.Setup(c => c.GetRepository<TestBid>()).Returns(mockBidRepo.Object);

            var logic = new AuctionLogicForTest(mockContext.Object);

            var success = logic.TryPlaceBid(1, 1200, out string error);

            Assert.That(success, Is.True);
            Assert.That(error, Is.Null);
            mockBidRepo.Verify(r => r.Insert(It.Is<TestBid>(b => b.ItemId == 1 && b.BidAmount == 1200)), Times.Once);
            mockItemRepo.Verify(r => r.Update(It.Is<TestItem>(i => i.HighestBid == 1200)), Times.Once);
        }

        [Test]
        public void TryPlaceBid_ShouldReturnError_WhenItemIsNull()
        {
            var mockItemRepo = new Mock<IRepository<TestItem>>();
            var mockBidRepo = new Mock<IRepository<TestBid>>();
            var mockContext = new Mock<IDataContext>();

            mockItemRepo.Setup(r => r.GetById(99, 0)).Returns((TestItem)null);
            mockContext.Setup(c => c.GetRepository<TestItem>()).Returns(mockItemRepo.Object);
            mockContext.Setup(c => c.GetRepository<TestBid>()).Returns(mockBidRepo.Object);

            var logic = new AuctionLogicForTest(mockContext.Object);

            var success = logic.TryPlaceBid(99, 1500, out string error);

            Assert.That(success, Is.False);
            Assert.That(error, Is.EqualTo("Tétel nem található."));
        }

        [Test]
        public void TryPlaceBid_ShouldReturnError_WhenBidAmountIsZero()
        {
            var mockItemRepo = new Mock<IRepository<TestItem>>();
            var mockBidRepo = new Mock<IRepository<TestBid>>();
            var mockContext = new Mock<IDataContext>();

            var item = new TestItem { ItemId = 1, HighestBid = 1000 };
            mockItemRepo.Setup(r => r.GetById(1, 0)).Returns(item);

            mockContext.Setup(c => c.GetRepository<TestItem>()).Returns(mockItemRepo.Object);
            mockContext.Setup(c => c.GetRepository<TestBid>()).Returns(mockBidRepo.Object);

            var logic = new AuctionLogicForTest(mockContext.Object);

            var success = logic.TryPlaceBid(1, 0, out string error);

            Assert.That(success, Is.False);
            Assert.That(error, Is.EqualTo("A licit nem lehet alacsonyabb vagy egyenlő a jelenlegi legmagasabbnál."));
        }

    }

    // Dummy osztályok a teszteléshez – átnevezve, hogy ne ütközzenek
    public class TestItem
    {
        public int ItemId { get; set; }
        public string Title { get; set; }
        public decimal HighestBid { get; set; }
    }

    public class TestBid
    {
        public int BidId { get; set; }
        public int ItemId { get; set; }
        public decimal BidAmount { get; set; }
    }

    public interface IDataContext
    {
        IRepository<T> GetRepository<T>() where T : class;
    }

    public interface IRepository<T>
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> Get();
        T GetById(int id, int moduleId);
    }
}

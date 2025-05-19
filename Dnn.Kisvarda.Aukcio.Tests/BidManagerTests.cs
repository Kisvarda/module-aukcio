using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Dnn.Kisvarda.Aukcio.Tests;

namespace Dnn.Kisvarda.Aukcio.Tests
{
    public class BidManagerForTest
    {
        private readonly IDataContext _context;

        public BidManagerForTest(IDataContext context)
        {
            _context = context;
        }

        public void CreateBid(Bid bid)
        {
            var repo = _context.GetRepository<Bid>();
            repo.Insert(bid);
        }

        public IEnumerable<Bid> GetBidsByItemId(int itemId)
        {
            var repo = _context.GetRepository<Bid>();
            return repo.Get().Where(b => b.ItemId == itemId);
        }
    }

    [TestFixture]
    public class BidManagerTests
    {
        [Test]
        public void CreateBid_ShouldInsertBidIntoRepository()
        {
            var mockRepository = new Mock<IRepository<Bid>>();
            var mockContext = new Mock<IDataContext>();

            var testBid = new Bid
            {
                BidId = 1,
                ItemId = 5,
                UserId = 2,
                BidAmount = 1500
            };

            mockContext.Setup(ctx => ctx.GetRepository<Bid>())
                       .Returns(mockRepository.Object);

            var bidManager = new BidManagerForTest(mockContext.Object);

            bidManager.CreateBid(testBid);

            mockRepository.Verify(r => r.Insert(It.Is<Bid>(b => b == testBid)), Times.Once);
        }

        [Test]
        public void GetBidsByItemId_ShouldReturnOnlyMatchingBids()
        {
            var mockRepository = new Mock<IRepository<Bid>>();
            var mockContext = new Mock<IDataContext>();

            var bidList = new List<Bid>
            {
                new Bid { BidId = 1, ItemId = 3, UserId = 1, BidAmount = 1000 },
                new Bid { BidId = 2, ItemId = 3, UserId = 2, BidAmount = 1200 },
                new Bid { BidId = 3, ItemId = 4, UserId = 1, BidAmount = 800 }
            };

            mockRepository.Setup(r => r.Get()).Returns(bidList);
            mockContext.Setup(ctx => ctx.GetRepository<Bid>()).Returns(mockRepository.Object);

            var bidManager = new BidManagerForTest(mockContext.Object);

            var result = bidManager.GetBidsByItemId(3).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(b => b.ItemId == 3));
        }
    }

    public class Bid
    {
        public int BidId { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public decimal BidAmount { get; set; }
    }
}

namespace AuctionTester
{
    public class Item
    {
        public int ItemId { get; set; }
        public decimal HighestBid { get; set; }
        public decimal MinimumBidIncrement { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ImageUrl { get; set; }
        public int ModuleId { get; set; }
        public decimal StartingPrice { get; set; }
        public System.DateTime? AuctionEndTime { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace AuctionTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tesztelés indítása...\n");

            // Licit validáció tesztek
            RunBidValidationTests();

            // Aukció lejárás kezelése tesztek
            RunAuctionEndTimeTests();

            Console.WriteLine("\nTesztelés befejezve.");
            Console.ReadLine();
        }

        static void RunBidValidationTests()
        {
            Console.WriteLine("\n--- Licit validáció tesztek ---\n");

            Item item = new Item
            {
                ItemId = 1,
                ModuleId = 1,
                HighestBid = 100m,
                MinimumBidIncrement = 10m
            };

            List<decimal> testBids = new List<decimal> { 120m, 130m, 131m, -50m, 0m, 1_000_000m, 1_000_020m };

            foreach (var userBidAmount in testBids)
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine($"Current highest bid: {item.HighestBid}");
                Console.WriteLine($"Minimum increment: {item.MinimumBidIncrement}");
                Console.WriteLine($"User bid amount: {userBidAmount}");

                if (userBidAmount <= 0)
                {
                    Console.WriteLine("Hiba: A licit nem lehet nulla vagy negatív!");
                }
                else if (userBidAmount < item.HighestBid + item.MinimumBidIncrement)
                {
                    Console.WriteLine("Hiba: A licit túl alacsony!");
                }
                else
                {
                    Console.WriteLine("Sikeres licitálás!");
                    item.HighestBid = userBidAmount;
                }

                Console.WriteLine("--------------------------------------------\n");
            }
        }

        static void RunAuctionEndTimeTests()
        {
            Console.WriteLine("\n--- Aukció lejárás kezelése tesztek ---\n");

            Item expiredItem = new Item
            {
                ItemId = 2,
                ModuleId = 1,
                HighestBid = 100m,
                MinimumBidIncrement = 10m,
                AuctionEndTime = DateTime.UtcNow.AddHours(-2) // már lejárt
            };

            Item activeItem = new Item
            {
                ItemId = 3,
                ModuleId = 1,
                HighestBid = 200m,
                MinimumBidIncrement = 20m,
                AuctionEndTime = DateTime.UtcNow.AddHours(1) // még aktív
            };

            TestBidOnItem(expiredItem, 120m); // Lejárt aukció
            TestBidOnItem(activeItem, 230m);  // Aktív aukció
        }

        static void TestBidOnItem(Item item, decimal userBidAmount)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine($"Item ID: {item.ItemId}");
            Console.WriteLine($"Current highest bid: {item.HighestBid}");
            Console.WriteLine($"Minimum increment: {item.MinimumBidIncrement}");
            Console.WriteLine($"Auction end time: {item.AuctionEndTime} (UTC)");
            Console.WriteLine($"User bid amount: {userBidAmount}");

            if (item.AuctionEndTime.HasValue && item.AuctionEndTime.Value < DateTime.UtcNow)
            {
                Console.WriteLine("Hiba: Az aukció már lezárult, nem lehet licitálni!");
            }
            else if (userBidAmount < item.HighestBid + item.MinimumBidIncrement)
            {
                Console.WriteLine("Hiba: A licit túl alacsony!");
            }
            else
            {
                Console.WriteLine("Sikeres licitálás!");
                item.HighestBid = userBidAmount;
            }

            Console.WriteLine("--------------------------------------------\n");
        }
    }
}

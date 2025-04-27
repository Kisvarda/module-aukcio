using System;
using System.Collections.Generic;

namespace AuctionTester
{
    class Program
    {
        static void Main(string[] args)
        {
            // Alap aukciós tárgy
            var item = new Item
            {
                ItemId = 1,
                ModuleId = 1,
                HighestBid = 100m,
                MinimumBidIncrement = 10m
            };

            // Tesztelendő licit összegek logikus sorrendben
            List<decimal> testBids = new List<decimal>
            {
                120m,      // sikeres, normál licit
                130m,      // sikeres, pont határon lévő licit
                131m,      // túl alacsony licit
                -50m,      // negatív licit
                0m,        // nulla licit
                1_000_000m, // nagyon nagy licit
                1_000_020m // újabb sikeres licit a nagy HighestBid után
            };

            Console.WriteLine("Tesztelés indítása...\n");

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

            Console.WriteLine("Tesztelés befejezve.");
            Console.ReadLine();
        }
    }
}

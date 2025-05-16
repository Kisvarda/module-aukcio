/*
' Copyright (c) 2025 Kisvarda
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Components;
using Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Models;
using System;
using System.Collections.Generic;
using System.Web.Compilation;
using System.Web.Mvc;

namespace Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Controllers
{

    [DnnHandleError]
    public class ItemController : DnnController
    {

        [HttpGet]
        public ActionResult Auctions()
        {
               
                
                var item1 = ItemManager.Instance.GetItem(1, ModuleContext.ModuleId);


                if (item1 == null)
                {
                    return HttpNotFound("Item not found.");
                }

                return View(new List<Item> { item1 });
        }

        // public ActionResult Auctions()
        //{

        //    Item newItem = new Item()
        //    {
        //        ItemId = 1,
        //        ItemName = "teszt",
        //        ItemDescription = "teszt",
        //        ImageUrl = "facebook.com",
        //        ModuleId = 1,
        //        HighestBid = 100,
        //        HighestBidUserId = 2,
        //        AuctionEndTime = DateTime.Now,
        //        MinimumBidIncrement = 1,
        //        StartingPrice = 1
        //    };

        //    return View(newItem);
        //}

        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public ActionResult Auctions(int? ItemId, int? UserId, decimal? BidAmount)

        {

            if (ModuleContext.ModuleId == null)
            {
                return HttpNotFound("Item not found.");

            }

            else
            {
               
                var item = ItemManager.Instance.GetItem(ItemId ?? 0, ModuleContext.ModuleId);

                if (item == null)
                {
                    return HttpNotFound("Item not found.");
                }

                if (BidAmount < item.HighestBid + item.MinimumBidIncrement)
                {
                    ModelState.AddModelError("", "Bid amount must be higher than the current highest bid plus the minimum increment.");
                    return RedirectToAction("Index");
                }

                var bid = new Bid
                {
                    ItemId = ItemId ?? 0,
                    UserId = UserId ?? 0,
                    Amount = BidAmount ?? 0,
                    BidTime = DateTime.UtcNow
                };

                BidManager.Instance.CreateBid(bid);

                item.HighestBid = BidAmount;
                ItemManager.Instance.UpdateItem(item);

                TempData["SuccessMessage"] = "Your bid has been successfully submitted!";

                return RedirectToAction("Index");

            }
        }
    }
}
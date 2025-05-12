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

using DotNetNuke.Data;
using DotNetNuke.Framework;
using Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Models;
using System.Collections.Generic;

namespace Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Components
{
    public interface IItemManager
    {
        void CreateItem(Item t);
        void DeleteItem(int itemId, int moduleId);
        void DeleteItem(Item t);
        IEnumerable<Item> GetItems(int moduleId);
        Item GetItem(int itemId, int moduleId);
        void UpdateItem(Item t);
    }

    public class ItemManager : ServiceLocator<IItemManager, ItemManager>, IItemManager
    {
        public void CreateItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Insert(t);
            }
        }

        public void DeleteItem(int itemId, int moduleId)
        {
            var t = GetItem(itemId, moduleId);
            DeleteItem(t);
        }

        public void DeleteItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Delete(t);
            }
        }

        public IEnumerable<Item> GetItems(int moduleId)
        {
            IEnumerable<Item> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                t = rep.Get(moduleId);
            }
            return t;
        }

        public Item GetItem(int itemId, int moduleId)
        {
            Item t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                t = rep.GetById(itemId, moduleId);
            }
            return t;
        }

        public void UpdateItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Update(t);
            }
        }

        protected override System.Func<IItemManager> GetFactory()
        {
            return () => new ItemManager();
        }
    }

    public class BidManager
    {
        private static readonly BidManager _instance = new BidManager();

        public static BidManager Instance => _instance;

        private BidManager() { }

        public void CreateBid(Bid bid)
        {
            using (var context = DataContext.Instance())
            {
                var repo = context.GetRepository<Bid>();
                repo.Insert(bid);
            }

        }

        public IEnumerable<Bid> GetBidsByItemId(int itemId)
        {
            using (var context = DataContext.Instance())
            {
                var repo = context.GetRepository<Bid>();
                return repo.Get(itemId);
            }

        }
    }
}

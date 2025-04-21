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

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Web.Caching;

namespace Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Models
{
    [TableName("Aukcio_Bids")]
    [PrimaryKey("BidId", AutoIncrement = true)]
    [Cacheable("Bids", CacheItemPriority.Default, 20)]
    [Scope("ItemId")]
    public class Bid
    {
        public int BidId { get; set; } = -1;
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; } = DateTime.UtcNow;
    }
}

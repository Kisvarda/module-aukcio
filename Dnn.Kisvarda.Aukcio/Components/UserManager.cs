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
using System.Collections.Generic;

namespace Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Components
{
    public class UserManager
    {
        private static readonly UserManager _instance = new UserManager();

        public static UserManager Instance => _instance;

        private UserManager() { }

      
        public IEnumerable<Models.User> GetAllUsers()
        {
            using (var context = DataContext.Instance())
            {
                var repo = context.GetRepository<Models.User>();
                return repo.Get();
            }
        }

       
        public Models.User GetUserById(int userId)
        {
            using (var context = DataContext.Instance())
            {
                var repo = context.GetRepository<Models.User>();
                return repo.GetById(userId);
            }
        }

 
        public void CreateUser(Models.User user)
        {
            using (var context = DataContext.Instance())
            {
                var repo = context.GetRepository<Models.User>();
                repo.Insert(user);
            }
        }

        public void UpdateUser(Models.User user)
        {
            using (var context = DataContext.Instance())
            {
                var repo = context.GetRepository<Models.User>();
                repo.Update(user);
            }
        }


        public void DeleteUser(int userId)
        {
            using (var context = DataContext.Instance())
            {
                var repo = context.GetRepository<Models.User>();
                var user = repo.GetById(userId);
                if (user != null)
                {
                    repo.Delete(user);
                }
            }
        }
    }
}

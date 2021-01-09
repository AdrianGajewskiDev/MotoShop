using MotoShop.Data.Models.User;
using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response.Administration
{
    public class GetAllUsersResponseModel<T>
    {
        public IEnumerable<T> Users { get; set; }
    }
}

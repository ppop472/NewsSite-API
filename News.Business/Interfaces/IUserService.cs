using Microsoft.Extensions.Hosting;
using News.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Business.Interfaces
{
    public interface IUserService
    {
        User Login(string mail, string password);

        User Add(string mail, string password, string firstName, string lastName);

        User Update(string mail, string password, int sourceid, int telephonnumber);
        List<User> Get();
    }
}

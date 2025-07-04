using News.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User Login(string mail, string password);

        User Add(string mail, string password, string firstName, string lastName);

        User Update(string mail, string password, int sourceid, int telephonenumber);

        List<User> Get();
    }
}
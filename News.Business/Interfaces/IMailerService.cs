using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.Business.Models;
using News.Repositories.Models;

namespace News.Business.Interfaces
{
    public interface IMailerService
    {
        Mail Send();
    }
}

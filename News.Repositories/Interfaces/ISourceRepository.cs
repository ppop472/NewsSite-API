using News.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Repositories.Interfaces
{
    public interface ISourceRepository
    {
        IEnumerable<Sources> Get();
    }
}

using News.Business.Models;
using News.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Business.Interfaces
{
    public interface INewsItemsService
    {
        NewsItemDto ToNewsItemDto(NewsItem newsItem, Languages languages, Sources sources);
        NewsItem ToNewsItem(NewsItemDto newsItemDto);
        NewsItemDto Get(int id);
        IEnumerable<NewsItemDto> Get();
        NewsItemDto Create(NewsItemDto newsItemDto);
        NewsItemDto Update(NewsItemDto newsItemDto);
        void Delete(NewsItemDto newsItemDto);
    }
}
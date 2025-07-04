using News.Repositories.Models;

namespace News.Repositories.Interfaces
{
    public interface INewsItemsRepository
    {
        NewsItem Get(int id);
        IEnumerable<NewsItem> Get();

        //NewsItem CheckNewNews();
        NewsItem Create(NewsItem newsItem);
        NewsItem Update(NewsItem newsItem);
        void Delete(NewsItem newsItem);
        List<NewsItem> NotSent();
    }
}

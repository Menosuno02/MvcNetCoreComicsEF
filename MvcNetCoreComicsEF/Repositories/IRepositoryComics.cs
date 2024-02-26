using MvcNetCoreComicsEF.Models;

namespace MvcNetCoreComicsEF.Repositories
{
    public interface IRepositoryComics
    {
        public Task<List<Comic>> GetComicsAsync();
        public Task<Comic> FindComicAsync(int idComic);
        public Task CreateComicAsync(Comic comic);
        public Task DeleteComicAsync(int idComic);
    }
}



using NewsApiDomin.Models;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using NewsApiServies.CRUD.Interfaces;
using Services.CRUD;

namespace NewsApiServies.CRUD
{
    public class ImageService:BaseCRUDService<Image> ,IImageService
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public ImageService(IUnitOfWorkRepo unitOfWorkRepo) :base(unitOfWorkRepo.ImageRepository)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }

        public async Task<List<Image>> GetAllByIdArticleAsync(int articleId)
        {
            List<Image> images = await unitOfWorkRepo.ImageRepository.GetAllAsync();
            return await Task.Run(() => images.Where(i => i.ArticleId == articleId).ToList());
        }
    }
}

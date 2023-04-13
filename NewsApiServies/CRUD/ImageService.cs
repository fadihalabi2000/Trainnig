

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

    }
}

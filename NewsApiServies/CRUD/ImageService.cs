using NewsApiDomin.Models;
using NewsApiServies.CRUD.Interfaces;
using Services.CRUD;
using Services.Transactions.Interfaces;

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

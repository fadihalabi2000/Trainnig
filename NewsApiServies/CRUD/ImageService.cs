using NewsApiDomin.Models;
using NewsApiServies.CRUD.Interfaces;
using Services.CRUD;
using Services.Transactions.Interfaces;


namespace NewsApiServies.CRUD
{
    public class ImageService:BaseCRUDService<Image> ,IImageService
    {
        private readonly IUnitOfWork unitOfWork;

        public ImageService(IUnitOfWork unitOfWork):base(unitOfWork.ImageRepository)
        {
            this.unitOfWork = unitOfWork;
        }

    }
}

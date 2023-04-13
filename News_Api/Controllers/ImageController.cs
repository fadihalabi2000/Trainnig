using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.ImageViewModel;
using NewsApiDomin.ViewModels.LikeViewModel;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Author")]
   
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public ImageController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ImageView>>> GetAll()
        {

            try
            {
                var images = await unitOfWorkService.ImageService.GetAllAsync();
                var imagesView = images.Select(i => new ImageView { Id = i.Id, ArticleId = i.ArticleId, ImageUrl = i.ImageUrl, ImageDescription = i.ImageDescription });
                if (imagesView.Count() > 0)
                    return Ok(imagesView);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        [Authorize]
        [HttpGet("{id}",Name = "GetImage")]
        public async Task<ActionResult<ImageView>> GetById(int id)
        {


            try
            {
                var image = await unitOfWorkService.ImageService.GetByIdAsync(id);
                var imageview=new ImageView { Id = image.Id,ArticleId=image.ArticleId, ImageDescription = image.ImageDescription,ImageUrl=image.ImageUrl };
                if (imageview == null)
                    return BadRequest();
                else
                    return Ok(imageview);

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

       
        [HttpPost]
        public async Task<ActionResult<ImageView>> Post(CreateImage createImage)
        {

            try
            {
                var image = new Image { ImageDescription=createImage.ImageDescription,ImageUrl= createImage.ImageUrl,ArticleId=createImage.ArticleId};

                await unitOfWorkService.ImageService.AddAsync(image);

                if (await unitOfWorkService.CommitAsync())
                {
                    var lastID = await unitOfWorkService.ImageService.GetAllAsync();
                    var imageId = lastID.Max(b => b.Id);
                    image = await unitOfWorkService.ImageService.GetByIdAsync(imageId);
                    var imageView = new ImageView { Id = image.Id, ArticleId = image.ArticleId, ImageDescription= image.ImageDescription,ImageUrl=image.ImageUrl };
                    return CreatedAtRoute("GetImage", new
                    {
                        id = imageId,
                    }, imageView);

                  
                }
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdateImage updateImage)
        {
            try
            {
              var image=  await unitOfWorkService.ImageService.GetByIdAsync(id);
                image.ImageDescription = updateImage.ImageDescription;
                image.ImageUrl = updateImage.ImageUrl;
                await unitOfWorkService.ImageService.UpdateAsync(image);
                if (await unitOfWorkService.CommitAsync())
                    return NoContent();
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await unitOfWorkService.ImageService.DeleteAsync(id);
                if (await unitOfWorkService.CommitAsync())
                    return NoContent();
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}

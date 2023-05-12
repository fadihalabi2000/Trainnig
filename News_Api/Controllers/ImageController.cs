using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.CategoryViewModel;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.ImageViewModel;
using NewsApiDomin.ViewModels.LikeViewModel;
using NewsApiServies.Auth.ClassStatic;
using Services.MyLogger;
using Services.Transactions.Interfaces;
using System.Text.Json;
using AutoMapper;
using DataAccess.Entities.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace NewsApi.Controllers
{
    [Route("api/Article/{articleId}/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Author")]

   
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;
        private readonly IMyLogger logger;
        private readonly IMapper mapper;

        public ImageController(IUnitOfWorkService unitOfWorkService, IMyLogger logger,IMapper mapper)
        {
            this.unitOfWorkService = unitOfWorkService;
            this.logger = logger;
            this.mapper = mapper;
        }




     


        [HttpGet]
        public async Task<ActionResult<(PaginationMetaData, List<ImageView>)>> GetAll(int articleId, int pageNumber = 1, int pageSize = 10)
        {
            var article = await unitOfWorkService.LikeService.GetByIdAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }
            try
            {
                var image = await unitOfWorkService.ImageService.GetAllByIdArticleAsync(articleId);
                if (image.Count() > 0)
                {
                    (image, var paginationData) = await unitOfWorkService.ImagePagination.GetPaginationAsync(pageNumber, pageSize, image);
                    if (image.Count() > 0)
                    {
                        List<ImageView> images = mapper.Map<List<ImageView>>(image);
                        Response.Headers.Add("X-Pagination",
                   JsonSerializer.Serialize(paginationData));
                        await logger.LogInformation("All Image table records fetched", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return Ok(new { paginationData, images });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    await logger.LogWarning("An Warning occurred while fetching all logs Image ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogWarning("An Warning occurred while fetching all logs Image ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }

        [Authorize]
        [HttpGet("{id}",Name = "GetImage")]
        public async Task<ActionResult<ImageView>> GetById(int articleId,int id)
        {
            var article = await unitOfWorkService.LikeService.GetByIdAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }

            try
            {
                var imageById = await unitOfWorkService.ImageService.GetByIdAsync(id);
                if (imageById == null)
                {
                    await logger.LogWarning("Failed to fetch Image with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
                else
                {
                    ImageView image = mapper.Map<ImageView>(imageById);
                    await logger.LogInformation("Image with ID " + id + " fetched ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return Ok(image);
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("Erorr to fetch Image with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }

       



        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int articleId,int id, UpdateImage updateImage)
        {
            var article = await unitOfWorkService.LikeService.GetByIdAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }
            try
            {
              Image image=  await unitOfWorkService.ImageService.GetByIdAsync(id);
                mapper.Map(updateImage, image);
                await unitOfWorkService.ImageService.UpdateAsync(image);
                if (await unitOfWorkService.CommitAsync())
                {
                    await logger.LogInformation("Image with ID " + id + " updated", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return NoContent();
                }
                else
                {
                    await logger.LogWarning("An warning occurred when updateing the Image ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when updateing the Image ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
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
                {
                    await logger.LogInformation("Image with ID " + id + " deleted", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return NoContent();
                }
                else
                {
                    await logger.LogWarning("An warning occurred when deleteing the Image ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when deleteing the Image ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }
        }
        //[HttpPost]
        //public async Task<ActionResult<ImageView>> Post(int articleId, CreateImage createImage)
        //{
        //    var article = await unitOfWorkService.LikeService.GetByIdAsync(articleId);
        //    if (article == null)
        //    {
        //        return NotFound();
        //    }
        //    try
        //    {
        //        Image images = mapper.Map<Image>(createImage);
        //        images.ArticleId = articleId;
        //        await unitOfWorkService.ImageService.AddAsync(images);

        //        if (await unitOfWorkService.CommitAsync())
        //        {
        //            var lastID = await unitOfWorkService.ImageService.GetAllAsync();
        //            var imageId = lastID.Max(b => b.Id);
        //            images = await unitOfWorkService.ImageService.GetByIdAsync(imageId);
        //            await logger.LogInformation("Image with ID " + imageId + " added", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
        //            ImageView image = mapper.Map<ImageView>(images);
        //            return await Task.Run(() => CreatedAtRoute("GetImage", new { articleId = articleId, id = imageId, }, image));


        //        }
        //        else
        //        {
        //            await logger.LogWarning("An warning occurred when adding the Image", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
        //            return BadRequest();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        await logger.LogErorr("An Erorr occurred when adding the Image", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
        //        return BadRequest();
        //    }
        //}
    }
}
//[HttpPost]
//public async Task<IActionResult> UploadImage(IFormFile imageFile)
//{

//    var contentPath = this.environment.ContentRootPath;
//    // path = "c://projects/productminiapi/uploads" ,not exactly something like that
//    var path = Path.Combine(contentPath, "Uploads");
//    if (!Directory.Exists(path))
//    {
//        Directory.CreateDirectory(path);
//    }

//    // Check the allowed extenstions
//    var ext = Path.GetExtension(imageFile.FileName);
//    var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
//    if (!allowedExtensions.Contains(ext))
//    {
//        string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
//        return BadRequest(msg);
//    }
//    string uniqueString = Guid.NewGuid().ToString();
//    // we are trying to create a unique filename here
//    var newFileName = uniqueString + ext;
//    var fileWithPath = Path.Combine(path, newFileName);
//    var stream = new FileStream(fileWithPath, FileMode.Create);
//    imageFile.CopyTo(stream);
//    stream.Close();
//    var imageEntity = new Image
//    {
//        ArticleId = 1,
//        ImageDescription = newFileName,
//        ImageUrl = fileWithPath,

//    };

//    return Ok(imageEntity);
//}
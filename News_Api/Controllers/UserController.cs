
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.ImageViewModel;
using NewsApiDomin.ViewModels.LogViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using Services.Transactions.Interfaces;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public UserController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<(PaginationMetaData, List<UserView>)>> GetAll(int pageNumber = 1, int pageSize = 10)
        {

            try
            {
                var user = await unitOfWorkService.UsersService.GetAllAsync();

                if (user.Count() > 0)
                {
                    (user, var paginationData) = await unitOfWorkService.UserPagination.GetPaginationAsync(pageNumber,pageSize, user);
                    var users = user.Select(u => new UserView
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        DisplayName = u.DisplayName,
                        ProfilePicture = u.ProfilePicture,
                        Email = u.Email
                    });
                    Response.Headers.Add("X-Pagination",
                    JsonSerializer.Serialize(paginationData));
                    return Ok(new { paginationData, users });
                }
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpGet("{id}",Name = "GetUser")]
        public async Task<ActionResult<UserWithoutLog>> GetById(int id)
        {


            try
            {
                var user = await unitOfWorkService.UsersService.GetByIdAsync(id);
                var userWithoutLog = new UserWithoutLog { Id = id, FirstName = user.FirstName, LastName = user.LastName, Comments = user.Comments, DisplayName = user.DisplayName, Email = user.Email, likes = user.likes, ProfilePicture = user.ProfilePicture };
                if (userWithoutLog == null)
                    return BadRequest();
                else
                    return Ok(userWithoutLog);

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        //[HttpPost]
        //public async Task<ActionResult<UserView>> Post(CreateUser createUser)
        //{

        //    try
        //    {
        //        var user = new User
        //        {
        //            FirstName = createUser.FirstName,
        //            LastName = createUser.LastName ,
        //            Email = createUser.Email,
        //            Password = createUser.Password,
        //            ProfilePicture = createUser.ProfilePicture,
        //            DisplayName= createUser.DisplayName,
        //        };

        //        await unitOfWorkService.UsersService.AddAsync(user);

        //        if (await unitOfWorkService.CommitAsync())
        //        {
        //            var lastID = await unitOfWorkService.UsersService.GetAllAsync();
        //            var userId = lastID.Max(b => b.Id);
        //            user = await unitOfWorkService.UsersService.GetByIdAsync(userId);
        //            var userView = new UserView { Id = user.Id,FirstName= user .FirstName,LastName= user.LastName,DisplayName= user.DisplayName,
        //                               ProfilePicture= user.ProfilePicture,Email=user.Email
        //            };
        //            return CreatedAtRoute("GetUser", new
        //            {
        //                id = userId,
        //            }, userView);
                   
        //        }
        //        else
        //            return BadRequest();
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}



        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdateUser updateUser)
        {
            try
            {
                var user = await unitOfWorkService.UsersService.GetByIdAsync(id);
                user.FirstName = updateUser.FirstName;
                user.LastName = updateUser.LastName;
                user.Password = updateUser.Password;
                user.ProfilePicture = updateUser.ProfilePicture;
                user.DisplayName= updateUser.DisplayName;
                await unitOfWorkService.UsersService.UpdateAsync(user);
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await unitOfWorkService.UsersService.DeleteAsync(id);
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

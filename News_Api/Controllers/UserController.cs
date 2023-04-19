
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.AuthorViewModel;
using NewsApiDomin.ViewModels.ImageViewModel;
using NewsApiDomin.ViewModels.LogViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiServies.Auth.ClassStatic;
using Services.MyLogger;
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
        private readonly IMyLogger logger;
        private readonly IMapper mapper;

        public UserController(IUnitOfWorkService unitOfWorkService, IMyLogger logger,IMapper mapper)
        {
            this.unitOfWorkService = unitOfWorkService;
            this.logger = logger;
            this.mapper = mapper;
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
                    (user, var paginationData) = await unitOfWorkService.UserPagination.GetPaginationAsync(pageNumber, pageSize, user);
                    //var users = user.Select(u => new UserView
                    //{
                    //    Id = u.Id,
                    //    FirstName = u.FirstName,
                    //    LastName = u.LastName,
                    //    DisplayName = u.DisplayName,
                    //    ProfilePicture = u.ProfilePicture,
                    //    Email = u.Email
                    //});
                    if (user.Count() > 0)
                    {
                        List<UserView> users = mapper.Map<List<UserView>>(user);
                        Response.Headers.Add("X-Pagination",
                        JsonSerializer.Serialize(paginationData));
                        await logger.LogInformation("All User table records fetched", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return Ok(new { paginationData, users });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    await logger.LogWarning("An Warning occurred while fetching all logs User ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogWarning("An Warning occurred while fetching all logs User ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }


        [HttpGet("{id}",Name = "GetUser")]
        public async Task<ActionResult<UserWithoutLog>> GetById(int id)
        {


            try
            {
                var userById = await unitOfWorkService.UsersService.GetByIdAsync(id);
                //var userWithoutLog = new UserWithoutLog { Id = id, FirstName = user.FirstName, LastName = user.LastName, Comments = user.Comments, DisplayName = user.DisplayName, Email = user.Email, likes = user.likes, ProfilePicture = user.ProfilePicture };
                UserWithoutLog user = mapper.Map<UserWithoutLog>(userById);
                if (user == null)
                {
                    await logger.LogWarning("Failed to fetch User with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
                else
                {
                    await logger.LogInformation("User with ID " + id + " fetched ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return Ok(user);
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("Erorr to fetch User with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
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


        [Authorize(Roles = "User,Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdateUser updateUser)
        {
            try
            {
                User user = await unitOfWorkService.UsersService.GetByIdAsync(id);

                //user.FirstName = updateUser.FirstName;
                //user.LastName = updateUser.LastName;
                //user.Password = updateUser.Password;
                //user.ProfilePicture = updateUser.ProfilePicture;
                //user.DisplayName= updateUser.DisplayName;
                var checkDisplayName = await unitOfWorkService.UsersService.CheckDisplayName(updateUser.DisplayName);
                if (checkDisplayName is null)
                {
                    user = mapper.Map(updateUser,user);
                    await unitOfWorkService.UsersService.UpdateAsync(user);
                    if (await unitOfWorkService.CommitAsync())
                    {
                        await logger.LogInformation("User with ID " + id + " updated", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return NoContent();
                    }
                    else
                    {
                        await logger.LogWarning("An warning occurred when updateing the User ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest(new { Message = "the displayName already exists " });
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when updateing the User ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }

        [Authorize(Roles = "User,Admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> patch(int id, JsonPatchDocument<UpdateUser> patchDocument)
        {
            try
            {
                User user = await unitOfWorkService.UsersService.GetByIdAsync(id);
                UpdateUser patchUser = mapper.Map<UpdateUser>(user);
                patchDocument.ApplyTo(patchUser);

               var checkDisplayName = await unitOfWorkService.UsersService.CheckDisplayName(patchUser.DisplayName);
                if (checkDisplayName is null)
                {
                    user = mapper.Map(patchUser, user);
                    await unitOfWorkService.UsersService.UpdateAsync(user);
                    if (await unitOfWorkService.CommitAsync())
                    {
                        await logger.LogInformation("User with ID " + id + " updated", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return NoContent();
                    }
                    else
                    {
                        await logger.LogWarning("An warning occurred when updateing the User ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest(new { Message = "the displayName already exists " });
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when updateing the User ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
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
                {
                    await logger.LogInformation("User with ID " + id + " deleted", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return NoContent();
                }
                else
                {
                    await logger.LogWarning("An warning occurred when deleteing the User ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when deleteing the User ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }
    }
}

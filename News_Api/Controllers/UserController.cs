
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.ImageViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using Services.Transactions.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public UserController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserView>>> GetAll()
        {

            try
            {
                var users = await unitOfWorkService.UsersService.GetAllAsync();
                var usersView = users.Select(u => new UserView { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, DisplayName = u.DisplayName,
                                                               ProfilePicture=u.ProfilePicture,Email=u.Email,Password=u.Password });
                if (usersView.Count() > 0)
                    return Ok(usersView);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpGet("{id}",Name = "GetUser")]
        public async Task<ActionResult<User>> GetById(int id)
        {


            try
            {
                var user = await unitOfWorkService.UsersService.GetByIdAsync(id);
                if (user == null)
                    return BadRequest();
                else
                    return Ok(user);

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpPost]
        public async Task<ActionResult<UserView>> Post(CreateUser createUser)
        {

            try
            {
                var user = new User
                {
                    FirstName = createUser.FirstName,
                    LastName = createUser.LastName ,
                    Email = createUser.Email,
                    Password = createUser.Password,
                    ProfilePicture = createUser.ProfilePicture,
                    DisplayName= createUser.DisplayName,
                };

                await unitOfWorkService.UsersService.AddAsync(user);

                if (await unitOfWorkService.CommitAsync())
                {
                    var lastID = await unitOfWorkService.UsersService.GetAllAsync();
                    var userId = lastID.Max(b => b.Id);
                    user = await unitOfWorkService.UsersService.GetByIdAsync(userId);
                    var userView = new UserView { Id = user.Id,FirstName= user .FirstName,LastName= user.LastName,DisplayName= user.DisplayName,
                                       ProfilePicture= user.ProfilePicture,Email=user.Email,Password = user.Password
                    };
                    return CreatedAtRoute("GetUser", new
                    {
                        id = userId,
                    }, userView);
                   
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


using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.UserViewModel;
using Services.Transactions.Interfaces;

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
        public async Task<ActionResult<List<User>>> GetAll()
        {

            try
            {
                var Users = await unitOfWorkService.UsersService.GetAllAsync();
                if (Users.Count() > 0)
                    return Ok(Users);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpGet("{id}")]
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
        public async Task<ActionResult> Post(CreateUser user)
        {

            try
            {
                var newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName
                                 ,
                    Email = user.Email,
                    Password = user.Password,
                    ProfilePicture = user.ProfilePicture,
                    IsDeleted = false,
                };

                await unitOfWorkService.UsersService.AddAsync(newUser);

                if (await unitOfWorkService.CommitAsync())
                    return Ok(user);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdateUser user)
        {
            try
            {
                var oldUser = await unitOfWorkService.UsersService.GetByIdAsync(id);
                oldUser.FirstName = user.FirstName;
                oldUser.LastName = user.LastName;
                oldUser.Password = user.Password;
                oldUser.ProfilePicture = user.ProfilePicture;
                await unitOfWorkService.UsersService.UpdateAsync(oldUser);
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

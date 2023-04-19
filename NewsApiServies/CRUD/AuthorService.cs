using NewsApiDomin.Models;
using Services.CRUD;
using NewsApiServies.CRUD.Interfaces;
using Repositories.Interfaces;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using Microsoft.EntityFrameworkCore;

namespace NewsApiServies.CRUD
{

    public class AuthorService : BaseCRUDService<Author>, IAuthorService
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public AuthorService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.AuthorRepository)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }

        public async Task<Author> CheckDisplayName(string displayName)
        {
            List<Author>  authors = await unitOfWorkRepo.AuthorRepository.GetAllAsync();
            return await Task.Run(() => authors.FirstOrDefault(a => a.DisplayName == displayName)!);

        }

        public async Task<Author> CheckNameAndEmail(string displayName, string email)
        {
            List<Author> authors = await unitOfWorkRepo.AuthorRepository.GetAllAsync();
            return await Task.Run(() => authors.FirstOrDefault(a => a.DisplayName == displayName&&a.Email==email)!);
        }
        public async Task<Author> AutherAuth(Login authorLogin)
        {
            List<Author> authors = await unitOfWorkRepo.AuthorRepository.GetAllAsync();
            return await Task.Run(() => authors.FirstOrDefault(a => a.Email == authorLogin.Email && a.Password == authorLogin.Password)!);
        }
    }
}

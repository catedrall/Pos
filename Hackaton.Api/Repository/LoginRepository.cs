using Hackaton.Api.Data;
using Hackaton.Models;
using Hackaton.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Hackaton.Api.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly DbContextClass _context;

        public LoginRepository(DbContextClass context)
        {
            _context = context;
        }

        public async Task CreateAsync(Login login, CancellationToken cancellationToken)
        {
            await _context.Login.AddAsync(login, cancellationToken);
        }

        public async Task<Login?> GetAsync(int id, CancellationToken cancellation)
        {
            return _context.Login.FirstOrDefault(w => w.Id == id);
        }

        public async Task<Login?> LoginAsync(string email, string senha, CancellationToken cancellationToken)
        {
            return _context.Login.FirstOrDefault(w => w.Email.Equals(email) && w.Senha.Equals(senha));
        }

        public async Task SalvarAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Login login, CancellationToken cancellationToken)
        {
            _context.Login.Update(login);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
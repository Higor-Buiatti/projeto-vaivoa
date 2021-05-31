using Microsoft.EntityFrameworkCore;
using Project.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repo
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ClienteContext _context;

        public ProjectRepository(ClienteContext context) // cria uma referência do Context.
        {
            _context = context;
        }


        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }





        public async Task<Cliente[]> GetAllClientes()
        {
            IQueryable<Cliente> query = _context.Clientes
                .Include(c => c.Cartoes);

            query = query.AsNoTracking().OrderBy(c => c.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Cliente> GetClientesById(int id)
        {
            IQueryable<Cliente> query = _context.Clientes
                .Include(c => c.Cartoes);

            query = query.AsNoTracking().OrderBy(c => c.Id);

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente[]> GetClientesByNome(string nome)
        {
            IQueryable<Cliente> query = _context.Clientes
                .Include(c => c.Cartoes);

            query = query.AsNoTracking().OrderBy(c => c.Email.Contains(nome));

            return await query.ToArrayAsync();
        }

        //Tentando

        public async Task<Cliente> GetClienteByEmail(string email)
        {
            IQueryable<Cliente> query = _context.Clientes
                .Include(c => c.Cartoes);

            query = query.AsNoTracking().OrderBy(c => c.Email.Equals(email));

            return await query.FirstOrDefaultAsync(c => c.Email == email);

        }
    }
}

using Project.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repo
{
    public interface IProjectRepository
    {
        //Metodo Genérico que recebe qualquer Tipo como parametro.
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        Task<bool> SaveChangeAsync();


        Task<Cliente[]> GetAllClientes();
        Task<Cliente> GetClientesById(int id);
        Task<Cliente[]> GetClientesByNome(string nome);
        Task<Cliente> GetClienteByEmail(string email);


    }
}

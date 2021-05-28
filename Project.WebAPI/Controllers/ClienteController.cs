using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Domain;
using Project.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteContext _context;
        public ClienteController(ClienteContext context)
        {
            _context = context;
        }

        // GET: api/Cliente/cartoes/usuario@email.com    Retorna uma lista em ordem de criação, dos cartões do cliente passando seu e-mail 
        [HttpGet("cartoes/{email}")]
        public ActionResult GetCartoes(string email)
        {
            var listCliente = _context.Clientes
                              .Where(c => c.Email.Equals(email))
                              .OrderBy(c => c.Id)
                              .Select(c => c.Cartoes)
                              .ToList();

            //var listCliente = (from cliente in _context.Clientes
            //                   where cliente.Nome.Contains(nome)
            //                   select cliente.Cartoes)
            //                   .ToList();

            return Ok(listCliente);

        }

        // GET: api/Cliente/1   Retorna um Cliente a partir de um ID (OK)
        [HttpGet("{id}")]
        public ActionResult GetId(int id)
        {

            var listCliente = (from cliente in _context.Clientes
                               where cliente.Id.Equals(id)
                               select cliente).FirstOrDefault();
            return Ok(listCliente);

        }

        // GET: api/Cliente/cartao/usuario@email.com  Cria um novo cartão para um email existente
        [HttpGet("cartao/{email}")]
        public ActionResult GetNome(string email)
        {
            if (_context.Clientes.AsNoTracking().FirstOrDefault(c => c.Email == email) != null)
            {
                var randomNumber = new Random();
                int numUm = randomNumber.Next(1000, 10000);
                int numDois = randomNumber.Next(1000, 10000);
                int numTres = randomNumber.Next(1000, 10000);
                int numQuatro = randomNumber.Next(1000, 10000);

                var rUm = Convert.ToString(numUm);
                var rDois = Convert.ToString(numDois);
                var rTres = Convert.ToString(numTres);
                var rQuatro = Convert.ToString(numQuatro);

                string result = rUm + " " + rDois + " " + rTres + " " + rQuatro;




                var listCliente = (from cliente in _context.Clientes
                                   where cliente.Email.Equals(email)
                                   select cliente).FirstOrDefault();

                listCliente.Cartoes = new List<Cartao>
                            {
                                new Cartao { NumeroDoCartao = result }
                            };

                _context.Clientes.Update(listCliente);
                _context.SaveChanges();


                return Ok(listCliente.Cartoes);
            }

            return NotFound("Usuario nao enontrado");


        }

        // GET: api/Cliente/criar/nome   cria um usuário e um cartão a partir do e-mail passado como requisiçao
        [HttpGet("criar/{email}")]
        public ActionResult GetName(string email)
        {
            try
            {
                if (_context.Clientes.AsNoTracking().FirstOrDefault(c => c.Email == email) != null)
                {
                    return BadRequest("Usuario ja existente");
                }

                var randomNumber = new Random();
                int numUm = randomNumber.Next(1000, 10000);
                int numDois = randomNumber.Next(1000, 10000);
                int numTres = randomNumber.Next(1000, 10000);
                int numQuatro = randomNumber.Next(1000, 10000);

                var rUm = Convert.ToString(numUm);
                var rDois = Convert.ToString(numDois);
                var rTres = Convert.ToString(numTres);
                var rQuatro = Convert.ToString(numQuatro);

                String result = rUm + " " + rDois + " " + rTres + " " + rQuatro;

                var cliente = new Cliente
                {
                    Email = email,
                    Cartoes = new List<Cartao>
                        {
                            new Cartao { NumeroDoCartao = result }
                        }
                };

                _context.Clientes.Add(cliente);
                _context.SaveChanges();

                return Ok(cliente);

            }
            catch (Exception ex)
            {

                return BadRequest($"Erro: {ex}");
            }

        }

        // GET: api/Cliente
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(new Cliente());
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex}");
            }


        }       
    }
}

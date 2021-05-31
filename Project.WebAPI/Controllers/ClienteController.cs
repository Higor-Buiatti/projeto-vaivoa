using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Domain;
using Project.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IProjectRepository _repo;

        public ClienteController(IProjectRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Cliente/cartoes/usuario@email.com    Retorna uma lista em ordem de criação, dos cartões do cliente passando seu e-mail 
        //[HttpGet("cartoes/{email}")]
        //public ActionResult GetCartoes(string email)
        //{
        //    var listCliente = _context.Clientes
        //                      .Where(c => c.Email.Equals(email))
        //                      .OrderBy(c => c.Id)
        //                      .Select(c => c.Cartoes)
        //                      .ToList();

        //    //var listCliente = (from cliente in _context.Clientes
        //    //                   where cliente.Nome.Contains(nome)
        //    //                   select cliente.Cartoes)
        //    //                   .ToList();

        //    return Ok(listCliente);

        //}

        //// GET: api/Cliente/1   Retorna um Cliente a partir de um ID (OK)
        //[HttpGet("{id}")]
        //public ActionResult GetId(int id)
        //{

        //    var listCliente = (from cliente in _context.Clientes
        //                       where cliente.Id.Equals(id)
        //                       select cliente).FirstOrDefault();
        //    return Ok(listCliente);

        //}

        //// GET: api/Cliente/cartao/usuario@email.com  Cria um novo cartão para um email existente
        //[HttpGet("cartao/{email}")]
        //public ActionResult GetNome(string email)
        //{
        //    if (_context.Clientes.AsNoTracking().FirstOrDefault(c => c.Email == email) != null)
        //    {
        //        var randomNumber = new Random();
        //        int numUm = randomNumber.Next(1000, 10000);
        //        int numDois = randomNumber.Next(1000, 10000);
        //        int numTres = randomNumber.Next(1000, 10000);
        //        int numQuatro = randomNumber.Next(1000, 10000);

        //        var rUm = Convert.ToString(numUm);
        //        var rDois = Convert.ToString(numDois);
        //        var rTres = Convert.ToString(numTres);
        //        var rQuatro = Convert.ToString(numQuatro);

        //        string result = rUm + " " + rDois + " " + rTres + " " + rQuatro;




        //        var listCliente = (from cliente in _context.Clientes
        //                           where cliente.Email.Equals(email)
        //                           select cliente).Single();


        //        listCliente.Cartoes = new List<Cartao> 
        //                    {
        //                    new Cartao { NumeroDoCartao = result}
        //                    };

        //        _context.Clientes.Update(listCliente);
        //        _context.SaveChanges();


        //        return Ok(listCliente);
        //    }

        //    return NotFound("Usuario nao enontrado");


        //}

        //// GET: api/Cliente/criar/nome   cria um usuário e um cartão a partir do e-mail passado como requisiçao
        //[HttpGet("criar/{email}")]
        //public ActionResult GetName(string email)
        //{
        //    try
        //    {
        //        if (_context.Clientes.AsNoTracking().FirstOrDefault(c => c.Email == email) != null)
        //        {
        //            return BadRequest("Usuario ja existente");
        //        }

        //        var randomNumber = new Random();
        //        int numUm = randomNumber.Next(1000, 10000);
        //        int numDois = randomNumber.Next(1000, 10000);
        //        int numTres = randomNumber.Next(1000, 10000);
        //        int numQuatro = randomNumber.Next(1000, 10000);

        //        var rUm = Convert.ToString(numUm);
        //        var rDois = Convert.ToString(numDois);
        //        var rTres = Convert.ToString(numTres);
        //        var rQuatro = Convert.ToString(numQuatro);

        //        String result = rUm + " " + rDois + " " + rTres + " " + rQuatro;

        //        var cliente = new Cliente
        //        {
        //            Email = email,
        //            Cartoes = new List<Cartao>
        //                {
        //                    new Cartao { NumeroDoCartao = result }
        //                }
        //        };

        //        _context.Clientes.Add(cliente);
        //        _context.SaveChanges();

        //        return Ok(cliente);

        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest($"Erro: {ex}");
        //    }

        //}

        // GET: api/Cliente
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clientes = await _repo.GetAllClientes();

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex}");
            }


        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetEmailCards(string email)
        {
            try
            {
                var clientes = await _repo.GetClienteByEmail(email);

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex}");
            }


        }


        [HttpPost]
        public async Task<IActionResult> Post(Cliente model)
        {
            try
            {
                _repo.Add(model);
                if (await _repo.SaveChangeAsync())
                {
                    return Ok("Done");
                }

            }
            catch (Exception ex)
            {

                return BadRequest($"erro: {ex}");
            }
            return BadRequest("Não salvou");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cliente = await _repo.GetClientesById(id);


                if (cliente != null)
                {
                    _repo.Delete(cliente);
                    if (await _repo.SaveChangeAsync())
                    {
                        return Ok("Done");
                    }

                }

            }
            catch (Exception ex)
            {

                return BadRequest($"erro: {ex}");
            }
            return BadRequest("Não Deletado");
        }


        //Testando
        [HttpGet("cartao/{email}")]
        public async Task<IActionResult> GetCard(string email)
        {
            try
            {
                var cliente = await _repo.GetClienteByEmail(email);

                if (cliente != null)
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


                    cliente.Cartoes = new List<Cartao>
                                        {
                                        new Cartao { NumeroDoCartao = result}
                                        };



                    _repo.Update(cliente);
                    if (await _repo.SaveChangeAsync())
                    {
                        return Ok(cliente);
                    }

                }

            }
            catch (Exception ex)
            {

                return BadRequest($"erro: {ex}");
            }
            return BadRequest("Não Criado");
        }
    }
}

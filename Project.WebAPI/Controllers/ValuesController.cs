using Microsoft.AspNetCore.Mvc;
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
    public class ValuesController : ControllerBase
    {
        public readonly ClienteContext _context;
        public ValuesController(ClienteContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            var listCliente = _context.Clientes.ToList();
            return Ok(listCliente);
        }

        // GET api/values/5
        [HttpGet("{nameCliente}")]
        public ActionResult Get(string nameCliente)
        {
            var cliente = new Cliente { Email = nameCliente };

            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            return Ok();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

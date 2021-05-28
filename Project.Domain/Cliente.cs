using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Domain
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public List<Cartao> Cartoes { get; set; }
    }
}

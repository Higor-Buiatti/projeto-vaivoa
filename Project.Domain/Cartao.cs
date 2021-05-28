using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Domain
{
    public class Cartao
    {
        public int Id { get; set; }
        public string NumeroDoCartao { get; set; }

        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
    }
}

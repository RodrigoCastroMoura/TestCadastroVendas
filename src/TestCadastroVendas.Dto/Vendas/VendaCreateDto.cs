using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCadastroVendas.Dto.Vendas
{
    public class VendaCreateDto
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Senha { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JuntosBusiness.Model
{
    public class UsuarioModel : EntidadeBaseModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}

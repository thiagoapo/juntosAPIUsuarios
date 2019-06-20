using System;
using System.Collections.Generic;
using System.Text;

namespace JuntosBusiness.Model
{
    public class UsuarioUpdateModel : EntidadeBaseModel
    {
        public string Nome { get; set; }
    }

    public class UsuarioUpdateSenhaModel
    {
        public string Email { get; set; }
        public string SenhaAntiga { get; set; }
        public string SenhaNova { get; set; }
    }
}

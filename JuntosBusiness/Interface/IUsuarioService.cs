using JuntosBusiness.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntosBusiness.Interface
{
    public interface IUsuarioService
    {
        UsuarioModel Obter(Guid id);
        UsuarioModel ObterPorEmailSenha(string email, string senha);
        List<UsuarioModel> ListarTodos();
        Resultado Incluir(UsuarioModel model);
        Resultado Atualizar(UsuarioUpdateModel model);
        Resultado AtualizarFull(UsuarioModel model);
        Resultado AtualizarSenha(UsuarioUpdateSenhaModel model);
        Resultado Excluir(Guid id);
    }
}

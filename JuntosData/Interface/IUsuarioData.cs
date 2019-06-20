using JuntosEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntosData.Interface
{
    public interface IUsuarioData
    {
        Usuario Obter(Guid id);

        Usuario ObterPorEmail(string email);

        Usuario ObterPorEmailSenha(string email, string senha);

        List<Usuario> ListarTodos();

        Usuario Salvar(Usuario usuario);

        void Excluir(Usuario usuario);
    }
}

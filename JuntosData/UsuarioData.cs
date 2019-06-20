using System;
using System.Collections.Generic;
using System.Linq;
using JuntosData.Context.Interface;
using JuntosData.Contexts;
using JuntosData.Interface;
using JuntosEntities;

namespace JuntosData
{
    public class UsuarioData : IUsuarioData
    {
        private readonly IApplicationDbContext _context;

        public UsuarioData(IApplicationDbContext context)
        {
            _context = context;
        }

        public Usuario Obter(Guid id)
        {
            return _context.Usuarios.Where(
                u => u.ID == id).FirstOrDefault();
        }

        public Usuario ObterPorEmail(string email)
        {
            return _context.Usuarios.Where(
                u => u.Email == email).FirstOrDefault();
        }

        public Usuario ObterPorEmailSenha(string email, string senha)
        {
            return _context.Usuarios.Where(
                u => u.Email == email && u.Senha == senha).FirstOrDefault();
        }

        public List<Usuario> ListarTodos()
        {
            return _context.Usuarios
                .OrderBy(p => p.Nome).ToList();
        }

        public Usuario Salvar(Usuario usuario)
        {
            if (usuario.ID == null || usuario.ID == Guid.Empty)
            {
                usuario.DataCriacao = DateTime.Now;
                _context.Usuarios.Add(usuario);
            }
            else
                usuario.DataAtualizacao = DateTime.Now;

            _context.Instance.SaveChanges();
            return usuario;
        }

        public void Excluir(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            _context.Instance.SaveChanges();
        }
    }
}
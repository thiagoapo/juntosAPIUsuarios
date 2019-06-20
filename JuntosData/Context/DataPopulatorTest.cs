using JuntosData.Contexts;
using JuntosEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntosData.Context
{
    public static class DataPopulatorTest
    {
        public static void Init(string hash)
        {
            using (var context = new ApplicationDbContext())
            {
                var data = new UsuarioData(context);

                var usuario = new Usuario()
                {
                    DataCriacao = DateTime.Now,
                    Email = "thiago.paulistana@gmail.com",
                    Nome = "Thiago",
                    Senha = hash
                };

                data.Salvar(usuario);
            }
        }
    }
}

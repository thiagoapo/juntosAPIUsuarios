using APIJuntosUsuarios.Configuration;
using APIJuntosUsuarios.Controllers;
using JuntosBusiness.Model;
using JuntosBusiness.Utils;
using JuntosData.Context;
using JuntosData.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using JuntosBusiness.Interface;

namespace JuntosTest
{
    public class UsuariosControllerTest
    {
        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private UsuariosController _controller;

        public UsuariosControllerTest()
        {
            _services = new ServiceCollection();
            _services.AddDbContext<ApplicationDbContext>();
            InjecaoDependencias.Configure(_services);
            _serviceProvider = _services.BuildServiceProvider();
            var usuarioService = (IUsuarioService)_serviceProvider.GetService(typeof(IUsuarioService));
            _controller = new UsuariosController(usuarioService);
        }

        #region SUCESS

        [Fact]
        public void ObterLista()
        {
            this.DataCreate();

            // Act
            var result = _controller.Get();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            var items = Assert.IsType<List<UsuarioModel>>(result);
            Assert.Single(items);
        }

        [Fact]
        public void ObterPorId()
        {
            this.DataCreate();

            // Act
            var result = _controller.Get();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            var items = Assert.IsType<List<UsuarioModel>>(result);
            Assert.Single(items);

            var okResult = _controller.Get(items.First().ID);
            Assert.NotNull(okResult);
            Assert.IsType<ObjectResult>(okResult);
            var ok = (ObjectResult)okResult;
            Assert.Equal(items.First().ID, ((UsuarioModel)ok.Value).ID);
        }

        [Fact]
        public void Post()
        {
            this.DataCreate();

            UsuarioModel model = new UsuarioModel()
            {
                Email = "roberto_justus@hotmail.com",
                Nome = "Roberto Justus",
                Senha = "O@prendiz"
            };
            // Act
            var result = _controller.Post(model);

            // Assert
            Assert.NotNull(result);
            var item = Assert.IsType<Resultado>(result);
            Assert.True(item.Sucesso);
            Assert.Empty(item.Inconsistencias);

            var list = _controller.Get();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            var items = Assert.IsType<List<UsuarioModel>>(list);
            Assert.Equal(2, items.Count);

            //Back
            var usuario = items.FirstOrDefault(p=> p.Email == "roberto_justus@hotmail.com");
            var removeResult = _controller.Delete(usuario.ID);
            Assert.NotNull(removeResult);
            var removeItem = Assert.IsType<Resultado>(removeResult);
            Assert.True(removeItem.Sucesso);
            Assert.Empty(removeItem.Inconsistencias);
        }

        [Fact]
        public void AtualizarSenha()
        {
            this.DataCreate();

            var senhaAntiga = "12345";
            var senha = "joT@111";
            var hash = PasswordEncrypt.Hash(senha);

            UsuarioUpdateSenhaModel model = new UsuarioUpdateSenhaModel()
            {
                Email = "thiago.paulistana@gmail.com",
                SenhaAntiga = senhaAntiga,
                SenhaNova = senha
            };
            // Act
            var result = _controller.AtualizarSenha(model);

            // Assert
            Assert.NotNull(result);
            var item = Assert.IsType<Resultado>(result);
            Assert.True(item.Sucesso);
            Assert.Empty(item.Inconsistencias);

            var list = _controller.Get();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            var items = Assert.IsType<List<UsuarioModel>>(list);
            Assert.Single(items);

            var usuario = items.First();

            Assert.Equal(hash, usuario.Senha);

            //Back
            model.SenhaAntiga = senha;
            model.SenhaNova = senhaAntiga;
            result = _controller.AtualizarSenha(model);
            Assert.NotNull(result);
            Assert.True(item.Sucesso);
            Assert.Empty(item.Inconsistencias);
        }

        //Altera somente o nome do usuário
        [Fact]
        public void Put()
        {
            this.DataCreate();

            var list = _controller.Get();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            var items = Assert.IsType<List<UsuarioModel>>(list);
            Assert.Single(items);

            var usuario = items.First();

            var nomeAntigo = usuario.Nome;
            var nome = "James Lavanda";

            UsuarioUpdateModel model = new UsuarioUpdateModel()
            {
                ID = usuario.ID,
                Nome = nome
            };

            // Act
            var result = _controller.Put(model);

            // Assert
            Assert.NotNull(result);
            var item = Assert.IsType<Resultado>(result);
            Assert.True(item.Sucesso);
            Assert.Empty(item.Inconsistencias);

            var resultGet = _controller.Get();

            Assert.NotNull(resultGet);
            Assert.NotEmpty(resultGet);
            var itemsResult = Assert.IsType<List<UsuarioModel>>(resultGet);
            Assert.Single(itemsResult);

            var usuarioResult = itemsResult.First();

            Assert.Equal(nome, usuarioResult.Nome);

            //Back
            model.Nome = nomeAntigo;
            result = _controller.Put(model);
            Assert.NotNull(result);
            Assert.True(item.Sucesso);
            Assert.Empty(item.Inconsistencias);
        }

        //Alteração completa
        [Fact]
        public void PutFull()
        {
            this.DataCreate();

            var list = _controller.Get();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            var items = Assert.IsType<List<UsuarioModel>>(list);
            Assert.Single(items);

            var usuario = items.First();

            var nomeAntigo = usuario.Nome;
            var emailAntigo = usuario.Email;
            var senhaAntiga = "12345";
            var nome = "Joana Dark";
            var email = "joana.dark@yahoo.com.br";
            var senha = "pepeta12";

            UsuarioModel model = new UsuarioModel()
            {
                ID = usuario.ID,
                Nome = nome,
                Email = email,
                Senha = senha
            };

            // Act
            var result = _controller.PutFull(model);

            // Assert
            Assert.NotNull(result);
            var item = Assert.IsType<Resultado>(result);
            Assert.True(item.Sucesso);
            Assert.Empty(item.Inconsistencias);

            var resultGet = _controller.Get();

            Assert.NotNull(resultGet);
            Assert.NotEmpty(resultGet);
            var itemsResult = Assert.IsType<List<UsuarioModel>>(resultGet);
            Assert.Single(itemsResult);

            var usuarioResult = itemsResult.First();

            Assert.Equal(nome, usuarioResult.Nome);

            //Back
            model.Nome = nomeAntigo;
            model.Email = emailAntigo;
            model.Senha = senhaAntiga;

            result = _controller.PutFull(model);
            Assert.NotNull(result);
            Assert.True(item.Sucesso);
            Assert.Empty(item.Inconsistencias);
        }

        [Fact]
        public void Delete()
        {
            this.DataCreate();

            var list = _controller.Get();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            var items = Assert.IsType<List<UsuarioModel>>(list);
            Assert.Single(items);

            var usuario = items.First();

            // Act
            var result = _controller.Delete(usuario.ID);

            // Assert
            Assert.NotNull(result);
            var item = Assert.IsType<Resultado>(result);
            Assert.True(item.Sucesso);
            Assert.Empty(item.Inconsistencias);

            var resultGet = _controller.Get();

            Assert.NotNull(resultGet);
            Assert.Empty(resultGet);
        }

        #endregion

        #region ERROR

        [Fact]
        public void ObterPorId_NotFound()
        {
            this.DataCreate();

            var okResult = _controller.Get(Guid.NewGuid());
            Assert.NotNull(okResult);
            Assert.IsType<NotFoundResult>(okResult);
            //Not Found
            Assert.Equal(404, ((NotFoundResult)okResult).StatusCode);
        }

        [Fact]
        public void Post_EmailDuplicado()
        {
            this.DataCreate();

            UsuarioModel model = new UsuarioModel()
            {
                Email = "thiago.paulistana@gmail.com",
                Nome = "Roberto Justus",
                Senha = "O@prendiz"
            };
            // Act
            var result = _controller.Post(model);

            // Assert
            Assert.NotNull(result);
            var item = Assert.IsType<Resultado>(result);
            Assert.False(item.Sucesso);
            Assert.NotEmpty(item.Inconsistencias);
            Assert.Equal("E-mail já cadastrado", item.Inconsistencias.First());
        }

        [Fact]
        public void Post_ValidaNomeEmpty()
        {
            this.DataCreate();

            UsuarioModel model = new UsuarioModel()
            {
                Email = "roberto_justus@hotmail.com",
                Senha = "O@prendiz"
            };
            // Act
            var result = _controller.Post(model);

            // Assert
            Assert.NotNull(result);
            var item = Assert.IsType<Resultado>(result);
            Assert.False(item.Sucesso);
            Assert.NotEmpty(item.Inconsistencias);
            Assert.Equal("Preencha o Nome do Usuário", item.Inconsistencias.First());
        }

        [Fact]
        public void Post_ValidaEmailEmpty()
        {
            this.DataCreate();

            UsuarioModel model = new UsuarioModel()
            {
                Nome = "Roberto Justus",
                Senha = "O@prendiz"
            };
            // Act
            var result = _controller.Post(model);

            // Assert
            Assert.NotNull(result);
            var item = Assert.IsType<Resultado>(result);
            Assert.False(item.Sucesso);
            Assert.NotEmpty(item.Inconsistencias);
            Assert.Equal("Preencha o Email do Usuário", item.Inconsistencias.First());
        }

        [Fact]
        public void Post_ValidaSenhaEmpty()
        {
            this.DataCreate();

            UsuarioModel model = new UsuarioModel()
            {
                Nome = "Roberto Justus",
                Email = "roberto_justus@hotmail.com",
            };
            // Act
            var result = _controller.Post(model);

            // Assert
            Assert.NotNull(result);
            var item = Assert.IsType<Resultado>(result);
            Assert.False(item.Sucesso);
            Assert.NotEmpty(item.Inconsistencias);
            Assert.Equal("Preencha a Senha do Usuário", item.Inconsistencias.First());
        }

        [Fact]
        public void AtualizarSenhaInvalida()
        {
            this.DataCreate();

            var senhaAntiga = "99879";
            var senha = "joT@111";

            UsuarioUpdateSenhaModel model = new UsuarioUpdateSenhaModel()
            {
                Email = "thiago.paulistana@gmail.com",
                SenhaAntiga = senhaAntiga,
                SenhaNova = senha
            };
            // Act
            var result = _controller.AtualizarSenha(model);

            // Assert
            Assert.NotNull(result);
            var item = Assert.IsType<Resultado>(result);
            Assert.False(item.Sucesso);
            Assert.NotEmpty(item.Inconsistencias);
            Assert.Equal("Usuário não encontrado", item.Inconsistencias.First());
        }

        [Fact]
        public void DeleteInvalido()
        {
            this.DataCreate();

            // Act
            var result = _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.NotNull(result);
            var item = Assert.IsType<Resultado>(result);
            Assert.False(item.Sucesso);
            Assert.NotEmpty(item.Inconsistencias);
            Assert.Equal("Usuário não encontrado", item.Inconsistencias.First());
        }
        #endregion

        private void DataCreate()
        {
            var result = _controller.Get();

            if (result == null || !result.Any( p=>p.Email == "thiago.paulistana@gmail.com"))
            {
                DataPopulatorTest.Init(PasswordEncrypt.Hash("12345"));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using AutoMapper;
using JuntosBusiness.Interface;
using JuntosBusiness.Model;
using JuntosBusiness.Utils;
using JuntosData.Interface;
using JuntosEntities;

namespace JuntosBusiness
{
    public class UsuarioService : IUsuarioService
    {
        private IUsuarioData _data;

        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioData data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public UsuarioModel Obter(Guid id)
        {
            Usuario usuario = _data.Obter(id);
            return _mapper.Map<UsuarioModel>(usuario);
        }

        public UsuarioModel ObterPorEmailSenha(string email, string senha)
        {
            var hash = PasswordEncrypt.Hash(senha);
            Usuario usuario = _data.ObterPorEmailSenha(email, hash);
            return _mapper.Map<UsuarioModel>(usuario);
        }

        public List<UsuarioModel> ListarTodos()
        {
            var usuarios = _data.ListarTodos();
            return _mapper.Map<List<UsuarioModel>>(usuarios);
        }

        public Resultado Incluir(UsuarioModel model)
        {
            Resultado resultado = DadosValidos(model);
            resultado.Acao = "Inclusão de Usuário";

            if (resultado.Inconsistencias.Count == 0 &&
                _data.ObterPorEmail(model.Email) != null)
            {
                resultado.Inconsistencias.Add(
                    "E-mail já cadastrado");
            }

            if (resultado.Inconsistencias.Count == 0)
            {
                model.Senha = PasswordEncrypt.Hash(model.Senha);
                _data.Salvar(_mapper.Map<Usuario>(model));
            }

            return resultado;
        }

        public Resultado Atualizar(UsuarioUpdateModel model)
        {
            Resultado resultado = DadosValidos(model);
            resultado.Acao = "Atualização de Usuário";

            if (resultado.Inconsistencias.Count == 0)
            {
                Usuario usuario = _data.Obter(model.ID);

                if (usuario == null)
                {
                    resultado.Inconsistencias.Add(
                        "Usuário não encontrado");
                }
                else
                {
                    //Só pode atualizar o nome
                    usuario.Nome = model.Nome;
                    _data.Salvar(usuario);
                }
            }

            return resultado;
        }

        public Resultado AtualizarFull(UsuarioModel model)
        {
            Resultado resultado = DadosValidos(model);
            resultado.Acao = "Atualização de Usuário";

            if (resultado.Inconsistencias.Count == 0)
            {
                Usuario usuario = _data.Obter(model.ID);

                if (usuario == null)
                {
                    resultado.Inconsistencias.Add(
                        "Usuário não encontrado");
                }
                else
                {
                    _mapper.Map(model, usuario);
                    usuario.Senha = PasswordEncrypt.Hash(usuario.Senha);
                    _data.Salvar(usuario);
                }
            }

            return resultado;
        }

        public Resultado AtualizarSenha(UsuarioUpdateSenhaModel model)
        {
            Resultado resultado = DadosValidos(model);
            resultado.Acao = "Troca de Senha do Usuário";

            if (resultado.Inconsistencias.Count == 0)
            {
                Usuario usuario = _data.ObterPorEmailSenha(model.Email, PasswordEncrypt.Hash(model.SenhaAntiga));

                if (usuario == null)
                {
                    resultado.Inconsistencias.Add(
                        "Usuário não encontrado");
                }
                else
                {
                    usuario.Senha = PasswordEncrypt.Hash(model.SenhaNova);
                    _data.Salvar(usuario);
                }
            }

            return resultado;
        }

        public Resultado Excluir(Guid id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Usuário";

            Usuario usuario = _data.Obter(id);
            if (usuario == null)
            {
                resultado.Inconsistencias.Add(
                    "Usuário não encontrado");
            }
            else
            {
                _data.Excluir(usuario);
            }

            return resultado;
        }

        #region VALIDACOES
        private Resultado DadosValidos(UsuarioModel model)
        {
            var resultado = new Resultado();
            if (model == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do Usuário");
            }
            else
            {
                if (String.IsNullOrWhiteSpace(model.Nome))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Nome do Usuário");
                }
                if (String.IsNullOrWhiteSpace(model.Email))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Email do Usuário");
                }
                if (String.IsNullOrWhiteSpace(model.Senha))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a Senha do Usuário");
                }
            }

            return resultado;
        }
        private Resultado DadosValidos(UsuarioUpdateModel model)
        {
            var resultado = new Resultado();
            if (model == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do Usuário");
            }
            else
            {
                if (String.IsNullOrWhiteSpace(model.Nome))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Nome do Usuário");
                }
            }

            return resultado;
        }
        private Resultado DadosValidos(UsuarioUpdateSenhaModel model)
        {
            var resultado = new Resultado();
            if (model == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do Usuário");
            }
            else
            {
                if (String.IsNullOrWhiteSpace(model.SenhaAntiga))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a Senha Antiga do Usuário");
                }
                if (String.IsNullOrWhiteSpace(model.SenhaNova))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a Senha Nova do Usuário");
                }
            }

            return resultado;
        }
        #endregion
    }
}
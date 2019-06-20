using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JuntosBusiness.Model;
using JuntosBusiness.Interface;
using System;

namespace APIJuntosUsuarios.Controllers
{
    [Authorize("Bearer")]

    //TODO:criei esse filter para verificar se o usuário logado foi existe, caso o msm seja deletado
    [Authorization]

    [Route("api/[controller]")]
    public class UsuariosController : Controller
    {
        private IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<UsuarioModel> Get()
        {
            return _service.ListarTodos();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var model = _service.Obter(id);
            if (model != null)
                return new ObjectResult(model);
            else
                return NotFound();
        }

        [HttpPost]
        public Resultado Post([FromBody]UsuarioModel model)
        {
            return _service.Incluir(model);
        }

        [Route("senha")]
        [HttpPut]
        public Resultado AtualizarSenha([FromBody]UsuarioUpdateSenhaModel model)
        {
            return _service.AtualizarSenha(model);
        }

        [HttpPut]
        public Resultado Put([FromBody]UsuarioUpdateModel model)
        {
            return _service.Atualizar(model);
        }


        [Route("full")]
        [HttpPut]
        public Resultado PutFull([FromBody]UsuarioModel model)
        {
            return _service.AtualizarFull(model);
        }

        [HttpDelete("{id}")]
        public Resultado Delete(Guid id)
        {
            return _service.Excluir(id);
        }
    }
}
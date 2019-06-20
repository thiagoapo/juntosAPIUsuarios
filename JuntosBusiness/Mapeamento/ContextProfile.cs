using AutoMapper;
using JuntosBusiness.Model;
using JuntosEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntosBusiness.Mapeamento
{
    public class ContextProfile : Profile
    {
        public ContextProfile()
        {
            CreateMap<Usuario, UsuarioModel>();
            CreateMap<UsuarioModel, Usuario>();
        }
    }
}

using AutoMapper;
using JuntosBusiness;
using JuntosBusiness.Interface;
using JuntosBusiness.Mapeamento;
using JuntosData;
using JuntosData.Context.Interface;
using JuntosData.Contexts;
using JuntosData.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIJuntosUsuarios.Configuration
{
    public static class InjecaoDependencias
    {
        public static void Configure(IServiceCollection services)
        {

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ContextProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddSingleton(mapper);

            //BUSINESS
            services.AddSingleton<IUsuarioService, UsuarioService>();

            //DATA
            services.AddSingleton<IApplicationDbContext, ApplicationDbContext>();
            services.AddSingleton<IUsuarioData, UsuarioData>();
        }
    }
}

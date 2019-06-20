using AutoMapper;
using System;

namespace JuntosEntities
{
    public class EntidadeBase
    {
        [IgnoreMap]
        public Guid ID { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}

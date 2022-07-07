using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Model
{
    public interface IClasseBase
    {
        int Id { get; set; }

        IEntityMap Mappings { get; set; }
    }

    public class ClasseBase : IClasseBase
    {
        public int Id { get; set; }

        public IEntityMap Mappings { get; set; }
    }
}
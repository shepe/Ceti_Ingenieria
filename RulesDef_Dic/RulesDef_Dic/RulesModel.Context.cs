﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace RulesDef_Dic
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class RulesModelContainer : DbContext
{
    public RulesModelContainer()
        : base("name=RulesModelContainer")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<Dictionary> DictionarySet { get; set; }

    public virtual DbSet<Rules> RulesSet { get; set; }

    public virtual DbSet<RulesDef> RulesDefSet { get; set; }

}

}


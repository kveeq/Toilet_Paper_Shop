﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Toilet_Paper_Shop.db
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ToiletPaper_dbEntities : DbContext
    {
        public ToiletPaper_dbEntities()
            : base("name=ToiletPaper_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialProd> MaterialProd { get; set; }
        public virtual DbSet<MaterialsAndProducts> MaterialsAndProducts { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<TypeProd> TypeProd { get; set; }
    }
}

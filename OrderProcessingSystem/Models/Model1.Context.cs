﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrderProcessingSystem.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class OrderProcessingSystemEntities : DbContext
    {
        public OrderProcessingSystemEntities()
            : base("name=OrderProcessingSystemEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order_Details> Order_Details { get; set; }
    
        public virtual ObjectResult<GetOrderDetails_Result> GetOrderDetails(Nullable<int> customer_Id)
        {
            var customer_IdParameter = customer_Id.HasValue ?
                new ObjectParameter("Customer_Id", customer_Id) :
                new ObjectParameter("Customer_Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetOrderDetails_Result>("GetOrderDetails", customer_IdParameter);
        }
    
        public virtual ObjectResult<GetProductList_Result> GetProductList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetProductList_Result>("GetProductList");
        }
    
        public virtual int InsertOrderDetails()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertOrderDetails");
        }
    }
}

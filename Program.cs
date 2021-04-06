using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Inclass10
{
    public class Order
    {
        public string ID { get; set; }
        public DateTime OrderDate { get; set; }
        public Double OrderAmount { get; set; }
        public List<Orderdetails> Manyproducts { get; set; }
    }


    public class Product
    {
        public string ID { get; set; }
        public string Productname { get; set; }
        public double Price { get; set; }
        public List<Orderdetails> Manyorders { get; set; }

    }

    public class Orderdetails
    {
        public string ID { get; set; }
        public Order Orders { get; set; }
        public Product Products { get; set; }
        public int Totalproductsinorder { get; set; }

    }

    class StoreDbContext : DbContext
    {
        public DbSet<Product> Allproducts { get; set; }
        public DbSet<Order> Allorders { get; set; }
        public DbSet<Orderdetails> Completeorderdetails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=InClass10;Trusted_Connection=True;MultipleActiveResultSets=true");

        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new StoreDbContext())
            {
                context.Database.EnsureCreated();
                Order[] orderlist = new Order[]
                {
                    new Order{ID="1A",OrderDate=DateTime.Parse("2021-01-01"),OrderAmount=400},
                    new Order{ID="2F",OrderDate=DateTime.Parse("2021-02-01"),OrderAmount=200.4},
                    new Order{ID="3C",OrderDate=DateTime.Parse("2021-03-01"),OrderAmount=298.75},
                    new Order{ID="4G",OrderDate=DateTime.Parse("2021-04-01"),OrderAmount=500.67}

                };
                Product[] productlist = new Product[]
                {
                    new Product{ID="001P",Productname="Milk bottle",Price=20.00},
                    new Product{ID="002RA",Productname="Mochi icecream",Price=8.00},
                    new Product{ID="003N3",Productname="Dasani waterpack",Price=9.00},
                    new Product{ID="004NJ7",Productname="Gluten-free bread",Price=5.00}
                };

                Orderdetails[] Orderdetailslist = new Orderdetails[]
                {
                    new Orderdetails{ID="01ABA",Orders=orderlist[0],Products=productlist[0],Totalproductsinorder =2},
                    new Orderdetails{ID="02ABB",Orders=orderlist[0],Products=productlist[1],Totalproductsinorder =1},
                    new Orderdetails{ID="03ABC",Orders=orderlist[1],Products=productlist[2],Totalproductsinorder =5},
                    new Orderdetails{ID="04ABD",Orders=orderlist[1],Products=productlist[3],Totalproductsinorder =2},
                    new Orderdetails{ID="05ABE",Orders=orderlist[1],Products=productlist[1],Totalproductsinorder =1}
                };

                //context.Allproducts.Add(productlist[0]);
                context.Allproducts.Add(productlist[1]);
                context.Allproducts.Add(productlist[2]);
                context.Allproducts.Add(productlist[3]);
                context.Allorders.Add(orderlist[0]);
                context.Allorders.Add(orderlist[1]);
                context.Allorders.Add(orderlist[2]);
                context.Allorders.Add(orderlist[3]);
                context.Completeorderdetails.Add(Orderdetailslist[0]);
                context.Completeorderdetails.Add(Orderdetailslist[1]);
                context.Completeorderdetails.Add(Orderdetailslist[2]);
                context.Completeorderdetails.Add(Orderdetailslist[3]);
                context.Completeorderdetails.Add(Orderdetailslist[4]);
                context.SaveChanges();

                // Display all orders where a product is sold

                Order readorder = context.Allorders.Include(o => o.Manyproducts).Where(o => o.Manyproducts.Count > 0).FirstOrDefault();

                // For a given product, find the order where it is sold the maximum.

                Product readproduct = context.Completeorderdetails.Where(s => s.Products.Productname = "Dasani waterpack")
                    .OrderbyDescending(p => p.Totalproductsinorder)
                    .First();

            }
        }
    }
}
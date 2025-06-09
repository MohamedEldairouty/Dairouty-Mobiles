using Org.BouncyCastle.Asn1.X509;
using System;
using System.Windows.Forms;

namespace Dairouty_Mobiles
{
   internal static class Classes
   {
      [STAThread]
      static void Main()
      {
         ApplicationConfiguration.Initialize();
         Application.Run(new Login());
      }
   }

   public abstract class Person
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Mobile { get; set; }

      public Person(int id, string name, string mobile)
      {
         Id = id;
         Name = name;
         Mobile = mobile;
      }
      public virtual void DisplayDetails()
      {
         Console.WriteLine($"Name: {Name}, Mobile: {Mobile}");
      }
   }

   public class Customer : Person
   {
      public string Address { get; set; }
      public Customer(int id, string name, string mobile, string address) : base(id, name, mobile)
      {
         Address = address;
      }

      public override void DisplayDetails()
      {
         base.DisplayDetails();
         Console.WriteLine($"Address: {Address}");
      }
   }

   public class DairoutyGroup : Person
   {
      public string Role { get; set; }
      public string Username { get; set; }
      public string Password { get; set; }

      public DairoutyGroup(int id, string name, string mobile, string role, string username, string password) : base(id, name, mobile)
      {
         Role = role;
         Username = username;
         Password = password;
      }

      public override void DisplayDetails()
      {
         base.DisplayDetails();
         Console.WriteLine($"Role: {Role}, Username: {Username}, Password: {Password}");
      }
   }

   public class Seller : DairoutyGroup
   {
      public decimal Salary { get; set; }
      public Seller(int id, string name, string mobile, string role, string username, string password, decimal salary) : base(id, name, mobile, role, username, password)
      {
         Salary = salary;
      }
      public override void DisplayDetails()
      {
         base.DisplayDetails();
         Console.WriteLine($"Salary: {Salary}");
      }
   }
   public class Admin : DairoutyGroup
   {
      public Admin(int id, string name, string mobile, string role, string username, string password) : base(id, name, mobile, role, username, password)
      {
      }
      public void ManageUsers()
      {
         Admin_Sellers admin_Sellers = new Admin_Sellers();
         admin_Sellers.Show();
      }
   }
   public abstract class Product
   {
      public int ProductId { get; set; }
      public string Name { get; set; }
      public decimal Price { get; set; }

      public Product(int productId, string name, decimal price)
      {
         ProductId = productId;
         Name = name;
         Price = price;
      }
      public virtual void DisplayProductInfo()
      {
         Console.WriteLine($"Product Name: {Name}, Price: {Price}");
      }
   }
   public class Tablet : Product
   {
      public string ScreenSize { get; set; }

      public Tablet(int productId, string name, decimal price, string screenSize): base(productId, name, price)
      {
         ScreenSize = screenSize;
      }
      public override void DisplayProductInfo()
      {
         base.DisplayProductInfo();
         Console.WriteLine($"Screen Size: {ScreenSize}");
      }
   }
   public class MobilePhone : Product
   {
      public string RAM { get; set; }
      public string ROM { get; set; }
      public MobilePhone(int productId, string name, decimal price, string ram, string rom): base(productId, name, price)
      {
         RAM = ram;
         ROM = rom;
      }
      public override void DisplayProductInfo()
      {
         base.DisplayProductInfo();
         Console.WriteLine($"RAM: {RAM}, ROM: {ROM}");
      }
   }
   public class New : MobilePhone
   {
      public DateTime ReleaseDate { get; set; }

      public New(int productId, string name, decimal price, string ram, string rom, DateTime releaseDate) : base(productId, name, price, ram, rom)
      {
         ReleaseDate = releaseDate;
      }
      public override void DisplayProductInfo()
      {
         base.DisplayProductInfo();
         Console.WriteLine($"Release Date: {ReleaseDate}");
      }
   }
   public class Old : MobilePhone
   {
      public int BatteryHealth { get; set; }
      public string Condition { get; set; }

      public Old(int productId, string name, decimal price, string ram, string rom, int batteryHealth, string condition)
          : base(productId, name, price, ram, rom)
      {
         BatteryHealth = batteryHealth;
         Condition = condition;
      }
      public override void DisplayProductInfo()
      {
         base.DisplayProductInfo();
         Console.WriteLine($"Battery Health: {BatteryHealth}, Condition: {Condition}");
      }
   }

   public class Invoice
   {
      public int InvoiceNumber { get; set; }
      public Product ProductSold { get; set; }
      public Customer Customer { get; set; }
      public DateTime InvoiceDate { get; set; }

      public Invoice(int invoiceNumber, Product productSold, Customer customer, DateTime invoiceDate)
      {
         InvoiceNumber = invoiceNumber;
         ProductSold = productSold;
         Customer = customer;
         InvoiceDate = invoiceDate;
      }
      public void DisplayInvoiceInfo()
      {
         Console.WriteLine($"Invoice Number: {InvoiceNumber}, Sale Date: {InvoiceDate}");
         ProductSold.DisplayProductInfo();
         Customer.DisplayDetails();
      }
   }
}

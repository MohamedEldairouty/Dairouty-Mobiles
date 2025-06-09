package dairoutymobiles.Phase1;

import java.util.ArrayList;

abstract class Employee
{
    protected static int NumberOfEmps = 0;
    private int Id;
    private String Name;
    private String Email;
    private String PhoneNumber;

    public Employee(int Id, String Name,String Email,String PhoneNumber)
    {
        this.Id = Id;
        this.Name = Name;
        this.Email = Email;
        this.PhoneNumber = PhoneNumber;
        NumberOfEmps++;
    }

    public String getName() { return Name; }

    public abstract void DisplayRole();
}

// Admin Class (Inheritance)
class Admin extends Employee
{
    public Admin(int Id, String Name,String Email,String PhoneNumber) { super(Id, Name,Email,PhoneNumber); }

    @Override
    public void DisplayRole()
    {
        System.out.println(getName() + " is an Admin");
    }
}

// Seller Class (Inheritance)
class Seller extends Employee
{
    private double Salary;

    public Seller(int Id, String Name,String Email,String PhoneNumber,double Salary)
    {
        super(Id, Name,Email,PhoneNumber);
        this.Salary = Salary;
    }

    public double getSalary()
    {
        return Salary;
    }
    public void setSalary(double salary) { Salary = salary; }

    @Override
    public void DisplayRole()
    {
        System.out.println(getName() + " is a Seller");
    }
}

// Product Class
class Product
{
    private int ProductId;
    private String ProductName;
    private double Price;

    public Product(int ProductId, String ProductName, double Price)
    {
        this.ProductId = ProductId;
        this.ProductName = ProductName;
        this.Price = Price;
    }

    public String getProductName() { return ProductName; }
    public double getPrice()
    {
        return Price;
    }
}

// Customer Class
class Customer
{
    private int CustomerId;
    private String CustomerName;
    private Address Address; // Composition

    public Customer(int CustomerId, String CustomerName, Address Address)
    {
        this.CustomerId = CustomerId;
        this.CustomerName = CustomerName;
        this.Address = Address;
    }

    public void DisplayCustomerInfo()
    {
        System.out.println("Customer: " + CustomerName + ", Address: " + Address.GetFullAddress());
    }
}

// Address Class (Composition Component)
class Address {
    private String street;
    private String city;

    public Address(String street, String city)
    {
        this.street = street;
        this.city = city;
    }

    public String GetFullAddress()
    {
        return street + ", " + city;
    }
}

// Invoice Class
class Invoice {
    private int InvoiceId;
    private Customer Customer;
    private ArrayList<Product> Products = new ArrayList<>(); // Aggregation
    private double Total = 0;
    private int Count = 1;

    public Invoice(int InvoiceId, Customer Customer)
    {
        this.InvoiceId = InvoiceId;
        this.Customer = Customer;
    }

    public void AddProduct(Product product)
    {
        Products.add(product);
    }

    public void DisplayInvoice() {
        System.out.println("Invoice ID: " + InvoiceId);
        Customer.DisplayCustomerInfo();
        for (Product product : Products) {
            System.out.println("Product " + Count + ": " + product.getProductName() + ", Price: " + product.getPrice() + " EGP");
            Total += product.getPrice();
            Count++;
        }
        System.out.println("Total: " + Total);
    }

    // Overloaded method
    public void DisplayInvoice(double discountAmount) {
        System.out.println("Invoice ID: " + InvoiceId);
        Customer.DisplayCustomerInfo();
        for (Product product : Products) {
            System.out.println("Product " + Count + ": " + product.getProductName() + ", Price: " + product.getPrice() + " EGP");
            Total += product.getPrice();
            Count++;
        }
        double discountedTotal = Total - discountAmount;
        System.out.println("Total before discount: " + Total);
        System.out.println("Total after discount: " + discountedTotal);
    }


    public double getTotal()
    {
        return Total;
    }
}

// Interface Discountable
interface IDiscountable
{
    double ApplyDiscount(double amount);
}

// Discount Class
class BlackFriday implements IDiscountable
{
    @Override
    public double ApplyDiscount(double amount)
    {
        return amount * 0.9; // 10% discount
    }
}

// Testing Class
class DairoutyMobilesSystem
{
    public static void main(String[] args)
    {
        // Upcasting (Polymorphism)
        Employee admin = new Admin(1, "Abdallah Eldairouty", "eldairoutysat@gmail.com", "01222138937");
        Employee seller = new Seller(2, "Mohamed Eldairouty", "dairocr20@gmail.com", "01277669139",12000);

        admin.DisplayRole();
        seller.DisplayRole();

        // Downcasting (Polymorphism)
        if (seller instanceof Seller)
        {
            Seller SellerDowncasted = (Seller) seller;
            SellerDowncasted.setSalary(SellerDowncasted.getSalary()*1.1);
            System.out.printf("%s is earning %.2f EGP%n", SellerDowncasted.getName(), SellerDowncasted.getSalary());
        }

        System.out.println("Total Number of Employees: " + Employee.NumberOfEmps);
        System.out.println();

        // Composition Example
        Address address = new Address("Smouha", "Alexandria");
        Customer customer = new Customer(101, "Dairo", address);

        // Aggregation Example
        Invoice invoice = new Invoice(1001, customer);
        Product product1 = new Product(201, "IPhone 16 Pro Max", 100000);
        Product product2 = new Product(202, "Samsung Galaxy S24 Ultra", 52000);

        invoice.AddProduct(product1);
        invoice.AddProduct(product2);
        invoice.DisplayInvoice();

        // Interface Implementation
        IDiscountable discount = new BlackFriday();
        double totalAmount = invoice.getTotal();
        double discountedAmount = discount.ApplyDiscount(totalAmount);
        System.out.println("Total after discount: " + discountedAmount);
    }
}
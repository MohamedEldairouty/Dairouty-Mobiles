package dairoutymobiles.DessignPattern.MVC;

public class Product
{
        private String productID;
        private String category;
        private String brand;
        private String model;
        private String color;
        private int ram;
        private int rom;
        private double price;
        private int totalStock;
        private int branchStock;

        public Product(String productID, String category, String brand, String model, String color,
                       int ram, int rom, double price, int totalStock, int branchStock) {
            this.productID = productID;
            this.category = category;
            this.brand = brand;
            this.model = model;
            this.color = color;
            this.ram = ram;
            this.rom = rom;
            this.price = price;
            this.totalStock = totalStock;
            this.branchStock = branchStock;
        }

        public String getProductID() {
            return productID;
        }

        public String getCategory() {
            return category;
        }

        public String getBrand() {
            return brand;
        }

        public String getModel() {
            return model;
        }

        public String getColor() {
            return color;
        }

        public int getRam() {
            return ram;
        }

        public int getRom() {
            return rom;
        }

        public double getPrice() {
            return price;
        }

        public int getTotalStock() {
            return totalStock;
        }

        public int getBranchStock() {
            return branchStock;
        }
    }

package dairoutymobiles.DessignPattern.MVC;

public class InvoiceLineItem {
        private String ProductID;
        private double unitPrice;
        private int quantity;

        public InvoiceLineItem(String productID, double unitPrice, int quantity) {
            this.ProductID = productID;
            this.unitPrice = unitPrice;
            this.quantity = quantity;
        }

        public String getProductID() {
            return ProductID;
        }

        public double getUnitPrice() {
            return unitPrice;
        }

        public int getQuantity() {
            return quantity;
        }

        public void setQuantity(int quantity) {
            this.quantity = quantity;
        }

        @Override
        public boolean equals(Object obj) {
            if (this == obj) return true;
            if (obj == null || getClass() != obj.getClass()) return false;
            InvoiceLineItem that = (InvoiceLineItem) obj;
            return ProductID != null && ProductID.equals(that.ProductID);
        }

        @Override
        public int hashCode() {
            return ProductID != null ? ProductID.hashCode() : 0;
        }

        @Override
        public String toString() {
            return "Product: " + ProductID + ", Price: " + unitPrice + ", Quantity: " + quantity;
        }
    }
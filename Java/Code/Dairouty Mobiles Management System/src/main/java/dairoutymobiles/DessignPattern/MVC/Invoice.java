package dairoutymobiles.DessignPattern.MVC;

import javafx.beans.property.SimpleDoubleProperty;
import javafx.beans.property.SimpleIntegerProperty;
import javafx.beans.property.SimpleObjectProperty;
import javafx.beans.property.SimpleStringProperty;

import java.sql.Date;

public class Invoice
{
    private final SimpleIntegerProperty invoiceId;
    private final SimpleObjectProperty<Date> date;
    private final SimpleStringProperty paymentMethod;
    private final SimpleStringProperty customerName;
    private final SimpleDoubleProperty total;

    public Invoice(int invoiceId, Date date, String paymentMethod, String customerName, double total) {
        this.invoiceId = new SimpleIntegerProperty(invoiceId);
        this.date = new SimpleObjectProperty<>(date);
        this.paymentMethod = new SimpleStringProperty(paymentMethod);
        this.customerName = new SimpleStringProperty(customerName);
        this.total = new SimpleDoubleProperty(total);
    }

    public int getInvoiceId() {
        return invoiceId.get();
    }

    public SimpleIntegerProperty invoiceIdProperty() {
        return invoiceId;
    }

    public Date getDate() {
        return date.get();
    }

    public SimpleObjectProperty<Date> dateProperty() {
        return date;
    }

    public String getPaymentMethod() {
        return paymentMethod.get();
    }

    public SimpleStringProperty paymentMethodProperty() {
        return paymentMethod;
    }

    public String getCustomerName() {
        return customerName.get();
    }

    public SimpleStringProperty customerNameProperty() {
        return customerName;
    }

    public double getTotal() {
        return total.get();
    }

    public SimpleDoubleProperty totalProperty() {
        return total;
    }
}

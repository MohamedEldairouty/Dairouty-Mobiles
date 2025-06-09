package dairoutymobiles.DessignPattern.MVC;

import javafx.beans.property.SimpleStringProperty;
import javafx.beans.property.StringProperty;

public class Customer {
        private StringProperty custID;
        private final StringProperty name;
        private final StringProperty phoneNum;

        public Customer(String custID, String name, String phoneNum) {
            this.custID = new SimpleStringProperty(custID);
            this.name = new SimpleStringProperty(name);
            this.phoneNum = new SimpleStringProperty(phoneNum);
        }

        public String getCustID() {
            return custID.get();
        }

        public StringProperty custIDProperty() {
            return custID;
        }

        public String getName() {
            return name.get();
        }

        public StringProperty nameProperty() {
            return name;
        }

        public String getPhoneNum() {
            return phoneNum.get();
        }

        public StringProperty phoneNumProperty() {
            return phoneNum;
        }
    }

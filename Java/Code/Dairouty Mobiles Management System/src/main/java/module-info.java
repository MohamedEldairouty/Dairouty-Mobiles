module dairoutymobiles {
    requires javafx.controls;
    requires javafx.fxml;
    requires transitive com.microsoft.sqlserver.jdbc;

    requires org.controlsfx.controls;
    requires com.dlsc.formsfx;
    requires org.kordamp.bootstrapfx.core;
    requires java.sql;
    requires org.apache.poi.ooxml;
    requires java.desktop;
    requires java.mail;

    exports dairoutymobiles;
    exports dairoutymobiles.Controller;
exports dairoutymobiles.DessignPattern.MVC;
exports dairoutymobiles.DessignPattern.Singleton;
    opens dairoutymobiles.Controller to javafx.fxml;
}

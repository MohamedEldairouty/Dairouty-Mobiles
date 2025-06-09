package dairoutymobiles.DessignPattern.Singleton;

import java.sql.*;

public class DataBaseManager {

    private static DataBaseManager instance;
    private Connection connection;

    private DataBaseManager() {
        try {
            connection = DriverManager.getConnection(
                    "jdbc:sqlserver://localhost:1433;databaseName=Dairouty_Mobiles;integratedSecurity=true;encrypt=true;trustServerCertificate=true;"
            );
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public static synchronized DataBaseManager getInstance() {
        if (instance == null) {
            instance = new DataBaseManager();
        }
        return instance;
    }

    public Connection getConnection() {
        return connection;
    }
}
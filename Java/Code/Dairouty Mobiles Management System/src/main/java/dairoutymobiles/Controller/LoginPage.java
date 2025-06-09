package dairoutymobiles.Controller;

import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.image.Image;
import javafx.stage.Stage;

import java.io.IOException;
import java.sql.*;

public class LoginPage {

    @FXML
    private TextField tUser;
    @FXML
    private PasswordField tPass;
    @FXML
    private TextField tPassText;
    @FXML
    private ComboBox<String> RoleB;
    @FXML
    private CheckBox ShowPass;
    private static final String DB_URL = "jdbc:sqlserver://localhost:1433;databaseName=Dairouty_Mobiles;integratedSecurity=true;encrypt=true;trustServerCertificate=true;";

    @FXML
    public void initialize() {
        if (RoleB != null) {
            RoleB.getItems().addAll("Admin", "Seller");
        }
    }

    @FXML
    private void handleShowPassword() {
        if (ShowPass.isSelected()) {
            tPass.setVisible(false);
            tPassText.setText(tPass.getText());
            tPassText.setVisible(true);
        }
        else {
            tPassText.setVisible(false);
            tPass.setText(tPassText.getText());
            tPass.setVisible(true);
        }
    }

    @FXML
    private void handleLogin() {
        String password;
        String username = tUser.getText();
        String role = RoleB.getValue();

        if (ShowPass.isSelected())
            password = (tPassText.getText());
        else
            password = (tPass.getText());


        if (username.isEmpty() || password.isEmpty() || role == null) {
            showAlert("Error", "Please fill in all fields!", Alert.AlertType.ERROR);
            return;
        }

        if (validateCredentials(username, password, role)) {
            navigateToDashboard(role);
        }
         else
            showAlert("Error", "Invalid Username or Password!", Alert.AlertType.ERROR);


    }

    private boolean validateCredentials(String username, String password, String role) {
        boolean isValid = false;
        String query = "SELECT Password FROM System_Accounts WHERE Username = ?";
        String dbPassword = null;

        try (Connection connection = DriverManager.getConnection(DB_URL);
             PreparedStatement preparedStatement = connection.prepareStatement(query))
        {
            preparedStatement.setString(1, username);
            ResultSet resultSet = preparedStatement.executeQuery();
            if (resultSet.next()) {
                dbPassword = resultSet.getString("Password");
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        if (dbPassword != null && dbPassword.equals(password)) {
            if (role.equals("Admin") && checkAdminRole(username)) {
                isValid = true;
            } else if (role.equals("Seller") && checkSellerRole(username)) {
                isValid = true;
            }
        }
        return isValid;
    }

    private boolean checkAdminRole(String username) {
        boolean isAdmin = false;
        String query = "SELECT 1 FROM Employees.Admins a JOIN System_Accounts sa ON a.Acc_ID = sa.Acc_ID WHERE sa.Username = ?";

        try (Connection connection = DriverManager.getConnection(DB_URL);
             PreparedStatement preparedStatement = connection.prepareStatement(query)) {

            preparedStatement.setString(1, username);
            ResultSet resultSet = preparedStatement.executeQuery();
            if (resultSet.next()) {
                isAdmin = true;
            }

        } catch (SQLException e) {
            e.printStackTrace();
        }

        return isAdmin;
    }

    private boolean checkSellerRole(String username) {
        boolean isSeller = false;
        String query = "SELECT 1 FROM Employees.Sellers s JOIN System_Accounts sa ON s.Acc_ID = sa.Acc_ID WHERE sa.Username = ?";

        try (Connection connection =DriverManager.getConnection(DB_URL);
             PreparedStatement preparedStatement = connection.prepareStatement(query)) {

            preparedStatement.setString(1, username);
            ResultSet resultSet = preparedStatement.executeQuery();
            if (resultSet.next()) {
                isSeller = true;
            }

        } catch (SQLException e) {
            e.printStackTrace();
        }

        return isSeller;
    }

    private void navigateToDashboard(String role) {
        if (role.equals("Admin")) {
            //try {
              //  showAlert("Welcome", "Hello " + tUser.getText()+" Welcome to Dairouty Mobiles!", Alert.AlertType.INFORMATION);
//                FXMLLoader loader = new FXMLLoader(getClass().getResource("/FXML/AdminDashboard.fxml"));
//                Parent root = loader.load();
//
//                Stage currentStage = (Stage) tUser.getScene().getWindow();
//                currentStage.close();
//
//                Stage newStage = new Stage();
//                newStage.setScene(new Scene(root));
//                newStage.setTitle("Dairouty Mobiles");
//                newStage.show();
//                Image icon = new Image(getClass().getResourceAsStream("/dairoutymobiles/Photos/Dairouty New.jpg"));
//                newStage.getIcons().add(icon);
//                newStage.setResizable(false);
//            } catch (IOException e) {
//                e.printStackTrace();
//            }
        } else if (role.equals("Seller")) {
            try {
                showAlert("Welcome", "Hello " + tUser.getText()+" Welcome to Dairouty Mobiles!", Alert.AlertType.INFORMATION);
                FXMLLoader loader = new FXMLLoader(getClass().getResource("/dairoutymobiles/FXML/Billing.fxml"));
                Parent root = loader.load();

                Stage currentStage = (Stage) tUser.getScene().getWindow();
                currentStage.close();

                Billing sellerBilling = loader.getController();
                sellerBilling.setSellerUsername(tUser.getText());

                Stage newStage = new Stage();
                newStage.setScene(new Scene(root));
                newStage.setTitle("Dairouty Mobiles");
                newStage.show();
                Image icon = new Image(getClass().getResourceAsStream("/dairoutymobiles/Photos/Dairouty New.jpg"));
                newStage.getIcons().add(icon);
                newStage.setResizable(false);
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    private void showAlert(String title, String message, Alert.AlertType alertType) {
        Alert alert = new Alert(alertType);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }

    @FXML
    private void handleExit() {
        System.exit(0);
    }

    @FXML
    private void handleForgetPassword() throws IOException {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/dairoutymobiles/FXML/ResetPassword.fxml"));
            Parent root = loader.load();

            Stage currentStage = (Stage) tUser.getScene().getWindow();
            currentStage.close();

            Stage newStage = new Stage();
            newStage.setScene(new Scene(root));
            newStage.setTitle("Reset Password");
            newStage.show();
            Image icon = new Image(getClass().getResourceAsStream("/dairoutymobiles/Photos/Dairouty New.jpg"));
            newStage.getIcons().add(icon);
            newStage.setResizable(false);
    }
}
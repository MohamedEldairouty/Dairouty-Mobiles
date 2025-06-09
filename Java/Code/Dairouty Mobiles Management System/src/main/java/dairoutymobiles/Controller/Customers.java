package dairoutymobiles.Controller;

import dairoutymobiles.DessignPattern.MVC.Customer;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.Label;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.stage.FileChooser;
import javafx.stage.Stage;
import javafx.scene.control.TextField;

import java.awt.*;
import java.io.*;
import java.net.URI;
import java.sql.*;


public class Customers {

    private String sellerUsername;
    private int branchID;
    private String sellerID;
    private boolean isInitialized = false;
    @FXML
    private ImageView profilepic;
    private final String DEFAULT_PROFILE_IMAGE_PATH = "/Photos/DefaultPP.png";
    private static final String DB_URL = "jdbc:sqlserver://localhost:1433;databaseName=Dairouty_Mobiles;integratedSecurity=true;encrypt=true;trustServerCertificate=true;";

    @FXML
    private Label branchl;

    @FXML
    private TextField nameField, phoneField;

    @FXML
    private TableView<Customer> customerTable;

    @FXML
    private TableColumn<Customer, String> idColumn;

    @FXML
    private TableColumn<Customer, String> nameColumn;

    @FXML
    private TableColumn<Customer, String> phoneColumn;

    private ObservableList<Customer> customerList;

    public void setSellerUsername(String sellerUsername) {
        this.sellerUsername = sellerUsername;
        initializeData();
    }

    private void initializeData() {
        if (!isInitialized) {
            isInitialized = true;
            sellerID = getSellerID(sellerUsername);
            branchID = getBranchID(sellerUsername);
            branchl.setText("Branch : " + branchID);
            initializeTableView();
            if (sellerID != null) {
                LoadProfilePicture(sellerID);
            } else {
                showAlert("Error", "Seller ID not found!", Alert.AlertType.ERROR);
            }
        }
    }
    private void initializeTableView() {
        idColumn.setCellValueFactory(data -> data.getValue().custIDProperty());
        nameColumn.setCellValueFactory(data -> data.getValue().nameProperty());
        phoneColumn.setCellValueFactory(data -> data.getValue().phoneNumProperty());

        customerList = FXCollections.observableArrayList();
        customerTable.setItems(customerList);

        loadCustomerData();

        customerTable.getSelectionModel().selectedItemProperty().addListener((obs, oldSelection, newSelection) -> {
            if (newSelection != null) {
                nameField.setText(newSelection.getName());
                phoneField.setText(newSelection.getPhoneNum());
            }
        });
    }
    private void loadCustomerData() {
        customerList.clear();
        try {
            String query = "SELECT Cust_ID, Name, Phone_Num FROM Customers";
            Statement statement = DriverManager.getConnection(DB_URL).createStatement();
            ResultSet resultSet = statement.executeQuery(query);

            while (resultSet.next()) {
                String custID = resultSet.getString("Cust_ID");
                String name = resultSet.getString("Name");
                String phoneNum = resultSet.getString("Phone_Num");
                customerList.add(new Customer(custID, name, phoneNum));
            }

        } catch (SQLException e) {
            e.printStackTrace();
        }
    }
    private String getSellerID(String username) {
        String query = "SELECT Seller_ID FROM System_Accounts SA JOIN Employees.Sellers S ON SA.Acc_ID = S.Acc_ID WHERE SA.Username = ?";
        try (Connection connection =DriverManager.getConnection(DB_URL);
             PreparedStatement preparedStatement = connection.prepareStatement(query)) {

            preparedStatement.setString(1, username);
            ResultSet resultSet = preparedStatement.executeQuery();
            if (resultSet.next()) {
                return resultSet.getString(1);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return null;
    }

    private int getBranchID(String username) {
        String query = "SELECT Branch_ID FROM System_Accounts SA JOIN Employees.Sellers S ON SA.Acc_ID = S.Acc_ID WHERE SA.Username = ?";
        try (Connection connection =DriverManager.getConnection(DB_URL);
             PreparedStatement preparedStatement = connection.prepareStatement(query)) {

            preparedStatement.setString(1, username);
            ResultSet resultSet = preparedStatement.executeQuery();
            if (resultSet.next()) {
                return resultSet.getInt(1);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return -1;
    }
    @FXML
    private void gotoInvoices () throws IOException
    {
        FXMLLoader loader = new FXMLLoader(getClass().getResource("/dairoutymobiles/FXML/Invoices.fxml"));
        Parent root = loader.load();

        Stage currentStage = (Stage) profilepic.getScene().getWindow();
        currentStage.close();

        Invoices sellerBilling = loader.getController();
        sellerBilling.setSellerUsername(sellerUsername);

        Stage newStage = new Stage();
        newStage.setScene(new Scene(root));
        newStage.setTitle("Dairouty Mobiles");
        newStage.show();
        Image icon = new Image(getClass().getResourceAsStream("/dairoutymobiles/Photos/Dairouty New.jpg"));
        newStage.getIcons().add(icon);
        newStage.setResizable(false);
    }
    private void LoadProfilePicture(String sellerID) {
        String query = "SELECT ProfilePicture FROM Employees.Sellers WHERE Seller_ID = ?";
        try (Connection connection = DriverManager.getConnection(DB_URL);
             PreparedStatement stmt = connection.prepareStatement(query)) {

            stmt.setString(1, sellerID);
            ResultSet resultSet = stmt.executeQuery();

            if (resultSet.next()) {
                byte[] profilePictureData = resultSet.getBytes("ProfilePicture");
                if (profilePictureData != null && profilePictureData.length > 0) {
                    InputStream inputStream = new ByteArrayInputStream(profilePictureData);
                    profilepic.setImage(new Image(inputStream));
                } else {
                    setDefaultProfilePicture();
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    private void setDefaultProfilePicture() {
        try {
            Image defaultImage = new Image(getClass().getResourceAsStream(DEFAULT_PROFILE_IMAGE_PATH));
            profilepic.setImage(defaultImage);
        } catch (Exception e) {
            e.printStackTrace();
            showAlert("Error", "Default profile picture not found.", Alert.AlertType.ERROR);
        }
    }

    @FXML
    private void removePfp() {
        setDefaultProfilePicture();

        String query = "UPDATE Employees.Sellers SET ProfilePicture = NULL WHERE Seller_ID = ?";
        try (Connection connection =DriverManager.getConnection(DB_URL);
             PreparedStatement pstmt = connection.prepareStatement(query)) {

            pstmt.setString(1, sellerID);
            pstmt.executeUpdate();
            showAlert("Removed", "Your profile picture has been removed.", Alert.AlertType.INFORMATION);
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    @FXML
    private void editProfilePicture() {
        FileChooser fileChooser = new FileChooser();
        fileChooser.setTitle("Choose Profile Picture");
        fileChooser.getExtensionFilters().addAll(
                new FileChooser.ExtensionFilter("Image Files", ".png", ".jpg", ".jpeg", ".gif")
        );

        Stage stage = (Stage) profilepic.getScene().getWindow();
        File selectedFile = fileChooser.showOpenDialog(stage);

        if (selectedFile != null) {
            try {
                Image selectedImage = new Image(new FileInputStream(selectedFile));
                profilepic.setImage(selectedImage);
                saveProfilePictureToDatabase(selectedFile);
                showAlert("Success", "Profile picture updated successfully.", Alert.AlertType.INFORMATION);
            } catch (IOException e) {
                e.printStackTrace();
                showAlert("Error", "Could not load the selected file.", Alert.AlertType.ERROR);
            }
        }
    }

    private void saveProfilePictureToDatabase(File file) {
        String query = "UPDATE Employees.Sellers SET ProfilePicture = ? WHERE Seller_ID = ?";
        try (Connection connection = DriverManager.getConnection(DB_URL);
             PreparedStatement pstmt = connection.prepareStatement(query);
             FileInputStream fis = new FileInputStream(file)) {

            pstmt.setBinaryStream(1, fis, (int) file.length());
            pstmt.setString(2, sellerID);
            pstmt.executeUpdate();
        } catch (SQLException | IOException e) {
            e.printStackTrace();
            showAlert("Database Error", "Could not save the profile picture to the database.", Alert.AlertType.ERROR);
        }
    }

    @FXML
    private void logout() throws IOException {
        FXMLLoader loader = new FXMLLoader(getClass().getResource("/dairoutymobiles/FXML/LoginPage.fxml"));
        Parent root = loader.load();

        Stage currentStage = (Stage) profilepic.getScene().getWindow();
        currentStage.close();

        Stage newStage = new Stage();
        newStage.setScene(new Scene(root));
        newStage.setTitle("Dairouty Mobiles");
        newStage.show();
        Image icon = new Image(getClass().getResourceAsStream("/dairoutymobiles/Photos/Dairouty New.jpg"));
        newStage.getIcons().add(icon);
        newStage.setResizable(false);
    }

    private void openUrlLink(String url) {
        try {
            if (Desktop.isDesktopSupported() && Desktop.getDesktop().isSupported(Desktop.Action.BROWSE))
                Desktop.getDesktop().browse(new URI(url));
        } catch (Exception e) {
            e.printStackTrace();
            showAlert("Error", "Unable to open the URL. Please try again.", Alert.AlertType.ERROR);
        }
    }

    @FXML
    private void openFacebook() {
        openUrlLink("https://www.facebook.com/DairoutyMobiles");
    }
    @FXML
    private void openInstagram() {
        openUrlLink("https://www.instagram.com/DairoutyMobiles");
    }
    @FXML
    private void openLocation() {
        openUrlLink("https://maps.app.goo.gl/yN2pVgbSbfegG4jm7");
    }



    private void showAlert(String title, String message, Alert.AlertType alertType) {
        Alert alert = new Alert(alertType);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }


    @FXML
    private void gotoBilling () throws IOException
    {
        FXMLLoader loader = new FXMLLoader(getClass().getResource("/dairoutymobiles/FXML/Billing.fxml"));
        Parent root = loader.load();

        Stage currentStage = (Stage) profilepic.getScene().getWindow();
        currentStage.close();

        Billing sellerBilling = loader.getController();
        sellerBilling.setSellerUsername(sellerUsername);

        Stage newStage = new Stage();
        newStage.setScene(new Scene(root));
        newStage.setTitle("Dairouty Mobiles");
        newStage.show();
        Image icon = new Image(getClass().getResourceAsStream("/dairoutymobiles/Photos/Dairouty New.jpg"));
        newStage.getIcons().add(icon);
        newStage.setResizable(false);
    }

    @FXML
    private void gotoProducts () throws IOException
    {
        FXMLLoader loader = new FXMLLoader(getClass().getResource("/dairoutymobiles/FXML/Products.fxml"));
        Parent root = loader.load();

        Stage currentStage = (Stage) profilepic.getScene().getWindow();
        currentStage.close();

        Products sellerBilling = loader.getController();
        sellerBilling.setSellerUsername(sellerUsername);

        Stage newStage = new Stage();
        newStage.setScene(new Scene(root));
        newStage.setTitle("Dairouty Mobiles");
        newStage.show();
        Image icon = new Image(getClass().getResourceAsStream("/dairoutymobiles/Photos/Dairouty New.jpg"));
        newStage.getIcons().add(icon);
        newStage.setResizable(false);
    }

    @FXML
    private void add() {
        String name = nameField.getText();
        String phoneNum = phoneField.getText();

        if (name.isEmpty() || phoneNum.isEmpty()) {
            showAlert("Error", "Name and Phone Number cannot be empty!", Alert.AlertType.ERROR);
            return;
        }

        try {
            String query = "INSERT INTO Customers (Name, Phone_Num) VALUES (?, ?)";
            PreparedStatement statement;
            statement = DriverManager.getConnection(DB_URL).prepareStatement(query);
            statement.setString(1, name);
            statement.setString(2, phoneNum);
            statement.executeUpdate();

            loadCustomerData();
            clearFields();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    @FXML
    private void edit() {
        Customer selectedCustomer = customerTable.getSelectionModel().getSelectedItem();
        if (selectedCustomer == null) {
            showAlert("Error", "No customer selected!", Alert.AlertType.ERROR);
            return;
        }

        String name = nameField.getText();
        String phoneNum = phoneField.getText();

        if (name.isEmpty() || phoneNum.isEmpty()) {
            showAlert("Error", "Name and Phone Number cannot be empty!", Alert.AlertType.ERROR);
            return;
        }

        try {
            String query = "UPDATE Customers SET Name = ?, Phone_Num = ? WHERE Cust_ID = ?";
            PreparedStatement statement;
            statement = DriverManager.getConnection(DB_URL).prepareStatement(query);
            statement.setString(1, name);
            statement.setString(2, phoneNum);
            statement.setString(3, selectedCustomer.getCustID());
            statement.executeUpdate();

            loadCustomerData();
            clearFields();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    private void clearFields() {
        nameField.clear();
        phoneField.clear();
    }
}
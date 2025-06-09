package dairoutymobiles.Controller;

import dairoutymobiles.DessignPattern.MVC.Invoice;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Label;
import javafx.scene.control.*;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.stage.FileChooser;
import javafx.stage.Stage;

import java.awt.*;
import java.io.*;
import java.net.URI;
import java.sql.*;


public class Invoices {

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
    private TableView<Invoice> invoiceTable;
    @FXML
    private TableColumn<Invoice, Double> totalColumn;
    @FXML
    private TableColumn<Invoice, Integer> invoiceIdColumn;
    @FXML
    private TableColumn<Invoice, Date> dateColumn;
    @FXML
    private TableColumn<Invoice, String> paymentMethodColumn;
    @FXML
    private TableColumn<Invoice, String> customerNameColumn;

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
        invoiceIdColumn.setCellValueFactory(cellData -> cellData.getValue().invoiceIdProperty().asObject());
        dateColumn.setCellValueFactory(cellData -> cellData.getValue().dateProperty());
        paymentMethodColumn.setCellValueFactory(cellData -> cellData.getValue().paymentMethodProperty());
        customerNameColumn.setCellValueFactory(cellData -> cellData.getValue().customerNameProperty());
        totalColumn.setCellValueFactory(cellData -> cellData.getValue().totalProperty().asObject());  // Bind the total column

        loadInvoices();

    }
    @FXML
    private void loadInvoices() {
        ObservableList<Invoice> invoices = FXCollections.observableArrayList();

        try (Connection connection =DriverManager.getConnection(DB_URL)) {
            String query = "SELECT I.Invoice_ID, I.Date, I.Payment_Method, C.Name AS Customer_Name, "
                    + "SUM(ILI.Unit_Price * ILI.Quantity) AS Total "
                    + "FROM Sales.Invoices I "
                    + "JOIN Customers C ON I.Cust_ID = C.Cust_ID "
                    + "JOIN Sales.Inv_Line_Items ILI ON I.Invoice_ID = ILI.Invoice_ID "
                    + "WHERE I.Seller_ID = ? AND I.Branch_ID = ? "
                    + "GROUP BY I.Invoice_ID, I.Date, I.Payment_Method, C.Name";

            try (PreparedStatement statement = connection.prepareStatement(query)) {
                statement.setString(1, sellerID);
                statement.setInt(2, branchID);

                ResultSet resultSet = statement.executeQuery();
                while (resultSet.next()) {
                    int invoiceId = resultSet.getInt("Invoice_ID");
                    Date date = resultSet.getDate("Date");
                    String paymentMethod = resultSet.getString("Payment_Method");
                    String customerName = resultSet.getString("Customer_Name");
                    double total = resultSet.getDouble("Total");

                    invoices.add(new Invoice(invoiceId, date, paymentMethod, customerName, total));
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        invoiceTable.setItems(invoices);
    }
    @FXML
    private void exportInvoices() {
        String defaultFileName = sellerUsername + "_Invoices.csv";

        FileChooser fileChooser = new FileChooser();
        fileChooser.getExtensionFilters().add(new FileChooser.ExtensionFilter("CSV Files", "*.csv"));
        fileChooser.setInitialFileName(defaultFileName);

        File file = fileChooser.showSaveDialog(new Stage());

        if (file != null) {
            try (BufferedWriter writer = new BufferedWriter(new FileWriter(file))) {
                writer.write("Invoice ID, Date, Payment Method, Customer Name, Total");
                writer.newLine();

                for (Invoice invoice : invoiceTable.getItems()) {
                    writer.write(invoice.getInvoiceId() + ",");
                    writer.write(invoice.getDate() + ",");
                    writer.write(invoice.getPaymentMethod() + ",");
                    writer.write(invoice.getCustomerName() + ",");
                    writer.write(invoice.getTotal() + "");
                    writer.newLine();
                }
                writer.flush();
                System.out.println("Export successful!");
            } catch (IOException e) {
                e.printStackTrace();
            }
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
        try (Connection connection = DriverManager.getConnection(DB_URL);
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
        try (Connection connection =DriverManager.getConnection(DB_URL);
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
    private void gotoCustomers () throws IOException
    {
        FXMLLoader loader = new FXMLLoader(getClass().getResource("/dairoutymobiles/FXML/Customers.fxml"));
        Parent root = loader.load();

        Stage currentStage = (Stage) profilepic.getScene().getWindow();
        currentStage.close();

        Customers sellerBilling = loader.getController();
        sellerBilling.setSellerUsername(sellerUsername);

        Stage newStage = new Stage();
        newStage.setScene(new Scene(root));
        newStage.setTitle("Dairouty Mobiles");
        newStage.show();
        Image icon = new Image(getClass().getResourceAsStream("/dairoutymobiles/Photos/Dairouty New.jpg"));
        newStage.getIcons().add(icon);
        newStage.setResizable(false);
    }


}
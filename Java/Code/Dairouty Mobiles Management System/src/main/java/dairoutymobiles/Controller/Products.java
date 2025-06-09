package dairoutymobiles.Controller;

import dairoutymobiles.DessignPattern.MVC.Product;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Label;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.stage.FileChooser;
import javafx.stage.Stage;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;

import java.awt.Desktop;
import java.io.*;
import java.net.URI;
import java.sql.*;


public class Products {

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
    private TableView<Product> ProductsTable;
    @FXML
    private TableColumn<Product, String> ProductID;
    @FXML
    private TableColumn<Product, String> Category;
    @FXML
    private TableColumn<Product, String> Brand;
    @FXML
    private TableColumn<Product, String> Model;
    @FXML
    private TableColumn<Product, String> Color;
    @FXML
    private TableColumn<Product, Integer> RAM;
    @FXML
    private TableColumn<Product, Integer> ROM;
    @FXML
    private TableColumn<Product, Double> Price;
    @FXML
    private TableColumn<Product, Integer> BranchStock;
    @FXML
    private TableColumn<Product, Integer> TotalStock;

    private ObservableList<Product> products = FXCollections.observableArrayList();

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

    private String getSellerID(String username) {
        String query = "SELECT Seller_ID FROM System_Accounts SA JOIN Employees.Sellers S ON SA.Acc_ID = S.Acc_ID WHERE SA.Username = ?";
        try (Connection connection = DriverManager.getConnection(DB_URL);
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
        try (Connection connection = DriverManager.getConnection(DB_URL);
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



    private void initializeTableView() {
        ProductID.setCellValueFactory(new PropertyValueFactory<>("productID"));
        Category.setCellValueFactory(new PropertyValueFactory<>("category"));
        Brand.setCellValueFactory(new PropertyValueFactory<>("brand"));
        Model.setCellValueFactory(new PropertyValueFactory<>("model"));
        Color.setCellValueFactory(new PropertyValueFactory<>("color"));
        RAM.setCellValueFactory(new PropertyValueFactory<>("ram"));
        ROM.setCellValueFactory(new PropertyValueFactory<>("rom"));
        Price.setCellValueFactory(new PropertyValueFactory<>("price"));
        TotalStock.setCellValueFactory(new PropertyValueFactory<>("totalStock"));
        BranchStock.setCellValueFactory(new PropertyValueFactory<>("branchStock"));

        loadProducts();
    }

    private void loadProducts() {
        ObservableList<Product> products = FXCollections.observableArrayList();

        String query = "SELECT p.Product_ID, p.Category, p.Brand, p.Model, p.Color, p.RAM_GB, p.ROM_GB, p.Price_EGP, p.Total_Stock, " +
                "ISNULL(bs.Quantity, 0) AS Branch_Stock " +
                "FROM Inventory.Products p " +
                "LEFT JOIN Inventory.Branches_Stock bs " +
                "ON p.Product_ID = bs.Product_ID AND bs.Branch_ID = ?";

        try (Connection connection = DriverManager.getConnection(DB_URL);
             PreparedStatement stmt = connection.prepareStatement(query)) {

            stmt.setInt(1, branchID);
            ResultSet resultSet = stmt.executeQuery();

            while (resultSet.next()) {
                String productId = resultSet.getString("Product_ID");
                String category = resultSet.getString("Category");
                String brand = resultSet.getString("Brand");
                String model = resultSet.getString("Model");
                String color = resultSet.getString("Color");
                int ramGB = resultSet.getInt("RAM_GB");
                int romGB = resultSet.getInt("ROM_GB");
                double priceEGP = resultSet.getDouble("Price_EGP");
                int totalStock = resultSet.getInt("Total_Stock");
                int branchStock = resultSet.getInt("Branch_Stock");

                products.add(new Product(productId, category, brand, model, color, ramGB, romGB, priceEGP, totalStock, branchStock));
            }

            ProductsTable.setItems(products);

        } catch (SQLException e) {
            e.printStackTrace();
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
    @FXML
    private void refresh ()
    {
        ProductsTable.getItems().clear();
        loadProducts();
    }
    @FXML
    private void export ()
    {
        FileChooser fileChooser = new FileChooser();
        fileChooser.getExtensionFilters().add(new FileChooser.ExtensionFilter("Excel Files", "*.xlsx"));
        fileChooser.setInitialFileName("Branch " + branchID + " Products.xlsx");
        File file = fileChooser.showSaveDialog(null);
        if (file != null) {
            exportToExcel(file);
        }
    }
    private void exportToExcel(File file) {
        XSSFWorkbook workbook = new XSSFWorkbook();
        XSSFSheet sheet = workbook.createSheet("Products");

        XSSFRow headerRow = sheet.createRow(0);
        String[] headers = { "Product ID", "Category", "Brand", "Model", "Color", "RAM (GB)", "ROM (GB)", "Price (EGP)", "Total Stock", "Branch Stock" };
        for (int i = 0; i < headers.length; i++) {
            headerRow.createCell(i).setCellValue(headers[i]);
        }

        int rowNum = 1;
        for (Product product : ProductsTable.getItems()) {
            XSSFRow row = sheet.createRow(rowNum++);
            row.createCell(0).setCellValue(product.getProductID());
            row.createCell(1).setCellValue(product.getCategory());
            row.createCell(2).setCellValue(product.getBrand());
            row.createCell(3).setCellValue(product.getModel());
            row.createCell(4).setCellValue(product.getColor());
            row.createCell(5).setCellValue(product.getRam());
            row.createCell(6).setCellValue(product.getRom());
            row.createCell(7).setCellValue(product.getPrice());
            row.createCell(8).setCellValue(product.getTotalStock());
            row.createCell(9).setCellValue(product.getBranchStock());
        }

        try (FileOutputStream fileOut = new FileOutputStream(file)) {
            workbook.write(fileOut);
            showAlert("Export Successful", "The product list has been successfully exported to Excel.", Alert.AlertType.INFORMATION);
        } catch (IOException e) {
            e.printStackTrace();
            showAlert("Export Error", "An error occurred while exporting the product list to Excel.", Alert.AlertType.ERROR);
        }
    }
    }
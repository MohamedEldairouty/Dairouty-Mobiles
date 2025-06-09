package dairoutymobiles.Controller;

import com.microsoft.sqlserver.jdbc.SQLServerDataTable;
import com.microsoft.sqlserver.jdbc.SQLServerException;
import com.microsoft.sqlserver.jdbc.SQLServerPreparedStatement;
import dairoutymobiles.DessignPattern.MVC.InvoiceLineItem;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.TextField;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.stage.FileChooser;
import javafx.stage.Stage;

import java.awt.Desktop;
import java.io.*;
import java.net.URI;
import java.sql.*;


public class Billing {
    private String sellerUsername;
    private int branchID;
    private String sellerID;
    private boolean isInitialized = false;
    @FXML
    private ImageView profilepic;
    private final String DEFAULT_PROFILE_IMAGE_PATH = "/Photos/DefaultPP.png";
    private static final String DB_URL = "jdbc:sqlserver://localhost:1433;databaseName=Dairouty_Mobiles;integratedSecurity=true;encrypt=true;trustServerCertificate=true;";
    @FXML
    private TextField cusname;
    @FXML
    private Label branchl;
    @FXML
    private TextField phonenum;
    @FXML
    private ComboBox<String> paymentmethod;
    @FXML
    private ComboBox<String> productID;
    @FXML
    private ComboBox<Integer> quantity;
    @FXML
    private TextField unitprice;
    @FXML
    private TextField GrandTotal;
    @FXML
    private TableView<InvoiceLineItem> InvLineItemsTable;
    @FXML
    private TableColumn<InvoiceLineItem, String> ProductNameCol;
    @FXML
    private TableColumn<InvoiceLineItem, Double> UnitPriceCol;
    @FXML
    private TableColumn<InvoiceLineItem, Integer> QuantityCol;

    private ObservableList<InvoiceLineItem> lineItems = FXCollections.observableArrayList();

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
            initializeComboBoxes();
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
        try (Connection connection =DriverManager.getConnection(DB_URL);
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
    private void initializeComboBoxes() {
        paymentmethod.setItems(FXCollections.observableArrayList("Cash", "Credit Card", "Instapay"));

        String productQuery = "SELECT Product_ID FROM Inventory.Branches_Stock  WHERE Branch_ID = ? AND Quantity > 0";
        try (Connection connection = DriverManager.getConnection(DB_URL);
             PreparedStatement stmt = connection.prepareStatement(productQuery))
        {
            stmt.setInt(1, branchID);
            ResultSet rs = stmt.executeQuery();
            ObservableList<String> productList = FXCollections.observableArrayList();
            while (rs.next()) {
                productList.add(rs.getString("Product_ID"));
            }
            productID.setItems(productList);
        } catch (SQLException e) {
            e.printStackTrace();
        }

        productID.setOnAction(event -> updateUnitPriceAndQuantity());
    }

    private void updateUnitPriceAndQuantity() {
        String selectedProduct = productID.getValue();
        if (selectedProduct != null) {
            String query ="SELECT P.Price_EGP, S.Quantity FROM Inventory.Products P, Inventory.Branches_Stock S WHERE P.Product_ID = ? AND P.Product_ID = S.Product_ID AND S.Branch_ID = ? AND S.Quantity > 0";
            try (Connection connection = DriverManager.getConnection(DB_URL);
                 PreparedStatement stmt = connection.prepareStatement(query)) {

                stmt.setString(1, selectedProduct);
                stmt.setInt(2, branchID);
                ResultSet rs = stmt.executeQuery();
                if (rs.next()) {
                    unitprice.setText(String.valueOf(rs.getDouble("Price_EGP")));
                    int stock = rs.getInt("Quantity");
                    ObservableList<Integer> quantityList = FXCollections.observableArrayList();
                    for (int i = 1; i <= stock; i++) {
                        quantityList.add(i);
                    }
                    quantity.setItems(quantityList);
                }
            } catch (SQLException e) {
                e.printStackTrace();
            }
        }
    }

    private void initializeTableView() {
        ProductNameCol.setCellValueFactory(new PropertyValueFactory<>("productID"));
        UnitPriceCol.setCellValueFactory(new PropertyValueFactory<>("unitPrice"));
        QuantityCol.setCellValueFactory(new PropertyValueFactory<>("quantity"));

        InvLineItemsTable.setItems(lineItems);
    }

    @FXML
    private void addItem() {
        if (productID.getValue() == null || unitprice.getText().isEmpty() || quantity.getValue() == null || cusname.getText().isEmpty() || phonenum.getText().isEmpty() || paymentmethod.getValue() == null) {
            showAlert("Error", "Please fill in all fields!", Alert.AlertType.ERROR);
            return;
        }

        String product = productID.getValue();
        double price = Double.parseDouble(unitprice.getText());
        int qty = quantity.getValue();

        boolean productExists = false;
        for (InvoiceLineItem item : lineItems) {
            if (item.getProductID().equals(product)) {
                item.setQuantity(qty);
                productExists = true;
                break;
            }
        }

        if (!productExists) {
            lineItems.add(new InvoiceLineItem(product, price, qty));
        }

        double total = 0;
        for (InvoiceLineItem item : lineItems) {
            total += item.getUnitPrice() * item.getQuantity();
        }
        GrandTotal.setText(String.valueOf(total));

        InvLineItemsTable.refresh();
        clearProductFields();
    }



    private void clearProductFields() {
        productID.getSelectionModel().clearSelection();
        unitprice.clear();
        quantity.getSelectionModel().clearSelection();
    }

    @FXML
    private void removeItem() {
        InvoiceLineItem selectedItem = InvLineItemsTable.getSelectionModel().getSelectedItem();
        if (selectedItem != null) {
            lineItems.remove(selectedItem);
            double total = Double.parseDouble(GrandTotal.getText());
            GrandTotal.setText(String.valueOf(total - (selectedItem.getUnitPrice() * selectedItem.getQuantity())));
        }
    }

    @FXML
    private void saveInvoice() throws SQLServerException {
        String customerName = cusname.getText();
        String phoneNumber = phonenum.getText();
        String payment = paymentmethod.getValue();

        SQLServerDataTable lineItemsTable = new SQLServerDataTable();
        lineItemsTable.addColumnMetadata("Product_ID", Types.NVARCHAR);
        lineItemsTable.addColumnMetadata("Unit_Price", Types.DECIMAL);
        lineItemsTable.addColumnMetadata("Quantity", Types.INTEGER);

        if (lineItems.isEmpty())
        {
            showAlert("Error", "Invoice is Empty!" , Alert.AlertType.ERROR);
            return;
        }

        for (InvoiceLineItem item : lineItems) {
            lineItemsTable.addRow(item.getProductID(), item.getUnitPrice(), item.getQuantity());
        }

        try (Connection connection = DriverManager.getConnection(DB_URL);) {
            connection.setAutoCommit(false);

            String insertInvoiceQuery = "EXEC Create_New_Inv ?, ?, ?, ?, ?, ?";

            try (SQLServerPreparedStatement stmt = (SQLServerPreparedStatement) connection.prepareStatement(insertInvoiceQuery)) {
                stmt.setString(1, customerName);
                stmt.setString(2, phoneNumber);
                stmt.setString(3, payment);
                stmt.setString(4, sellerID);
                stmt.setInt(5, branchID);
                stmt.setStructured(6, "dbo.LineItemType", lineItemsTable);

                stmt.executeUpdate();
                connection.commit();
                showAlert("Success", "Invoice saved successfully!", Alert.AlertType.INFORMATION);
            }
        } catch (SQLException e) {
            showAlert("Error", "Failed to save invoice. " +  e.getMessage(), Alert.AlertType.ERROR);
        }
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
    private void clearTable() {
        lineItems.clear();
        GrandTotal.clear();
        clearProductFields();
        cusname.clear();
        phonenum.clear();
        paymentmethod.getSelectionModel().clearSelection();
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
    private String getProductNameById(String productId) {
        String query = "SELECT Model + ' ' + Color AS ProductName FROM Inventory.Products WHERE Product_ID = ?";
        try (Connection connection = DriverManager.getConnection(DB_URL);
             PreparedStatement stmt = connection.prepareStatement(query)) {
            stmt.setString(1, productId);
            ResultSet rs = stmt.executeQuery();
            if (rs.next()) {
                return rs.getString("ProductName");
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return null;
    }

    @FXML
    private void printInvoice() {
        if (lineItems.isEmpty()) {
            showAlert("Error", "Cannot print an empty invoice.", Alert.AlertType.ERROR);
            return;
        }

        StringBuilder invoiceContent = new StringBuilder();
        invoiceContent.append("Invoice\n");
        invoiceContent.append("==============================\n");
        invoiceContent.append("Customer Name: ").append(cusname.getText()).append("\n");
        invoiceContent.append("Phone Number: ").append(phonenum.getText()).append("\n");
        invoiceContent.append("Payment Method: ").append(paymentmethod.getValue()).append("\n");
        invoiceContent.append("==============================\n");
        invoiceContent.append("Product ID\tProduct Name\tUnit Price\tQuantity\tTotal\n");

        for (InvoiceLineItem item : lineItems) {
            String productName = getProductNameById(item.getProductID());
            double total = item.getUnitPrice() * item.getQuantity();
            invoiceContent.append(item.getProductID()).append("\t")
                    .append(productName).append("\t")
                    .append(item.getUnitPrice()).append("\t")
                    .append(item.getQuantity()).append("\t")
                    .append(total).append("\n");
        }

        invoiceContent.append("==============================\n");
        invoiceContent.append("Grand Total: ").append(GrandTotal.getText()).append("\n");

        try {
            File tempFile = File.createTempFile("Invoice_", ".txt");
            try (BufferedWriter writer = new BufferedWriter(new FileWriter(tempFile))) {
                writer.write(invoiceContent.toString());
            }

            Desktop desktop = Desktop.getDesktop();
            if (desktop.isSupported(Desktop.Action.PRINT)) {
                desktop.print(tempFile);
                showAlert("Success", "Invoice sent to printer.", Alert.AlertType.INFORMATION);
            } else {
                showAlert("Error", "Printing is not supported on this system.", Alert.AlertType.ERROR);
            }

        } catch (IOException e) {
            e.printStackTrace();
            showAlert("Error", "Failed to print invoice. " + e.getMessage(), Alert.AlertType.ERROR);
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
    private void gotoProd () throws IOException
    {
        FXMLLoader loader = new FXMLLoader(getClass().getResource("/dairoutymobiles/FXML/Products.fxml"));
        Parent root = loader.load();

        Stage currentStage = (Stage) profilepic.getScene().getWindow();
        currentStage.close();

        Products products = loader.getController();
        products.setSellerUsername(sellerUsername);

        Stage newStage = new Stage();
        newStage.setScene(new Scene(root));
        newStage.setTitle("Dairouty Mobiles");
        newStage.show();
        Image icon = new Image(getClass().getResourceAsStream("/dairoutymobiles/Photos/Dairouty New.jpg"));
        newStage.getIcons().add(icon);
        newStage.setResizable(false);
    }

}
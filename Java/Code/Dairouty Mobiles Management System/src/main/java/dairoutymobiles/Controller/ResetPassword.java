package dairoutymobiles.Controller;


import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.image.Image;
import javafx.stage.Stage;


import javax.mail.*;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import java.io.IOException;
import java.sql.*;
import java.util.Properties;

public class ResetPassword
{
    @FXML
    private TextField tUser;
    @FXML
    private TextField temail;
    @FXML
    private Button ResetB;
    @FXML
    private CheckBox ShowPass;
    @FXML
    private TextField tPassnewtext;
    @FXML
    private TextField totp;
    @FXML
    private PasswordField tPassnew;
    @FXML
    private Button SendOTP;
    @FXML
    private Button VerifyOTP;

    private String genratedotp;
    private static final String DB_URL = "jdbc:sqlserver://localhost:1433;databaseName=Dairouty_Mobiles;integratedSecurity=true;encrypt=true;trustServerCertificate=true;";

    @FXML
    private void handleShowPassword() {
        if (ShowPass.isSelected()) {
            tPassnew.setVisible(false);
            tPassnewtext.setText(tPassnew.getText());
            tPassnewtext.setVisible(true);
        }
        else {
            tPassnewtext.setVisible(false);
            tPassnew.setText(tPassnewtext.getText());
            tPassnew.setVisible(true);
        }
    }

    @FXML
    private void handleExit() {
        System.exit(0);
    }

    private boolean verifyuseremail(String username,String email)
    {
        boolean isvalid = false;
        String query = "SELECT Email FROM Employees.Admins A , System_Accounts S WHERE S.Acc_ID = A.Acc_ID AND S.Username = ?";
        String dbemail = null;
        try (Connection connection = DriverManager.getConnection(DB_URL);
             PreparedStatement preparedStatement = connection.prepareStatement(query)) {

            preparedStatement.setString(1, username);
            ResultSet resultSet = preparedStatement.executeQuery();
            if (resultSet.next())
                dbemail = resultSet.getString("Email");

        }
        catch (SQLException e) {
            e.printStackTrace();
        }
        if (dbemail == null)
        {
            query = "SELECT Email FROM Employees.Sellers A , System_Accounts S WHERE S.Acc_ID = A.Acc_ID AND S.Username = ?";
            try (Connection connection =  DriverManager.getConnection(DB_URL);
                 PreparedStatement preparedStatement = connection.prepareStatement(query)) {

                preparedStatement.setString(1, username);
                ResultSet resultSet = preparedStatement.executeQuery();
                if (resultSet.next())
                    dbemail = resultSet.getString("Email");

            }
            catch (SQLException e) {
                e.printStackTrace();
            }
        }
        if (dbemail != null && dbemail.equals(email)) isvalid = true;
        return isvalid;
    }

    @FXML
    private void SendEmailOTP()
    {
        boolean isvalid = verifyuseremail(tUser.getText(),temail.getText());
        if (isvalid)
        {
            showAlert("OTP Sent", "OTP Sent to your mail", Alert.AlertType.INFORMATION);
            totp.setDisable(false);
            SendOTP.setDisable(true);
            VerifyOTP.setDisable(false);
            genratedotp = GenerateOTP();
            String email = temail.getText();
            sendtoemail(email,genratedotp);
            System.out.println("OTP: " + genratedotp);
        }
        else
            showAlert("Error", "Invalid Username or Email!", Alert.AlertType.ERROR);

    }

    private String GenerateOTP()
    {
        int randomPin   =(int)(Math.random()*9000)+1000;
        return String.valueOf(randomPin);
    }

    private void sendtoemail(String email, String otp) {
    String from = "dairocr20@gmail.com";
    String to = email;
    String subject = "Password Reset OTP";
    String body = "Your OTP for password reset is: " + otp;

    Properties props = new Properties();
    props.put("mail.smtp.host", "smtp.gmail.com");
    props.put("mail.smtp.port", "587");
    props.put("mail.smtp.auth", "true");
    props.put("mail.smtp.starttls.enable", "true");

    Session session = Session.getInstance(props, new Authenticator() {
        @Override
        protected PasswordAuthentication getPasswordAuthentication() {
            return new PasswordAuthentication("dairocr20@gmail.com", "hdvj zzsw jjkr flug");
        }
    });

    try {
        Message message = new MimeMessage(session);
        message.setFrom(new InternetAddress(from));
        message.setRecipients(Message.RecipientType.TO, InternetAddress.parse(to));
        message.setSubject(subject);
        message.setText(body);
        Transport.send(message);
        System.out.println("OTP sent successfully to " + to);
    } catch (MessagingException mex) {
        mex.printStackTrace();
    }
}

    @FXML
    private void VerifyOTP()
    {
        if (totp.getText().equals(genratedotp))
        {
            showAlert("Success", "OTP Verified", Alert.AlertType.INFORMATION);
            tPassnew.setDisable(false);
            tPassnewtext.setDisable(false);
            ResetB.setDisable(false);
        }
        else
            showAlert("Error", "Invalid OTP!", Alert.AlertType.ERROR);
    }

    @FXML
    private void ResetPass() throws IOException
    {
        if (ShowPass.isSelected())
            tPassnewtext.setText(tPassnew.getText());
        else
            tPassnewtext.setText(tPassnewtext.getText());
        String newpass = tPassnewtext.getText();
        if (newpass.isEmpty())
        {
            showAlert("Error", "Please fill in all fields!", Alert.AlertType.ERROR);
            return;
        }

        String username = tUser.getText();
        String dbUrl = "jdbc:sqlserver://localhost:1433;databaseName=Dairouty_Mobiles;integratedSecurity=true;encrypt=true;trustServerCertificate=true;";
        String query = "UPDATE System_Accounts SET Password = ? WHERE Username = ?";
        try (Connection connection =  DriverManager.getConnection(DB_URL);
             PreparedStatement preparedStatement = connection.prepareStatement(query)) {

            preparedStatement.setString(1, newpass);
            preparedStatement.setString(2, username);
            preparedStatement.executeUpdate();
            showAlert("Success", "Password Reset Successfully", Alert.AlertType.INFORMATION);
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/dairoutymobiles/FXML/LoginPage.fxml"));
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
        catch (SQLException e) {
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
}

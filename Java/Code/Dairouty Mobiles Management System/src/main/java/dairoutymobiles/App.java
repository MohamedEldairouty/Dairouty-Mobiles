package dairoutymobiles;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.image.Image;
import javafx.stage.Stage;

import java.io.IOException;

public class App extends Application {
    @Override
    public void start(Stage primaryStage) throws IOException {
        Image icon = new Image(getClass().getResourceAsStream("/dairoutymobiles/Photos/Dairouty New.jpg"));
        primaryStage.getIcons().add(icon);

        FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("/dairoutymobiles/FXML/LoginPage.fxml"));
        Scene scene = new Scene(fxmlLoader.load());
        primaryStage.setScene(scene);
        primaryStage.setResizable(false);
        primaryStage.setTitle("Dairouty Mobiles");
        primaryStage.show();
    }

    public static void main(String[] args) {
        launch(args);
    }
}
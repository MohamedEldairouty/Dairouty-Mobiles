using System;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using MimeKit;
using Microsoft.Data.SqlClient;


namespace Dairouty_Mobiles
{
   public partial class Reset_Pass : Form
   {
      private string generatedOTP;
      private const string ClientId = "1042439327753-24ajmjaf83u1ledud1fh782pltk82ut1.apps.googleusercontent.com";
      private const string ClientSecret = "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";

      public Reset_Pass()
      {
         InitializeComponent();
      }

      private void ExitB_Click(object sender, EventArgs e)
      {
         Application.Exit();
      }

      private void ShowPass_CheckedChanged(object sender, EventArgs e)
      {
         NewPasswordTextBox.UseSystemPasswordChar = !ShowPass.Checked;
      }

      private void Login_Load(object sender, EventArgs e)
      {
         MaximizeBox = false;
         MinimizeBox = false;
         ControlBox = false;
      }

      private async void SendOTPButton_Click(object sender, EventArgs e)
      {
         string username = UsernameTextBox.Text;
         string email = EmailTextBox.Text;

         if (isValidUser(username, email))
         {
            generatedOTP = GenerateOTP();
            await SendEmail(email, generatedOTP);
            OTPTextBox.Enabled = true;
            VerifyOTPButton.Enabled = true;
         }
         else
         {
            MessageBox.Show("Invalid Username or Email", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
      private void VerifyOTPButton_Click(object sender, EventArgs e)
      {
         if (OTPTextBox.Text == generatedOTP)
         {
            MessageBox.Show("OTP verified! You can now reset your password.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NewPasswordTextBox.Enabled = true;
            ResetPasswordButton.Enabled = true;
         }
         else
         {
            MessageBox.Show("Invalid OTP!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private async void ResetPasswordButton_Click(object sender, EventArgs e)
      {
         string username = UsernameTextBox.Text;
         string newPassword = NewPasswordTextBox.Text;

         if (await ResetPassword(username, newPassword))
         {
            MessageBox.Show("Password reset successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Login login = new Login();
            login.Show();
            Visible = false;
         }
         else
         {
            MessageBox.Show("Failed to reset password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private bool isValidUser(string username, string email)
      {
         try
         {
            string connectionString = "Data Source=DAIRO\\SQLEXPRESS;Initial Catalog=Dairouty Mobiles;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
               con.Open();
               string query = "SELECT COUNT(*) FROM [Dairouty Mobiles Login] WHERE Username=@Username AND Email=@Email";
               using (SqlCommand cmd = new SqlCommand(query, con))
               {
                  cmd.Parameters.AddWithValue("@Username", username);
                  cmd.Parameters.AddWithValue("@Email", email);
                  int count = (int)cmd.ExecuteScalar();
                  return count > 0;
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }
      }

      private string GenerateOTP()
      {
         Random rnd = new Random();
         return rnd.Next(100000, 999999).ToString();
      }

      private async Task SendEmail(string email, string otp)
      {
         try
         {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                   ClientId = ClientId,
                   ClientSecret = ClientSecret
                },
                new[] { "https://mail.google.com/" },
                "user",
                System.Threading.CancellationToken.None);

            var gmailService = new GmailService(new BaseClientService.Initializer()
            {
               HttpClientInitializer = credential
            });

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Dairouty Mobiles", "dairocr20@gmail.com"));
            message.To.Add(new MailboxAddress(UsernameTextBox.Text, email));
            message.Subject = "Your OTP Code";
            message.Body = new TextPart("plain")
            {
               Text = $"Your OTP code is {otp}"
            };

            await gmailService.Users.Messages.Send(new Google.Apis.Gmail.v1.Data.Message
            {
               Raw = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message.ToString()))
            }, "me").ExecuteAsync();

            MessageBox.Show($"OTP sent to {email}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
         catch (Exception ex)
         {
            MessageBox.Show($"Failed to send email: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Console.WriteLine($"Failed to send email: {ex}");
         }
      }
      private async Task<bool> ResetPassword(string username, string newPassword)
      {
         try
         {
            string connectionString = "Data Source=DAIRO\\SQLEXPRESS;Initial Catalog=Dairouty Mobiles;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
               await con.OpenAsync();
               string query = "UPDATE [Dairouty Mobiles Login] SET Password=@Password WHERE Username=@Username";
               using (SqlCommand cmd = new SqlCommand(query, con))
               {
                  cmd.Parameters.AddWithValue("@Username", username);
                  cmd.Parameters.AddWithValue("@Password", newPassword);
                  int rowsAffected = await cmd.ExecuteNonQueryAsync();
                  return rowsAffected > 0;
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }
      }


   }
}

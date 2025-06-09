using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Dairouty_Mobiles
{
   public partial class Login : Form
   {
      public Login()
      {
         InitializeComponent();
      }
      private void LoginB_Click(object sender, EventArgs e)
      {
         try
         {
            string connectionString = "Data Source=DAIRO\\SQLEXPRESS;Initial Catalog=\"Dairouty Mobiles\";Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
               con.Open();
               string query = "SELECT COUNT(*) FROM [Dairouty Mobiles Login] WHERE Username=@Username AND Password=@Password AND Role=@Role";
               using (SqlCommand cmd = new SqlCommand(query, con))
               {
                  cmd.Parameters.AddWithValue("@Username", tUser.Text);
                  cmd.Parameters.AddWithValue("@Password", tPass.Text);
                  cmd.Parameters.AddWithValue("@Role", RoleB.Text);
                  int count = (int)cmd.ExecuteScalar();

                  if (count > 0 && RoleB.Text == "Admin")
                  {
                     Admin_Home p = new Admin_Home();
                     p.Show();
                     Visible = false;
                  }
                  else if (count > 0 && RoleB.Text == "Seller")
                  {
                     Seller_Billing p = new Seller_Billing();
                     p.Show();
                     Visible = false;
                  }
                  else
                  {
                     MessageBox.Show("Incorrect Username or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
      private void ExitB_Click(object sender, EventArgs e)
      {
         Application.Exit();
      }
      private void ShowPass_CheckedChanged(object sender, EventArgs e)
      {
         tPass.UseSystemPasswordChar = !ShowPass.Checked;
      }

      private void Login_Load(object sender, EventArgs e)
      {
         MaximizeBox = false;
         MinimizeBox = false;
         ControlBox = false;
      }

      private void label3_Click(object sender, EventArgs e)
      {
          Reset_Pass reset_Pass = new Reset_Pass();
         reset_Pass.Show();
         Visible = false;  
      }
   }
}

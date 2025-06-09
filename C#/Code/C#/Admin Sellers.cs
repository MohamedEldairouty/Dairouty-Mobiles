using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Versioning;
using System.Net;

namespace Dairouty_Mobiles
{
   public partial class Admin_Sellers : Form
   {
      public Admin_Sellers()
      {
         InitializeComponent();
         dataGridView1.SelectionChanged += new EventHandler(SELECT);
      }

      private void Admin(object sender, EventArgs e)
      {
         MaximizeBox = false;
         MinimizeBox = false;
         ControlBox = false;
         displayusers();
      }

      private void Close_Click(object sender, EventArgs e)
      {
         Application.Exit();
      }

      private void FB_Click(object sender, EventArgs e)
      {
         try
         {
            Process.Start(new ProcessStartInfo
            {
               FileName = "https://www.facebook.com/DairoutyMobiles",
               UseShellExecute = true
            });
         }
         catch (Exception ex)
         {
            MessageBox.Show($"An error occurred: {ex.Message}");
         }
      }
      private void Location_Click(object sender, EventArgs e)
      {
         try
         {
            Process.Start(new ProcessStartInfo
            {
               FileName = "https://www.google.com/maps/place/Eldairouty+Mobiles/@31.2125634,29.9439588,17z/data=!3m1!4b1!4m6!3m5!1s0x14f5c4914b486f15:0x9e9cb04b17d6b716!8m2!3d31.2125588!4d29.9465337!16s%2Fg%2F11f54r5h7r?entry=ttu",
               UseShellExecute = true
            });
         }
         catch (Exception ex)
         {
            MessageBox.Show($"An error occurred: {ex.Message}");
         }
      }
      private void Insta_Click(object sender, EventArgs e)
      {
         try
         {
            Process.Start(new ProcessStartInfo
            {
               FileName = "https://www.instagram.com/dairoutymobiles/",
               UseShellExecute = true
            });
         }
         catch (Exception ex)
         {
            MessageBox.Show($"An error occurred: {ex.Message}");
         }
      }

      private void pictureBox3_Click(object sender, EventArgs e)
      {
         Login p = new Login();
         p.Show();
         Visible = false;
      }

      private void Home_Click(object sender, EventArgs e)
      {
         Admin_Home p = new Admin_Home();
         p.Show();
         Visible = false;
      }
      private void pictureBox5_Click(object sender, EventArgs e)
      {
         Admin_Home p = new Admin_Home();
         p.Show();
         Visible = false;
      }

      private void Products_Click(object sender, EventArgs e)
      {
         Dairouty_Mobiles_Products p = new Dairouty_Mobiles_Products();
         p.Show();
         Visible = false;
      }

      private void pictureBox2_Click(object sender, EventArgs e)
      {
         Dairouty_Mobiles_Products p = new Dairouty_Mobiles_Products();
         p.Show();
         Visible = false;
      }

      private void Customers_Click(object sender, EventArgs e)
      {
         Admin_Customers p = new Admin_Customers();
         p.Show();
         Visible = false;
      }

      private void pictureBox6_Click(object sender, EventArgs e)
      {
         Admin_Customers p = new Admin_Customers();
         p.Show();
         Visible = false;
      }

      private void Orders_Click(object sender, EventArgs e)
      {
         Admin_Orders p = new Admin_Orders();
         p.Show();
         Visible = false;
      }

      private void pictureBox7_Click(object sender, EventArgs e)
      {
         Admin_Orders p = new Admin_Orders();
         p.Show();
         Visible = false;
      }
      string connectionString = "Data Source=DAIRO\\SQLEXPRESS;Initial Catalog=\"Dairouty Mobiles\";Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
      private void ADD_Click(object sender, EventArgs e)
      {
         using (SqlConnection con = new SqlConnection(connectionString))
         {
            try
            {
               con.Open();
               int newId = 1;
               string fetchIdQuery = "SELECT MAX(ID) FROM [Dairouty Mobiles Login]";
               using (SqlCommand fetchIdCommand = new SqlCommand(fetchIdQuery, con))
               {
                  object result = fetchIdCommand.ExecuteScalar();
                  if (result != DBNull.Value)
                  {
                     newId = Convert.ToInt32(result) + 1;
                  }
               }
               string query = "INSERT INTO [Dairouty Mobiles Login] (ID, Name, Mobile, Email, Username, Password, Role, Salary) VALUES (@ID, @Name, @Mobile, @Email, @Username, @Password, @Role, @Salary)";

               using (SqlCommand command = new SqlCommand(query, con))
               {
                  command.Parameters.AddWithValue("@ID", newId);
                  command.Parameters.AddWithValue("@Name", name.Text);
                  command.Parameters.AddWithValue("@Mobile", Mobi.Text);
                  command.Parameters.AddWithValue("@Email", Email.Text);
                  command.Parameters.AddWithValue("@Username", Username.Text);
                  command.Parameters.AddWithValue("@Password", Pass.Text);
                  command.Parameters.AddWithValue("@Role", Role.Text);
                  if (string.IsNullOrWhiteSpace(Salary.Text) || !decimal.TryParse(Salary.Text, out decimal salary))
                  {
                     command.Parameters.AddWithValue("@Salary", DBNull.Value);
                  }
                  else
                  {
                     command.Parameters.AddWithValue("@Salary", salary);
                  }

                  command.ExecuteNonQuery();
               }
               MessageBox.Show("Successfully Added");
               displayusers();
            }
            catch (Exception ex)
            {
               MessageBox.Show("An error occurred: " + ex.Message);
            }
         }
      }

      void displayusers()
      {
         using (SqlConnection con = new SqlConnection(connectionString))
         {
            try
            {
               con.Open();
               SqlCommand command = new SqlCommand("SELECT ID, Name, Mobile, Email, Username, Password, Role, Salary FROM [Dairouty Mobiles Login]", con);
               SqlDataAdapter adapter = new SqlDataAdapter(command);
               DataTable dt = new DataTable();
               adapter.Fill(dt);
               dataGridView1.DataSource = dt;
               dataGridView1.Sort(dataGridView1.Columns["ID"], ListSortDirection.Ascending);
            }
            catch (Exception ex)
            {
               MessageBox.Show("An error occurred: " + ex.Message);
            }
         }
      }
      private void SELECT(object sender, EventArgs e)
      {
         if (dataGridView1.SelectedRows.Count > 0)
         {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            name.Text = selectedRow.Cells["Name"].Value.ToString();
            Mobi.Text = selectedRow.Cells["Mobile"].Value.ToString();
            Email.Text = selectedRow.Cells["Email"].Value.ToString();
            Username.Text = selectedRow.Cells["Username"].Value.ToString();
            Pass.Text = selectedRow.Cells["Password"].Value.ToString();
            Role.Text = selectedRow.Cells["Role"].Value.ToString();
            Salary.Text = selectedRow.Cells["Salary"].Value.ToString();
         }
      }

      private void EDIT_Click(object sender, EventArgs e)
      {
         if (dataGridView1.SelectedRows.Count > 0)
         {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            int Id = Convert.ToInt32(selectedRow.Cells["ID"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
               try
               {
                  con.Open();
                  string query = "UPDATE [Dairouty Mobiles Login] SET Name = @Name, Mobile = @Mobile, Email = @Email, Username = @Username, Password = @Password, Role = @Role, Salary = @Salary WHERE ID = @ID";

                  using (SqlCommand command = new SqlCommand(query, con))
                  {
                     command.Parameters.AddWithValue("@Name", string.IsNullOrEmpty(name.Text) ? DBNull.Value : (object)name.Text);
                     command.Parameters.AddWithValue("@Mobile", string.IsNullOrEmpty(Mobi.Text) ? DBNull.Value : (object)Mobi.Text);
                     command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(Email.Text) ? DBNull.Value : (object)Email.Text);
                     command.Parameters.AddWithValue("@Username", string.IsNullOrEmpty(Username.Text) ? DBNull.Value : (object)Username.Text);
                     command.Parameters.AddWithValue("@Password", string.IsNullOrEmpty(Pass.Text) ? DBNull.Value : (object)Pass.Text);
                     command.Parameters.AddWithValue("@Role", string.IsNullOrEmpty(Role.Text) ? DBNull.Value : (object)Role.Text);
                     if (decimal.TryParse(Salary.Text, out decimal salary))
                     {
                        command.Parameters.AddWithValue("@Salary", salary);
                     }
                     else
                     {
                        command.Parameters.AddWithValue("@Salary", DBNull.Value);
                     }
                     command.Parameters.AddWithValue("@ID", Id);
                     command.ExecuteNonQuery();
                  }

                  MessageBox.Show("Successfully Edited");
                  displayusers();
               }
               catch (Exception ex)
               {
                  MessageBox.Show("An error occurred: " + ex.Message);
               }
            }
         }
         else
         {
            MessageBox.Show("Please select a row to edit.");
         }
      }

      private void DELETE_Click(object sender, EventArgs e)
      {
         if (dataGridView1.SelectedRows.Count > 0)
         {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            int Id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
               try
               {
                  con.Open();
                  string query = "DELETE FROM [Dairouty Mobiles Login] WHERE ID = @ID";

                  using (SqlCommand command = new SqlCommand(query, con))
                  {
                     command.Parameters.AddWithValue("@ID", Id);
                     command.ExecuteNonQuery();
                  }

                  MessageBox.Show("Successfully Deleted");
                  displayusers();
               }
               catch (Exception ex)
               {
                  MessageBox.Show("An error occurred: " + ex.Message);
               }
            }
         }
         else
         {
            MessageBox.Show("Please select a row to delete.");
         }
      }

      private void SEARCH_Click(object sender, EventArgs e)
      {
         using (SqlConnection con = new SqlConnection(connectionString))
         {
            try
            {
               con.Open();
               StringBuilder query = new StringBuilder("SELECT ID, Name, Mobile, Email, Username, Password, Role, Salary FROM [Dairouty Mobiles Login] WHERE 1=1");

               if (!string.IsNullOrEmpty(name.Text))
               {
                  query.Append(" AND Name = @Name");
               }
               if (!string.IsNullOrEmpty(Mobi.Text))
               {
                  query.Append(" AND Mobile = @Mobile");
               }
               if (!string.IsNullOrEmpty(Email.Text))
               {
                  query.Append(" AND Email = @Email");
               }
               if (!string.IsNullOrEmpty(Username.Text))
               {
                  query.Append(" AND Username = @Username");
               }
               if (!string.IsNullOrEmpty(Pass.Text))
               {
                  query.Append(" AND Password = @Password");
               }
               if (!string.IsNullOrEmpty(Role.Text))
               {
                  query.Append(" AND Role = @Role");
               }
               if (!string.IsNullOrEmpty(Salary.Text) && decimal.TryParse(Salary.Text, out decimal maxSalary))
               {
                  query.Append(" AND Salary <= @MaxSalary");
               }

               using (SqlCommand command = new SqlCommand(query.ToString(), con))
               {
                  if (!string.IsNullOrEmpty(name.Text))
                  {
                     command.Parameters.AddWithValue("@Name", name.Text);
                  }
                  if (!string.IsNullOrEmpty(Mobi.Text))
                  {
                     command.Parameters.AddWithValue("@Mobile", Mobi.Text);
                  }
                  if (!string.IsNullOrEmpty(Email.Text))
                  {
                     command.Parameters.AddWithValue("@Email", Email.Text);
                  }
                  if (!string.IsNullOrEmpty(Username.Text))
                  {
                     command.Parameters.AddWithValue("@Username", Username.Text);
                  }
                  if (!string.IsNullOrEmpty(Pass.Text))
                  {
                     command.Parameters.AddWithValue("@Password", Pass.Text);
                  }
                  if (!string.IsNullOrEmpty(Role.Text))
                  {
                     command.Parameters.AddWithValue("@Role", Role.Text);
                  }
                  if (!string.IsNullOrEmpty(Salary.Text) && decimal.TryParse(Salary.Text, out decimal maxiSalary))
                  {
                     command.Parameters.AddWithValue("@MaxSalary", maxiSalary);
                  }

                  SqlDataAdapter adapter = new SqlDataAdapter(command);
                  DataTable dt = new DataTable();
                  adapter.Fill(dt);
                  dataGridView1.DataSource = dt;
               }
            }
            catch (Exception ex)
            {
               MessageBox.Show("An error occurred: " + ex.Message);
            }
         }
      }

      private void RESET_Click(object sender, EventArgs e)
      {
         name.Clear();
         Mobi.Clear();
         Email.Clear();
         Username.Clear();
         Pass.Clear();
         Salary.Clear();
         Role.SelectedIndex = -1;
         displayusers();
      }
   }
}

using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Dairouty_Mobiles
{
   public partial class Seller_Customers : Form
   {
      private string connectionString = "Data Source=DAIRO\\SQLEXPRESS;Initial Catalog=\"Dairouty Mobiles\";Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
      public Seller_Customers()
      {
         InitializeComponent();
         dataGridView1.SelectionChanged += new EventHandler(SELECT);
      }
      private void Admin(object sender, EventArgs e)
      {
         MaximizeBox = false;
         MinimizeBox = false;
         ControlBox = false;
         displaycustomers();
      }
      private void NavigateToForm(Form form)
      {
         form.Show();
         Visible = false;
      }
      private void ADD_Click(object sender, EventArgs e)
      {
         using (SqlConnection con = new SqlConnection(connectionString))
         {
            try
            {
               con.Open();
               int newId = GetNewCustomerId(con);
               string query = "INSERT INTO [Dairouty Customers] (Customer_ID, Customer_Name, Mobile, Address) VALUES (@Customer_ID, @Customer_Name, @Mobile, @Address)";
               using (SqlCommand command = new SqlCommand(query, con))
               {
                  command.Parameters.AddWithValue("@Customer_ID", newId);
                  command.Parameters.AddWithValue("@Customer_Name", CName.Text);
                  command.Parameters.AddWithValue("@Mobile", Mob.Text);
                  command.Parameters.AddWithValue("@Address", Address.Text);
                  command.ExecuteNonQuery();
               }
               MessageBox.Show("Successfully Added");
               displaycustomers();
            }
            catch (Exception ex)
            {
               MessageBox.Show("An error occurred: " + ex.Message);
            }
         }
      }
      private int GetNewCustomerId(SqlConnection con)
      {
         int newId = 1;
         string fetchIdQuery = "SELECT MAX(Customer_ID) FROM [Dairouty Customers]";
         using (SqlCommand fetchIdCommand = new SqlCommand(fetchIdQuery, con))
         {
            object result = fetchIdCommand.ExecuteScalar();
            if (result != DBNull.Value)
            {
               newId = Convert.ToInt32(result) + 1;
            }
         }
         return newId;
      }
      void displaycustomers()
      {
         using (SqlConnection con = new SqlConnection(connectionString))
         {
            try
            {
               con.Open();
               SqlCommand command = new SqlCommand("SELECT DISTINCT Customer_ID, Customer_Name, Mobile, Address FROM [Dairouty Customers]", con);
               SqlDataAdapter adapter = new SqlDataAdapter(command);
               DataTable dt = new DataTable();
               adapter.Fill(dt);
               dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
               MessageBox.Show("An error occurred: " + ex.Message);
            }
         }
      }
      private void EDIT_Click(object sender, EventArgs e)
      {
         if (dataGridView1.SelectedRows.Count > 0)
         {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            int Id = Convert.ToInt32(selectedRow.Cells["Customer_ID"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
               try
               {
                  con.Open();
                  string query = "UPDATE [Dairouty Customers] SET Customer_Name = @Customer_Name, Mobile = @Mobile, Address = @Address WHERE Customer_ID = @Customer_ID";
                  using (SqlCommand command = new SqlCommand(query, con))
                  {
                     command.Parameters.AddWithValue("@Customer_ID", Id);
                     command.Parameters.AddWithValue("@Customer_Name", string.IsNullOrEmpty(CName.Text) ? DBNull.Value : (object)CName.Text);
                     command.Parameters.AddWithValue("@Mobile", string.IsNullOrEmpty(Mob.Text) ? DBNull.Value : (object)Mob.Text);
                     command.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(Address.Text) ? DBNull.Value : (object)Address.Text);
                     command.ExecuteNonQuery();
                  }

                  MessageBox.Show("Successfully Edited");
                  displaycustomers();
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
            int Id = Convert.ToInt32(selectedRow.Cells["Customer_ID"].Value);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
               try
               {
                  con.Open();
                  string query = "DELETE FROM [Dairouty Customers] WHERE Customer_ID = @Customer_ID";

                  using (SqlCommand command = new SqlCommand(query, con))
                  {
                     command.Parameters.AddWithValue("@Customer_ID", Id);
                     command.ExecuteNonQuery();
                  }

                  MessageBox.Show("Successfully Deleted");
                  displaycustomers();
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
               StringBuilder query = new StringBuilder("SELECT DISTINCT Customer_ID, Customer_Name, Mobile, Address FROM [Dairouty Customers] WHERE 1=1");
               if (!string.IsNullOrEmpty(ID.Text))
               {
                  query.Append(" AND Customer_ID = @Customer_ID");
               }
               if (!string.IsNullOrEmpty(CName.Text))
               {
                  query.Append(" AND Customer_Name = @Customer_Name");
               }
               if (!string.IsNullOrEmpty(Mob.Text))
               {
                  query.Append(" AND Mobile = @Mobile");
               }
               if (!string.IsNullOrEmpty(Address.Text))
               {
                  query.Append(" AND Address = @Address");
               }
               using (SqlCommand command = new SqlCommand(query.ToString(), con))
               {
                  if (!string.IsNullOrEmpty(ID.Text))
                  {
                     command.Parameters.AddWithValue("@Customer_ID", ID.Text);
                  }
                  if (!string.IsNullOrEmpty(CName.Text))
                  {
                     command.Parameters.AddWithValue("@Customer_Name", CName.Text);
                  }
                  if (!string.IsNullOrEmpty(Mob.Text))
                  {
                     command.Parameters.AddWithValue("@Mobile", Mob.Text);
                  }

                  if (!string.IsNullOrEmpty(Address.Text))
                  {
                     command.Parameters.AddWithValue("@Address", Address.Text);
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
         ID.Clear();
         CName.Clear();
         Mob.Clear();
         Address.Clear();
         displaycustomers();
      }
      private void SELECT(object sender, EventArgs e)
      {
         if (dataGridView1.SelectedRows.Count > 0)
         {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            ID.Text = selectedRow.Cells["Customer_ID"].Value?.ToString() ?? string.Empty;
            CName.Text = selectedRow.Cells["Customer_Name"].Value?.ToString() ?? string.Empty;
            Mob.Text = selectedRow.Cells["Mobile"].Value?.ToString() ?? string.Empty;
            Address.Text = selectedRow.Cells["Address"].Value?.ToString() ?? string.Empty;
         }
      }
      private void Close_Click1(object sender, EventArgs e)
      {
         Application.Exit();
      }

      private void FB_Click1(object sender, EventArgs e)
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
      private void Location_Click1(object sender, EventArgs e)
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
      private void Insta_Click1(object sender, EventArgs e)
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
      private void Billing_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Billing());

      }

      private void pictureBox5_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Billing());

      }

      private void Products_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Products());
      }

      private void pictureBox2_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Products());
      }

      private void Orders_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Orders());
      }

      private void pictureBox7_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Orders());
      }

      private void pictureBox3_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Login());
      }

   }
}

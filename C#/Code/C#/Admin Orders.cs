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
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Intrinsics.Arm;

namespace Dairouty_Mobiles
{
   public partial class Admin_Orders : Form
   {
      private string connectionString = "Data Source=DAIRO\\SQLEXPRESS;Initial Catalog=\"Dairouty Mobiles\";Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
      public Admin_Orders()
      {
         InitializeComponent();
         dataGridView1.SelectionChanged += new EventHandler(SELECT);
         Customer_Names();
         displayorders();
      }
      private void Customer_Names()
      {
         using (SqlConnection con = new SqlConnection(connectionString))
         {
            try
            {
               con.Open();
               string query = "SELECT DISTINCT Customer_Name FROM [Dairouty Customers]";
               using (SqlCommand command = new SqlCommand(query, con))
               {
                  SqlDataReader reader = command.ExecuteReader();
                  while (reader.Read())
                  {
                     CName.Items.Add(reader["Customer_Name"].ToString());
                  }
               }
            }
            catch (Exception ex)
            {
               MessageBox.Show("An error occurred: " + ex.Message);
            }
         }
      }
      private void Admin(object sender, EventArgs e)
      {
         MaximizeBox = false;
         MinimizeBox = false;
         ControlBox = false;
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
      private void pictureBox3_Click(object sender, EventArgs e)
      {
         Login p = new Login();
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

      private void Sellers_Click(object sender, EventArgs e)
      {
         Admin_Sellers p = new Admin_Sellers();
         p.Show();
         Visible = false;
      }

      private void pictureBox8_Click(object sender, EventArgs e)
      {
         Admin_Sellers p = new Admin_Sellers();
         p.Show();
         Visible = false;
      }

      void displayorders()
      {
         using (SqlConnection con = new SqlConnection(connectionString))
         {
            try
            {
               con.Open();
               SqlCommand command = new SqlCommand("SELECT Invoice_Number,Invoice_Date, Customer_Name, Grand_Total FROM [Dairouty Customers]", con);
               SqlDataAdapter adapter = new SqlDataAdapter(command);
               DataTable dt = new DataTable();
               adapter.Fill(dt);
               dataGridView1.DataSource = dt;
               dataGridView1.Sort(dataGridView1.Columns["Invoice_Number"], ListSortDirection.Ascending);
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
            int n = Convert.ToInt32(selectedRow.Cells["Invoice_Number"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
               try
               {
                  con.Open();
                  string query = "UPDATE [Dairouty Customers] SET Invoice_Date = @Invoice_Date, Customer_Name = @Customer_Name, Grand_Total = @Grand_Total WHERE Invoice_Number = @Invoice_Number";
                  using (SqlCommand command = new SqlCommand(query, con))
                  {
                     command.Parameters.AddWithValue("@Invoice_Number", n);
                     command.Parameters.AddWithValue("@Customer_Name", string.IsNullOrEmpty(CName.Text) ? DBNull.Value : (object)CName.Text);
                     if (DateTime.TryParse(Date.Text, out DateTime invoiceDate))
                     {
                        command.Parameters.AddWithValue("@Invoice_Date", invoiceDate);
                     }
                     else
                     {
                        command.Parameters.AddWithValue("@Invoice_Date", DBNull.Value);
                     }
                     if (decimal.TryParse(Total.Text, out decimal gtotal))
                     {
                        command.Parameters.AddWithValue("@Grand_Total", gtotal);
                     }
                     else
                     {
                        command.Parameters.AddWithValue("@Grand_Total", DBNull.Value);
                     }
                     command.ExecuteNonQuery();
                  }

                  MessageBox.Show("Successfully Edited");
                  displayorders();
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
            int n = Convert.ToInt32(selectedRow.Cells["Invoice_Number"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
               try
               {
                  con.Open();

                  string prodsString = string.Empty;
                  string queryProds = "SELECT Prods FROM [Dairouty Customers] WHERE Invoice_Number = @Invoice_Number";
                  using (SqlCommand commandProds = new SqlCommand(queryProds, con))
                  {
                     commandProds.Parameters.AddWithValue("@Invoice_Number", n);
                     object prodsObject = commandProds.ExecuteScalar();
                     if (prodsObject != null)
                     {
                        prodsString = prodsObject.ToString();
                     }
                  }
                  string[] prodsArray = prodsString.Split(' ');
                  Dictionary<int, int> productQuantities = new Dictionary<int, int>();
                  foreach (string prodID in prodsArray)
                  {
                     int productId = Convert.ToInt32(prodID);
                     if (productQuantities.ContainsKey(productId))
                     {
                        productQuantities[productId]++;
                     }
                     else
                     {
                        productQuantities[productId] = 1;
                     }
                  }
                  string updateQuery = "UPDATE [Dairouty Products] SET Quantity = Quantity + @Quantity WHERE ProductID = @ProductID";
                  foreach (var entry in productQuantities)
                  {
                     using (SqlCommand updateCommand = new SqlCommand(updateQuery, con))
                     {
                        updateCommand.Parameters.AddWithValue("@ProductID", entry.Key);
                        updateCommand.Parameters.AddWithValue("@Quantity", entry.Value);
                        updateCommand.ExecuteNonQuery();
                     }
                  }

                  string deleteQuery = "DELETE FROM [Dairouty Customers] WHERE Invoice_Number = @Invoice_Number";
                  using (SqlCommand command = new SqlCommand(deleteQuery, con))
                  {
                     command.Parameters.AddWithValue("@Invoice_Number", n);
                     command.ExecuteNonQuery();
                  }

                  MessageBox.Show("Successfully Deleted");
                  displayorders();
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
               StringBuilder query = new StringBuilder("SELECT Invoice_Number, Invoice_Date, Customer_Name, Grand_Total FROM [Dairouty Customers] WHERE 1=1");
               if (!string.IsNullOrEmpty(inv.Text))
               {
                  query.Append(" AND Invoice_Number = @Invoice_Number");
               }
               if (!string.IsNullOrEmpty(CName.Text))
               {
                  query.Append(" AND Customer_Name = @Customer_Name");
               }
               if (!string.IsNullOrEmpty(Date.Text))
               {
                  query.Append(" AND Invoice_Date = @Invoice_Date");
               }
               if (!string.IsNullOrEmpty(Total.Text))
               {
                  query.Append(" AND Grand_Total = @Grand_Total");
               }
               using (SqlCommand command = new SqlCommand(query.ToString(), con))
               {
                  if (!string.IsNullOrEmpty(inv.Text))
                  {
                     command.Parameters.AddWithValue("@Invoice_Number", inv.Text);
                  }
                  if (!string.IsNullOrEmpty(CName.Text))
                  {
                     command.Parameters.AddWithValue("@Customer_Name", CName.Text);
                  }
                  if (!string.IsNullOrEmpty(Date.Text))
                  {
                     command.Parameters.AddWithValue("@Invoice_Date", Date.Text);
                  }

                  if (!string.IsNullOrEmpty(Total.Text))
                  {
                     command.Parameters.AddWithValue("@Grand_Total", Total.Text);
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
         inv.Clear();
         Date.Clear();
         CName.SelectedIndex = -1;
         displayorders();
      }

      private void SELECT(object sender, EventArgs e)
      {
         if (dataGridView1.SelectedRows.Count > 0)
         {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            inv.Text = selectedRow.Cells["Invoice_Number"].Value?.ToString() ?? string.Empty;
            CName.Text = selectedRow.Cells["Customer_Name"].Value?.ToString() ?? string.Empty;
            Date.Text = selectedRow.Cells["Invoice_Date"].Value?.ToString() ?? string.Empty;
            Total.Text = selectedRow.Cells["Grand_Total"].Value?.ToString() ?? string.Empty;
         }
      }
   }
}

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

namespace Dairouty_Mobiles
{
   public partial class Seller_Products : Form
   {
      public Seller_Products()
      {
         InitializeComponent();
         dataGridView1.SelectionChanged += new EventHandler(SELECT);

      }

      private void Dairouty_Mobiles_Products_Load(object sender, EventArgs e)
      {
         MaximizeBox = false;
         MinimizeBox = false;
         ControlBox = false;
         displayproducts();
      }
      string connectionString = "Data Source=DAIRO\\SQLEXPRESS;Initial Catalog=\"Dairouty Mobiles\";Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
      private void ADD_Click(object sender, EventArgs e)
      {
         using (SqlConnection con = new SqlConnection(connectionString))
         {
            try
            {
               con.Open();
               int newProductId = 1;
               string fetchIdQuery = "SELECT MAX(ProductID) FROM [Dairouty Products]";
               using (SqlCommand fetchIdCommand = new SqlCommand(fetchIdQuery, con))
               {
                  object result = fetchIdCommand.ExecuteScalar();
                  if (result != DBNull.Value)
                  {
                     newProductId = Convert.ToInt32(result) + 1;
                  }
               }

               string query = "INSERT INTO [Dairouty Products] (ProductID, Category, Model, Color, RAM, ROM, Price, Quantity) VALUES (@ProductID, @Category, @Model, @Color, @RAM, @ROM, @Price, @Quantity)";

               using (SqlCommand command = new SqlCommand(query, con))
               {
                  command.Parameters.AddWithValue("@ProductID", newProductId);
                  command.Parameters.AddWithValue("@Category", string.IsNullOrEmpty(Category.Text) ? DBNull.Value : (object)Category.Text);
                  command.Parameters.AddWithValue("@Model", string.IsNullOrEmpty(textBox1.Text) ? DBNull.Value : (object)textBox1.Text);
                  command.Parameters.AddWithValue("@Color", string.IsNullOrEmpty(comboBox3.Text) ? DBNull.Value : (object)comboBox3.Text);

                  if (int.TryParse(RAM.Text, out int ram))
                  {
                     command.Parameters.AddWithValue("@RAM", ram);
                  }
                  else
                  {
                     command.Parameters.AddWithValue("@RAM", DBNull.Value);
                  }

                  if (int.TryParse(ROM.Text, out int rom))
                  {
                     command.Parameters.AddWithValue("@ROM", rom);
                  }
                  else
                  {
                     command.Parameters.AddWithValue("@ROM", DBNull.Value);
                  }

                  if (decimal.TryParse(Price.Text, out decimal price))
                  {
                     command.Parameters.AddWithValue("@Price", price);
                  }
                  else
                  {
                     command.Parameters.AddWithValue("@Price", DBNull.Value);
                  }

                  if (int.TryParse(Quantity.Text, out int quantity))
                  {
                     command.Parameters.AddWithValue("@Quantity", quantity);
                  }
                  else
                  {
                     command.Parameters.AddWithValue("@Quantity", DBNull.Value);
                  }

                  command.ExecuteNonQuery();
               }
               MessageBox.Show("Successfully Added");
               displayproducts();
            }
            catch (Exception ex)
            {
               MessageBox.Show("An error occurred: " + ex.Message);
            }
         }
      }

      void displayproducts()
      {
         using (SqlConnection con = new SqlConnection(connectionString))
         {
            try
            {
               con.Open();
               SqlCommand command = new SqlCommand("SELECT ProductID, Category, Model, Color, RAM, ROM, Price, Quantity FROM [Dairouty Products]", con);
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
      private void SELECT(object sender, EventArgs e)
      {
         if (dataGridView1.SelectedRows.Count > 0)
         {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            Category.Text = selectedRow.Cells["Category"].Value.ToString();
            textBox1.Text = selectedRow.Cells["Model"].Value.ToString();
            comboBox3.Text = selectedRow.Cells["Color"].Value.ToString();
            RAM.Text = selectedRow.Cells["RAM"].Value.ToString();
            ROM.Text = selectedRow.Cells["ROM"].Value.ToString();
            Price.Text = selectedRow.Cells["Price"].Value.ToString();
            Quantity.Text = selectedRow.Cells["Quantity"].Value.ToString();
         }
      }
      private void EDIT_Click(object sender, EventArgs e)
      {
         if (dataGridView1.SelectedRows.Count > 0)
         {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
               try
               {
                  con.Open();
                  string query = "UPDATE [Dairouty Products] SET Category = @Category, Model = @Model, Color = @Color, RAM = @RAM, ROM = @ROM, Price = @Price, Quantity = @Quantity WHERE ProductID = @ProductID";

                  using (SqlCommand command = new SqlCommand(query, con))
                  {
                     command.Parameters.AddWithValue("@Category", string.IsNullOrEmpty(Category.Text) ? DBNull.Value : (object)Category.Text);
                     command.Parameters.AddWithValue("@Model", string.IsNullOrEmpty(textBox1.Text) ? DBNull.Value : (object)textBox1.Text);
                     command.Parameters.AddWithValue("@Color", string.IsNullOrEmpty(comboBox3.Text) ? DBNull.Value : (object)comboBox3.Text);
                     if (int.TryParse(RAM.Text, out int ram))
                     {
                        command.Parameters.AddWithValue("@RAM", ram);
                     }
                     else
                     {
                        command.Parameters.AddWithValue("@RAM", DBNull.Value);
                     }

                     if (int.TryParse(ROM.Text, out int rom))
                     {
                        command.Parameters.AddWithValue("@ROM", rom);
                     }
                     else
                     {
                        command.Parameters.AddWithValue("@ROM", DBNull.Value);
                     }

                     if (decimal.TryParse(Price.Text, out decimal price))
                     {
                        command.Parameters.AddWithValue("@Price", price);
                     }
                     else
                     {
                        command.Parameters.AddWithValue("@Price", DBNull.Value);
                     }

                     if (int.TryParse(Quantity.Text, out int quantity))
                     {
                        command.Parameters.AddWithValue("@Quantity", quantity);
                     }
                     else
                     {
                        command.Parameters.AddWithValue("@Quantity", DBNull.Value);
                     }
                     command.Parameters.AddWithValue("@ProductID", productId);
                     command.ExecuteNonQuery();
                  }

                  MessageBox.Show("Successfully Edited");
                  displayproducts();
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
            int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
               try
               {
                  con.Open();
                  string query = "DELETE FROM [Dairouty Products] WHERE ProductID = @ProductID";

                  using (SqlCommand command = new SqlCommand(query, con))
                  {
                     command.Parameters.AddWithValue("@ProductID", productId);
                     command.ExecuteNonQuery();
                  }

                  MessageBox.Show("Successfully Deleted");
                  displayproducts();
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
               StringBuilder query = new StringBuilder("SELECT ProductID, Category, Model, Color, RAM, ROM, Price, Quantity FROM [Dairouty Products] WHERE 1=1");

               if (!string.IsNullOrEmpty(Category.Text))
               {
                  query.Append(" AND Category = @Category");
               }
               if (!string.IsNullOrEmpty(textBox1.Text))
               {
                  query.Append(" AND Model = @Model");
               }
               if (!string.IsNullOrEmpty(comboBox3.Text))
               {
                  query.Append(" AND Color = @Color");
               }
               if (!string.IsNullOrEmpty(RAM.Text))
               {
                  query.Append(" AND RAM = @RAM");
               }
               if (!string.IsNullOrEmpty(ROM.Text))
               {
                  query.Append(" AND ROM = @ROM");
               }
               if (!string.IsNullOrEmpty(Price.Text) && int.TryParse(Price.Text, out int maxPrice))
               {
                  query.Append(" AND Price <= @MaxPrice");
               }

               using (SqlCommand command = new SqlCommand(query.ToString(), con))
               {
                  if (!string.IsNullOrEmpty(Category.Text))
                  {
                     command.Parameters.AddWithValue("@Category", Category.Text);
                  }
                  if (!string.IsNullOrEmpty(textBox1.Text))
                  {
                     command.Parameters.AddWithValue("@Model", textBox1.Text);
                  }
                  if (!string.IsNullOrEmpty(comboBox3.Text))
                  {
                     command.Parameters.AddWithValue("@Color", comboBox3.Text);
                  }
                  if (!string.IsNullOrEmpty(RAM.Text))
                  {
                     command.Parameters.AddWithValue("@RAM", int.Parse(RAM.Text));
                  }
                  if (!string.IsNullOrEmpty(ROM.Text))
                  {
                     command.Parameters.AddWithValue("@ROM", int.Parse(ROM.Text));
                  }
                  if (!string.IsNullOrEmpty(Price.Text) && int.TryParse(Price.Text, out int maxiPrice))
                  {
                     command.Parameters.AddWithValue("@MaxPrice", maxiPrice);
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
         Category.SelectedIndex = -1;
         textBox1.Clear();
         comboBox3.SelectedIndex = -1;
         RAM.SelectedIndex = -1;
         ROM.SelectedIndex = -1;
         Price.Clear();
         Quantity.Clear();
         displayproducts();
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

      private void pictureBox5_Click_1(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Billing());

      }
      private void Customers_Click_1(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Customers());

      }
      private void pictureBox6_Click_1(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Customers());
      }

      private void Orders_Click_1(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Orders());
      }

      private void pictureBox7_Click_1(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Orders());
      }
      private void NavigateToForm(Form form)
      {
         form.Show();
         Visible = false;
      }
      private void pictureBox3_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Login());
      }
   }
}
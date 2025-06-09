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

   public partial class Admin_Home : Form
   {
      public Admin_Home()
      {
         InitializeComponent();
         RefreshData();
      }

      private void Admin(object sender, EventArgs e)
      {
         MaximizeBox = false;
         MinimizeBox = false;
         ControlBox = false;
      }
      private string connectionString = "Data Source=DAIRO\\SQLEXPRESS;Initial Catalog=\"Dairouty Mobiles\";Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
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
      private int GetStockCount()
      {
         int stockcount = 0;

         string query = "SELECT SUM(quantity) FROM [Dairouty Products]";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
               try
               {
                  connection.Open();
                  object result = command.ExecuteScalar();
                  if (result != DBNull.Value)
                  {
                     stockcount = Convert.ToInt32(result);
                  }
               }
               catch (Exception ex)
               {
                  MessageBox.Show($"An error occurred: {ex.Message}");
               }
            }
         }
         return stockcount;
      }
      private int GetOrdersCount()
      {
         int ordersCount = 0;
         string today = DateTime.Now.ToString("yyyy-MM-dd");
         string query = "SELECT COUNT(*) FROM [Dairouty Customers] WHERE CONVERT(date, [Invoice_Date]) = @Today";
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
               try
               {
                  connection.Open();
                  command.Parameters.AddWithValue("@Today", today);
                  ordersCount = (int)command.ExecuteScalar();
               }
               catch (Exception ex)
               {
                  MessageBox.Show($"An error occurred: {ex.Message}");
               }
            }
         }
         return ordersCount;
      }

      private int GetCustomersCount()
      {
         int CuatomersCount = 0;

         string query = "SELECT COUNT(DISTINCT [Customer_Name]) FROM [Dairouty Customers]";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
               try
               {
                  connection.Open();
                  CuatomersCount = (int)command.ExecuteScalar();
               }
               catch (Exception ex)
               {
                  MessageBox.Show($"An error occurred: {ex.Message}");
               }
            }
         }
         return CuatomersCount;
      }
      private int GetSellersCount()
      {
         int SellersCount = 0;

         string query = "SELECT COUNT(*) FROM [Dairouty Mobiles Login] WHERE [Role] = 'Seller'";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
               try
               {
                  connection.Open();
                  SellersCount = (int)command.ExecuteScalar();
               }
               catch (Exception ex)
               {
                  MessageBox.Show($"An error occurred: {ex.Message}");
               }
            }
         }
         return SellersCount;
      }
      private decimal GetTotalSalary()
      {
         decimal totalSalary = 0;
         string query = "SELECT SUM([Salary]) FROM [Dairouty Mobiles Login] WHERE [Role] = 'Seller'";
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
               try
               {
                  connection.Open();
                  var result = command.ExecuteScalar();
                  if (result != DBNull.Value)
                  {
                     totalSalary = (decimal)result;
                  }
               }
               catch (Exception ex)
               {
                  MessageBox.Show($"An error occurred: {ex.Message}");
               }
            }
         }
         return totalSalary;
      }
      private decimal GetTodayMoney()
      {
         decimal totalmoney = 0;
         string today = DateTime.Now.ToString("yyyy-MM-dd");
         string query = "SELECT SUM([Grand_Total]) FROM [Dairouty Customers] WHERE CONVERT(date, [Invoice_Date]) = @Today";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
               try
               {
                  connection.Open();
                  command.Parameters.Add(new SqlParameter("@Today", SqlDbType.Date) { Value = today });

                  var result = command.ExecuteScalar();
                  if (result != DBNull.Value)
                  {
                     totalmoney = (decimal)result;
                  }
               }
               catch (Exception ex)
               {
                  MessageBox.Show($"An error occurred: {ex.Message}");
               }
            }
         }
         return totalmoney;
      }

      private void button1_Click(object sender, EventArgs e)
      {
         RefreshData();
      }
      private void RefreshData()
      {
         int stockCount = GetStockCount();
         ProdC.Text = $"{stockCount}";

         int customersCount = GetCustomersCount();
         CustomersC.Text = $"{customersCount}";

         int sellersCount = GetSellersCount();
         decimal totalSalary = GetTotalSalary();
         SellersC.Text = $"{sellersCount} ({totalSalary} EGP)";

         int todayOrdersCount = GetOrdersCount();
         decimal todayMoney = GetTodayMoney();
         OrdersC.Text = $"{todayOrdersCount} ({todayMoney} EGP)";
      }
   }
}

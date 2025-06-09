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
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Drawing.Printing;
using Microsoft.VisualBasic.Logging;
using static System.Windows.Forms.AxHost;

namespace Dairouty_Mobiles
{
   public partial class Seller_Billing : Form
   {
      private void Seller_Billing_Load(object sender, EventArgs e)
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
      private void Products_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Products());
      }

      private void pictureBox2_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Products());
      }

      private void Customers_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Customers());
      }

      private void pictureBox6_Click(object sender, EventArgs e)
      {
         NavigateToForm(new Seller_Customers());
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

      private void NavigateToForm(Form form)
      {
         form.Show();
         Visible = false;
      }
      string connectionString = "Data Source=DAIRO\\SQLEXPRESS;Initial Catalog=\"Dairouty Mobiles\";Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
      private DataTable invoiceTable;
      public Seller_Billing()
      {
         InitializeComponent();
         invoiceTable = new DataTable();
         invoiceTable.Columns.Add("ProductID", typeof(string));
         invoiceTable.Columns.Add("Product", typeof(string));
         invoiceTable.Columns.Add("Price", typeof(decimal));
         invoiceTable.Columns.Add("Quantity", typeof(int));
         dataGridView1.DataSource = invoiceTable;
         dataGridView1.Columns["Product"].Width = 300;
         ProductIdsList();
         Prods.SelectedIndexChanged += QuantityList;
      }
      private void ProductIdsList()
      {
         Prods.Items.Clear();

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT ProductID FROM [Dairouty Products] WHERE Quantity > 0", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
               Prods.Items.Add(reader["ProductID"].ToString());
            }
         }
      }
      private void QuantityList(object sender, EventArgs e)
      {
         if (Prods.SelectedItem != null)
         {
            string selectedProductId = Prods.SelectedItem.ToString();
            int availableQuantity = 0;
            decimal price = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
               connection.Open();
               SqlCommand command = new SqlCommand("SELECT Quantity, Price FROM [Dairouty Products] WHERE ProductID = @ProductId", connection);
               command.Parameters.AddWithValue("@ProductId", selectedProductId);
               SqlDataReader reader = command.ExecuteReader();
               if (reader.Read())
               {
                  availableQuantity = Convert.ToInt32(reader["Quantity"]);
                  price = Convert.ToDecimal(reader["Price"]);
               }
            }
            Quantity.Items.Clear();
            for (int i = 1; i <= availableQuantity; i++)
            {
               Quantity.Items.Add(i);
            }
            Price.Text = price.ToString();
         }
      }
      private void ADD_Click(object sender, EventArgs e)
      {
         string productId = Prods.SelectedItem.ToString();
         int quantity = Convert.ToInt32(Quantity.SelectedItem);

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT Category, Model, Color, RAM, ROM, Price FROM [Dairouty Products] WHERE ProductID = @ProductId AND Quantity > 0", connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
               string productDetails = $"{reader["Category"]} {reader["Model"]} {reader["Color"]} {reader["RAM"]}/{reader["ROM"]}";
               decimal price = Convert.ToDecimal(reader["Price"]);
               DataRow newRow = invoiceTable.NewRow();
               newRow["ProductID"] = productId;
               newRow["Product"] = productDetails;
               newRow["Price"] = price;
               newRow["Quantity"] = quantity;
               invoiceTable.Rows.Add(newRow);
               UpdateGrandTotal();
            }
            else
            {
               MessageBox.Show("Selected product is not available in stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         }
      }
      private int NewInvoiceNumber()
      {
         int nextInvoiceNumber = 1;
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT MAX(Invoice_Number) FROM [Dairouty Customers]", connection);
            var result = command.ExecuteScalar();
            if (result != DBNull.Value)
            {
               nextInvoiceNumber = Convert.ToInt32(result) + 1;
            }
         }
         return nextInvoiceNumber;
      }
      private int NewCustomerID(string customerName, string customerMobile, string customerAddress)
      {
         int customerID = 1;
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT Customer_ID FROM [Dairouty Customers] WHERE Customer_Name = @CustomerName AND Mobile = @CustomerMobile AND Address = @CustomerAddress", connection);
            command.Parameters.AddWithValue("@CustomerName", customerName);
            command.Parameters.AddWithValue("@CustomerMobile", customerMobile);
            command.Parameters.AddWithValue("@CustomerAddress", customerAddress);
            var result = command.ExecuteScalar();
            if (result != null)
            {
               customerID = Convert.ToInt32(result);
            }
            else
            {
               SqlCommand command2 = new SqlCommand("SELECT MAX(Customer_ID) FROM [Dairouty Customers]", connection);
               var result2 = command2.ExecuteScalar();
               if (result2 != DBNull.Value)
               {
                  customerID = Convert.ToInt32(result2) + 1;
               }
            }
         }
         return customerID;
      }
      private void SAVE_Click(object sender, EventArgs e)
      {
         string customerName = Cname.Text;
         string customerMobile = Cmob.Text;
         string customerAddress = Caddress.Text;
         decimal grandTotal = Convert.ToDecimal(GTotal.Text);
         int invnumber = NewInvoiceNumber();
         int id = NewCustomerID(customerName, customerMobile, customerAddress);
         StringBuilder productIDsBuilder = new StringBuilder();

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();
            SqlCommand command = new SqlCommand("INSERT INTO [Dairouty Customers] (Invoice_Number, Invoice_Date, Customer_Name, Customer_ID, Mobile, Address, Grand_Total, Prods) VALUES (@InvoiceNumber, @InvoiceDate, @CustomerName, @CustomerID, @CustomerMobile, @CustomerAddress, @GrandTotal, @Prods)", connection);
            command.Parameters.AddWithValue("@InvoiceDate", DateTime.Today);
            command.Parameters.AddWithValue("@InvoiceNumber", invnumber);
            command.Parameters.AddWithValue("@CustomerID", id);
            command.Parameters.AddWithValue("@CustomerName", customerName);
            command.Parameters.AddWithValue("@CustomerMobile", customerMobile);
            command.Parameters.AddWithValue("@CustomerAddress", customerAddress);
            command.Parameters.AddWithValue("@GrandTotal", grandTotal);

            foreach (DataRow row in invoiceTable.Rows)
            {
               string productID = row["ProductID"].ToString(); 
               int quantity = Convert.ToInt32(row["Quantity"]);
               for (int i = 0; i < quantity; i++)
               {
                  productIDsBuilder.Append(productID);
                  productIDsBuilder.Append(" ");
               }
               SqlCommand updateCommand = new SqlCommand("UPDATE [Dairouty Products] SET Quantity = Quantity - @Quantity WHERE ProductID = @ProductID", connection);
               updateCommand.Parameters.AddWithValue("@Quantity", quantity); 
               updateCommand.Parameters.AddWithValue("@ProductID", productID);
               updateCommand.ExecuteNonQuery();
            }
            string productIDs = productIDsBuilder.ToString().TrimEnd(' ');
            command.Parameters.AddWithValue("@Prods", productIDs);
            command.ExecuteNonQuery();
         }

         MessageBox.Show("Invoice saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
         ClearForm();
      }

      private void REMOVE_Click(object sender, EventArgs e)
      {
         if (dataGridView1.SelectedRows.Count > 0)
         {
            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            UpdateGrandTotal();
         }
      }
      private void UpdateGrandTotal()
      {
         decimal grandTotal = 0;
         foreach (DataRow row in invoiceTable.Rows)
         {
            decimal price = Convert.ToDecimal(row["Price"]);
            int quantity = Convert.ToInt32(row["Quantity"]);
            grandTotal += price * quantity;
         }
         GTotal.Text = grandTotal.ToString();
      }
      private void CLEAR_Click(object sender, EventArgs e)
      {
         ClearForm();
      }
      private void ClearForm()
      {
         Cname.Clear();
         Cmob.Clear();
         Caddress.Clear();
         Prods.SelectedIndex = -1;
         Quantity.SelectedIndex = -1;
         invoiceTable.Clear();
         GTotal.Clear();
      }
      private void PRINT_Click(object sender, EventArgs e)
      {
         PrintDocument pd = new PrintDocument();
         pd.PrintPage += new PrintPageEventHandler(PrintPage);
         PrintDialog printDlg = new PrintDialog();
         printDlg.Document = pd;
         if (printDlg.ShowDialog() == DialogResult.OK)
         {
            pd.Print();
         }
      }
      private void PrintPage(object sender, PrintPageEventArgs e)
      {
         Graphics g = e.Graphics;
         float pageWidth = e.PageBounds.Width;
         float pageHeight = e.PageBounds.Height;
         Image logo = Image.FromFile(@"D:\Prog\C#\Project\Dairouty Mobiles\Photos\Picture1.jpg");
         float logoWidth = pageWidth / 4;
         float logoHeight = (logoWidth / logo.Width) * logo.Height;
         float logoX = 10;
         float logoY = 10;
         g.DrawImage(logo, logoX, logoY, logoWidth, logoHeight);
         SizeF dairoutySize = g.MeasureString("Dairouty Mobiles", new Font("Jokerman", 24));
         float centerX_Dairouty = (pageWidth - dairoutySize.Width) / 2;
         SizeF invoiceSize = g.MeasureString("Invoice", new Font("Forte", 20, FontStyle.Italic));
         float centerX_Invoice = (pageWidth - invoiceSize.Width) / 2;
         float headerY = Math.Max(logoY + logoHeight + 10, dairoutySize.Height + 20);
         float invoiceY = headerY + dairoutySize.Height + 10;
         g.DrawString("Dairouty Mobiles", new Font("Jokerman", 24, FontStyle.Bold), new SolidBrush(Color.Navy), centerX_Dairouty, headerY);
         g.DrawString("Invoice", new Font("Forte", 24, FontStyle.Italic), new SolidBrush(Color.Navy), centerX_Invoice, invoiceY);
         float lineY = Math.Max(headerY + dairoutySize.Height + 10, invoiceY + invoiceSize.Height + 10) + 10;
         g.DrawLine(Pens.Black, 1, lineY, pageWidth, lineY);

         SizeF size = g.MeasureString("Customer Name :  ", new Font("Algerian", 16));
         g.DrawString("Customer Name :", new Font("Algerian", 14, FontStyle.Bold), new SolidBrush(Color.Red), 10, lineY += 50);
         g.DrawString(Cname.Text, new Font("Ariel", 16, FontStyle.Bold), new SolidBrush(Color.Black), size.Width, lineY);
         g.DrawString("Customer Mobile :", new Font("Algerian", 14, FontStyle.Bold), new SolidBrush(Color.Red), 10, lineY += 50);
         size = g.MeasureString("Customer Mobile :\t", new Font("Algerian", 14));
         g.DrawString(Cmob.Text, new Font("Ariel", 16, FontStyle.Bold), new SolidBrush(Color.Black), size.Width, lineY);
         g.DrawString("Customer Address :", new Font("Algerian", 14, FontStyle.Bold), new SolidBrush(Color.Red), 10, lineY += 50);
         size = g.MeasureString("Customer Address :\t", new Font("Algerian", 14));
         g.DrawString(Caddress.Text, new Font("Ariel", 16, FontStyle.Bold), new SolidBrush(Color.Black), size.Width, lineY);
         g.DrawLine(Pens.Black, 1, lineY + 70, pageWidth, lineY += 70);
         g.DrawString("Products :-\n", new Font("Algerian", 16, FontStyle.Bold), new SolidBrush(Color.Red), 10, lineY += 50);
         foreach (DataRow row in invoiceTable.Rows)
         {
            string product = row["Product"].ToString();
            string price = row["Price"].ToString() + "  EGP";
            string quantity = row["Quantity"].ToString();
            string line = price + "  x" + quantity;
            g.DrawString(product, new Font("Ariel", 16, FontStyle.Bold), new SolidBrush(Color.Black), 10, lineY += 50);
            g.DrawString(line, new Font("Ariel", 16, FontStyle.Bold), new SolidBrush(Color.Black), pageWidth - 300, lineY);
         }
         g.DrawLine(Pens.Black, 1, lineY + 70, pageWidth, lineY += 70);
         g.DrawString("Grand Total :\t", new Font("Algerian", 16, FontStyle.Bold), new SolidBrush(Color.Red), 10, lineY += 50);
         size = g.MeasureString("Grand Total :\t", new Font("Algerian", 16));
         g.DrawString(GTotal.Text + "  EGP", new Font("Ariel", 16, FontStyle.Bold), new SolidBrush(Color.Black), size.Width, lineY);
         g.DrawString("Seller Signiture :", new Font("Algerian", 16, FontStyle.Bold), new SolidBrush(Color.Navy), pageWidth - 300, pageHeight-70);
         g.DrawLine(Pens.Navy, pageWidth-330, pageHeight-15, pageWidth-30, pageHeight - 15);
      }
      private void Preview_Click(object sender, EventArgs e)
      {
         PrintDocument pd = new PrintDocument();
         pd.PrintPage += new PrintPageEventHandler(PrintPage);
         PrintPreviewDialog printPreviewDlg = new PrintPreviewDialog();
         printPreviewDlg.Document = pd;
         printPreviewDlg.ShowDialog();
      }
   }
}
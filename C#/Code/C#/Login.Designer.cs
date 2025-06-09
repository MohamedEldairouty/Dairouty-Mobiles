namespace Dairouty_Mobiles
{
   partial class Login
   {
      private System.ComponentModel.IContainer components = null;

      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
         pictureBox1 = new PictureBox();
         label1 = new Label();
         LoginB = new Button();
         ExitB = new Button();
         tUser = new TextBox();
         RoleB = new ComboBox();
         SelectRole = new Label();
         tPass = new TextBox();
         ShowPass = new CheckBox();
         label2 = new Label();
         label3 = new Label();
         ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
         SuspendLayout();
         // 
         // pictureBox1
         // 
         pictureBox1.BackgroundImageLayout = ImageLayout.Center;
         pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
         pictureBox1.Location = new Point(-303, 9);
         pictureBox1.Name = "pictureBox1";
         pictureBox1.Size = new Size(989, 545);
         pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
         pictureBox1.TabIndex = 0;
         pictureBox1.TabStop = false;
         // 
         // label1
         // 
         label1.AutoSize = true;
         label1.Font = new Font("Jokerman", 36F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
         label1.ForeColor = Color.Gold;
         label1.Location = new Point(193, 9);
         label1.Name = "label1";
         label1.Size = new Size(560, 88);
         label1.TabIndex = 1;
         label1.Text = "Dairouty Mobiles";
         label1.TextAlign = ContentAlignment.MiddleCenter;
         // 
         // LoginB
         // 
         LoginB.BackColor = Color.Green;
         LoginB.Font = new Font("Algerian", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
         LoginB.ForeColor = Color.AliceBlue;
         LoginB.Location = new Point(470, 420);
         LoginB.Name = "LoginB";
         LoginB.Size = new Size(205, 76);
         LoginB.TabIndex = 2;
         LoginB.Text = "Login";
         LoginB.UseVisualStyleBackColor = false;
         LoginB.Click += LoginB_Click;
         // 
         // ExitB
         // 
         ExitB.BackColor = Color.Red;
         ExitB.Font = new Font("Algerian", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
         ExitB.ForeColor = Color.AliceBlue;
         ExitB.Location = new Point(715, 420);
         ExitB.Name = "ExitB";
         ExitB.Size = new Size(205, 76);
         ExitB.TabIndex = 3;
         ExitB.Text = "Exit";
         ExitB.UseVisualStyleBackColor = false;
         ExitB.Click += ExitB_Click;
         // 
         // tUser
         // 
         tUser.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
         tUser.ForeColor = Color.DarkBlue;
         tUser.Location = new Point(564, 209);
         tUser.Name = "tUser";
         tUser.PlaceholderText = "Enter Username";
         tUser.Size = new Size(266, 46);
         tUser.TabIndex = 4;
         // 
         // RoleB
         // 
         RoleB.AccessibleName = "";
         RoleB.DropDownStyle = ComboBoxStyle.DropDownList;
         RoleB.Font = new Font("Segoe Script", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
         RoleB.ForeColor = Color.DarkBlue;
         RoleB.FormattingEnabled = true;
         RoleB.Items.AddRange(new object[] { "Admin", "Seller" });
         RoleB.Location = new Point(564, 133);
         RoleB.Name = "RoleB";
         RoleB.Size = new Size(266, 58);
         RoleB.TabIndex = 7;
         RoleB.Tag = "";
         // 
         // SelectRole
         // 
         SelectRole.AutoSize = true;
         SelectRole.Font = new Font("Viner Hand ITC", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
         SelectRole.ForeColor = Color.Blue;
         SelectRole.Location = new Point(330, 141);
         SelectRole.Name = "SelectRole";
         SelectRole.Size = new Size(200, 48);
         SelectRole.TabIndex = 8;
         SelectRole.Text = "Select Role :";
         // 
         // tPass
         // 
         tPass.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
         tPass.ForeColor = Color.DarkBlue;
         tPass.Location = new Point(564, 271);
         tPass.Name = "tPass";
         tPass.PlaceholderText = "Enter Password";
         tPass.Size = new Size(266, 46);
         tPass.TabIndex = 9;
         tPass.UseSystemPasswordChar = true;
         // 
         // ShowPass
         // 
         ShowPass.AutoSize = true;
         ShowPass.Font = new Font("Cooper Black", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
         ShowPass.ForeColor = Color.Gold;
         ShowPass.Location = new Point(692, 335);
         ShowPass.Name = "ShowPass";
         ShowPass.Size = new Size(223, 30);
         ShowPass.TabIndex = 10;
         ShowPass.Text = "Show Password";
         ShowPass.UseVisualStyleBackColor = true;
         ShowPass.CheckedChanged += ShowPass_CheckedChanged;
         // 
         // label2
         // 
         label2.AutoSize = true;
         label2.Font = new Font("Chiller", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
         label2.ForeColor = Color.White;
         label2.Location = new Point(12, 465);
         label2.Name = "label2";
         label2.Size = new Size(124, 34);
         label2.TabIndex = 11;
         label2.Text = "Version 1.0";
         // 
         // label3
         // 
         label3.AutoSize = true;
         label3.Font = new Font("Viner Hand ITC", 12F, FontStyle.Bold | FontStyle.Underline);
         label3.ForeColor = Color.Cyan;
         label3.Location = new Point(715, 368);
         label3.Name = "label3";
         label3.Size = new Size(192, 32);
         label3.TabIndex = 12;
         label3.Text = "Forget Password ?";
         label3.Click += label3_Click;
         // 
         // Login
         // 
         AutoScaleDimensions = new SizeF(20F, 43F);
         AutoScaleMode = AutoScaleMode.Font;
         BackColor = Color.Black;
         ClientSize = new Size(932, 508);
         ControlBox = false;
         Controls.Add(label3);
         Controls.Add(label2);
         Controls.Add(ShowPass);
         Controls.Add(tPass);
         Controls.Add(SelectRole);
         Controls.Add(RoleB);
         Controls.Add(tUser);
         Controls.Add(ExitB);
         Controls.Add(LoginB);
         Controls.Add(label1);
         Controls.Add(pictureBox1);
         Font = new Font("Jokerman", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
         ForeColor = Color.Blue;
         Icon = (Icon)resources.GetObject("$this.Icon");
         Margin = new Padding(8, 6, 8, 6);
         MaximizeBox = false;
         MinimizeBox = false;
         Name = "Login";
         Text = "Dairouty Mobiles ";
         Load += Login_Load;
         ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
         ResumeLayout(false);
         PerformLayout();
      }

      #endregion

      private PictureBox pictureBox1;
      private Label label1;
      private Button LoginB;
      private Button ExitB;
      public TextBox tUser;
      private ComboBox RoleB;
      private Label SelectRole;
      private TextBox tPass;
      private CheckBox ShowPass;
      private Label label2;
      private Label label3;
   }
}

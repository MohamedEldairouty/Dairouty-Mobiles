namespace Dairouty_Mobiles
{
   partial class Reset_Pass
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reset_Pass));
         pictureBox1 = new PictureBox();
         label1 = new Label();
         ResetPasswordButton = new Button();
         ExitB = new Button();
         UsernameTextBox = new TextBox();
         EmailTextBox = new TextBox();
         ShowPass = new CheckBox();
         label2 = new Label();
         OTPTextBox = new TextBox();
         SendOTPButton = new Button();
         VerifyOTPButton = new Button();
         NewPasswordTextBox = new TextBox();
         ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
         SuspendLayout();
         // 
         // pictureBox1
         // 
         pictureBox1.BackgroundImageLayout = ImageLayout.Center;
         pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
         pictureBox1.Location = new Point(-218, 25);
         pictureBox1.Name = "pictureBox1";
         pictureBox1.Size = new Size(776, 417);
         pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
         pictureBox1.TabIndex = 0;
         pictureBox1.TabStop = false;
         // 
         // label1
         // 
         label1.AutoSize = true;
         label1.Font = new Font("Jokerman", 36F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
         label1.ForeColor = Color.Gold;
         label1.Location = new Point(174, 9);
         label1.Name = "label1";
         label1.Size = new Size(560, 88);
         label1.TabIndex = 1;
         label1.Text = "Dairouty Mobiles";
         label1.TextAlign = ContentAlignment.MiddleCenter;
         // 
         // ResetPasswordButton
         // 
         ResetPasswordButton.BackColor = Color.Green;
         ResetPasswordButton.Font = new Font("Algerian", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
         ResetPasswordButton.ForeColor = Color.AliceBlue;
         ResetPasswordButton.Location = new Point(470, 420);
         ResetPasswordButton.Name = "ResetPasswordButton";
         ResetPasswordButton.Size = new Size(205, 76);
         ResetPasswordButton.TabIndex = 2;
         ResetPasswordButton.Text = "Reset";
         ResetPasswordButton.UseVisualStyleBackColor = false;
         ResetPasswordButton.Click += ResetPasswordButton_Click;
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
         // UsernameTextBox
         // 
         UsernameTextBox.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
         UsernameTextBox.ForeColor = Color.DarkBlue;
         UsernameTextBox.Location = new Point(303, 115);
         UsernameTextBox.Name = "UsernameTextBox";
         UsernameTextBox.PlaceholderText = "Enter Username";
         UsernameTextBox.Size = new Size(304, 46);
         UsernameTextBox.TabIndex = 4;
         UsernameTextBox.TextAlign = HorizontalAlignment.Center;
         // 
         // EmailTextBox
         // 
         EmailTextBox.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
         EmailTextBox.ForeColor = Color.DarkBlue;
         EmailTextBox.Location = new Point(303, 177);
         EmailTextBox.Name = "EmailTextBox";
         EmailTextBox.PlaceholderText = "Enter Email";
         EmailTextBox.Size = new Size(304, 46);
         EmailTextBox.TabIndex = 9;
         EmailTextBox.TextAlign = HorizontalAlignment.Center;
         // 
         // ShowPass
         // 
         ShowPass.AutoSize = true;
         ShowPass.Font = new Font("Cooper Black", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
         ShowPass.ForeColor = Color.Gold;
         ShowPass.Location = new Point(643, 308);
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
         // OTPTextBox
         // 
         OTPTextBox.Enabled = false;
         OTPTextBox.Font = new Font("Arial Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
         OTPTextBox.ForeColor = Color.DarkBlue;
         OTPTextBox.Location = new Point(303, 239);
         OTPTextBox.Name = "OTPTextBox";
         OTPTextBox.PlaceholderText = "Enter OTP";
         OTPTextBox.Size = new Size(304, 46);
         OTPTextBox.TabIndex = 12;
         OTPTextBox.TextAlign = HorizontalAlignment.Center;
         // 
         // SendOTPButton
         // 
         SendOTPButton.BackColor = Color.Cyan;
         SendOTPButton.Font = new Font("Lucida Calligraphy", 16F, FontStyle.Bold);
         SendOTPButton.ForeColor = Color.Black;
         SendOTPButton.Location = new Point(643, 177);
         SendOTPButton.Name = "SendOTPButton";
         SendOTPButton.Size = new Size(242, 46);
         SendOTPButton.TabIndex = 13;
         SendOTPButton.Text = "Send OTP";
         SendOTPButton.UseVisualStyleBackColor = false;
         SendOTPButton.Click += SendOTPButton_Click;
         // 
         // VerifyOTPButton
         // 
         VerifyOTPButton.BackColor = Color.Cyan;
         VerifyOTPButton.Font = new Font("Lucida Calligraphy", 16F, FontStyle.Bold);
         VerifyOTPButton.ForeColor = Color.Black;
         VerifyOTPButton.Location = new Point(643, 239);
         VerifyOTPButton.Name = "VerifyOTPButton";
         VerifyOTPButton.Size = new Size(242, 46);
         VerifyOTPButton.TabIndex = 14;
         VerifyOTPButton.Text = "Verify OTP";
         VerifyOTPButton.UseVisualStyleBackColor = false;
         VerifyOTPButton.Click += VerifyOTPButton_Click;
         // 
         // NewPasswordTextBox
         // 
         NewPasswordTextBox.Enabled = false;
         NewPasswordTextBox.Font = new Font("Arial Black", 15F, FontStyle.Bold);
         NewPasswordTextBox.ForeColor = Color.DarkBlue;
         NewPasswordTextBox.Location = new Point(303, 297);
         NewPasswordTextBox.Name = "NewPasswordTextBox";
         NewPasswordTextBox.PlaceholderText = "Enter New Password";
         NewPasswordTextBox.Size = new Size(304, 43);
         NewPasswordTextBox.TabIndex = 15;
         NewPasswordTextBox.TextAlign = HorizontalAlignment.Center;
         NewPasswordTextBox.UseSystemPasswordChar = true;
         // 
         // Reset_Pass
         // 
         AutoScaleDimensions = new SizeF(20F, 43F);
         AutoScaleMode = AutoScaleMode.Font;
         BackColor = Color.Black;
         ClientSize = new Size(932, 508);
         ControlBox = false;
         Controls.Add(NewPasswordTextBox);
         Controls.Add(VerifyOTPButton);
         Controls.Add(SendOTPButton);
         Controls.Add(OTPTextBox);
         Controls.Add(label2);
         Controls.Add(ShowPass);
         Controls.Add(EmailTextBox);
         Controls.Add(UsernameTextBox);
         Controls.Add(ExitB);
         Controls.Add(ResetPasswordButton);
         Controls.Add(label1);
         Controls.Add(pictureBox1);
         Font = new Font("Jokerman", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
         ForeColor = Color.Blue;
         Icon = (Icon)resources.GetObject("$this.Icon");
         Margin = new Padding(8, 6, 8, 6);
         MaximizeBox = false;
         MinimizeBox = false;
         Name = "Reset_Pass";
         Text = "Dairouty Mobiles ";
         Load += Login_Load;
         ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
         ResumeLayout(false);
         PerformLayout();
      }

      #endregion

      private PictureBox pictureBox1;
      private Label label1;
      private Button ResetPasswordButton;
      private Button ExitB;
      public TextBox UsernameTextBox;
      private TextBox EmailTextBox;
      private CheckBox ShowPass;
      private Label label2;
      private TextBox OTPTextBox;
      private Button SendOTPButton;
      private Button VerifyOTPButton;
      private TextBox NewPasswordTextBox;
   }
}

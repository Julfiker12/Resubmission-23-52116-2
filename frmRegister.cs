using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LoginAndRegister
{
    public class frmRegister : Form
    {
        private static string myConn = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        private TextBox txtUsername, txtPassword, txtConPassword;
        private Button btnRegister;

        public frmRegister()
        {
            Text="Register"; StartPosition=FormStartPosition.CenterScreen; ClientSize=new Size(450,300);
            FormBorderStyle=FormBorderStyle.FixedSingle; MaximizeBox=false;

            Controls.Add(new Label{Text="Create Account",Font=new Font("Segoe UI",18,FontStyle.Bold),AutoSize=true,Location=new Point(135,25)});
            Controls.Add(new Label{Text="Username",AutoSize=true,Location=new Point(55,85)});
            Controls.Add(new Label{Text="Password",AutoSize=true,Location=new Point(55,130)});
            Controls.Add(new Label{Text="Confirm Password",AutoSize=true,Location=new Point(55,175)});

            txtUsername=new TextBox{Name="txtUsername",Location=new Point(190,80),Width=200};
            txtPassword=new TextBox{Name="txtPassword",Location=new Point(190,125),Width=200,UseSystemPasswordChar=true};
            txtConPassword=new TextBox{Name="txtConPassword",Location=new Point(190,170),Width=200,UseSystemPasswordChar=true};
            btnRegister=new Button{Name="btnRegister",Text="Register",Location=new Point(190,220),Width=100,Height=32};
            Controls.AddRange(new Control[]{txtUsername,txtPassword,txtConPassword,btnRegister});
            btnRegister.Click+=btnRegister_Click;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if(txtUsername.Text.Trim()=="" || txtPassword.Text=="" || txtConPassword.Text=="")
            { MessageBox.Show("Username and password fields cannot be empty.","Register Failed",MessageBoxButtons.OK,MessageBoxIcon.Error); return; }

            if(txtPassword.Text!=txtConPassword.Text)
            {
                MessageBox.Show("Passwords do not match, please re-enter.","Register Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtPassword.Text=""; txtConPassword.Text=""; txtPassword.Focus(); return;
            }

            try
            {
                using(SqlConnection con=new SqlConnection(myConn))
                {
                    con.Open();
                    using(SqlCommand check=new SqlCommand("SELECT COUNT(*) FROM tbl_users WHERE username = @username",con))
                    {
                        check.Parameters.AddWithValue("@username",txtUsername.Text.Trim());
                        if(Convert.ToInt32(check.ExecuteScalar())>0)
                        { MessageBox.Show("That username is already taken."); txtUsername.Focus(); return; }
                    }

                    string register="INSERT INTO tbl_users (username, password) VALUES (@username, @password)";
                    using(SqlCommand cmd=new SqlCommand(register,con))
                    {
                        cmd.Parameters.AddWithValue("@username",txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@password",txtPassword.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Your account has been successfully created.","Registration Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                txtUsername.Text=""; txtPassword.Text=""; txtConPassword.Text=""; txtUsername.Focus();
            }
            catch(Exception ex)
            { MessageBox.Show("Database error:\n\n"+ex.Message,"Database Error",MessageBoxButtons.OK,MessageBoxIcon.Error); }
        }
    }
}
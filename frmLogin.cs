using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LoginAndRegister
{
    public class frmLogin : Form
    {
        private static string myConn =
            ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        private TextBox txtUsername;
        private TextBox txtPassword;

        private Button btnLogin;
        private Button btnRegister;
        private Button btnClear;
        private Button btnClose;

        private CheckBox chkShowPassword;

        public frmLogin()
        {
            Text = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(430, 330);

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            // Title
            Controls.Add(new Label
            {
                Text = "Login",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(175, 25)
            });

            // Username label
            Controls.Add(new Label
            {
                Text = "Username",
                AutoSize = true,
                Location = new Point(55, 90)
            });

            // Password label
            Controls.Add(new Label
            {
                Text = "Password",
                AutoSize = true,
                Location = new Point(55, 135)
            });

            // Username textbox
            txtUsername = new TextBox
            {
                Name = "txtUsername",
                Location = new Point(155, 85),
                Width = 210
            };

            // Password textbox
            txtPassword = new TextBox
            {
                Name = "txtPassword",
                Location = new Point(155, 130),
                Width = 210,
                UseSystemPasswordChar = true
            };

            // Show password checkbox
            chkShowPassword = new CheckBox
            {
                Name = "chkShowPassword",
                Text = "Show password",
                AutoSize = true,
                Location = new Point(155, 160)
            };

            // Login button
            btnLogin = new Button
            {
                Name = "btnLogin",
                Text = "Login",
                Location = new Point(55, 205),
                Width = 80,
                Height = 32
            };

            // Register button
            btnRegister = new Button
            {
                Name = "btnRegister",
                Text = "Register",
                Location = new Point(145, 205),
                Width = 80,
                Height = 32
            };

            // Clear button
            btnClear = new Button
            {
                Name = "btnClear",
                Text = "Clear",
                Location = new Point(235, 205),
                Width = 80,
                Height = 32
            };

            // Close button
            btnClose = new Button
            {
                Name = "btnClose",
                Text = "Close",
                Location = new Point(325, 205),
                Width = 80,
                Height = 32
            };

            Controls.AddRange(new Control[]
            {
                txtUsername,
                txtPassword,
                chkShowPassword,
                btnLogin,
                btnRegister,
                btnClear,
                btnClose
            });

            // Events
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;
            btnLogin.Click += btnLogin_Click;
            btnRegister.Click += btnRegister_Click;
            btnClear.Click += btnClear_Click;
            btnClose.Click += btnClose_Click;

            AcceptButton = btnLogin;
            CancelButton = btnClose;
        }

        // Show / Hide password
        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        // Login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "" || txtPassword.Text == "")
            {
                MessageBox.Show(
                    "Please enter both username and password.",
                    "Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(myConn))
                {
                    con.Open();

                    string login =
                        "SELECT COUNT(*) FROM tbl_users " +
                        "WHERE username = @username AND password = @password";

                    using (SqlCommand cmd = new SqlCommand(login, con))
                    {
                        cmd.Parameters.AddWithValue(
                            "@username",
                            txtUsername.Text.Trim()
                        );

                        cmd.Parameters.AddWithValue(
                            "@password",
                            txtPassword.Text
                        );

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 1)
                        {
                            frmDashboard dashboard = new frmDashboard();

                            // When dashboard closes, return to login
                            dashboard.FormClosed += Dashboard_FormClosed;

                            dashboard.Show();

                            Hide();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Wrong username or password, please try again.",
                                "Login Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );

                            ClearLogin();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database error:\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Dashboard closed -> return to Login
        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClearLogin();
            Show();
        }

        // Open Registration form
        private void btnRegister_Click(object sender, EventArgs e)
        {
            using (frmRegister register = new frmRegister())
            {
                Hide();

                register.ShowDialog(this);

                Show();

                txtUsername.Focus();
            }
        }

        // Clear button
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearLogin();
        }

        // Close application
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Clear login fields
        private void ClearLogin()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            chkShowPassword.Checked = false;

            txtUsername.Focus();
        }
    }
}
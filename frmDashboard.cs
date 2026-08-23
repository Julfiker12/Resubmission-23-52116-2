using System;
using System.Drawing;
using System.Windows.Forms;

namespace LoginAndRegister
{
    public class frmDashboard : Form
    {
        private Button btnLogout;
        public frmDashboard()
        {
            Text="Dashboard"; StartPosition=FormStartPosition.CenterScreen; ClientSize=new Size(500,300);
            FormBorderStyle=FormBorderStyle.FixedSingle; MaximizeBox=false;
            Controls.Add(new Label{Text="Welcome to the Dashboard",Font=new Font("Segoe UI",18,FontStyle.Bold),AutoSize=true,Location=new Point(110,65)});
            Controls.Add(new Label{Text="You have successfully logged in.",AutoSize=true,Location=new Point(155,120)});
            btnLogout=new Button{Name="btnLogout",Text="Logout",Location=new Point(195,175),Width=110,Height=35};
            Controls.Add(btnLogout); btnLogout.Click+=btnLogout_Click;
        }
        private void btnLogout_Click(object sender,EventArgs e)
        {
            DialogResult result=MessageBox.Show("Are you sure you want to logout?","Logout",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(result==DialogResult.Yes) Close();
        }
    }
}
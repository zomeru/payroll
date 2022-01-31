using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Payroll.User
{
    public partial class frmChangePassword : Form
    {
        public frmChangePassword()
        {
            InitializeComponent();
        }

        private void ChangeFocus(TextBox firstTxt, TextBox secondtext, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (firstTxt.Text.Length > 0)
                {
                    secondtext.Focus();
                }
                else
                {
                    firstTxt.Focus();
                }
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            ChangeFocus(txtUsername, txtOldPass, e);
        }

        private void txtOldPass_KeyDown(object sender, KeyEventArgs e)
        {
            ChangeFocus(txtOldPass, txtNewPass, e);
        }

        private void txtNewPass_KeyDown(object sender, KeyEventArgs e)
        {
            ChangeFocus(txtNewPass, txtConfirmNewPass, e);
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtUsername;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to change your password?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                DbConnection con = new DbConnection();
                con.dataGet("SELECT 1 FROM [Users] WHERE Username = '" + txtUsername.Text + "' AND Password = '"+txtOldPass.Text+"'");
                DataTable dt = new DataTable();
                con.sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (txtNewPass.Text == txtConfirmNewPass.Text)
                    {
                        if (txtNewPass.Text.Length > 3)
                        {
                            con.dataSend("UPDATE [Users] SET Password = '" + txtNewPass.Text + "' WHERE Username = '" + txtUsername.Text + "' AND Password = '"+txtOldPass.Text+"'");
                            MessageBox.Show("Password Changed Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            errorProvider1.SetError(txtNewPass, "Please enter minimum of 4 characters!");
                            errorProvider1.SetError(txtConfirmNewPass, "Please enter minimum of 4 characters!");
                        }
                    }
                    else
                    {
                        errorProvider1.SetError(txtNewPass, "New Password do not match!");
                        errorProvider1.SetError(txtConfirmNewPass, "New Password do not match!");
                    }
                }
                else
                {
                    errorProvider1.SetError(txtUsername, "Please check Username and Password!");
                    errorProvider1.SetError(txtOldPass, "Please check Username and Password!");
                }
            }
        }
    }
}

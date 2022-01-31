using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Payroll.User
{
    public partial class frmUserRegister : Form
    {
        public frmUserRegister()
        {
            InitializeComponent();
        }

        DbConnection con = new DbConnection();

        private void changeFocus(TextBox txtFirst, TextBox txtSecond, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (txtFirst.Text.Length > 0)
                {
                    txtSecond.Focus();
                }
                else
                {
                    txtFirst.Focus();
                }
            }
        }

        private void changeFocus(TextBox txtFirst, DateTimePicker txtSecond, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (txtFirst.Text.Length > 0)
                {
                    txtSecond.Focus();
                }
                else
                {
                    txtFirst.Focus();
                }
            }
        }

        private void changeFocus(DateTimePicker txtFirst, ComboBox txtSecond, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (txtFirst.Text.Length > 0)
                {
                    txtSecond.Focus();
                }
                else
                {
                    txtFirst.Focus();
                }
            }
        }

        private void changeFocus(ComboBox txtFirst, RichTextBox txtSecond, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (txtFirst.Text.Length > 0)
                {
                    txtSecond.Focus();
                }
                else
                {
                    txtFirst.Focus();
                }
            }
        }

        private void frmUserRegister_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtName;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            LoadData();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            changeFocus(txtName, txtUsername, e);
        }
        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            changeFocus(txtUsername, txtPassword, e);
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            changeFocus(txtPassword, txtEmail, e);
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            changeFocus(txtEmail, dtpBirthday, e);
        }

        private void dtpBirthday_KeyDown(object sender, KeyEventArgs e)
        {
            changeFocus(dtpBirthday, cbRole, e);
        }

        private void cbRole_KeyDown(object sender, KeyEventArgs e)
        {
            changeFocus(cbRole, richTxtAddress, e);
        }

        private void ClearData ()
        {
            txtName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            richTxtAddress.Clear();
            cbRole.SelectedIndex = -1;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            dtpBirthday.Value = DateTime.Now;
        }

        private bool Validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtName, "Name is required!");
            }
            else if (string.IsNullOrEmpty(txtUsername.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtUsername, "Username is required!");
            }
            else if (string.IsNullOrEmpty(txtPassword.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtPassword, "Password is required!");
            }
            else if (txtPassword.Text.Length < 4)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtPassword, "Minimum of 4 characters for password is required!");
            }
            else if (string.IsNullOrEmpty(txtEmail.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtEmail, "Email is required!");
            }
            else if (cbRole.SelectedIndex == -1)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(cbRole, "Role is required!");
            }
            else
            {
                errorProvider1.Clear();
                result = true;
            }

            return result;
        }

        private bool IfUserExists(string username, string email)
        {
            string query = "SELECT 1 FROM Users WHERE Username = '" + username + "' OR Email = '" + email + "'";
            
            con.dataGet(query);
            DataTable dt = new DataTable();
            con.sda.Fill(dt);

            return dt.Rows.Count > 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                if (IfUserExists(txtUsername.Text, txtEmail.Text))
                {
                    MessageBox.Show("Username or Email already exists!");
                }
                else
                {
                    string query = "INSERT INTO [Users] VALUES ('" + txtName.Text + "','" + txtEmail.Text + "','" + txtUsername.Text + "','" + txtPassword.Text + "','" + cbRole.Text + "','" + dtpBirthday.Value.ToString("MM/dd/yyyy") + "','" + richTxtAddress.Text + "')";
                    con.dataSend(query);
                    MessageBox.Show("Record saved successfully");
                    ClearData();
                    LoadData();
                }
            }
        }

        private void LoadData()
        {
            string query = "SELECT * FROM [Users]";
            con.dataGet(query);
            DataTable dt = new DataTable();
            con.sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach(DataRow row in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells["dgSerialNumber"].Value = n + 1;
                dataGridView1.Rows[n].Cells["dgName"].Value = row["Name"].ToString();
                dataGridView1.Rows[n].Cells["dgBirthday"].Value = Convert.ToDateTime(row["Birthdate"].ToString()).ToString("dd/MM/yyyy");
                dataGridView1.Rows[n].Cells["dgEmail"].Value = row["Email"].ToString();
                dataGridView1.Rows[n].Cells["dgUsername"].Value = row["Username"].ToString();
                dataGridView1.Rows[n].Cells["dgRole"].Value = row["Role"].ToString();
                dataGridView1.Rows[n].Cells["dgAddress"].Value = row["Address"].ToString();
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtName.Text = dataGridView1.SelectedRows[0].Cells["dgName"].Value.ToString();
            txtUsername.Text = dataGridView1.SelectedRows[0].Cells["dgUsername"].Value.ToString();
            txtEmail.Text = dataGridView1.SelectedRows[0].Cells["dgEmail"].Value.ToString();
            richTxtAddress.Text = dataGridView1.SelectedRows[0].Cells["dgAddress"].Value.ToString();
            dtpBirthday.Text = dataGridView1.SelectedRows[0].Cells["dgBirthday"].Value.ToString();
            cbRole.Text = dataGridView1.SelectedRows[0].Cells["dgRole"].Value.ToString();
            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to update this record?", "Update", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                string query = "UPDATE Users SET Name ='" + txtName.Text + "', Email ='" + txtEmail.Text + "', Role ='" + cbRole.Text + "', Birthdate ='" + dtpBirthday.Value.ToString("MM/dd/yyyy") + "', Address = '"+richTxtAddress.Text+"' WHERE Username = '"+txtUsername.Text+"'";
                con.dataSend(query);
                MessageBox.Show("Updated Successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearData();
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Delete", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                con.dataSend("DELETE FROM [Users] WHERE Username = '" + txtUsername.Text + "'");
                MessageBox.Show("Deleted Successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearData();
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = true;
            }
        }
    }
}

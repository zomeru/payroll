using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Payroll.Employee
{
    public partial class frmEmployeeRegister : Form
    {
        string fileName;

        public frmEmployeeRegister()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                    lblFileName.Text = fileName;
                    picBox.Image = Image.FromFile(fileName);
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            picBox.Image = null;
            lblFileName.Text = "File Name:";
        }

        private void frmEmployeeRegister_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtName;
            LoadData();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false; 
        }

        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private bool Validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtName, "Name is required!");
            }
            else if (string.IsNullOrEmpty(txtMobile.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtMobile, "Mobile is required!");
            }
            else if (string.IsNullOrEmpty(txtEmail.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtEmail, "Email is required!");
            }
            else if (string.IsNullOrEmpty(richTxtAddress.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(richTxtAddress, "Address is required!");
            }
            else
            {
                errorProvider1.Clear();
                result = true;
            }

            return result;
        }


        DbConnection con = new DbConnection();
        private bool IfEmployeeExists(string name, string mobile)
        {
            string query = "SELECT 1 FROM Employee WHERE Name = '" + name + "' OR Mobile = '" + mobile + "'";
            con.dataGet(query);
            DataTable dt = new DataTable();
            con.sda.Fill(dt);

            return dt.Rows.Count > 0;
        }

        byte[] ConvertImageToBinary(Image img)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                if (IfEmployeeExists(txtName.Text, txtMobile.Text))
                {
                    MessageBox.Show("Employee already exists!");
                }
                else
                {
                    string query = @"INSERT INTO Employee (EmployeeId, Name, Mobile, Email, Birthday, BankDetails, Address, FileName, ImageData) 
VALUES ('"+Convert.ToInt32(txtEmployeeId.Text)+"', '" + txtName.Text + "', '"+txtMobile.Text+"', '"+txtEmail.Text+"', '"+dtpBirthday.Value.ToString("MM/dd/yyy")+"', '"+richTxtBank.Text+"', '"+richTxtAddress.Text+"', '"+fileName+"', '"+ ConvertImageToBinary(picBox.Image)+ "')";
                    con.dataSend(query);
                    MessageBox.Show("Successfully Saved!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearData();
                    LoadData();
                }
            }
        }

        private void ClearData()
        {
            txtEmail.Clear();
            txtName.Clear();
            txtEmployeeId.Clear();
            txtMobile.Clear();
            dtpBirthday.Value = DateTime.Now;
            richTxtAddress.Clear();
            richTxtBank.Clear();
            picBox.Image = null;
            lblFileName.Text = "File Name:";
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void LoadData()
        {
            con.dataGet("SELECT * FROM Employee");
            DataTable dt = new DataTable();
            con.sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach(DataRow row in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells["dgEmpId"].Value = row["EmployeeId"].ToString();
                dataGridView1.Rows[n].Cells["dgName"].Value = row["Name"].ToString();
                dataGridView1.Rows[n].Cells["dgBirthday"].Value = Convert.ToDateTime(row["Birthday"].ToString()).ToString("dd/MM/yyyy");
                dataGridView1.Rows[n].Cells["dgEmail"].Value = row["Email"].ToString();
                dataGridView1.Rows[n].Cells["dgMobile"].Value = row["Mobile"].ToString();
                dataGridView1.Rows[n].Cells["dgBank"].Value = row["BankDetails"].ToString();
                dataGridView1.Rows[n].Cells["dgAddress"].Value = row["EmployeeId"].ToString();
                dataGridView1.Rows[n].Cells["dgFilename"].Value = row["FileName"].ToString();
                dataGridView1.Rows[n].Cells["dgImage"].Value = row["ImageData"].ToString();
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtEmployeeId.Text = dataGridView1.SelectedRows[0].Cells["dgEmpId"].Value.ToString();
            txtName.Text = dataGridView1.SelectedRows[0].Cells["dgName"].Value.ToString();
            txtMobile.Text = dataGridView1.SelectedRows[0].Cells["dgMobile"].Value.ToString();
            txtEmail.Text = dataGridView1.SelectedRows[0].Cells["dgEmail"].Value.ToString();
            dtpBirthday.Text = dataGridView1.SelectedRows[0].Cells["dgBirthday"].Value.ToString();
            richTxtBank.Text = dataGridView1.SelectedRows[0].Cells["dgBank"].Value.ToString();
            richTxtAddress.Text = dataGridView1.SelectedRows[0].Cells["dgAddress"].Value.ToString();
            lblFileName.Text = dataGridView1.SelectedRows[0].Cells["dgFilename"].Value.ToString();
            picBox.Image = Image.FromFile(dataGridView1.SelectedRows[0].Cells["dgFilename"].Value.ToString());
            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to update this record?", "Update", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string query = "UPDATE Employee SET Email = '"+txtEmail.Text+"', BankDetails = '"+richTxtBank.Text+"', Address = '"+richTxtAddress.Text+"', FileName = '"+fileName+"', ImageData = '"+ConvertImageToBinary(picBox.Image)+"' WHERE EmployeeId = '"+txtEmployeeId.Text+"'";
                con.dataSend(query);
                MessageBox.Show("Updated Successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string query = "DELETE FROM Employee WHERE EmployeeId = '" + txtEmployeeId.Text + "'";
                con.dataSend(query);
                MessageBox.Show("Deleted Successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }
    }
}

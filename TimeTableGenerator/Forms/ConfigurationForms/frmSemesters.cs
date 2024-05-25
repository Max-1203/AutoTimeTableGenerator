using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTableGenerator.Forms.ConfigurationForms
{
    public partial class frmSemesters : Form
    {
        public frmSemesters()
        {
            InitializeComponent();
        }

        public void FillGrid(string searchvalue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchvalue.Trim()))
                {
                    query = "select SemesterID [ID], SemesterName [Semester], IsActive [Status] from SemesterTable";
                }
                else
                {
                    query = "select SemesterID [ID], SemesterName [Semester], IsActive [Status] from SemesterTable where SemesterName like '%" + searchvalue.Trim() + "%'";
                }

                DataTable semesterlist = DatabaseLayer.Retrieve(query);
                dgvSemester.DataSource = semesterlist;

                if (dgvSemester.Rows.Count > 0)
                {
                    dgvSemester.Columns[0].Width = 80;
                    dgvSemester.Columns[1].Width = 150;
                    dgvSemester.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {
                MessageBox.Show("Some unexpected issue has occured. Please try again.");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dgvSession_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtSemesterName.Text.Length == 0)
            {
                ep.SetError(txtSemesterName, "Please Enter Correct Semester Name!");
                txtSemesterName.Focus();
                txtSemesterName.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from SemesterTable where SemesterName = '" + txtSemesterName.Text.Trim() + "' and  SemesterID != '" + Convert.ToString(dgvSemester.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSemesterName, "Already Exists");
                    txtSemesterName.Focus();
                    txtSemesterName.SelectAll();
                    return;
                }
            }

            string updatequery = string.Format("update SemesterTable set SemesterName = '{0}', isActive = '{1}' where SemesterID = '{2}'", txtSemesterName.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvSemester.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Semester Details!");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtSessionTitle_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtSemesterName.Text.Length == 0)
            {
                ep.SetError(txtSemesterName, "Please Enter Correct Semester Name!");
                txtSemesterName.Focus();
                txtSemesterName.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from SemeaterTable where SemesterName = '" + txtSemesterName.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSemesterName, "Already Exists");
                    txtSemesterName.Focus();
                    txtSemesterName.SelectAll();
                    return;
                }
            }

            string insertquery = string.Format("insert into SemesterTable(SemesterName, isActive) values('{0}', '{1}')  ", txtSemesterName.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Saved Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Semester Details!");
            }
        }

        public void ClearForm()
        {
            txtSemesterName.Clear();
            chkStatus.Checked = false;
        }

        public void EnableComponents()
        {
            dgvSemester.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvSemester.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            btnUpdate.Visible = false;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableComponents();
        }

        private void chkStatus_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void cmsOption_Opening(object sender, CancelEventArgs e)
        {

        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            if (dgvSemester != null)
            {
                if (dgvSemester.Rows.Count > 0)
                {
                    if (dgvSemester.SelectedRows.Count == 1)
                    {
                        txtSemesterName.Text = Convert.ToString(dgvSemester.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvSemester.CurrentRow.Cells[2].Value);
                        EnableComponents();
                    }
                    else
                    {
                        MessageBox.Show("Please Select 1 Record!");
                    }
                }
                else
                {
                    MessageBox.Show("List Is Empty!");
                }
            }
            else
            {
                MessageBox.Show("List Is Empty!");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmSemesters_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }
    }
}

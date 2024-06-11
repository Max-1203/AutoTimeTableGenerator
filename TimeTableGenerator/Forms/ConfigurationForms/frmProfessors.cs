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
    public partial class frmProfessors : Form
    {
        public frmProfessors()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void FillGrid(string searchvalue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchvalue.Trim()))
                {
                    query = "select ProfessorID [ID], FullName [Professor], ContactNo [Contact No], IsActive [Status] from ProfessorTable";
                }
                else
                {
                    query = "select ProfessorID [ID], FullName [Professor], ContactNo [Contact No], IsActive [Status] from ProfessorTable where (FullName + ' ' + ContactNo) like '%" + searchvalue.Trim() + "%'";
                }

                DataTable roomlist = DatabaseLayer.Retrieve(query);
                dgvProfessors.DataSource = roomlist;

                if (dgvProfessors.Rows.Count > 0)
                {
                    dgvProfessors.Columns[0].Width = 80;
                    dgvProfessors.Columns[1].Width = 150;
                    dgvProfessors.Columns[2].Width = 100;
                    dgvProfessors.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {
                MessageBox.Show("Some unexpected issue has occured. Please try again.");
            }
        }

        private void frmProfessors_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        public void ClearForm()
        {
            txtProfName.Clear();
            txtContactNo.Clear();
            chkStatus.Checked = false;
        }

        public void EnableComponents()
        {
            dgvProfessors.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvProfessors.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            btnUpdate.Visible = false;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtProfName.Text.Length == 0)
            {
                ep.SetError(txtProfName, "Please Enter Correct Professor Name!");
                txtProfName.Focus();
                txtProfName.SelectAll();
                return;
            }

            if (txtContactNo.Text.Length < 11)
            {
                ep.SetError(txtContactNo, "Please Enter Correct Contact No.!");
                txtContactNo.Focus();
                txtContactNo.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from ProfessorTable where FullName = '" + txtProfName.Text.Trim().ToUpper() + "' and ContactNo = '" + txtContactNo.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtProfName, "Already Exists");
                    txtProfName.Focus();
                    txtProfName.SelectAll();
                    return;
                }
            }

            string insertquery = string.Format("insert into ProfessorTable(FullName, ContactNo, IsActive) values('{0}', '{1}', '{2}')  ", txtProfName.Text.Trim().ToUpper(), txtContactNo.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Saved Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Professor Details!");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtProfName.Text.Length == 0)
            {
                ep.SetError(txtProfName, "Please Enter Correct Professor Name!");
                txtProfName.Focus();
                txtProfName.SelectAll();
                return;
            }

            if (txtContactNo.Text.Length < 11)
            {
                ep.SetError(txtContactNo, "Please Enter Correct Contact No.!");
                txtContactNo.Focus();
                txtContactNo.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from ProfessorTable where FullName = '" + txtProfName.Text.Trim().ToUpper() + "' and  ContactNo = '" + txtContactNo.Text.Trim() + "' and  ProfessorID != '" + Convert.ToString(dgvProfessors.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtProfName, "Already Exists");
                    txtProfName.Focus();
                    txtProfName.SelectAll();
                    return;
                }
            }

            string updatequery = string.Format("update ProfessorTable set FullName = '{0}', ContactNo = '{3}', IsActive = '{1}' where ProfessorID = '{2}'", txtProfName.Text.Trim().ToUpper(), chkStatus.Checked, Convert.ToString(dgvProfessors.CurrentRow.Cells[0].Value), txtContactNo.Text.Trim());
            bool result = DatabaseLayer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Professor Details!");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableComponents();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            if (dgvProfessors != null)
            {
                if (dgvProfessors.Rows.Count > 0)
                {
                    if (dgvProfessors.SelectedRows.Count == 1)
                    {
                        txtProfName.Text = Convert.ToString(dgvProfessors.CurrentRow.Cells[1].Value);
                        txtContactNo.Text = Convert.ToString(dgvProfessors.CurrentRow.Cells[2].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvProfessors.CurrentRow.Cells[3].Value);
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
    }
}

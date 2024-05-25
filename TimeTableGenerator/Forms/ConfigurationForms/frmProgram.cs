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
    public partial class frmProgram : Form
    {
        public frmProgram()
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
                    query = "select ProgramID [ID], Title, IsActive [Status] from ProgramTable";
                }
                else
                {
                    query = "select ProgramID [ID], Title, IsActive [Status] from ProgramTable where Title like '%" + searchvalue.Trim() + "%'";
                }

                DataTable programlist = DatabaseLayer.Retrieve(query);
                dgvProgrames.DataSource = programlist;

                if (dgvProgrames.Rows.Count > 0)
                {
                    dgvProgrames.Columns[0].Width = 80;
                    dgvProgrames.Columns[1].Width = 150;
                    dgvProgrames.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {
                MessageBox.Show("Some unexpected issue has occured. Please try again.");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmProgram_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }

        private void txtSessionTitle_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtProgramTitle.Text.Length == 0)
            {
                ep.SetError(txtProgramTitle, "Please Enter Correct Program Title!");
                txtProgramTitle.Focus();
                txtProgramTitle.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from ProgramTable where Title = '" + txtProgramTitle.Text.Trim() + "' and  ProgramID != '" + Convert.ToString(dgvProgrames.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtProgramTitle, "Already Exists");
                    txtProgramTitle.Focus();
                    txtProgramTitle.SelectAll();
                    return;
                }
            }

            string updatequery = string.Format("update ProgramTable set Title = '{0}', IsActive = '{1}' where ProgramID = '{2}'", txtProgramTitle.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvProgrames.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Program Details!");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtProgramTitle.Text.Length == 0)
            {
                ep.SetError(txtProgramTitle, "Please Enter Correct Program Title!");
                txtProgramTitle.Focus();
                txtProgramTitle.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from ProgramTable where Title = '" + txtProgramTitle.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtProgramTitle, "Already Exists");
                    txtProgramTitle.Focus();
                    txtProgramTitle.SelectAll();
                    return;
                }
            }

            string insertquery = string.Format("insert into ProgramTable(Title, IsActive) values('{0}', '{1}')  ", txtProgramTitle.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Saved Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Session Details!");
            }
        }

        public void ClearForm()
        {
            txtProgramTitle.Clear();
            chkStatus.Checked = false;
        }

        public void EnableComponents()
        {
            dgvProgrames.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvProgrames.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            btnUpdate.Visible = false;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableComponents();
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            if (dgvProgrames != null)
            {
                if (dgvProgrames.Rows.Count > 0)
                {
                    if (dgvProgrames.SelectedRows.Count == 1)
                    {
                        txtProgramTitle.Text = Convert.ToString(dgvProgrames.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvProgrames.CurrentRow.Cells[2].Value);
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

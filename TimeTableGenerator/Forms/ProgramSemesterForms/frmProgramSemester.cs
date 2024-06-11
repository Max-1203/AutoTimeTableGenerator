using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTableGenerator.SourceCode;

namespace TimeTableGenerator.Forms.ProgramSemesterForms
{
    public partial class frmProgramSemester : Form
    {
        public frmProgramSemester()
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
                    query = "select ProgramSemesterID [ID], Title, ProgramSemesterIsActive [Status], ProgramID , SemesterID from v_ProgramSemesterActiveList";
                }
                else
                {
                    query = "select ProgramSemesterID [ID], Title, ProgramSemesterIsActive [Status], ProgramID , SemesterID from v_ProgramSemesterActiveList where Title like '%" + searchvalue.Trim() + "%'";
                }

                DataTable roomlist = DatabaseLayer.Retrieve(query);
                dgvProgramSemester.DataSource = roomlist;

                if (dgvProgramSemester.Rows.Count > 0)
                {
                    dgvProgramSemester.Columns[0].Width = 80;
                    dgvProgramSemester.Columns[1].Width = 250;
                    dgvProgramSemester.Columns[2].Width = 100;
                    dgvProgramSemester.Columns[3].Visible = false;
                    dgvProgramSemester.Columns[4].Visible = false;
                }
            }
            catch
            {
                MessageBox.Show("Some unexpected issue has occured. Please try again.");
            }
        }

        private void frmProgramSemester_Load(object sender, EventArgs e)
        {
            ComboHelper.Semesters(cmbSelectSemester);
            ComboHelper.Programs(cmbSelectProgram);
            FillGrid(string.Empty);
        }

        private void cmbSelectProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!cmbSelectProgram.Text.Contains("Select"))
            {
                if(cmbSelectSemester.SelectedIndex > 0)
                {
                    txtProgramSemester.Text = cmbSelectProgram.Text + " " + cmbSelectSemester.Text;
                }
            }
        }

        private void cmbSelectSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbSelectSemester.Text.Contains("Select"))
            {
                if (cmbSelectProgram.SelectedIndex > 0)
                {
                    txtProgramSemester.Text = cmbSelectProgram.Text + " " + cmbSelectSemester.Text;
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        public void ClearForm()
        {
            txtProgramSemester.Clear();
            cmbSelectProgram.SelectedIndex = 0;
            cmbSelectSemester.SelectedIndex = 0;
            chkStatus.Checked = false;
        }

        public void EnableComponents()
        {
            dgvProgramSemester.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvProgramSemester.Enabled = true;
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
            if (txtProgramSemester.Text.Length == 0)
            {
                ep.SetError(txtProgramSemester, "Please Select Again!");
                txtProgramSemester.Focus();
                txtProgramSemester.SelectAll();
                return;
            }

            if (cmbSelectProgram.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectProgram, "Please Select Program!");
                cmbSelectProgram.Focus();
                return;
            }

            if (cmbSelectSemester.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectSemester, "Please Select Semester!");
                cmbSelectSemester.Focus();  
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from ProgramSemesterTable where ProgramID = '" + cmbSelectProgram.SelectedValue + "' and SemesterID = '" + cmbSelectSemester.SelectedValue + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtProgramSemester, "Already Exists");
                    txtProgramSemester.Focus();
                    txtProgramSemester.SelectAll();
                    return;
                }
            }

            string insertquery = string.Format("insert into ProgramSemesterTable(Title, ProgramID, SemesterID, IsActive) values('{0}', '{1}', '{2}', '{3}')  ", txtProgramSemester.Text.Trim(), cmbSelectProgram.SelectedValue, cmbSelectSemester.SelectedValue, chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Saved Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Details!");
            }
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
            if (dgvProgramSemester != null)
            {
                if (dgvProgramSemester.Rows.Count > 0)
                {
                    if (dgvProgramSemester.SelectedRows.Count == 1)
                    {
                        txtProgramSemester.Text = Convert.ToString(dgvProgramSemester.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvProgramSemester.CurrentRow.Cells[2].Value);
                        cmbSelectProgram.SelectedValue = Convert.ToString(dgvProgramSemester.CurrentRow.Cells[3].Value);
                        cmbSelectSemester.SelectedValue = Convert.ToString(dgvProgramSemester.CurrentRow.Cells[4].Value);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtProgramSemester.Text.Length == 0)
            {
                ep.SetError(txtProgramSemester, "Please Select Again!");
                txtProgramSemester.Focus();
                txtProgramSemester.SelectAll();
                return;
            }

            if (cmbSelectProgram.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectProgram, "Please Select Program!");
                cmbSelectProgram.Focus();
                return;
            }

            if (cmbSelectSemester.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectSemester, "Please Select Semester!");
                cmbSelectSemester.Focus();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from ProgramSemesterTable where ProgramID = '" + cmbSelectProgram.SelectedValue + "' and SemesterID = '" + cmbSelectSemester.SelectedValue + "' and ProgramSemesterID != '" + Convert.ToString(dgvProgramSemester.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtProgramSemester, "Already Exists");
                    txtProgramSemester.Focus();
                    txtProgramSemester.SelectAll();
                    return;
                }
            }

            string updatequery = string.Format("update ProgramSemesterTable set Title = '{0}', ProgramID = '{1}', SemesterID = '{2}', IsActive = '{3}' where ProgramSemesterID = '{4}'", txtProgramSemester.Text.Trim(), cmbSelectProgram.SelectedValue, cmbSelectSemester.SelectedValue, chkStatus.Checked, Convert.ToString(dgvProgramSemester.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Details!");
            }
        }
    }
}

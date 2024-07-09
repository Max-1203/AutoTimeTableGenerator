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
                    query = "select ProgramSemesterID [ID], Title, Capacity, ProgramSemesterIsActive [Status], ProgramID , SemesterID from v_ProgramSemesterActiveList";
                }
                else
                {
                    query = "select ProgramSemesterID [ID], Title, Capacity, ProgramSemesterIsActive [Status], ProgramID , SemesterID from v_ProgramSemesterActiveList where Title like '%" + searchvalue.Trim() + "%'";
                }

                DataTable roomlist = DatabaseLayer.Retrieve(query);
                dgvProgramSemester.DataSource = roomlist;

                if (dgvProgramSemester.Rows.Count > 0)
                {
                    dgvProgramSemester.Columns[0].Width = 50; //ID
                    dgvProgramSemester.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; //Title
                    dgvProgramSemester.Columns[2].Width = 90; //Capacity
                    dgvProgramSemester.Columns[3].Width = 70; //Status
                    dgvProgramSemester.Columns[4].Visible = false; //ProgramID
                    dgvProgramSemester.Columns[5].Visible = false; //SemesterID
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
            //txtCapacity.Clear();
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

            if(txtCapacity.Text.Trim().Length == 0)
            {
                ep.SetError(txtCapacity, "Please Enter Semester Capacity!");
                txtCapacity.Focus();
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

            string insertquery = string.Format("insert into ProgramSemesterTable(Title, ProgramID, SemesterID, IsActive, Capacity) values('{0}', '{1}', '{2}', '{3}', '{4}')  ", txtProgramSemester.Text.Trim(), cmbSelectProgram.SelectedValue, cmbSelectSemester.SelectedValue, chkStatus.Checked, txtCapacity.Text.Trim());
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
                        txtCapacity.Text = Convert.ToString(dgvProgramSemester.CurrentRow.Cells[2].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvProgramSemester.CurrentRow.Cells[3].Value);
                        cmbSelectProgram.SelectedValue = Convert.ToString(dgvProgramSemester.CurrentRow.Cells[4].Value);
                        cmbSelectSemester.SelectedValue = Convert.ToString(dgvProgramSemester.CurrentRow.Cells[5].Value);
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

            if (txtCapacity.Text.Trim().Length == 0)
            {
                ep.SetError(txtCapacity, "Please Enter Semester Capacity!");
                txtCapacity.Focus();
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

            string updatequery = string.Format("update ProgramSemesterTable set Title = '{0}', ProgramID = '{1}', SemesterID = '{2}', IsActive = '{3}', Capacity = '{5}' where ProgramSemesterID = '{4}'", txtProgramSemester.Text.Trim(), cmbSelectProgram.SelectedValue, cmbSelectSemester.SelectedValue, chkStatus.Checked, Convert.ToString(dgvProgramSemester.CurrentRow.Cells[0].Value), txtCapacity.Text.Trim());
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

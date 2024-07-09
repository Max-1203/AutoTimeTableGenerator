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


namespace TimeTableGenerator.Forms.ProfessorSubjectForms
{
    public partial class frmProfessorSubject : Form
    {
        public frmProfessorSubject()
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
                    query = "select ProfessorSubjectID [ID], ProfessorSubjectTitle [Title], ProfessorID, FullName [Professor], CourseID, Title [Course], IsActive [Status] from v_AllSubjectProfessors";
                }
                else
                {
                    query = "select ProfessorSubjectID [ID], ProfessorSubjectTitle [Title], ProfessorID, FullName [Professor], CourseID, Title [Course], IsActive [Status] from v_AllSubjectProfessors where (ProfessorSubjectTitle + ' ' + FullName + ' ' + Title) like '%" + searchvalue.Trim() + "%'";
                }

                DataTable roomlist = DatabaseLayer.Retrieve(query);
                dgvProfessorSubjects.DataSource = roomlist;

                if (dgvProfessorSubjects.Rows.Count > 0)
                {
                    dgvProfessorSubjects.Columns[0].Visible = false; //ProfessorSubjectID
                    dgvProfessorSubjects.Columns[1].Width = 180; //ProfessorSubjectTitle
                    dgvProfessorSubjects.Columns[2].Visible = false; //ProfessorID
                    dgvProfessorSubjects.Columns[3].Width = 120; //FullName
                    dgvProfessorSubjects.Columns[4].Visible = false; //CourseID
                    dgvProfessorSubjects.Columns[5].Width = 100; //Title
                    dgvProfessorSubjects.Columns[6].Width = 60; //IsActive
                }
            }
            catch
            {
                MessageBox.Show("Some unexpected issue has occured. Please try again.");
            }
        }

        private void frmProfessorSubject_Load(object sender, EventArgs e)
        {
            ComboHelper.AllProfessors(cmbSelectProfessor);
            ComboHelper.AllSubjects(cmbSelectSubject);
            FillGrid(string.Empty);
        }

        public void ClearForm()
        {
            txtProfessorSubject.Clear();
            cmbSelectProfessor.SelectedIndex = 0;
            cmbSelectSubject.SelectedIndex = 0;
            chkStatus.Checked = false;
        }

        public void DisableComponents()
        {
            dgvProfessorSubjects.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            { 
                ep.Clear();
                if (cmbSelectProfessor.SelectedIndex == 0)
                {
                    ep.SetError(cmbSelectProfessor, "Please Select Professor!");
                    cmbSelectProfessor.Focus();
                    return;
                }

                if (cmbSelectSubject.SelectedIndex == 0)
                {
                    ep.SetError(cmbSelectSubject, "Please Select Subject!");
                    cmbSelectSubject.Focus();
                    return;
                }

                DataTable checktitle = DatabaseLayer.Retrieve("select * from ProfessorSubjectTable where ProfessorID = '" + cmbSelectProfessor.SelectedValue + "' and CourseID = '" + cmbSelectSubject.SelectedValue + "'");
                if (checktitle != null)
                {
                    if (checktitle.Rows.Count > 0)
                    {
                        ep.SetError(txtProfessorSubject, "Already Registered");
                        txtProfessorSubject.Focus();
                        txtProfessorSubject.SelectAll();
                        return;
                    }
                }

                string insertquery = string.Format("insert into ProfessorSubjectTable(ProfessorSubjectTitle, ProfessorID, CourseID, IsActive) values('{0}', '{1}', '{2}', '{3}')  ", txtProfessorSubject.Text.Trim(), cmbSelectProfessor.SelectedValue, cmbSelectSubject.SelectedValue, chkStatus.Checked);
                bool result = DatabaseLayer.Insert(insertquery);
                if (result == true)
                {
                    MessageBox.Show("Saved Successfully!");
                    DisableComponents();
                }
                else
                {
                    MessageBox.Show("Some Unexpected Error Occured! Please Try Again!");
                }
            } 
            catch
            {
                MessageBox.Show("Please Check Sql Server Agent Connectivity!");
            }
        }

        private void cmbSelectProfessor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbSelectProfessor.Text.Contains("Select"))
            {
                if (cmbSelectSubject.SelectedIndex > 0)
                {
                    txtProfessorSubject.Text = cmbSelectSubject.Text + " (" + cmbSelectProfessor.Text + ")";
                }
            }
        }

        private void cmbSelectSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbSelectSubject.Text.Contains("Select"))
            {
                if (cmbSelectProfessor.SelectedIndex > 0)
                {
                    txtProfessorSubject.Text = cmbSelectSubject.Text + " (" + cmbSelectProfessor.Text + ")";
                }
            }
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if(dgvProfessorSubjects != null)
                {
                    if(dgvProfessorSubjects.Rows.Count > 0)
                    {
                        if(dgvProfessorSubjects.SelectedRows.Count == 1)
                        {
                            if (MessageBox.Show("Are You Sure You Want To Update Selected Record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                string id = Convert.ToString(dgvProfessorSubjects.CurrentRow.Cells[0].Value);
                                bool status = Convert.ToBoolean(dgvProfessorSubjects.CurrentRow.Cells[6].Value) == true ? false : true;
                                string updatequery = "update ProfessorSubjectTable set IsActive = '" + status + "' where ProfessorSubjectID = '" + id + "'";
                                bool result = DatabaseLayer.Update(updatequery);
                                if (result == true)
                                {
                                    MessageBox.Show("Status Updated Successfully!");
                                    DisableComponents();
                                }
                                else
                                {
                                    MessageBox.Show("Some Unexpected Error Occured! Please Try Again!");
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                
            }
        }
    }
}

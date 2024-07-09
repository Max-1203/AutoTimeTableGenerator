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

namespace TimeTableGenerator.Forms.ConfigurationForms
{
    public partial class frmCourses : Form
    {
        public frmCourses()
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
                    query = "select CourseID [ID], Title [Course], CrHrs, RoomTypeID, TypeName [Type], IsActive from v_AllSubjects";
                }
                else
                {
                    query = "select CourseID [ID], Title [Course], CrHrs, RoomTypeID, TypeName [Type], IsActive from v_AllSubjects where (Title + ' ' + TypeName) like '%" + searchvalue.Trim() + "%'";
                }

                DataTable roomlist = DatabaseLayer.Retrieve(query);
                dgvCourse.DataSource = roomlist;

                if (dgvCourse.Rows.Count > 0)
                {
                    dgvCourse.Columns[0].Width = 40; // CourseID
                    dgvCourse.Columns[1].Width = 220; // Title
                    dgvCourse.Columns[2].Width = 60; // CrHrs
                    dgvCourse.Columns[3].Visible = false; // RoomTypeID
                    dgvCourse.Columns[4].Width = 60; // TypeName
                    dgvCourse.Columns[5].Width = 80; // IsActive
                }
            }
            catch
            {
                MessageBox.Show("Some unexpected issue has occured. Please try again.");
            }
        }

        private void frmCourses_Load(object sender, EventArgs e)
        {
            cmbCrHrs.SelectedIndex = 0;
            ComboHelper.RoomTypes(cmbSelectType);
            FillGrid(string.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        public void ClearForm()
        {
            txtCourseTitle.Clear();
            cmbSelectType.SelectedIndex = 0;
            cmbCrHrs.SelectedIndex = 0;
            chkStatus.Checked = false;
        }

        public void EnableComponents()
        {
            dgvCourse.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvCourse.Enabled = true;
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
            if (txtCourseTitle.Text.Length == 0)
            {
                ep.SetError(txtCourseTitle, "Please Enter Course Title!");
                txtCourseTitle.Focus();
                txtCourseTitle.SelectAll();
                return;
            }

            if (cmbSelectType.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectType, "Please Select Type!");
                cmbSelectType.Focus();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from CourseTable where Title = '" + txtCourseTitle.Text.Trim() + "' and RoomTypeID = '" + cmbSelectType.SelectedValue + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtCourseTitle, "Already Exists");
                    txtCourseTitle.Focus();
                    txtCourseTitle.SelectAll();
                    return;
                }
            }

            string insertquery = string.Format("insert into CourseTable(Title, CrHrs, RoomTypeID, IsActive) values('{0}', '{1}', '{2}', '{3}')  ", txtCourseTitle.Text.Trim().ToUpper(), cmbCrHrs.Text, cmbSelectType.SelectedValue, chkStatus.Checked);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtCourseTitle.Text.Length == 0)
            {
                ep.SetError(txtCourseTitle, "Please Enter Course Title!");
                txtCourseTitle.Focus();
                txtCourseTitle.SelectAll();
                return;
            }

            if (cmbSelectType.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectType, "Please Select Type!");
                cmbSelectType.Focus();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from CourseTable where Title = '" + txtCourseTitle.Text.Trim() + "' and RoomTypeID = '" + cmbSelectType.SelectedValue + "' and CourseID != '" + Convert.ToString(dgvCourse.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtCourseTitle, "Already Exists");
                    txtCourseTitle.Focus();
                    txtCourseTitle.SelectAll();
                    return;
                }
            }

            string updatequery = string.Format("update CourseTable set Title = '{0}', CrHrs = '{1}', RoomTypeID = '{2}', IsActive = '{3}' where CourseID = '{4}'", txtCourseTitle.Text.Trim().ToUpper(), cmbCrHrs.Text, cmbSelectType.SelectedValue, chkStatus.Checked, Convert.ToString(dgvCourse.CurrentRow.Cells[0].Value));
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

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            if (dgvCourse != null)
            {
                if (dgvCourse.Rows.Count > 0)
                {
                    if (dgvCourse.SelectedRows.Count == 1)
                    {
                        txtCourseTitle.Text = Convert.ToString(dgvCourse.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvCourse.CurrentRow.Cells[5].Value);
                        cmbSelectType.SelectedValue = Convert.ToString(dgvCourse.CurrentRow.Cells[3].Value);
                        cmbCrHrs.Text = Convert.ToString(dgvCourse.CurrentRow.Cells[2].Value);
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

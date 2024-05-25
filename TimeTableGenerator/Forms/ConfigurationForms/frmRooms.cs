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
    public partial class frmRooms : Form
    {
        public frmRooms()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCapacity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public void FillGrid(string searchvalue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchvalue.Trim()))
                {
                    query = "select RoomID [ID], RoomNo [Room], Capacity, IsActive [Status] from RoomTable";
                }
                else
                {
                    query = "select RoomID [ID], RoomNo [Room], Capacity, IsActive [Status] from RoomTable where RoomNo like '%" + searchvalue.Trim() + "%'";
                }

                DataTable roomlist = DatabaseLayer.Retrieve(query);
                dgvRooms.DataSource = roomlist;

                if (dgvRooms.Rows.Count > 0)
                {
                    dgvRooms.Columns[0].Width = 80;
                    dgvRooms.Columns[1].Width = 150;
                    dgvRooms.Columns[2].Width = 100;
                    dgvRooms.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {
                MessageBox.Show("Some unexpected issue has occured. Please try again.");
            }
        }

        private void frmRooms_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        public void ClearForm()
        {
            txtRoomNo.Clear();
            txtCapacity.Clear();
            chkStatus.Checked = false;
        }

        public void EnableComponents()
        {
            dgvRooms.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvRooms.Enabled = true;
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
            if (txtRoomNo.Text.Length == 0)
            {
                ep.SetError(txtRoomNo, "Please Enter Correct Room No/Name!");
                txtRoomNo.Focus();
                txtRoomNo.SelectAll();
                return;
            }

            if (txtCapacity.Text.Length == 0)
            {
                ep.SetError(txtCapacity, "Please Enter Correct Room Capacity!");
                txtCapacity.Focus();
                txtCapacity.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from RoomTable where RoomNo = '" + txtRoomNo.Text.Trim().ToUpper() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtRoomNo, "Already Exists");
                    txtRoomNo.Focus();
                    txtRoomNo.SelectAll();
                    return;
                }
            }

            string insertquery = string.Format("insert into RoomTable(RoomNo, Capacity, IsActive) values('{0}', '{1}', '{2}')  ", txtRoomNo.Text.Trim().ToUpper(), txtCapacity.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Saved Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Room Details!");
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
            if (txtRoomNo.Text.Length == 0)
            {
                ep.SetError(txtRoomNo, "Please Enter Correct Room No/Name!");
                txtRoomNo.Focus();
                txtRoomNo.SelectAll();
                return;
            }

            if (txtCapacity.Text.Length == 0)
            {
                ep.SetError(txtCapacity, "Please Enter Correct Room Capacity!");
                txtCapacity.Focus();
                txtCapacity.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from RoomTable where RoomNo = '" + txtRoomNo.Text.Trim().ToUpper() + "' and  RoomID != '" + Convert.ToString(dgvRooms.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtRoomNo, "Already Exists");
                    txtRoomNo.Focus();
                    txtRoomNo.SelectAll();
                    return;
                }
            }

            string updatequery = string.Format("update RoomTable set RoomNo = '{0}', Capacity = '{3}', IsActive = '{1}' where RoomID = '{2}'", txtRoomNo.Text.Trim().ToUpper(), chkStatus.Checked, Convert.ToString(dgvRooms.CurrentRow.Cells[0].Value), txtCapacity.Text.Trim());
            bool result = DatabaseLayer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Room Details!");
            }
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            if (dgvRooms != null)
            {
                if (dgvRooms.Rows.Count > 0)
                {
                    if (dgvRooms.SelectedRows.Count == 1)
                    {
                        txtRoomNo.Text = Convert.ToString(dgvRooms.CurrentRow.Cells[1].Value);
                        txtCapacity.Text = Convert.ToString(dgvRooms.CurrentRow.Cells[2].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvRooms.CurrentRow.Cells[3].Value);
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

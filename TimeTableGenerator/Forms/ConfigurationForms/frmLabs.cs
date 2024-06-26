﻿using System;
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
    public partial class frmLabs : Form
    {
        public frmLabs()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCapacity_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

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
                    query = "select LabID [ID], LabNo [Lab], Capacity, IsActive [Status] from LabTable";
                }
                else
                {
                    query = "select LabID [ID], LabNo [Lab], Capacity, IsActive [Status] from LabTable where LabNo like '%" + searchvalue.Trim() + "%'";
                }

                DataTable roomlist = DatabaseLayer.Retrieve(query);
                dgvLabs.DataSource = roomlist;

                if (dgvLabs.Rows.Count > 0)
                {
                    dgvLabs.Columns[0].Width = 80;
                    dgvLabs.Columns[1].Width = 150;
                    dgvLabs.Columns[2].Width = 100;
                    dgvLabs.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {
                MessageBox.Show("Some unexpected issue has occured. Please try again.");
            }
        }

        private void frmLabs_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        public void ClearForm()
        {
            txtLabNo.Clear();
            txtCapacity.Clear();
            chkStatus.Checked = false;
        }

        public void EnableComponents()
        {
            dgvLabs.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvLabs.Enabled = true;
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
            if (txtLabNo.Text.Length == 0)
            {
                ep.SetError(txtLabNo, "Please Enter Correct Lab No/Name!");
                txtLabNo.Focus();
                txtLabNo.SelectAll();
                return;
            }

            if (txtCapacity.Text.Length == 0)
            {
                ep.SetError(txtCapacity, "Please Enter Correct Lab Capacity!");
                txtCapacity.Focus();
                txtCapacity.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from LabTable where LabNo = '" + txtLabNo.Text.Trim().ToUpper() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtLabNo, "Already Exists");
                    txtLabNo.Focus();
                    txtLabNo.SelectAll();
                    return;
                }
            }

            string insertquery = string.Format("insert into LabTable(LabNo, Capacity, IsActive) values('{0}', '{1}', '{2}')  ", txtLabNo.Text.Trim().ToUpper(), txtCapacity.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Saved Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Lab Details!");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtLabNo.Text.Length == 0)
            {
                ep.SetError(txtLabNo, "Please Enter Correct Lab No/Name!");
                txtLabNo.Focus();
                txtLabNo.SelectAll();
                return;
            }

            if (txtCapacity.Text.Length == 0)
            {
                ep.SetError(txtCapacity, "Please Enter Correct Lab Capacity!");
                txtCapacity.Focus();
                txtCapacity.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from LabTable where LabNo = '" + txtLabNo.Text.Trim().ToUpper() + "' and  LabID != '" + Convert.ToString(dgvLabs.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtLabNo, "Already Exists");
                    txtLabNo.Focus();
                    txtLabNo.SelectAll();
                    return;
                }
            }

            string updatequery = string.Format("update LabTable set LabNo = '{0}', Capacity = '{3}', IsActive = '{1}' where LabID = '{2}'", txtLabNo.Text.Trim().ToUpper(), chkStatus.Checked, Convert.ToString(dgvLabs.CurrentRow.Cells[0].Value), txtCapacity.Text.Trim());
            bool result = DatabaseLayer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated Successfully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Lab Details!");
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
            if (dgvLabs != null)
            {
                if (dgvLabs.Rows.Count > 0)
                {
                    if (dgvLabs.SelectedRows.Count == 1)
                    {
                        txtLabNo.Text = Convert.ToString(dgvLabs.CurrentRow.Cells[1].Value);
                        txtCapacity.Text = Convert.ToString(dgvLabs.CurrentRow.Cells[2].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvLabs.CurrentRow.Cells[3].Value);
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

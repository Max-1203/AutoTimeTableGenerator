using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTableGenerator.AllModels;
using TimeTableGenerator.SourceCode;

namespace TimeTableGenerator.Forms.TimeSlotForms
{
    public partial class frmDayTimeSlots : Form
    {
        public frmDayTimeSlots()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
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
                    query = "select DayTimeSlotID [ID], Row_Number() over (Order by DayTimeSlotID) [S.No.], DayID, Name [Day], SlotTitle [Slot Title], StartTime [Start Time], EndTime [End Time], IsActive [Status] from v_AllTimeSlots where IsActive = 1";
                }
                else
                {
                    query = "select DayTimeSlotID [ID], Row_Number() over (Order by DayTimeSlotID) [S.No.], DayID, Name [Day], SlotTitle [Slot Title], StartTime [Start Time], EndTime [End Time], IsActive [Status] from v_AllTimeSlots" +
                            "where IsActive = 1 and (Name + ' ' + SlotTitle) like '%"+ searchvalue.Trim() +"%'";
                }

                DataTable slotlist = DatabaseLayer.Retrieve(query);
                dgvSlots.DataSource = slotlist;

                if (dgvSlots.Rows.Count > 0)
                {
                    dgvSlots.Columns[0].Visible = false; // DayTimeSlotID
                    dgvSlots.Columns[1].Width = 60; // S.No.
                    dgvSlots.Columns[2].Visible = false; // DayID
                    dgvSlots.Columns[3].Width = 110; // Name
                    dgvSlots.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // SlotTitle
                    dgvSlots.Columns[5].Visible = false; // StartTime
                    dgvSlots.Columns[6].Visible = false; // EndTime
                    dgvSlots.Columns[7].Width = 70; // IsActive
                }
            }
            catch
            {
                MessageBox.Show("Some unexpected issue has occured. Please try again.");
            }
        }

        private void frmDayTimeSlots_Load(object sender, EventArgs e)
        {
            dtpStartTime.Value = new DateTime(2024, 6, 15, 9, 0, 0);
            dtpEndTime.Value = new DateTime(2024, 6, 15, 18, 0, 0);
            ComboHelper.AllDays(cmbDays);
            ComboHelper.TimeSlotsNumber(cmbNumSlots, 18);
            FillGrid(string.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        public void ClearForm()
        {
            cmbDays.SelectedIndex = 0;
            cmbNumSlots.SelectedIndex = 0;
            chkStatus.Checked = true;
        }

        public void EnableComponents()
        {
            dgvSlots.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvSlots.Enabled = true;
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ep.Clear();
                if (cmbDays.SelectedIndex == 0)
                {
                    ep.SetError(cmbDays, "Please Select Day!");
                    cmbDays.Focus();
                    return;
                }

                if (cmbNumSlots.SelectedIndex == 0)
                {
                    ep.SetError(cmbNumSlots, "Please Select Number of Time Slots per Day!");
                    cmbNumSlots.Focus();
                    return;
                }

                string updatequery = "update DayTimeSlotTable set IsActive = 0 where DayID = '" + cmbDays.SelectedValue + "'";
                bool updateresult = DatabaseLayer.Update(updatequery);
                if (updateresult)
                {
                    List<TimeSlotsMV> timeslots = new List<TimeSlotsMV>();

                    TimeSpan time = dtpEndTime.Value - dtpStartTime.Value;
                    int totalminutes = (int)time.TotalMinutes;
                    int numberoftimeslots = Convert.ToInt32(cmbNumSlots.SelectedValue);
                    int slot_size = totalminutes / numberoftimeslots; ;

                    int i = 0;
                    do
                    {
                        var timeslot = new TimeSlotsMV();
                        var FromTime = (dtpStartTime.Value).AddMinutes(slot_size * i);
                        i++;
                        var ToTime = (dtpStartTime.Value).AddMinutes(slot_size * i);
                        string title = FromTime.ToString("hh:mm tt") + " - " + ToTime.ToString("hh:mm tt");
                        timeslot.FromTime = FromTime;
                        timeslot.ToTime = ToTime;
                        timeslot.SlotTitle = title;
                        timeslots.Add(timeslot);
                    }
                    while (i < numberoftimeslots);

                    bool insertstatus = true;
                    foreach (TimeSlotsMV slot in timeslots)
                    {
                        string insertquery = string.Format("insert into DayTimeSlotTable(DayID, SlotTitle, StartTime, EndTime, IsActive) values('{0}', '{1}', '{2}', '{3}', '{4}') ", cmbDays.SelectedValue, slot.SlotTitle, slot.FromTime.TimeOfDay, slot.ToTime.TimeOfDay, chkStatus.Checked);

                        bool result = DatabaseLayer.Insert(insertquery);
                        if (result == false)
                        {
                            insertstatus = false;
                        }
                    }
                    if (insertstatus == true)
                    {
                        MessageBox.Show("Slots Created Successfully!");
                        DisableComponents();
                    }
                    else
                    {
                        MessageBox.Show("Please Provide Correct Details and Try Again!");
                    }
                }
                else
                {
                    MessageBox.Show("Please Provide Correct Details and Try Again!");
                }
            }
            catch
            {
                MessageBox.Show("Check SQL Server Agent Connectivity!");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            if (dgvSlots != null)
            {
                if (dgvSlots.Rows.Count > 0)
                {
                    if (dgvSlots.SelectedRows.Count == 1)
                    {
                        string slotid = Convert.ToString(dgvSlots.CurrentRow.Cells[0].Value);
                        string updatequery = "update DayTimeSlotTable set IsActive = 0 where DayTimeSlotID = '" + slotid + "'";
                        bool result = DatabaseLayer.Update(updatequery);
                        if (result == true)
                        {
                            MessageBox.Show("Break Time Is Marked!");
                            DisableComponents();
                        }
                    }
                }
            }
        }
    }
}

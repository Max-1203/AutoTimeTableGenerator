using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTableGenerator.SourceCode
{
    public class ComboHelper
    {
        public static void Semesters(ComboBox cmb)
        {
            DataTable dtSemesters = new DataTable();
            dtSemesters.Columns.Add("SemesterID");
            dtSemesters.Columns.Add("SemesterName");
            dtSemesters.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select SemesterID, SemesterName from SemesterTable where IsActive = 1");
                if(dt != null)
                {
                    if(dt.Rows.Count > 0)
                    {
                        foreach(DataRow semester in dt.Rows)
                        {
                            dtSemesters.Rows.Add(semester["SemesterID"], semester["SemesterName"]);
                        }
                    }
                }
                cmb.DataSource = dtSemesters;
                cmb.ValueMember = "SemesterID";
                cmb.DisplayMember = "SemesterName";
            }
            catch
            {
                cmb.DataSource = dtSemesters;
            }
        }

        public static void Programs(ComboBox cmb)
        {
            DataTable dtPrograms = new DataTable();
            dtPrograms.Columns.Add("ProgramID");
            dtPrograms.Columns.Add("Title");
            dtPrograms.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select ProgramID, Title from ProgramTable where IsActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow program in dt.Rows)
                        {
                            dtPrograms.Rows.Add(program["ProgramID"], program["Title"]);
                        }
                    }
                }
                cmb.DataSource = dtPrograms;
                cmb.ValueMember = "ProgramID";
                cmb.DisplayMember = "Title";
            }
            catch
            {
                cmb.DataSource = dtPrograms;
            }
        }

        public static void RoomTypes(ComboBox cmb)
        {
            DataTable dtTypes = new DataTable();
            dtTypes.Columns.Add("RoomTypeID");
            dtTypes.Columns.Add("TypeName");
            dtTypes.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select RoomTypeID, TypeName from RoomTypeTable");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow type in dt.Rows)
                        {
                            dtTypes.Rows.Add(type["RoomTypeID"], type["TypeName"]);
                        }
                    }
                }
                cmb.DataSource = dtTypes;
                cmb.ValueMember = "RoomTypeID";
                cmb.DisplayMember = "TypeName";
            }
            catch
            {
                cmb.DataSource = dtTypes;
            }
        }

        public static void AllDays(ComboBox cmb)
        {
            DataTable dtlist = new DataTable();
            dtlist.Columns.Add("DayID");
            dtlist.Columns.Add("Name");
            dtlist.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select DayID, Name from DayTable where IsActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow type in dt.Rows)
                        {
                            dtlist.Rows.Add(type["DayID"], type["Name"]);
                        }
                    }
                }
                cmb.DataSource = dtlist;
                cmb.ValueMember = "DayID";
                cmb.DisplayMember = "Name";
            }
            catch
            {
                cmb.DataSource = dtlist;
            }
        }

        public static void TimeSlotsNumber(ComboBox cmb, int n)
        {
            DataTable dtlist = new DataTable();
            dtlist.Columns.Add("ID");
            dtlist.Columns.Add("Number");
            dtlist.Rows.Add("0", "---Select---");
            
            for(int i = 1; i <= n; i++)
            {
                dtlist.Rows.Add(i, i);
            }

            cmb.DataSource = dtlist;
            cmb.ValueMember = "ID";
            cmb.DisplayMember = "Number";
            
        }

        public static void AllProfessors(ComboBox cmb)
        {
            DataTable dtlist = new DataTable();
            dtlist.Columns.Add("ProfessorID");
            dtlist.Columns.Add("FullName");
            dtlist.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select ProfessorID, FullName from ProfessorTable where IsActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow type in dt.Rows)
                        {
                            dtlist.Rows.Add(type["ProfessorID"], type["FullName"]);
                        }
                    }
                }
                cmb.DataSource = dtlist;
                cmb.ValueMember = "ProfessorID";
                cmb.DisplayMember = "FullName";
            }
            catch
            {
                cmb.DataSource = dtlist;
            }
        }

        public static void AllSubjects(ComboBox cmb)
        {
            DataTable dtlist = new DataTable();
            dtlist.Columns.Add("CourseID");
            dtlist.Columns.Add("Title");
            dtlist.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select CourseID, Title from CourseTable where IsActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow type in dt.Rows)
                        {
                            dtlist.Rows.Add(type["CourseID"], type["Title"]);
                        }
                    }
                }
                cmb.DataSource = dtlist;
                cmb.ValueMember = "CourseID";
                cmb.DisplayMember = "Title";
            }
            catch
            {
                cmb.DataSource = dtlist;
            }
        }
    }
}

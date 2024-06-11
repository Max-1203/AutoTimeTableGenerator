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
    }
}

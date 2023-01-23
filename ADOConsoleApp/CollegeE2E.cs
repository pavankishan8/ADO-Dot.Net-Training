using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ADOConsoleApp
{

    class CollegeProgram
    {
        static string strConnection = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;

        const string query = "SELECT * FROM TBLSTUDENTS ; SELECT * FROM TBLBRANCHES";
        static DataSet disconnectedObj = new DataSet("All Records");
        static SqlDataAdapter ada = null;
        static void fillRecords()
        {
            SqlConnection sqlCon = new SqlConnection(strConnection);
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);

            ada = new SqlDataAdapter(sqlCmd);
            SqlCommandBuilder builder = new SqlCommandBuilder(ada);

            ada.Fill(disconnectedObj);
            disconnectedObj.Tables[0].TableName = "StdList";
            if (disconnectedObj.Tables[0].PrimaryKey.Length == 0)
                disconnectedObj.Tables[0].PrimaryKey = new DataColumn[]
                {
                    disconnectedObj.Tables[0].Columns[0]
                };
            disconnectedObj.Tables[1].TableName = "BranchList";

            Trace.WriteLine("Connection State: " + ada.SelectCommand.Connection.State);

        }
        static void Main(string[] args)
        {
            fillRecords();
            //insertStudent(6,"Arjun","Rajasthan",97, 1);
            //deleteStudent(5);
            updateStudent(1, "Pavan R", "Bangalore", 97, 1);
            DisplayStudentsofBranch("BCA");
        }

        private static void updateStudent(int id, string name, string address, int salary, int branchId)
        {
            var selectedRow = disconnectedObj.Tables[0].Rows.Find(id);
            selectedRow[0] = id;
            selectedRow[1] = name;
            selectedRow[2] = address;
            selectedRow[3] = salary;
            selectedRow[4] = branchId;
            ada.Update(disconnectedObj, "StdList");
        }

        private static void deleteStudent(int delId)
        {
            foreach (DataRow row in disconnectedObj.Tables[0].Rows)
            {
                if (row[0].ToString() == delId.ToString())
                {
                    row.Delete();
                    break;
                }
            }
            ada.Update(disconnectedObj, "StdList");
        }

        private static void insertStudent(int id,string name, string address, int percent, int branchId)
        {
            DataRow newRow = disconnectedObj.Tables[0].NewRow();
            newRow[0] = id;
            newRow[1] = name;
            newRow[2] = address;
            newRow[3] = percent;
            newRow[4] = branchId;

            disconnectedObj.Tables[0].Rows.Add(newRow);
            ada.Update(disconnectedObj, "StdList");
        }
        
        static void DisplayStudentsofBranch(string branchName)
        {
            int branchId = 0;
            foreach (DataRow row in disconnectedObj.Tables["BranchList"].Rows)
            {
                if (row["BranchName"].ToString() == branchName)
                {
                    branchId = (int)row["BranchId"];
                    break;
                }
            }
            foreach (DataRow row in disconnectedObj.Tables["StdList"].Rows)
            {
                if (row[4].ToString() == branchId.ToString())
                {
                    Console.WriteLine(row["StdName"]);
                }
            }

        }
    }
}
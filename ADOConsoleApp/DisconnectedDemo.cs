using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ADOConsoleApp
{

    class DisconnectedProgram
    {
        static string strConnection = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;

        const string query = "SELECT * FROM TBLEMPLOYEE ; SELECT * FROM TBLDEPT";
        static DataSet disconnectedObj = new DataSet("All Records");
        static SqlDataAdapter ada = null;
        static void fillRecords()
        {
            SqlConnection sqlCon = new SqlConnection(strConnection);
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);

            ada = new SqlDataAdapter(sqlCmd);
            SqlCommandBuilder builder = new SqlCommandBuilder(ada);

            ada.Fill(disconnectedObj);
            disconnectedObj.Tables[0].TableName = "EmpList";
            if (disconnectedObj.Tables[0].PrimaryKey.Length == 0)
                disconnectedObj.Tables[0].PrimaryKey = new DataColumn[]
                {
                    disconnectedObj.Tables[0].Columns[0]
                };
            disconnectedObj.Tables[1].TableName = "DeptList";

            Trace.WriteLine("Connection State: " + ada.SelectCommand.Connection.State);
            
        }
        static void Main(string[] args)
        {
            fillRecords();

            //DisplayEmployeesofDept("Utilities");
            //insertEmployee("Arvind","Shivmoga",50000, 4);
            //deleteEmployee(1175);
            updateEmployee(1181,"Ratha", "North", 45000, 4);
        }

        private static void updateEmployee(int id,string name, string address, int salary, int deptId)
        {
             var selectedRow = disconnectedObj.Tables[0].Rows.Find(id);
            
                    selectedRow[1] = name;
                    selectedRow[2] = address;
                    selectedRow[3] = salary;
                    selectedRow[4] = deptId;
                    ada.Update(disconnectedObj, "EmpList");
        }

        private static void deleteEmployee(int recId)
        {
            foreach (DataRow row in disconnectedObj.Tables[0].Rows)
            {
                if (row[0].ToString() == recId.ToString())
                {
                    row.Delete();
                    break;
                }
            }
            ada.Update(disconnectedObj, "EmpList");
        }

        
        private static void insertEmployee(string name, string address, int salary, int deptId)
        {
            DataRow newRow = disconnectedObj.Tables[0].NewRow();
            newRow[1] = name;
            newRow[2] = address;
            newRow[3] = salary;
            newRow[4] = deptId;

            disconnectedObj.Tables[0].Rows.Add(newRow);
            ada.Update(disconnectedObj, "EmpList");
        }

        static void DisplayEmployeesofDept(string deptName)
        {
            int deptId = 0;
            foreach (DataRow row in disconnectedObj.Tables["DeptList"].Rows)
            {
                if (row["DeptName"].ToString() == deptName)
                {
                    deptId = (int)row["DeptId"];
                    break;
                }
            }
            foreach (DataRow row in disconnectedObj.Tables["EmpList"].Rows)
            {
                if (row[4].ToString() == deptId.ToString())
                {
                    Console.WriteLine(row["EmpName"]);
                }
            }

        }
    }
}
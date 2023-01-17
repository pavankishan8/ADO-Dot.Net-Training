using System;
using System.Data;
using System.Data.SqlClient;

namespace ADOConsoleApp
{
    static class ConnectedDemo
    {
        const string STRCONNECTION = "Data Source=192.168.171.36;Initial Catalog=3342;Integrated Security=True";

        const string STRQUERY = "SELECT * FROM tblEmployee";
        const string STRFIND = "SELECT * FROM tblEmployee WHERE EmpName = @name";
        const string STRINSERT = "INSERT INTO tblEmployee VALUES( @name, @address, @salary, @deptId)";
        //const string STRINPUTINSERT = "INSERT INTO tblEmployee VALUES( @name, @address, @salary, @deptId)";
        const string STRDEPTTABLE = "SELECT * from tblDept";
        const string STRINSERTPROC = "INSERTEMPLOYEE";

        static void Main(string[] args)
        {
            //readingdata();
            //displayAsTable();
            //displayDetails("Pavan R");
            //displayDetailsUsingParameters("Pavan R");
            //AddNewRecord("Sachin", "Mysore", 65000, 6);
            //AddNewRecordFromInputs();
            addNewRecordUsingStoredProc("Rajaiah", "Mangalore", 99000, 3);
            displayAsTable();
        }

        private static void addNewRecordUsingStoredProc(string name, string address, int salary, int deptId)
        {
            int empId = 0;
            SqlConnection sqlCon = new SqlConnection(STRCONNECTION);
            SqlCommand sqlCmd = new SqlCommand(STRINSERTPROC, sqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@empname", name);
            sqlCmd.Parameters.AddWithValue("@empAddress", address);
            sqlCmd.Parameters.AddWithValue("@empSalary", salary);
            sqlCmd.Parameters.AddWithValue("@deptId", deptId);
            sqlCmd.Parameters.AddWithValue("@empId", empId);
            sqlCmd.Parameters[4].Direction = ParameterDirection.Output;

            try
            {
                sqlCon.Open();
                sqlCmd.ExecuteNonQuery();
                empId = Convert.ToInt32(sqlCmd.Parameters[4].Value);
                Console.WriteLine("The EmpID of the newly added Employee is " + empId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }

        }

        private static void AddNewRecordFromInputs()
        {
            Console.WriteLine("Enter the Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the Address:");
            string address = Console.ReadLine();
            Console.WriteLine("Enter the Salary:");
            int salary = int.Parse(Console.ReadLine());
            Console.WriteLine("Select the Department:");
            deptIdDisplayer();
            int deptId = int.Parse(Console.ReadLine());

            AddNewRecord(name, address, salary, deptId);

        }

        private static void deptIdDisplayer()
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRDEPTTABLE, con);

            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["deptName"]}  ({reader[0]})");
                }
                Console.WriteLine();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private static void AddNewRecord(string name, string address, int salary, int deptId)
        {
            SqlConnection sqlCon = new SqlConnection(STRCONNECTION);
            SqlCommand sqlcmd = new SqlCommand(STRINSERT, sqlCon);

            
            sqlcmd.Parameters.AddWithValue("@name", name);
            sqlcmd.Parameters.AddWithValue("@address", address);
            sqlcmd.Parameters.AddWithValue("@salary", salary);
            sqlcmd.Parameters.AddWithValue("@deptId", deptId);

            try
            {
                sqlCon.Open();
                var rowsAffected = sqlcmd.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    throw new Exception("Failed to add the record to the database");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private static void displayDetailsUsingParameters(string name)
        {
            SqlCommand cmd = new SqlCommand(STRFIND, new SqlConnection(STRCONNECTION));

            try
            {
                cmd.Parameters.AddWithValue("@name",name);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();

                while(reader.Read())
                    {
                    Console.WriteLine($"EmpName: {reader[1]}\nEmpAddress: {reader[2]}\nEmpSalary: {reader[3]:c}\n");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
                {
                cmd.Connection.Close();
            }
        }

        private static void displayDetails(string name)
        {
            string query = $"SELECT * FROM tblEmployee where EmpName like '%{name}%'";
                SqlConnection con = new SqlConnection(STRCONNECTION);
                SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"EmpName: {reader[1]}\nEmpAddress: {reader[2]}\nEmpSalary: {reader[3]:c}\n");
                }
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                 con.Close();
            }
        }

        public static DataTable GetAllRecords()
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRQUERY, con);

            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                DataTable table = new DataTable("EmpRecords");
                table.Load(reader);
                return table;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private static void displayAsTable()
        {
            try
            {
                var table = ConnectedDemo.GetAllRecords();
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine($"EmpId: {row[0]}\nEmpName: {row[1]}\nEmpAddress: {row[2]}\nEmpSalary: {row[3]}\n");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private static void readingdata()
        {
            SqlConnection sqlCon = new SqlConnection();
            sqlCon.ConnectionString = STRCONNECTION;

            SqlCommand sqlCommand = sqlCon.CreateCommand();
            sqlCommand.CommandText = STRQUERY;

            try
            {
                sqlCon.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["EmpName"]} from {reader[2]}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if(sqlCon.State == System.Data.ConnectionState.Open)
                    sqlCon.Close();
            }
        }
    }
}

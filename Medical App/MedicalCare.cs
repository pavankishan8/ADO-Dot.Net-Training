using SampleDataAccessApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using USMedicalCare.Entities;

namespace USMedicalCare
{
    namespace Entities
    {
        class Patient
        {
            public int PatId { get; set; }
            public string PatName { get; set; }
            public string PatAddress { get; set; }
            public string PatSubs { get; set; }
            public int DocId { get; set; }
        }

        class Doctors
        {
            public int DocId { get; set; }
            public string DocName { get; set; }
        }
    }

    namespace DaLayer
    {
        interface IDataAccessComponent
        {
            void AddNewPatient(Patient pat);
            void UpdatePatient(Patient pat);
            void DeletePatient(int id);
            List<Patient> GetAllPatients();
            List<Doctors> GetAllDoctors();
            List<Patient> GetSinglePatient(int id);
        }

        class DataComponent : IDataAccessComponent
        {
            private string strCon = string.Empty;

            #region AllStatements
            const string STRCONNECTION = "Data Source=192.168.171.36;Initial Catalog=3342;Integrated Security=True";
            const string STRINSERT = "InsertPatient";
            const string STRUPDATE = "Update tblPatients Set PatName = @patName, PatAddress = @patAddress, PatSubs = @patSubs WHERE PatId = @patId";
            const string STRALL = "SELECT * FROM TBLPATIENTS";
            const string STRONEP = "SelectSinglePatient";
            const string STRALLDOCTORS = "SELECT * FROM TBLDOCTOR";
            const string STRDELETE = "DELETE FROM TBLPATIENTS WHERE PATID = @id";
            #endregion

            #region HELPERS
            private void NonQueryExecute(string query, SqlParameter[] parameters, CommandType type = CommandType.Text)
            {
                SqlConnection con = new SqlConnection(strCon);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = type;
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            private DataTable GetRecords(string query, SqlParameter[] parameters, CommandType type = CommandType.Text)
            {
                SqlConnection con = new SqlConnection(strCon);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = type;
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }
                try
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    DataTable table = new DataTable("Records");
                    table.Load(reader);
                    return table;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            #endregion

            public DataComponent(string connectionString)
            {
                strCon = connectionString;
            }

            #region IDataAccessComponentImpl
            public void AddNewPatient(Patient pat)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@patName", pat.PatName));
                parameters.Add(new SqlParameter("@patAddress", pat.PatAddress));
                parameters.Add(new SqlParameter("@patSubs", pat.PatSubs));
                parameters.Add(new SqlParameter("@docId", pat.DocId));
                parameters.Add(new SqlParameter("@patId", pat.PatId));

                try
                {
                    NonQueryExecute(STRINSERT, parameters.ToArray(), CommandType.StoredProcedure);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Patient added Successfully");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Utilities.Prompt("Press Enter to Clear the Screen");
                    Console.Clear();
                    UILayer.MainProgram.Menu();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            public void DeletePatient(int id)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@id", id));

                try
                {
                    NonQueryExecute(STRDELETE, parameters.ToArray());
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Patient deleted Successfully");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Utilities.Prompt("Press Enter to Clear the Screen");
                    Console.Clear();
                    UILayer.MainProgram.Menu();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public List<Doctors> GetAllDoctors()
            {
                var table = GetRecords(STRALLDOCTORS, null);
                List<Doctors> doctors = new List<Doctors>();
                foreach (DataRow row in table.Rows)
                {
                    Doctors docs = new Doctors
                    {
                        DocId = Convert.ToInt32(row[0]),
                        DocName = row[1].ToString()
                    };
                    doctors.Add(docs);
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("List of Doctors\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                return doctors;

            }

            public List<Patient> GetAllPatients()
            {
                var table = GetRecords(STRALL, null);
                List<Patient> patlist = new List<Patient>();
                foreach (DataRow row in table.Rows)
                {   
                    Patient pat = new Patient
                    {
                        PatId = (int)row[0],
                        PatName = row[1].ToString(),
                        PatAddress = row[2].ToString(),
                        PatSubs = row[3].ToString(),
                        DocId = (int)row[4]
                    };
                    patlist.Add(pat);
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nList of Patients\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                return patlist;

            }

            public List<Patient> GetSinglePatient(int id)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@patId", id));
                var table = GetRecords(STRONEP,parameters.ToArray(), CommandType.StoredProcedure);

                List<Patient> patlist = new List<Patient>();
                foreach (DataRow row in table.Rows)
                {

                    Patient pat = new Patient
                    {
                        PatId = (int)row[0],
                        PatName = row[1].ToString(),
                        PatAddress = row[2].ToString(),
                        PatSubs = row[3].ToString(),
                        DocId = (int)row[4]
                    };
                    patlist.Add(pat);
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nPatient Details\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                return patlist;

            }
            public void UpdatePatient(Patient pat)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@patName", pat.PatName));
                parameters.Add(new SqlParameter("@patAddress", pat.PatAddress));
                parameters.Add(new SqlParameter("@patSubs", pat.PatSubs));
                parameters.Add(new SqlParameter("@patId", pat.PatId));

                try
                {
                    NonQueryExecute(STRUPDATE, parameters.ToArray());
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Patient Details updated Successfully");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Utilities.Prompt("Press Enter to Clear the Screen");
                    Console.Clear();
                    UILayer.MainProgram.Menu();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            #endregion 
        }
    }

    namespace UILayer
    {
        using SampleDataAccessApp;
        using System.Configuration;
        using USMedicalCare.DaLayer;

        class MainProgram
        {
            static IDataAccessComponent component = null;
            static string connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;

            public static void Menu()
            {
                Console.WriteLine("--------------------------------------------Medical App------------------------------------------------\n");
                Console.WriteLine("Add New Patient------------------------------>PRESS 1<--------------------------------------------------");
                Console.WriteLine("Update Patient------------------------------->PRESS 2<--------------------------------------------------");
                Console.WriteLine("Delete Patient------------------------------->PRESS 3<--------------------------------------------------");
                Console.WriteLine("Show All Patients---------------------------->PRESS 4<--------------------------------------------------");
                Console.WriteLine("Show Single Patient-------------------------->PRESS 5<--------------------------------------------------");
                Console.WriteLine("Show All Doctors----------------------------->PRESS 6<--------------------------------------------------");
                Console.WriteLine("Exit----------------------------------------->PRESS 7<--------------------------------------------------");

                component = new DataComponent(connectionString);

            retry:
                int choice = Utilities.GetNumber("\nEnter your Choice:");

                if (choice == 1)
                {
                    component.AddNewPatient(new Patient
                    {
                        PatName = Utilities.Prompt("Enter the Patient Name:"),
                        PatAddress = Utilities.Prompt("Enter the Patient Address:"),
                        PatSubs = Utilities.Prompt("Enter Insurance Type: Private Insurance or Public Insurance"),
                        DocId = Utilities.GetNumber("Enter the DocId:")
                    });
                }

                else if (choice == 2)
                {
                    component.UpdatePatient(new Patient
                    {
                        PatId = Utilities.GetNumber("Enter the Patient ID:"),
                        PatName = Utilities.Prompt("Enter the Updated Name:"),
                        PatAddress = Utilities.Prompt("Enter the Updated Address:"),
                        PatSubs = Utilities.Prompt("Enter the Updated Subscription Type:")

                    });
                }

                else if (choice == 3)
                {
                    int PatId = Utilities.GetNumber("Enter the Patient ID:");
                    component.DeletePatient(PatId);

                    var data = component.GetAllDoctors();
                    foreach (var doc in data)
                        Console.WriteLine(doc.DocName);
                }

                else if (choice == 4)
                {
                    var patdata = component.GetAllPatients();
                    foreach (var pat in patdata)
                        Console.WriteLine(pat.PatName);
                    Utilities.Prompt("\nPress Enter to Clear the Screen");
                    Console.Clear();
                    UILayer.MainProgram.Menu();
                }

                else if (choice == 5)
                {
                    int id = Utilities.GetNumber("Enter the Patient ID:");
                    var patdata = component.GetSinglePatient(id);
                    foreach (var pat in patdata)
                        Console.WriteLine(pat.PatName);
                    Utilities.Prompt("\nPress Enter to Clear the Screen");
                    Console.Clear();
                    UILayer.MainProgram.Menu();
                }

                else if (choice == 6)
                {
                    var docdata = component.GetAllDoctors();
                    foreach (var doc in docdata)
                        Console.WriteLine(doc.DocName);
                    Utilities.Prompt("\nPress Enter to Clear the Screen");
                    Console.Clear();
                    UILayer.MainProgram.Menu();
                }

                else if (choice == 7)
                {
                    Console.WriteLine("Thank You, See you again!");
                    Console.Clear();
                }

                else { goto retry; }
            }
            static void Main(string[] args)
            {
                Menu();
            }

        }
    }
}




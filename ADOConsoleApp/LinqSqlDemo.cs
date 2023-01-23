using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOConsoleApp
{
    class LinqSqlDemo
    {
        static void Main(string[] args)
        {
            //readingRecords();
            //addRecord();
            updateRecord(1209, "Ishan", "Delhi", 100000, 5);

        }

        private static void updateRecord(int id, string name, string address, int salary, int deptId)
        {
            var context = new DataClasses1DataContext();
            var found = (from emp in context.tblEmployees
                         where emp.EmpId == id
                         select emp).First();
            found.EmpName = name;
            found.EmpAddress = address;
            found.EmpSalary = salary;
            found.DeptId = deptId;
            context.SubmitChanges();
        }

        private static void addRecord()
        {
            var rec = new tblEmployee
            {
                EmpName = "Rohit",
                EmpAddress = "mumbai",
                EmpSalary = 5000,
                DeptId = 3
,
            };

            var context = new DataClasses1DataContext();
            context.tblEmployees.InsertOnSubmit(rec);
            context.SubmitChanges();
        }

        private static void readingRecords()
        {
            var context = new DataClasses1DataContext();
            var empList = from emp in context.tblEmployees
                          select emp;
            foreach (var emp in empList)
            {
                Console.WriteLine(emp.EmpName + " belongs to " + emp.tblDept.DeptName);
            }
        }
    }
}

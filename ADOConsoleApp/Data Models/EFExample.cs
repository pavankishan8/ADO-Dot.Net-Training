using SampleDataAccessApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADOConsoleApp.Data_Models
{
    interface IDataComponent
    {
        List<tblEmployee> GetAllEmployees();
        List<tblDept> GetAllDepts();
        void AddNewEmployee(tblEmployee emp);
        void UpdateEmployee(tblEmployee emp);
        void DeleteEmployee(int id);
    }

    class DataComponentImpl : IDataComponent
    {
        public void AddNewEmployee(tblEmployee emp)
        {
            var context = new Entities();
            context.tblEmployees.Add(emp);
            context.SaveChanges();
        }

        public void DeleteEmployee(int id)
        {
            var context = new Entities();
            var found = context.tblEmployees.FirstOrDefault((e) => e.EmpId == id);
            if (found == null)
                throw new Exception("Employee not found to delete");
            context.tblEmployees.Remove(found);
            context.SaveChanges();

        }

        public List<tblDept> GetAllDepts()
        {
            var context = new Entities();
            List<tblDept> depts = context.tblDepts.ToList();
            return depts;

        }

        public List<tblEmployee> GetAllEmployees()
        {
            var context = new Entities();
            var empList = context.tblEmployees.ToList();
            return empList;

        }

        public void UpdateEmployee(tblEmployee emp)
        {
            var context = new Entities();
            var foundEmp = (from rec in context.tblEmployees
                            where rec.EmpId == emp.EmpId
                            select rec).FirstOrDefault();
            if (foundEmp == null)
            {
                throw new Exception("Employee not found!!!");
            }

            foundEmp.EmpName = emp.EmpName;
            foundEmp.EmpAddress = emp.EmpAddress;
            foundEmp.EmpSalary = emp.EmpSalary;
            foundEmp.DeptId = emp.DeptId;

            context.SaveChanges();

        }
    }

    class UI
    {
        static IDataComponent component = new DataComponentImpl();

        static void Main(string[] args)
        {
            //displayAllDepts();
            //displayAllEmployees();
            //addingEmployee("Virat", 50000,"Ranchi", 3);
            //updatingEmployee(1209,"Virat","Bangalore", 100000, 5);
            deletingEmployee(1208);
        }

        private static void deletingEmployee(int empId)
        {
            int Id = Utilities.GetNumber("Enter the Employee ID:");
            component.DeleteEmployee(Id);
        }

        private static void updatingEmployee(int id, string name, string address, int salary, int dept)
        {
            component.UpdateEmployee(new tblEmployee {

                EmpId = id,
                DeptId = dept,
                EmpAddress = address,
                EmpName = name,
                EmpSalary = salary

            });
        }

        private static void addingEmployee(string name, int salary, string address, int deptId)
        {
            component.AddNewEmployee(new tblEmployee
            {
                EmpName = name,
                EmpSalary = salary,
                EmpAddress = address,
                DeptId = deptId

            });

        }

        private static void displayAllEmployees()
        {
            List<tblEmployee> empList = component.GetAllEmployees();

            foreach (var emp in empList)
                Console.WriteLine($"{emp.EmpName} works in {emp.tblDept.DeptName} department");
        }

        private static void displayAllDepts()
        {
            List<tblDept> depts = component.GetAllDepts();

            foreach(var dept in depts)
                Console.WriteLine(dept.DeptName);
        }
    }
}
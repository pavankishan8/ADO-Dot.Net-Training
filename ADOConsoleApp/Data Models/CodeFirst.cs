using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace ADOConsoleApp.Data_Models
{
   public  class Machine
    {
        public int MachineID { get; set; }
        public string MachineBrand { get; set; }
        public int MachinePrice { get; set; }
        public int TypeID { get; set; }
        public Type Type { get; set; }
    }

    public class Type
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public int TypePrice { get; set; }
        public ICollection<Machine> Machines { get; set; }
    }

    public partial class MachineDb : DbContext
    {
        public MachineDb()
            : base("name=MachineDB")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public DbSet<Machine> Machines { get; set; }
        public DbSet<Type> Types { get; set; }
    }

    class CodeFirstMain
    {
        static void Main(string[] args)
        {
            addingMachine();
            //getAllMachines(2);
            //update, delete, findByName
        }

        private static void getAllMachines(int TypeId)
        {
            var context = new MachineDb();
            var Type = context.Types.Include(d => d.Machines).FirstOrDefault((d) => d.TypeID == TypeId);
            if (Type.Machines != null)
            {
                foreach (var machine in Type.Machines)
                {
                    Console.WriteLine(machine.MachineBrand);
                }
            }
        }

        private static void addingMachine()
        {
            var context = new MachineDb();

            context.Types.Add(new Type
            {
                TypeID = 2,
                TypeName = "Mouse"
            });
            context.SaveChanges();
            context.Machines.Add(new Machine
            {
                MachineID = 4,
                TypeID = 2,
                MachinePrice = 145,
                MachineBrand = "HP"
            });
            context.SaveChanges();
        }
    }

}

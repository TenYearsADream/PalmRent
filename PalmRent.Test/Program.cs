using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalmRent.Service;

namespace PalmRent.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //codefirst生成数据库
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                ctx.Database.Delete();
                ctx.Database.Create();
            }

            Console.WriteLine("ok");
            Console.ReadKey();
        }
    }
}

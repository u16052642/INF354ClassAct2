using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassAct2
{
    public class Class1
    {
        public Class1()
        {

        }

        public void blabla()
        {
            using (var context = new HardwareDBEntities1())
            {
                var results = context.lginvoices.Include("lgemployee").ToList();
            }
        }
    }
}
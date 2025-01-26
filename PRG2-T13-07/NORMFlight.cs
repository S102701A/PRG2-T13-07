using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10270132
// Student Name : Muhammad Hafizuddin bin Norrudin
// Partner Name : Daymas Shield Koh Yee Sing
//=========================================================

namespace PRG2_T13_07
{
    internal class NORMFlight: Flight
    {
        public NORMFlight()
        {

        }

        public NORMFlight(string fN, string ori, string dest, DateTime eT, string stat): base(fN,ori,dest,eT,stat)
        {

        }

        public override double CalculateFees()
        {
            return base.CalculateFees();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

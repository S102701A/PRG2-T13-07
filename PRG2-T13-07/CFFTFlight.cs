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

    internal class CFFTFlight: Flight
    {
        private double requestFee;
        public double RequestFee { get; set; }

        public CFFTFlight()
        {

        }

        public CFFTFlight(string fN, string ori, string dest, DateTime eT, string stat, BoardingGate bg, double rF): base(fN, ori, dest, eT, stat, bg)
        {
            RequestFee = rF;
        }

        public override double CalculateFees()
        {
            return base.CalculateFees()+150.0;
        }

        public override string ToString()
        {
            return base.ToString()+"Special Code: CFFT";
        }
    }
}

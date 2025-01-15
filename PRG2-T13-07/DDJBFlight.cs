﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_07
{
    internal class DDJBFlight: Flight
    {
        private double requestFee;

        public double RequestFee { get; set; }

        public DDJBFlight()
        {

        }

        public DDJBFlight(string fN, string ori, string dest, DateTime eT, string stat, double rF) : base(fN, ori, dest, eT, stat)
        {
            RequestFee = rF;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_07
{
    internal class Flight
    {
        private string flightNumber;
        private string origin;
        private string destination;
        private DateTime expectedTime;
        private string status;

        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public Flight()
        {

        }

        public Flight(string fN, string ori, string dest, DateTime eT, string stat)
        {
            FlightNumber = fN;
            Origin = ori;
            Destination = dest;
            ExpectedTime = eT;
            Status = stat;
        }


    }
}

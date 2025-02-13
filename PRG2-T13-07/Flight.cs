﻿using System;
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
    internal class Flight: IComparable<Flight>
    {
        private string flightNumber;
        private string origin;
        private string destination;
        private DateTime expectedTime;
        private string status;
        private BoardingGate boardingGate;
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public BoardingGate BoardingGate { get; set; }

        public Flight()
        {

        }

        public Flight(string fN, string ori, string dest, DateTime eT, string stat, BoardingGate bg)
        {
            FlightNumber = fN;
            Origin = ori;
            Destination = dest;
            ExpectedTime = eT;
            Status = stat;
            BoardingGate = bg;
        }

        public virtual double CalculateFees()
        {
            if (Origin == "SIN")
            {
                return 500.0;
            }
            else
                return 800.0;
        }
        public int CompareTo(Flight other)
        {
            return this.ExpectedTime.CompareTo(other.ExpectedTime);
        }

        public override string ToString()
        {
            if (Status==null)
                Status = "Scheduled";
            return "FlightNumber: "+FlightNumber+"\nOrigin: "+Origin + "\nDestination: "+Destination+"\nExpectedTime: "+ExpectedTime+"\nStatus: "+Status;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_07
{
    internal class Airline
    {
        private string name;
        private string code;
        private Dictionary<string, Flight> flights;

        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }

        public Airline()
        {

        }

        public Airline(string n,string c,Dictionary<string, Flight> f)
        {
            Name = n;
            Code = c;
            Flights = f;
        }
         public bool AddAirline (Airline airline)
        {
            if (airline == null)
            {
                return false;
            }
            airline.Add(airline.Code, airline);
            return true;
        }
        public AddBoardingGate 
    }
}

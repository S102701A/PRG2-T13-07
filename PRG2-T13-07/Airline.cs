using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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

        public bool AddFlight(Flight flight)
        {
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                return false;
            }

            Flights.Add(flight.FlightNumber, flight);
            return true;
        }

        public double CalculateFees()
        {

            double totalFees = 0;
            double discount = 0;

         
            foreach (var flight in Flights.Values)
            {
                totalFees += flight.CalculateFees();  
                                                      
                totalFees += 300;

                
                if (flight is DDJBFlight)
                {
                    totalFees += 300; 
                }
                else if (flight is CFFTFlight)
                {
                    totalFees += 150; 
                }
                else if (flight is LWTTFlight)
                {
                    totalFees += 500; 
                }
            }

            
            if (Flights.Count >= 3)
            {
                discount += (Flights.Count / 3) * 350;
            }

            if (Flights.Count > 5)
            {
                discount += totalFees * 0.03; 
            }

            foreach (var flight in Flights.Values)
            {
                if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour > 21)
                {
                    discount += 110;
                }
            }

            foreach (var flight in Flights.Values)
            {
                if (flight.Origin == "DXB" || flight.Origin == "BKK" || flight.Origin == "NRT")
                {
                    discount += 25;
                }
            }

            foreach (var flight in Flights.Values)
            {
                if (flight is NORMFlight)
                {
                    discount += 50; 
                }
            }

            totalFees -= discount;
            return totalFees;

        }

        public bool RemoveFlight(string flightNumber)
        {
            if (Flights.ContainsKey(flightNumber))
            {
                Flights.Remove(flightNumber); 
                return true; 
            }

            return false; 
        }

        public override string ToString()
        {
            string flightDetails = "";
            foreach (var flight in Flights.Values)
            {
                flightDetails += flight.ToString() + "\n";
            }

            return $"Airline: {Name} ({Code})\nNumber of Flights: {Flights.Count}\nFlight Details:\n{flightDetails}";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

//==========================================================
// Student Number : S10270132
// Student Name : Muhammad Hafizuddin bin Norrudin
// Partner Name : Daymas Shield Koh Yee Sing
//=========================================================

namespace PRG2_T13_07
{
    internal class Terminal
    {
        private string terminalName;
        private Dictionary<string, Airline> airlines;
        private Dictionary<string, Flight> flights;
        private Dictionary<string, BoardingGate> boardingGates;
        private Dictionary<string, double> gateFees;

        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Dictionary<string, BoardingGate> BoardingGates { get; set; }
        private Dictionary<string, double> GateFees { get; set; }

        public Terminal()
        {

        }

        public Terminal(string tN, Dictionary<string, Airline> al, Dictionary<string, Flight> fl, Dictionary<string, BoardingGate> bG, Dictionary<string, double> gF)
        {
            TerminalName = tN;
            Airlines = al;
            Flights = fl;
            BoardingGates = bG;
            GateFees = gF;
        }
        public bool AddAirline (Airline airline)
        {
            if (Airlines.ContainsKey(airline.Code))
            {
                return false;
            }

            airlines.Add(airline.Code, airline);
            return true;
        }
        public bool AddBoardingGate(BoardingGate boardinggate)
        {
            if (BoardingGates.ContainsKey(boardinggate.GateName))
            { 
                return false; 
            }
            boardingGates.Add(boardinggate.GateName, boardinggate);
            return true;
        }
        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (var airline in Airlines.Values)
            {
                if (airline.Flights.ContainsKey(flight.FlightNumber))
                {
                    return airline;
                }
                
            }
            return null;
            
        }
        public void PrintAirlineFees()
        {
            foreach (var airline in airlines.Values)
            {
                Console.WriteLine($"Airline: {airline.Name}, Fees: {airline.CalculateFees()}");
            }
        }
        public override string ToString()
        {
            string airlineDetails = "";
            foreach (var airline in Airlines.Values)
            {
                airlineDetails += airline.ToString() + "\n";
            }

            string flightDetails = "";
            foreach (var flight in Flights.Values)
            {
                flightDetails += flight.ToString() + "\n";
            }

            string boardingGateDetails = "";
            foreach (var gate in BoardingGates.Values)
            {
                boardingGateDetails += $"Gate Name: {gate.GateName}, Supports CFFT: {gate.SupportsCFFT}, Supports DDJB: {gate.SupportsDDJB}, Supports LWTT: {gate.SupportsLWTT}\n";
            }

            return $"Terminal: {TerminalName}\n" +
                   $"Number of Airlines: {Airlines.Count}\n" +
                   $"Number of Flights: {Flights.Count}\n" +
                   $"Number of Boarding Gates: {BoardingGates.Count}\n\n" +
                   $"Airlines:\n{airlineDetails}\n" +
                   $"Flights:\n{flightDetails}\n" +
                   $"Boarding Gates:\n{boardingGateDetails}";
        }

    }
}

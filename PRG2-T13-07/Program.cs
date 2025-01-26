using Microsoft.VisualBasic;
using PRG2_T13_07;
using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

//==========================================================
// Student Number : S10270132
// Student Name : Muhammad Hafizuddin bin Norrudin
// Partner Name : Daymas Shield Koh Yee Sing
//=========================================================

class Program
{
    static void Main()
    {
        
    }

    private static Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
    private static Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
    private static Dictionary<string, Flight> flightsdictionary = new Dictionary<string, Flight>();
    private static Dictionary<string, string> specialRequestCodes = new Dictionary<string, string>();

    private static void LoadAirlines() // FEATURE 1.1
    {
        string[] lines = File.ReadAllLines("airlines.csv");
        for (int i = 1; i < lines.Length; i++) 
        {
            string[] details = lines[i].Split(',');
            string code = details[0]; 
            string name = details[1]; 

            Airline airline = new Airline(name, code, new Dictionary<string, Flight>());
            airlines.Add(code, airline);
        }

        Console.WriteLine("Airlines loaded successfully!");
    }
    private static void LoadBoardingGates() // FEATURE 1.2
    {
        string[] lines = File.ReadAllLines("boardinggates.csv");
        for (int i = 1; i < lines.Length; i++) 
        {
            string[] details = lines[i].Split(',');
            string gateName = details[0]; 
            bool supportsCFFT = bool.Parse(details[1]); 
            bool supportsDDJB = bool.Parse(details[2]);
            bool supportsLWTT = bool.Parse(details[3]);

            BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT, null);
            boardingGates.Add(gateName, gate);
        }

        Console.WriteLine("Boarding gates loaded successfully!");
    }

    static void Flight() // FEATURE 2
    {
        string[] csvLines = File.ReadAllLines("flights.csv");

        for (int i = 1; i < csvLines.Length; i++)
        {
            string line = csvLines[i];
            string[] columns = line.Split(','); 
            string flightnumber = columns[0];
            string origin = columns[1];    
            string destination = columns[2];     
            DateTime expectedtime = DateTime.Parse(columns[3]); 
            string specialrequestcode = columns[4];

            string status = null;

            Flight flight = new Flight(flightnumber, origin, destination, expectedtime, status);
            flightsdictionary[flightnumber] = flight;


            specialRequestCodes[flightnumber] = specialrequestcode;
        }
    }

    //Feature 3 
    static void ListAllFlights(Dictionary<string, Flight> flightsdictionary, Dictionary<string, string> airlines)
    {
        //feature 7 
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("Airline Code    Airline Name");
        foreach (var airline in airlines)
        {
            Console.WriteLine($"{airline.Key,-15}{airline.Value,-30}");
        }

        Console.Write("Enter Airline Code: ");
        string airlineCode = Console.ReadLine().ToUpper();

        if (!airlines.ContainsKey(airlineCode))
        {
            Console.WriteLine("Invalid Airline Code. Please try again.");
            return;
        }

        //feature 3
        Console.WriteLine("=============================================");
        Console.WriteLine($"List of Flights for Changi Airport Terminal");
        Console.WriteLine("=============================================");
        Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time");

        bool hasFlights = false;
        foreach (var flight in flightsdictionary.Values)
        {
            if (flight.FlightNumber.StartsWith(airlineCode)) 
            {
                hasFlights = true;
                Console.WriteLine(
                    $"{flight.FlightNumber, -15}" + 
                    $"{airlines[airlineCode], -20}" + 
                    $"{flight.Origin, -22}" + 
                    $"{flight.Destination, -22}" + 
                    $"{flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt")}" 
                );
            }
        }

        if (!hasFlights)
        {
            Console.WriteLine("No flights found for this airline.");
        }
    }

    private static void ListBoardingGates() // FEATURE 4 
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Gate Name",-15}{"DDJB",-20}{"CFFT",-20}{"LWTT"}");

        foreach (var gate in boardingGates.Values) 
        {
            Console.WriteLine($"{gate.GateName,-15}{gate.SupportsDDJB,-20}{gate.SupportsCFFT,-20}{gate.SupportsLWTT}");
        }

        Console.WriteLine(); 
    }

    
    static void AssignBoardingGate(Dictionary<string, Flight> flightsDictionary)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Assign a Boarding Gate to a Flight");
        Console.WriteLine("=============================================");
        Console.WriteLine("Enter flight number: ");
        string flightNumber = Console.ReadLine();
        Console.WriteLine("Enter Boarding Gate Name:");
        string gatename = Console.ReadLine();
        if (flightsdictionary.ContainsKey(flightNumber))
        {
            
            var flight = flightsdictionary[flightNumber];

            Console.WriteLine($"Flight Number: {flight.FlightNumber}");
            Console.WriteLine($"Origin: {flight.Origin}");
            Console.WriteLine($"Destination: {flight.Destination}");
            Console.WriteLine($"Expected Time: {flight.ExpectedTime}");


            if (boardingGates.ContainsKey(gatename))
            {
                var boardingGate = boardingGates[gatename];
                Console.WriteLine($"Boarding Gate Name: {boardingGate.GateName}");
                Console.WriteLine($"Supports DDJB: {boardingGate.SupportsDDJB}");
                Console.WriteLine($"Supports CFFT: {boardingGate.SupportsCFFT}");
                Console.WriteLine($"Supports LWTT: {boardingGate.SupportsLWTT}");

                boardingGate.Flight = flight;
                

                while (true)
                {
                    Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
                    string option = Console.ReadLine();
                    if (option=="Y")
                    {
                        Console.WriteLine("1.Delayed");
                        Console.WriteLine("2.Boarding");
                        Console.WriteLine("3.On Time");
                        Console.WriteLine("Please select the new status of the flight:");
                        int statusoption = Convert.ToInt32(Console.ReadLine());
                        if (statusoption==1)
                        {
                            flight.Status = "Delayed";
                        }
                        else if (statusoption==2)
                        {
                            flight.Status = "Boarding";
                        }
                        else if (statusoption==3)
                        {
                            flight.Status = "On Time";
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                    Console.WriteLine($"Flight {flightNumber} has been successfully assigned to Gate {gatename}!");
            }
            else
            {
                Console.WriteLine("Invalid Boarding Gate Name entered. Please try again.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Flight Number entered. Please try again.");
        }

        
    }
    static void CreateNewFlight(Dictionary<string, Flight> flightsDictionary) //feature 6 
    {
        while (true)
        {
            Console.WriteLine("Enter a new Flight number: ");
            string fN = Console.ReadLine();
            Console.WriteLine("Enter the origin of the flight");
            string ori = Console.ReadLine();
            Console.WriteLine("Enter the destination of the flight: ");
            string dest = Console.ReadLine();
            Console.WriteLine("Enter the Expected Departure/Arrival: ");
            string eT = Console.ReadLine();
            Console.WriteLine("Any special request code ?");
            string SRC = Console.ReadLine();
            Flight newFlight = new Flight(fN, ori, dest, eT, SRC);
            flightsDictionary.Add(fN, newFlight);
            string data = "fN" + "ori" + "dest" + "eT" + "SRC";
            using (StreamWriter sw = new StreamWriter("flights.csv", true))
            {
                sw.WriteLine(data);
            }
            Console.WriteLine("Data has been successfully added");
            Console.WriteLine("Would you like to add another Flight? ");
            string response = Console.ReadLine();
            if (response == "Y")
                return;
            else if (response == "N") break;
        }
    }

    static void DisplayScheduledFlights(List<Flight> flights)
    {

        flights.Sort();

        Console.WriteLine("=============================================");
        Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time     Status          Boarding Gate");

        foreach (var flight in flights)
        {
            Console.WriteLine(
                $"{flight.FlightNumber, -20}" + 
                $"{flight.Origin, -22}" +
                $"{flight.Destination, -22}" +
                $"{flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt"), -30}" +
                $"{flight.Status, -15}" +
                $"{flight.BoardingGate.GateName}" 
            );
        }
    }
}
    








       


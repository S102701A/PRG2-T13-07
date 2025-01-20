using Microsoft.VisualBasic;
using PRG2_T13_07;
using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;
//Feature 2 & 3 
class Program
{
    static void Main()
    {
        Flight();
        AssignBoardingGate();
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

    //feature 5 & 6 
    //Hi Hafiz, if you're reading this the features arent done yet, still have abit of changes to make, if you can identify and find out whats wrong please help
    //I have also added the boarding gate and flights file to the solution, so you dont have to add those two

    // I edited your feature 5 but i havent take a look on your feature 6. I havent really finished feature 5 left abit more only. i think you can do it. i alrdy put the comment there
    // refer to the sample output file cus got one part you never do is when they ask for the new status. yeah that part i havent do yet.
    //btw for your feature 2 i added a new dictionary for the specialrequestcode because specialrequestcode is not part of the flight. so i just put the status for flight null then in feature 5 we will
    //  have to ask for the status. i will maybe continue tonight after you do. for feature 6 right i think no need put while true first cus we will put it in the main method then we call feature 6 there while running the
    //  while true. i also change some of the naming conventions edy.
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

                // Assign the flight to the gate
                boardingGate.Flight = flight;

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
                    //HAVENT FINISH THIS PART 
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
    static void CreateNewFlight(Dictionary<string, Flight> flightsDictionary) //this is for feature 6, anything just change and lmk 
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

}


       


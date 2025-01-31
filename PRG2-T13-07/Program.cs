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

    private static Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
    private static Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
    private static Dictionary<string, Flight> flightsdictionary = new Dictionary<string, Flight>();
    private static Dictionary<string, string> specialRequestCodes = new Dictionary<string, string>();
    static void Main()
    {
        LoadAirlines();
        LoadBoardingGates();
        Flight();
        PrintBlankLines(4);
        while (true) // Loop until the user decides to exit
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("Welcome to Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("1.List All Flights");
            Console.WriteLine("2.List Boarding Gates");
            Console.WriteLine("3.Assign a Boarding Gate to a Flight");
            Console.WriteLine("4.Create Flight");
            Console.WriteLine("5.Display Airline Flights");
            Console.WriteLine("6.Modify Flight Details");
            Console.WriteLine("7.Display Flight Schedule");
            Console.WriteLine("8.Process Unassigned Flights");
            Console.WriteLine("Display Total Fee Per Airline");
            Console.WriteLine("0.Exit");

            PrintBlankLines(1);

            Console.WriteLine("Please select your option: ");

            try
            {
                // Attempt to parse the user's input
                int option = int.Parse(Console.ReadLine());

                if (option == 0)
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                else if (option == 1)
                {
                    ListAllFlights(flightsdictionary, airlines);
                    PrintBlankLines(4);
                }
                else if (option == 2)
                {
                    ListBoardingGates();
                    PrintBlankLines(4);
                }
                else if (option == 3)
                {
                    AssignBoardingGate(flightsdictionary,boardingGates);
                    PrintBlankLines(4);
                }
                else if (option == 4)
                {
                    CreateNewFlight(flightsdictionary);
                    PrintBlankLines(4);
                }
                else if (option == 5)
                {
                    ListAllFlightsFromSpecificAirlines(flightsdictionary, airlines.ToDictionary(a => a.Key, a => a.Value.Name));
                    PrintBlankLines(4);
                }
                else if (option == 6)
                {
                    ModifyFlightDetails(airlines, new Dictionary<string, string>());
                    PrintBlankLines(4);
                }
                else if (option == 7)
                {
                    DisplayScheduledFlights(flightsdictionary.Values.ToList());
                    PrintBlankLines(4);
                }
                else if (option == 8)
                {
                    ProcessUnassignedFlights(flightsdictionary, boardingGates);
                    PrintBlankLines(4);
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
            catch (FormatException) 
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }




    

    private static void PrintBlankLines(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine();
        }
    }


    private static void LoadAirlines() // FEATURE 1.1
    {
        Console.WriteLine("Loading Airlines...");
        string[] lines = File.ReadAllLines("airlines.csv");
        for (int i = 1; i < lines.Length; i++) 
        {
            string[] details = lines[i].Split(',');
            string name = details[0]; 
            string code = details[1]; 

            Airline airline = new Airline(name, code, new Dictionary<string, Flight>());
            airlines[code] = airline;
        }
        Console.WriteLine($"{lines.Length - 1} Airlines Loaded!");
    }
    private static void LoadBoardingGates() // FEATURE 1.2
    {
        Console.WriteLine("Loading Boarding Gates...");
        string[] lines = File.ReadAllLines("boardinggates.csv");
        for (int i = 1; i < lines.Length; i++) 
        {
            string[] details = lines[i].Split(',');
            string gateName = details[0];
            bool supportsDDJB = bool.Parse(details[1]);
            bool supportsCFFT = bool.Parse(details[2]); 
            bool supportsLWTT = bool.Parse(details[3]);

            BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT, null);
            boardingGates[gateName] = gate;
        }
        Console.WriteLine($"{lines.Length - 1} Boarding Gates Loaded!");
    }

    static void Flight() // FEATURE 2
    {
        Console.WriteLine("Loading Flights...");
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
            BoardingGate bG = null;

            Flight flight = new Flight(flightnumber, origin, destination, expectedtime, status, bG);
            flightsdictionary[flightnumber] = flight;
            specialRequestCodes[flightnumber] = specialrequestcode;
        }
        Console.WriteLine($"{csvLines.Length - 1} Flights Loaded!");
    }

    static void ListAllFlights(Dictionary<string, Flight> flightsdictionary, Dictionary<string, Airline> airlines) //FEATURE 3
    {
        Console.WriteLine("=============================================");
        Console.WriteLine($"List of Flights for Changi Airport Terminal");
        Console.WriteLine("=============================================");
        Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time");


        foreach (var flight in flightsdictionary.Values)
        {
            string airlineCode = flight.FlightNumber.Substring(0, 2);

            // Check if the airline exists in the dictionary
            
            string airlineName = airlines[airlineCode].Name; // Get the airline name
            Console.WriteLine(
                $"{flight.FlightNumber,-16}" +
                $"{airlineName,-23}" +
                $"{flight.Origin,-23}" +
                $"{flight.Destination,-23}" +
                $"{flight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
        }
    }

    private static void ListBoardingGates() //FEATURE 4 
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


    static void AssignBoardingGate(Dictionary<string, Flight> flightsdictionary, Dictionary<string, BoardingGate> boardingGates)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Assign a Boarding Gate to a Flight");
        Console.WriteLine("=============================================");

        while (true)
        {
            string flightNumber = "";
            bool validFlightNumber = false;
            while (!validFlightNumber)
            {
                try
                {
                    Console.WriteLine("Enter flight number: ");
                    flightNumber = Console.ReadLine();
                    if (flightsdictionary.ContainsKey(flightNumber))
                    {
                        validFlightNumber = true; // valid flight number, break loop
                    }
                    else
                    {
                        Console.WriteLine("Invalid Flight Number entered. Please try again.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid format. Please enter a valid flight number.");
                }
            }

            string gateName = "";
            bool validGateName = false;
            while (!validGateName)
            {
                try
                {
                    Console.WriteLine("Enter Boarding Gate Name: ");
                    gateName = Console.ReadLine();
                    if (boardingGates.ContainsKey(gateName))
                    {
                        validGateName = true; // valid gate name, break loop
                    }
                    else
                    {
                        Console.WriteLine("Invalid Boarding Gate Name entered. Please try again.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid format. Please enter a valid boarding gate name.");
                }
            }

            var flight = flightsdictionary[flightNumber];
            Console.WriteLine($"Flight Number: {flight.FlightNumber}");
            Console.WriteLine($"Origin: {flight.Origin}");
            Console.WriteLine($"Destination: {flight.Destination}");
            Console.WriteLine($"Expected Time: {flight.ExpectedTime}");

            var boardingGate = boardingGates[gateName];
            Console.WriteLine($"Boarding Gate Name: {boardingGate.GateName}");
            Console.WriteLine($"Supports DDJB: {boardingGate.SupportsDDJB}");
            Console.WriteLine($"Supports CFFT: {boardingGate.SupportsCFFT}");
            Console.WriteLine($"Supports LWTT: {boardingGate.SupportsLWTT}");

            boardingGate.Flight = flight;

            bool validStatusOption = false;
            while (true)
            {
                Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
                string option = Console.ReadLine().ToUpper();
                if (option == "Y")
                {
                    while (!validStatusOption)
                    {
                        Console.WriteLine("1. Delayed");
                        Console.WriteLine("2. Boarding");
                        Console.WriteLine("3. On Time");
                        Console.WriteLine("Please select the new status of the flight:");
                        try
                        {
                            int statusOption = Convert.ToInt32(Console.ReadLine());
                            if (statusOption == 1)
                            {
                                flight.Status = "Delayed";
                                validStatusOption = true;
                            }
                            else if (statusOption == 2)
                            {
                                flight.Status = "Boarding";
                                validStatusOption = true;
                            }
                            else if (statusOption == 3)
                            {
                                flight.Status = "On Time";
                                validStatusOption = true;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please try again.");
                            }
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid option. Please enter a number.");
                        }
                    }
                    break;
                }
                else if (option == "N")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                }
            }

            Console.WriteLine($"Flight {flightNumber} has been successfully assigned to Boarding Gate {gateName}!");
            break;
        }
    }

    static void CreateNewFlight(Dictionary<string, Flight> flightsdictionary) //FEATURE 6
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
            DateTime eT = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Any special request code ?");
            string SRC = Console.ReadLine();
            BoardingGate bG = null;
            Flight newFlight = new Flight(fN, ori, dest, eT, SRC,bG);
            flightsdictionary.Add(fN, newFlight);
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

    static void ListAllFlightsFromSpecificAirlines(Dictionary<string, Flight> flightsDictionary, Dictionary<string, string> airlines) //FEATURE 7 COMPLETED
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Airline Code",-15}{"Airline Name",-30}");

        
        foreach (var airline in airlines)
        {
            Console.WriteLine($"{airline.Key,-15}{airline.Value,-30}");
        }
        while (true)
        {
            Console.Write("\nEnter Airline Code: ");
            string airlineCode = Console.ReadLine().ToUpper();


            if (!airlines.ContainsKey(airlineCode))
            {
                Console.WriteLine("Invalid Airline Code. Please try again.");
                continue;
            }

            else
            {
                Console.WriteLine("\n=============================================");
                Console.WriteLine($"List of Flights for {airlines[airlineCode]}");
                Console.WriteLine("=============================================");
                Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-30}{"Origin",-20}{"Destination",-20}{"Expected Departure/Arrival Time"}");

                bool hasFlights = false;


                foreach (var flight in flightsDictionary.Values)
                {

                    if (flight.FlightNumber.StartsWith(airlineCode))
                    {
                        hasFlights = true;
                        Console.WriteLine(
                            $"{flight.FlightNumber,-15}" +
                            $"{airlines[airlineCode],-30}" +
                            $"{flight.Origin,-20}" +
                            $"{flight.Destination,-20}" +
                            $"{flight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
                    }
                }
                break;
            }
        }
    }

    static void ModifyFlightDetails(Dictionary<string, Airline> airlinesDictionary, Dictionary<string, string> boardingGateAssignments)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Airline Code",-15}{"Airline Name"}");

        // Display available airlines
        foreach (var airlines in airlinesDictionary.Values)
        {
            Console.WriteLine($"{airlines.Code,-15}{airlines.Name}");
        }

        while (true)
        {
            Console.Write("\nEnter Airline Code: ");
            string airlineCode = Console.ReadLine().ToUpper();

            // Validate airline code
            if (!airlinesDictionary.ContainsKey(airlineCode))
            {
                Console.WriteLine("Invalid Airline Code. Please try again.");
                continue;
            }

            var airline = airlinesDictionary[airlineCode];

            Console.WriteLine($"\nList of Flights for {airline.Name}");
            Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-20}{"Origin",-20}{"Destination",-20}{"Expected Departure/Arrival Time"}");

            foreach (var flight in airline.Flights.Values)
            {
                Console.WriteLine(
                    $"{flight.FlightNumber,-15}" +
                    $"{airline.Name,-20}" +
                    $"{flight.Origin,-20}" +
                    $"{flight.Destination,-20}" +
                    $"{flight.ExpectedTime:dd/MM/yyyy h:mm:ss tt}");
            }



            Console.Write("\nChoose an existing Flight to modify or delete: ");
            while (true)
            {
                string flightNumber = Console.ReadLine().ToUpper();

                if (!airline.Flights.ContainsKey(flightNumber))
                {
                    Console.WriteLine("Invalid Flight Number. Please try again.");
                    continue;
                }

                var flightToModify = airline.Flights[flightNumber];
                while (true)
                {
                    Console.WriteLine("\n1. Modify Flight");
                    Console.WriteLine("2. Delete Flight");
                    while (true)
                    {
                        Console.Write("Choose an option: ");

                        string option = Console.ReadLine();

                        if (option == "1") // Modify Flight
                        {
                            Console.WriteLine("\n1. Modify Basic Information");
                            Console.WriteLine("2. Modify Status");
                            Console.WriteLine("3. Modify Special Request Code");
                            Console.WriteLine("4. Modify Boarding Gate");
                            Console.Write("Choose an option: ");
                            string modifyOption = Console.ReadLine();

                            if (modifyOption == "1") // Modify Basic Information
                            {
                                while (true)
                                {
                                    try
                                    {
                                        Console.Write("Enter new Origin: ");
                                        flightToModify.Origin = Console.ReadLine();
                                        break;
                                    }

                                    catch (FormatException)
                                    {
                                        Console.WriteLine("Invalid option. Please try again.");
                                        continue;
                                    }
                                }

                                while (true)
                                {
                                    try
                                    {
                                        Console.Write("Enter new Destination: ");
                                        flightToModify.Destination = Console.ReadLine();
                                        break;
                                    }

                                    catch
                                    {
                                        Console.WriteLine("Invalid option. Please try again.");
                                        continue;
                                    }
                                }

                                while (true)
                                {
                                    try
                                    {
                                        Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                                        flightToModify.ExpectedTime = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", null);
                                        break;
                                    }

                                    catch
                                    {
                                        Console.WriteLine("Invalid option. Please try again.");
                                        continue;
                                    }
                                }
                                Console.WriteLine("\nFlight updated!");
                            }
                            else if (modifyOption == "2") // Modify Status
                            {
                                while (true)
                                {
                                    try
                                    {
                                        Console.Write("Enter new Status (On Time, Delayed, Boarding): ");
                                        flightToModify.Status = Console.ReadLine();
                                        break;
                                    }

                                    catch (FormatException)
                                    {
                                        Console.WriteLine("Invalid option. Please try again.");
                                        continue;
                                    }
                                }
                                Console.WriteLine("\nFlight status updated!");
                            }
                            else if (modifyOption == "3") // Modify Special Request Code
                            {
                                while (true)
                                {
                                    try
                                    {
                                        Console.Write("Enter new Special Request Code (CFFT/DDJB/LWTT/None): ");
                                        specialRequestCodes[flightNumber] = Console.ReadLine();
                                        break;
                                    }

                                    catch (FormatException)
                                    {
                                        Console.WriteLine("Invalid option. Please try again.");
                                    }
                                }
                                Console.WriteLine("\nSpecial Request Code updated!");
                            }
                            else if (modifyOption == "4") // Modify Boarding Gate
                            {
                                while (true)
                                {
                                    try
                                    {
                                        Console.Write("Enter new Boarding Gate Name: ");
                                        string newGate = Console.ReadLine();
                                        boardingGateAssignments[flightNumber] = newGate;
                                        break;
                                    }

                                    catch (FormatException)
                                    {
                                        Console.WriteLine("Invalid option. Please try again.");
                                    }
                                }
                                Console.WriteLine("\nBoarding Gate updated!");

                            }
                            else
                            {
                                Console.WriteLine("Invalid option. Please try again.");
                                continue;
                            }

                            // Display updated flight details
                            Console.WriteLine("\nFlight Details:");
                            Console.WriteLine($"Flight Number: {flightToModify.FlightNumber}");
                            Console.WriteLine($"Airline Name: {airline.Name}");
                            Console.WriteLine($"Origin: {flightToModify.Origin}");
                            Console.WriteLine($"Destination: {flightToModify.Destination}");
                            Console.WriteLine($"Expected Departure/Arrival Time: {flightToModify.ExpectedTime:dd/MM/yyyy h:mm:ss tt}");
                            Console.WriteLine($"Status: {flightToModify.Status}");
                            Console.WriteLine($"Special Request Code: {specialRequestCodes[flightNumber]}");
                            Console.WriteLine($"Boarding Gate: {(boardingGateAssignments.ContainsKey(flightNumber) ? boardingGateAssignments[flightNumber] : "Unassigned")}");
                            break;
                        }
                        else if (option == "2") // Delete Flight
                        {
                            while (true)
                            {
                                Console.Write("Are you sure you want to delete this flight? (Y/N): ");
                                string confirmation = Console.ReadLine().ToUpper();

                                if (confirmation == "Y")
                                {
                                    airline.Flights.Remove(flightNumber);
                                    Console.WriteLine("\nFlight has been deleted successfully!");
                                    break;
                                }
                                else if (confirmation == "N")
                                {
                                    Console.WriteLine("\nFlight deletion canceled.");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option. Please try again.");
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid option. Please try again.");
                            continue;
                        }
                    }
                }
            }
        }
    }


    static void DisplayScheduledFlights(List<Flight> flights) //FEATURE 9
    {

        flights.Sort();

        Console.WriteLine("=============================================");
        Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time     Status          Boarding Gate");

        foreach (var flight in flights)
        {
            Console.WriteLine(
                $"{flight.FlightNumber,-20}" +
                $"{flight.Origin,-22}" +
                $"{flight.Destination,-22}" +
                $"{flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt"),-30}" +
                $"{flight.Status,-15}" +
                $"{flight.BoardingGate.GateName}"
            );
        }
    }

    //advanced feature B 
    static void DisplayTotalFeePerAirline(Dictionary<string, Flight> flightsdictionary, Dictionary<string, string> airlines, Dictionary<string, string> specialRequestCodes)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Total Fees per Airline for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");

        double totalFees = 0;
        double totalDiscounts = 0;

        var groupedFlights = new Dictionary<string, List<Flight>>();

        foreach (var flight in flightsdictionary.Values)
        {
            string airlineCode = flight.FlightNumber.Substring(0, 2);
            if (!groupedFlights.ContainsKey(airlineCode))
            {
                groupedFlights[airlineCode] = new List<Flight>();
            }
            groupedFlights[airlineCode].Add(flight);
        }

        foreach (var airlineFlights in groupedFlights)
        {
            string airlineCode = airlineFlights.Key;
            string airlineName = airlines.ContainsKey(airlineCode) 

            double airlineFees = 0;
            double airlineDiscounts = 0;

            Console.WriteLine($"Airline: {airlineName} ({airlineCode})");

            foreach (var flight in airlineFlights.Value)
            {
                double flightFee = 0;

                if (flight.Origin == "Singapore (SIN)")
                    flightFee += 800;
                if (flight.Destination == "Singapore (SIN)")
                    flightFee += 500;

                if (specialRequestCodes.ContainsKey(flight.FlightNumber))
                {
                    string specialCode = specialRequestCodes[flight.FlightNumber];
                    if (specialCode == "DDJB")
                        flightFee += 300;
                    else if (specialCode == "CFFT")
                        flightFee += 150;
                    else if (specialCode == "LWTT")
                        flightFee += 500;
                }

                flightFee += 300;

                Console.WriteLine($"Flight {flight.FlightNumber}: ${flightFee}");
                airlineFees += flightFee;
            }

            int flightCount = airlineFlights.Value.Count;
            airlineDiscounts += (flightCount / 3) * 350;
            if (flightCount > 5)
                airlineDiscounts += airlineFees * 0.03;

            Console.WriteLine($"Subtotal Fees: ${airlineFees}");
            Console.WriteLine($"Subtotal Discounts: -${airlineDiscounts}");
            Console.WriteLine($"Total Fees for {airlineName}: ${airlineFees - airlineDiscounts}");
            Console.WriteLine("---------------------------------------------");

            totalFees += airlineFees;
            totalDiscounts += airlineDiscounts;
        }

        double finalTotalFees = totalFees - totalDiscounts;
        double discountPercentage = (totalDiscounts / totalFees) * 100;

        Console.WriteLine("=============================================");
        Console.WriteLine("Summary for All Airlines");
        Console.WriteLine("=============================================");
        Console.WriteLine($"Total Fees (Before Discounts): ${totalFees}");
        Console.WriteLine($"Total Discounts: -${totalDiscounts}");
        Console.WriteLine($"Final Total Fees: ${finalTotalFees}");
        Console.WriteLine($"Discount Percentage: {discountPercentage:F2}%");
    }


    static void ProcessUnassignedFlights(Dictionary<string, Flight> flightsdictionary, Dictionary<string, BoardingGate> boardingGates)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Bulk Processing of Unassigned Flights to Boarding Gates");
        Console.WriteLine("=============================================");

        Queue<Flight> unassignedFlightsQueue = new Queue<Flight>();

        int totalUnassignedFlights = 0;
        foreach (var flight in flightsdictionary.Values)
        {
            if (flight.BoardingGate == null) 
            {
                unassignedFlightsQueue.Enqueue(flight);
                totalUnassignedFlights++;
            }
        }
        Console.WriteLine($"Total Unassigned Flights: {totalUnassignedFlights}");

        int totalUnassignedGates = 0;
        foreach (var gate in boardingGates.Values)
        {
            if (gate.Flight == null) 
            {
                totalUnassignedGates++;
            }
        }
        Console.WriteLine($"Total Unassigned Boarding Gates: {totalUnassignedGates}");

        int processedFlights = 0;
        int processedGates = 0;

        while (unassignedFlightsQueue.Count > 0)
        {
            Flight flight = unassignedFlightsQueue.Dequeue();
            BoardingGate assignedGate = null;

            if (!string.IsNullOrEmpty(flight.SpecialRequestCode))
            {
                foreach (var gate in boardingGates.Values)
                {
                    if (gate.Flight == null)
                    {
                        if ((flight.SpecialRequestCode == "CFFT" && gate.SupportsCFFT) ||
                            (flight.SpecialRequestCode == "DDJB" && gate.SupportsDDJB) ||
                            (flight.SpecialRequestCode == "LWTT" && gate.SupportsLWTT))
                        {
                            assignedGate = gate;
                            break;
                        }
                    }
                }
            }

            if (assignedGate == null) 
            {
                foreach (var gate in boardingGates.Values)
                {
                    if (gate.Flight == null)
                    {
                        assignedGate = gate;
                        break;
                    }
                }
            }

            if (assignedGate != null)
            {
                assignedGate.Flight = flight;
                flight.BoardingGate = assignedGate;
                processedFlights++;
                processedGates++;

                Console.WriteLine("\nAssigned Flight:");
                Console.WriteLine($"Flight Number: {flight.FlightNumber}");

                string airlineCode = flight.FlightNumber.Substring(0, 2);
                string airlineName = "Unknown Airline";
                if (airlines.ContainsKey(airlineCode))
                {
                    airlineName = airlines[airlineCode].Name;
                }

                Console.WriteLine($"Airline Name: {airlineName}");
                Console.WriteLine($"Origin: {flight.Origin}");
                Console.WriteLine($"Destination: {flight.Destination}");
                Console.WriteLine($"Expected Departure/Arrival Time: {flight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
                Console.WriteLine($"Special Request Code: {flight.SpecialRequestCode}");
                Console.WriteLine($"Assigned Boarding Gate: {assignedGate.GateName}");
            }
        }

        Console.WriteLine("\n=============================================");
        Console.WriteLine("Summary of Automated Assignments");
        Console.WriteLine("=============================================");
        Console.WriteLine($"Total Flights Processed and Assigned: {processedFlights}");
        Console.WriteLine($"Total Boarding Gates Processed and Assigned: {processedGates}");

        if (totalUnassignedFlights > 0)
        {
            double percentageFlightsAssigned = ((double)processedFlights / totalUnassignedFlights) * 100;
            Console.WriteLine($"Percentage of Flights Automatically Assigned: {percentageFlightsAssigned:F2}%");
        }
        else
        {
            Console.WriteLine("Percentage of Flights Automatically Assigned: 0.00%");
        }

        if (totalUnassignedGates > 0)
        {
            double percentageGatesAssigned = ((double)processedGates / totalUnassignedGates) * 100;
            Console.WriteLine($"Percentage of Boarding Gates Automatically Assigned: {percentageGatesAssigned:F2}%");
        }
        else
        {
            Console.WriteLine("Percentage of Boarding Gates Automatically Assigned: 0.00%");
        }
    }
}
    








       


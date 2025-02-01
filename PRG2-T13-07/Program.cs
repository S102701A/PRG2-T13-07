using Microsoft.VisualBasic;
using PRG2_T13_07;
using System;
using System.ComponentModel.Design;
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
            Console.WriteLine("9.Display Total Fee Per Airline");
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
                    AssignBoardingGate(flightsdictionary, boardingGates);
                    PrintBlankLines(4);
                }
                else if (option == 4)
                {
                    CreateNewFlight(flightsdictionary, airlines);
                    PrintBlankLines(4);
                }
                else if (option == 5)
                {
                    ListAllFlightsFromSpecificAirlines(flightsdictionary, airlines);
                    PrintBlankLines(4);
                }
                else if (option == 6)
                {
                    ModifyFlightDetails(airlines, flightsdictionary, new Dictionary<string, string>());
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
                else if (option == 9)
                {
                    DisplayTotalFeePerAirline(flightsdictionary, airlines, specialRequestCodes);
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
        BoardingGate nullgate = new BoardingGate("Unassigned", false, false, false, null);
        boardingGates[nullgate.GateName] = nullgate;
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
            string airlineCode = flightnumber.Substring(0, 2);
            Flight flight = new Flight(flightnumber, origin, destination, expectedtime, status, bG);
            flightsdictionary[flightnumber] = flight;
            specialRequestCodes[flightnumber] = specialrequestcode;
            airlines[airlineCode].Flights[flightnumber] = flight;
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

    static void CreateNewFlight(Dictionary<string, Flight> flightsdictionary, Dictionary<string, Airline> airlines) //FEATURE 6
    {
        while (true)
        {
            // Flight Number Validation
            string fN;
            while (true)
            {
                Console.WriteLine("Enter a new Flight number (format: XX 1234): ");
                fN = Console.ReadLine();
                if (fN.Length == 6 && Char.IsLetter(fN[0]) && Char.IsLetter(fN[1]) && fN[2] == ' ' && Char.IsDigit(fN[3]) && Char.IsDigit(fN[4]) && Char.IsDigit(fN[5]))
                {
                    string airlineCode = fN.Substring(0, 2); // Extract airline code
                    if (airlines.ContainsKey(airlineCode)) // Validate against airlines dictionary
                    {
                        break; // Valid flight number, exit the loop
                    }
                    else
                    {
                        Console.WriteLine("Invalid airline code. Please enter a valid flight number.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid flight number format. Please use the format 'XX 1234' (2 letters followed by a space and 4 digits).");
                }
            }

            // Origin Validation
            string origin = string.Empty;
            bool isValidOrigin = false;
            while (!isValidOrigin)
            {
                Console.WriteLine("Enter the origin of the flight: ");
                origin = Console.ReadLine();
                isValidOrigin = false;

                foreach (var flight in flightsdictionary.Values)
                {
                    if (origin == flight.Origin || origin == flight.Destination)
                    {
                        isValidOrigin = true;
                        break;
                    }
                }

                if (!isValidOrigin)
                {
                    Console.WriteLine("Invalid option. The origin does not match any available flight origin or destination. Please try again.");
                }
            }

            // Destination Validation (Similar to Origin)
            string destination = string.Empty;
            bool isValidDestination = false;
            while (!isValidDestination)
            {
                Console.WriteLine("Enter the destination of the flight: ");
                destination = Console.ReadLine();
                isValidDestination = false;

                foreach (var flight in flightsdictionary.Values)
                {
                    if (destination == flight.Origin || destination == flight.Destination)
                    {
                        isValidDestination = true;
                        break;
                    }
                }

                if (!isValidDestination)
                {
                    Console.WriteLine("Invalid option. The destination does not match any available flight origin or destination. Please try again.");
                }
            }

            DateTime eT = DateTime.Now;
            bool isValid = false;

            while (!isValid)
            {
                Console.WriteLine("Enter the Expected Departure/Arrival (format: h:mmtt): ");
                string timeInput = Console.ReadLine();

                if (DateTime.TryParse(timeInput, out eT))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("Invalid time format. Please enter in 'h:mmtt' format.");
                }
            }

            string SRC;
            while (true)
            {
                Console.WriteLine("Any special request code? (CFFT/DDJB/LWTT/None): ");
                SRC = Console.ReadLine();
                if (SRC == "CFFT" || SRC == "DDJB" || SRC == "LWTT" || SRC == "None")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid special request code. Please enter 'CFFT', 'DDJB', 'LWTT', or 'None'.");
                }
            }

            BoardingGate bG = null;

            specialRequestCodes[fN] = SRC;

            Flight newFlight = new Flight(fN, origin, destination, eT, null, bG);

            flightsdictionary.Add(fN, newFlight);

            string data = fN + "," + origin + "," + destination + "," + eT.ToString("hh:mm tt") + "," + SRC;
            using (StreamWriter sw = new StreamWriter("flights.csv", true))
            {
                sw.WriteLine(data);
            }

            Console.WriteLine("Data has been successfully added");

            Console.WriteLine("Would you like to add another Flight? (Y/N): ");
            string response = Console.ReadLine();
            if (response == "N" || response == "n")
            {
                break; // Exit the loop if user doesn't want to add more flights
            }
        }
    }



    static void ListAllFlightsFromSpecificAirlines(Dictionary<string, Flight> flightsdictionary, Dictionary<string, Airline> airlines) //FEATURE 7 COMPLETED
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Airline Code",-15}{"Airline Name",-30}");


        foreach (var airline in airlines)
        {
            Console.WriteLine($"{airline.Key,-15}{airline.Value.Name,-30}");
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
                var airline = airlines[airlineCode];

                Console.WriteLine("\n=============================================");
                Console.WriteLine($"List of Flights for {airline.Name}");
                Console.WriteLine("=============================================");
                Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-30}{"Origin",-20}{"Destination",-20}{"Expected Departure/Arrival Time"}");

                bool hasFlights = false;

                foreach (var flight in flightsdictionary.Values)
                {

                    if (flight.FlightNumber.StartsWith(airlineCode))
                    {
                        hasFlights = true;
                        Console.WriteLine(
                            $"{flight.FlightNumber,-15}" +
                            $"{airline.Name,-30}" +
                            $"{flight.Origin,-20}" +
                            $"{flight.Destination,-20}" +
                            $"{flight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
                    }
                }
                break;
            }
        }
    }

    static void ModifyFlightDetails(Dictionary<string, Airline> airlines, Dictionary<string, Flight> flightsdictionary, Dictionary<string, string> boardingGateAssignments)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Airline Code",-15}{"Airline Name"}");

        // Display available airlines
        foreach (var airline in airlines.Values)
        {
            Console.WriteLine($"{airline.Code,-15}{airline.Name}");
        }

        bool exitModifyorDelete = false;
        while (!exitModifyorDelete)
        {
            Console.Write("\nEnter Airline Code: ");
            string airlineCode = Console.ReadLine().ToUpper();

            // Validate airline code
            if (!airlines.ContainsKey(airlineCode))
            {
                Console.WriteLine("Invalid Airline Code. Please try again.");
                continue;
            }

            var airline = airlines[airlineCode];
            Console.WriteLine($"\nList of Flights for {airline.Name}");
            Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-30}{"Origin",-20}{"Destination",-20}{"Expected Departure/Arrival Time"}");

            bool hasFlights = false;

            foreach (var flight in flightsdictionary.Values)
            {
                if (flight.FlightNumber.StartsWith(airlineCode))
                {
                    hasFlights = true;
                    Console.WriteLine(
                        $"{flight.FlightNumber,-15}" +
                        $"{airline.Name,-30}" +
                        $"{flight.Origin,-20}" +
                        $"{flight.Destination,-20}" +
                        $"{flight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
                }
            }

            while (true)
            {
                Console.Write("\nChoose an existing Flight to modify or delete: ");
                string flightNumber = Console.ReadLine().Trim().ToUpper();

                if (!airline.Flights.ContainsKey(flightNumber))
                {
                    Console.WriteLine("Invalid Flight Number. Please try again.");
                    continue;
                }

                var flightToModify = airline.Flights[flightNumber];

                bool exitModify = false;  // Flag to track when to exit all loops

                while (true)
                {
                    Console.WriteLine("\n1. Modify Flight");
                    Console.WriteLine("2. Delete Flight");
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
                            bool exitModifyBasic = false;  // Flag to control the exit of the basic info modification loop

                            while (!exitModifyBasic)
                            {
                                try
                                {
                                    Console.Write("Enter new Origin: ");
                                    string origin = Console.ReadLine();

                                    bool isValid = false;
                                    foreach (var flight in flightsdictionary.Values)
                                    {
                                        if (origin == flight.Origin || origin == flight.Destination)
                                        {
                                            flightToModify.Origin = origin;
                                            isValid = true;
                                        }
                                    }

                                    if (!isValid)
                                    {
                                        Console.WriteLine("Invalid option. The origin does not match any available flight origin or destination. Please try again.");
                                    }
                                    else
                                    {
                                        exitModifyBasic = true;  // Exit the loop
                                        break;
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Invalid format. Please enter a valid origin.");
                                }
                            }

                            // Destination modification
                            bool exitModifyDestination = false;
                            while (!exitModifyDestination)
                            {
                                try
                                {
                                    Console.Write("Enter new Destination: ");
                                    string destination = Console.ReadLine();

                                    bool isValid = false;
                                    foreach (var flight in flightsdictionary.Values)
                                    {
                                        if (destination == flight.Origin || destination == flight.Destination)
                                        {
                                            flightToModify.Destination = destination;
                                            isValid = true;
                                            Console.WriteLine("Destination has been updated.");
                                            break;
                                        }
                                    }

                                    if (!isValid)
                                    {
                                        Console.WriteLine("Invalid option. The destination does not match any available flight's origin or destination. Please try again.");
                                    }
                                    else
                                    {
                                        exitModifyDestination = true; // Exit the loop
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Invalid option. Please try again.");
                                    continue;
                                }
                            }

                            // Expected Time modification
                            while (true)
                            {
                                Console.Write("Enter new Expected Departure/Arrival Time (dd/M/yyyy H:mm): ");
                                string userInput = Console.ReadLine();
                                try
                                {
                                    flightToModify.ExpectedTime = DateTime.ParseExact(userInput, "d/M/yyyy H:mm", null);
                                    break; // Exit the loop after successful input
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Invalid format. Please use (dd/MM/yyyy HH:mm) and try again.");
                                }
                            }
                            Console.WriteLine("Flight updated!");
                            Console.WriteLine(flightToModify.ToString());
                            Console.WriteLine("Special Request Code: " + specialRequestCodes[flightToModify.FlightNumber]);
                            if (flightToModify.BoardingGate == null)
                                Console.WriteLine("Boarding Gate: Unassigned");
                            else
                                Console.WriteLine("Boarding Gate: " + flightToModify.BoardingGate.GateName);
                            exitModifyorDelete = true;
                            exitModify = true;
                        }
                        else if (modifyOption == "2") // Modify Status
                        {
                            bool exitModifyStatus = false;
                            while (!exitModifyStatus)
                            {
                                try
                                {
                                    Console.Write("Enter new Status (On Time, Delayed, Boarding): ");
                                    string inputStatus = Console.ReadLine();
                                    if (inputStatus == "On Time" || inputStatus == "Delayed" || inputStatus == "Boarding")
                                    {
                                        flightToModify.Status = inputStatus;
                                        Console.WriteLine("The new status has been set.");
                                        exitModifyStatus = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input. Please enter 'On Time', 'Delayed', or 'Boarding'.");
                                        continue;
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Invalid option. Please try again.");
                                    continue;
                                }
                            }
                            Console.WriteLine("\nFlight status updated!");
                            flightToModify.ToString();
                            Console.WriteLine("Special Request Code: " + specialRequestCodes[flightToModify.FlightNumber]);
                            Console.WriteLine("Boarding Gate: " + flightToModify.BoardingGate.GateName);
                            exitModifyorDelete = true;
                            exitModify = true;
                        }
                        else if (modifyOption == "3") // Modify Special Request Code
                        {
                            bool exitModifyRequest = false;
                            while (!exitModifyRequest)
                            {
                                try
                                {
                                    Console.Write("Enter new Special Request Code (CFFT/DDJB/LWTT/None): ");
                                    string inputRequestCode = Console.ReadLine();
                                    if (inputRequestCode == "CFFT" || inputRequestCode == "DDJB" || inputRequestCode == "LWTT" || inputRequestCode == "None")
                                    {
                                        specialRequestCodes[flightNumber] = inputRequestCode;
                                        Console.WriteLine("The new Special Request Code has been set.");
                                        exitModifyRequest = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input. Please enter 'CFFT', 'DDJB', 'LWTT', or 'None'.");
                                        continue;
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Invalid option. Please try again.");
                                }
                            }
                            Console.WriteLine("\nSpecial Request Code updated!");
                            flightToModify.ToString();
                            Console.WriteLine("Special Request Code: " + specialRequestCodes[flightToModify.FlightNumber]);
                            Console.WriteLine("Boarding Gate: " + flightToModify.BoardingGate.GateName);
                            exitModifyorDelete = true;
                            exitModify = true;
                        }
                        else if (modifyOption == "4") // Modify Boarding Gate
                        {
                            bool exitModifyGate = false;
                            while (!exitModifyGate)
                            {
                                try
                                {
                                    Console.Write("Enter new Boarding Gate Name: ");
                                    string newGate = Console.ReadLine();
                                    bool gateExists = false;
                                    foreach (var gate in boardingGates)
                                    {
                                        if (newGate == gate.Key)
                                        {
                                            boardingGateAssignments[flightNumber] = newGate;
                                            Console.WriteLine("Boarding Gate assigned successfully.");
                                            gateExists = true;
                                            break;
                                        }
                                    }

                                    if (!gateExists)
                                    {
                                        Console.WriteLine("Invalid Boarding Gate. Please enter a valid gate name.");
                                        continue;
                                    }
                                    exitModifyGate = true;  // Exit the loop after assigning a valid gate
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Invalid option. Please try again.");
                                    continue;
                                }
                            }
                            Console.WriteLine("\nBoarding Gate updated!");
                            flightToModify.ToString();
                            Console.WriteLine("Special Request Code: " + specialRequestCodes[flightToModify.FlightNumber]);
                            Console.WriteLine("Boarding Gate: " + flightToModify.BoardingGate.GateName);
                            exitModifyorDelete = true;
                            exitModify = true;

                        }
                        else
                        {
                            Console.WriteLine("Invalid option. Please try again.");
                            continue;
                        }

                        // If we reach here, that means the modification is complete, so exit the outer loop
                        exitModify = true; // Set the flag to exit the main loop
                    }
                    else if (option == "2") // Delete Flight
                    {
                        airline.Flights.Remove(flightNumber);
                        Console.WriteLine("Flight has been deleted.");
                        exitModifyorDelete = true;
                        exitModify = true;

                    }
                    if (exitModifyorDelete)//this chekcs whether the block is true, if it is, it will exit out of this method and go back to the main loop 
                    {
                        return;
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
            if (flight.BoardingGate == null)
            {
                flight.BoardingGate = boardingGates["Unassigned"];
            }
            if (flight.Status == null)
            {
                flight.Status = "Scheduled";
            }
            string airlinecode = flight.FlightNumber.Substring(0, 2);
            var airline = airlines[airlinecode]; 
            Console.WriteLine(
                $"{flight.FlightNumber,-16}" +
                $"{airline.Name,-23}" +
                $"{flight.Origin,-23}" +
                $"{flight.Destination,-23}" +
                $"{flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt"),-36}" +
                $"{flight.Status,-16}" +
                $"{flight.BoardingGate.GateName}"
            );
        }
    }

    //advanced feature B 
    static void DisplayTotalFeePerAirline(Dictionary<string, Flight> flightsdictionary, Dictionary<string, Airline> airlines, Dictionary<string, string> specialRequestCodes)
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

            double airlineFees = 0;
            double airlineDiscounts = 0;

            Console.WriteLine($"Airline: {airlineCode}");

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
            Console.WriteLine($"Total Fees for {airlineCode}: ${airlineFees - airlineDiscounts}");
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

            if (!string.IsNullOrEmpty(specialRequestCodes[flight.FlightNumber]))
            {
                foreach (var gate in boardingGates.Values)
                {
                    if (gate.Flight == null)
                    {
                        if ((specialRequestCodes[flight.FlightNumber] == "CFFT" && gate.SupportsCFFT) ||
                            (specialRequestCodes[flight.FlightNumber] == "DDJB" && gate.SupportsDDJB) ||
                            (specialRequestCodes[flight.FlightNumber] == "LWTT" && gate.SupportsLWTT))
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
                Console.WriteLine($"Special Request Code: {specialRequestCodes[flight.FlightNumber]}");
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
    








       


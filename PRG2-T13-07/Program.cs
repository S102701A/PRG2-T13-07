using PRG2_T13_07;
using System;
using System.IO;
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
    static void Flight()
    {
        string[] csvLines = File.ReadAllLines("flights.csv");
        Dictionary<string, Flight> flightsDictionary = new Dictionary<string, Flight>();

        for (int i = 1; i < csvLines.Length; i++)
        {
            string line = csvLines[i];
            string[] columns = line.Split(',');//I changed the names of the columns to fit what is in your classes, if you read this already you can just delete it 
            string fN = columns[0];
            string ori = columns[1];    
            string dest = columns[2];     
            DateTime eT = DateTime.Parse(columns[3]); 
            string SRC = columns[4];
            Flight flight = new Flight(fN, ori, dest, eT, SRC);
            flightsDictionary[fN] = flight;
            Console.WriteLine($"Flight Number: {fN}, Origin: {ori}, Destination: {dest}, Expected Departure/Arrival: {eT}, Special Request Code: {SRC}");
        }
    }
    //feature 5 & 6 
    //Hi Hafiz, if you're reading this the features arent done yet, still have abit of changes to make, if you can identify and find out whats wrong please help
    //I have also added the boarding gate and flights file to the solution, so you dont have to add those two
    static void AssignBoardingGate(Dictionary<string, Flight> flightsDictionary, )
    {
        string[] csvLines = File.ReadAllLines("boardinggates.csv");
        Dictionary<string, BoardingGate> BoardinggateDictionary = new Dictionary<string, BoardingGate>();
        for (int i = 1; i < csvLines.Length; i++)
        {
            string Line = csvLines[i];
            string[] columns = Line.Split(",");
            string gN = columns[0];
            string DDJB = columns[1];
            string CFFT = columns[2];
            string LWTT = columns[3];
            BoardingGate Boardinggate = new BoardingGate(gN , DDJB, CFFT, LWTT);
            BoardinggateDictionary[gN] = Boardinggate;
            Console.WriteLine(($"Boarding Gate Name: {gN}, Supports DDJB: {DDJB}, Supports CFFT: {CFFT}, Supports LWTT: {LWTT} "));
        }
        Console.WriteLine("=============================================");
        Console.WriteLine("3. Assign a Boarding Gate to a Flight");
        Console.WriteLine("=============================================");
        Console.WriteLine("Enter flight number: ");
        string FlightNumber = Console.ReadLine();
        Console.WriteLine("Enter Boarding Gate Name:");
        string gN = Console.ReadLine();
        if (flightsDictionary.ContainsKey(FlightNumber))
        {
            foreach (var flight in flightsDictionary.Values)
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}, \n Origin: {flight.Origin}, \n Destination: {flight.Destination}, \n Expected Time: {flight.eT} Special Request Code: {SRC}");
            }
        }
        if (BoardinggateDictionary.ContainsKey(gN))
        {
            Console.WriteLine(($"Boarding Gate Name: {gN}, Supports DDJB: {DDJB}, Supports CFFT: {CFFT}, Supports LWTT: {LWTT} "));//TBC 
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


       


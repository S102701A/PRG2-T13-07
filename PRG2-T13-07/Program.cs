using PRG2_T13_07;
using System;
using System.IO;
//Feature 2 & 3 
class Program
{
    static void Main()
    {
        Flight();
    }
    static void Flight()
    {
        string[] csvLines = File.ReadAllLines("flights.csv");
        Dictionary<string, Flight> flightsDictionary = new Dictionary<string, Flight>();

        for (int i = 1; i < csvLines.Length; i++)
        {
            string line = csvLines[i];
            string[] columns = line.Split(',');
            string Number = columns[0];
            string Origin = columns[1];    
            string Destination = columns[2];     
            DateTime ExpectedDeparture_Arrival = DateTime.Parse(columns[3]); 
            string Special_Request_Code = columns[4];
            Flight flight = new Flight(Number, Origin, Destination, ExpectedDeparture_Arrival, Special_Request_Code);
            flightsDictionary[Number] = flight;
            Console.WriteLine($"Flight Number: {Number}, Origin: {Origin}, Destination: {Destination}, Expected Departure/Arrival: {ExpectedDeparture_Arrival}, Special Request Code: {Special_Request_Code}");
        }
    }

}


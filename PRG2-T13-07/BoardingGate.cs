using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_07
{
    internal class BoardingGate
    {
        private string gateName;
        private bool supportsCFFT;
        private bool supportsDDJB;
        private bool supportsLWTT;
        private Flight flight;

        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }

        public Flight Flight { get; set; }

        public BoardingGate()
        {

        }

        public BoardingGate(string gN, bool sCFFT,bool sDDJB,bool sLWTT,Flight f)
        {
            GateName = gN;
            SupportsCFFT = sCFFT;
            SupportsDDJB = sDDJB;
            SupportsLWTT = sLWTT;
            Flight = f;
        }
    }
}

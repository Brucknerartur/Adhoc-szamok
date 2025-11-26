using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adhoc_szamok
{
    public class Szamok
    {
        public int Id { get; set; }
        public string Cim { get; set; }
        public Double Hossz { get; set; }
        public string Szerzo { get; set; }
        public string Szovegiro { get; set; }
        public List<string> Stilus { get; set; }
        public bool Kiadva { get; set;}
        public int Keletkezes { get; set; }
        public Szamok()
        {
            
        }
    }
}

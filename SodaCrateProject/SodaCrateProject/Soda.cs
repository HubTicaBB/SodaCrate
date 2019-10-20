using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaCrateProject
{
    class Soda
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public SodaType Type { get; set; }

        public Soda(string name, double price, SodaType type)
        {
            Name = name;
            Price = price;
            Type = type;
        }

        public override string ToString()   // Metod som formaterar hur objektets utskrift ser ut
        {
            return string.Format("{0} \t| \"{1}\" \t| {2,7:f2} SEK", Type.ToString(), Name.First().ToString().ToUpper() + Name.Substring(1), Price);
        }
    }

}

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
            return $" {Type,-8}| {Name,-14}| {Price,9:c}";
        }
    }

}

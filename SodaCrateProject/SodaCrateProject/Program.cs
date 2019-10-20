using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaCrateSlutprojekt
{
    public class Sodacrate
    {
        private Soda[] mySodacrate = new Soda[25];
        static int AntalFlaskor = 0;

        public Sodacrate()
        {
            Run();
        }

        public void Run()
        {
            PrintMenu();
            int selectedOption = Select();
            SwitchSelection(selectedOption);
            Run();
        }

        private void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("*************************************************\n" +
                              "Välkommen till fantastiska läskbacken-simulatorn!\n\n" +
                              "                     * * *\n" +
                              "Välj alternativ:\n\n" +
                              "[1] Lägga till en läsk i läskbacken\n" +
                              "[2] Skriva ut innehållet i läskbacken\n" +
                              "[3] Beräkna det totala värdet av backen\n" +
                              "[4] Beräkna medelvärdet av alla dryck i backen\n" +
                              "[5] Sök efter en läsk\n" +
                              "[6] Sortera läsk i läskbacken och skriva den ut\n" +
                              "[7] Ta ut en flaska\n" +
                              "[0] Avsluta programmet");
            Console.Write("Ange numret vid onskade alternativet: ");
        }

        private int Select()
        {
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.Write("\nFelaktig inmatning.\nAnge gärna ett heltal: ");
                Select();
            }
            return 0;
        }

        private void SwitchSelection(int selectedOption)
        {
            switch (selectedOption)
            {
                case 1:
                    AddSoda();
                    break;
                case 2:
                    PrintCrate();
                    break;
                case 3:
                    PrintTotal();
                    break;
                case 4:
                    PrintAverage();
                    break;
                case 5:
                    FindSoda();
                    break;
                case 6:
                    SortSoda();
                    break;
                case 7:
                    TakeSoda();
                    break;
                case 0:
                    Console.WriteLine("----------------------------------\nProgrammet avslutas");
                    break;
                default:
                    break;
            }
        }

        public void AddSoda()               // Metod för att lägga till en läsk
        {
            if (AntalFlaskor < 25)          // Om läskbacken inte är full, fråga efter namn, pris och typ
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Vilken läsk vill du lägga till?");

                Console.Write("Namn: ");
                string name = Console.ReadLine().ToLower();                     // Användarens inmatning omvandlas till gemener och lagras till variabel name
                name = name.First().ToString().ToUpper() + name.Substring(1);   // Omvandla första bokstav till versal

                bool ask = true;            // För att kontrollera huruvida frågor om pris och typ ställs eller inte (dvs. när priset och typen är bekant)

                double price = 0.0;
                string type = "";

                if (AntalFlaskor == 0)      // Dvs. om det är den första flaskan som läggs till
                {
                    price = AskPrice();     // Metodanrop, fråga efter pris - skrivit som en separat metod, därför att koden används två gånger
                    type = AskType();       // Samma som ovan
                    ask = false;            // dvs. fråga inte igen
                }

                foreach (Soda flaska in mySodacrate)    // För att kontrollera om samma drycken redan finns i backen, dvs. om pris är typ är bekanta
                {
                    if (flaska != null)             // Hoppa över tomma platser i backen
                    {
                        if (flaska.GetName() == name)   // Om det redan finns samma drycken i backen
                        {
                            price = flaska.GetPrice();  // Ta de bekanta pris och typ
                            type = flaska.GetTyp();
                            ask = false;                // Fråga inte igen
                            break;
                        }
                    }
                }
                if (ask)                                // Om priset och typet inte bekant
                {
                    price = AskPrice();     // Metodanrop, fråga efter pris
                    type = AskType();       // Samma som ovan
                }

                mySodacrate[AntalFlaskor] = new Soda(name, price, type);    // Lagra objekt i vektorn i första tomma positionen
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("En {0} á {1:f2} SEK är lagt till läskbacken.", name, price); // Upplysa användaren att drycken är lagt till
                AntalFlaskor++;                                         // Öka antal flaskor med 1 efter varje tillägning
            }
            else      // Om antalFlaskor i backen är 25 upplysa användaren att backen är full
            {
                Console.WriteLine("Läskbacken är full. För att tillägga en flaska, måste du först ta en annan ut.");
                Console.WriteLine("I så fall, välj alternativ 7 i menyn nedan");
            }

        }

        public double AskPrice()    // Metod för att fråga efter pris
        {                           // Separat, eftersom koden används två gånger
            bool loop = true;

            double price = 0.0;

            Console.Write("Pris: ");    // Be användaren ange priset
            do
            {
                do
                {
                    try                                                                 // Säkerställa att programmet inte kraschar 
                    {                                                                   // genom att omvandla användarens inmatning till flyttal (double) i try-blocket
                        price = double.Parse(Console.ReadLine().Replace('.', ','));     // Replace()-metoden säkerställer att programmet inte kraschar 
                        loop = false;                                                   // oavsett om användaren skriver punkt eller komma som decimaltalseparator
                    }
                    catch
                    {
                        Console.Write("Felaktig inmatning. Ange priset en gång till: ");// Om omvandlingen misslyckades, upplysa användaren och be honom ange priset en gång till
                    }
                } while (loop);                                                         // Loopen körs så länge användaren inte skrivit ett flyttal (eller ett heltal som också går att omvandla till flyttal)

                if (price <= 0 && !loop)                                                // Säkerställa att priset är positivt tal
                {
                    Console.Write("Felaktig inmatning. Ange priset en gång till: ");
                    loop = true;
                }
            } while (price <= 0);                                                       // Hela loopen körs så länge användaren inte skrivit ett positivt tal

            return price;                                                               // metodens returvärde
        }

        public string AskType()                                 // Metod för att fråga efter typ av dryck
        {                                                       // Separat, eftersom koden används två gånger
            Console.Write("Typ (Läsk, vatten eller lättöl): "); // Be användaren skriva in typ av dryck
            string type = "";
            do                                                                                      // Säkerställa att inmatningen är bara en av de tre typerna: läsk, vatten, lättöl
            {
                type = Console.ReadLine().Replace("la", "lä").Replace('o', 'ö').ToLower();  // Användarens inmatning omvandlas till gemener och tilldelas variablen Type 
                                                                                            // Replace()-metoden gör möjligt att använda ett icke-svenkst tangentbord        
                type = type.First().ToString().ToUpper() + type.Substring(1);               // types första bokstav omvandlas till versal

                if (type != "Läsk" && type != "Vatten" && type != "Lättöl")                 // Om användarens inmatning inte är en av de tre typerna be honom ange typ igen
                {
                    Console.WriteLine("Fel typ. I läskbacken passar bara läsk, vatten och lättöl.");
                    Console.Write("Försök igen: ");
                }
            } while (type != "Läsk" && type != "Vatten" && type != "Lättöl");               // Körs tills användaren skrivit en av de tre typerna

            return type;            // metodens returvärde                            
        }

        public void PrintCrate()    // Metod för att skriva ut läskbackens innehål
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("  TYP   |      NAMN     |    PRIS");
            Console.WriteLine("-------------------------------------");
            foreach (Soda flaska in mySodacrate)    // Loopa genom vektorn mySodacrate
            {
                if (flaska != null)             // Om något är lagrat i positionen, om den är inte tom
                {
                    Console.WriteLine(flaska);  // skriv ut objektets egenskaper (typ, namn och pris)
                }
            }
            if (AntalFlaskor < 25)              // Om det finns några tomma positionen i vektorn dvs. backen
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Det finns {0} tomma platser kvar i läskbacken", 25 - AntalFlaskor);  // Skriv ut hur många platser i backen är tomma
            }
        }

        public double Total()   // Metod för att beräkna det totala värdet av backen, return value se koristi u PrintTotal() i PrintAverage()
        {                       // Separat, därför att metodens returvärde används i både PrintTotal()- och PrintAverage()-metoder
            double total = 0.0;
            foreach (Soda flaska in mySodacrate)        // Loopa genom vektorn mySodacrate
            {
                if (flaska != null)                 // Om positionen i vektorn inte är tom
                {
                    total += flaska.GetPrice();     // öka total med objektets pris, genom att anropa GetPrice()-metod av klassen Soda
                }
            }
            return total;                           // metodens returvärde
        }

        public void PrintTotal()        // Metod för att skriva ut det totala värdet
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Totala värdet av backen är {0:f2} SEK", Total());    // Total()-metodanrop
        }                                                                           // Skriva ut metodens returvärde

        public void PrintAverage()      // Metod för att skriva ut medelvärdet
        {
            Console.WriteLine("----------------------------------------------------------");
            if (AntalFlaskor > 0)                               // Om backen inte är tomm
            {
                Console.WriteLine("Medelvärdet av alla drycker i läskbacken är {0:f2} SEK", Total() / AntalFlaskor);    // Skriv ut mädelvärdet (Total()-returvärde delad med antal flaskor
            }
            else
            {
                Console.WriteLine("Läskbacken är tom");         // Upplysa användaren att backen är tomm
            }
        }

        public void FindSoda()      // Metod för att söka efter en dryck
        {
            Console.WriteLine("----------------------------------------");
            Console.Write("Vilken dryck söker du?  ");
            string find = Console.ReadLine().ToLower();         // Använcarens inmatning omvandlas till gemener och tilldelas variabeln find
            find = find.First().ToString().ToUpper() + find.Substring(1);   // Första bokstav omvandlas till versal
            int found = 0;                                      // Håller reda på antal hittade drycker

            foreach (Soda flaska in mySodacrate)                    // Loopa genom vektorn mySodacrate
            {
                if (flaska != null)                             // Hoppa över tomma positioner i vektorn
                {
                    if (flaska.GetName() == find)               // Om objektets namn är lika med användarens inmatning
                    {
                        found++;                                // öka antal hittade drycker med 1
                    }
                }
            }
            Console.WriteLine("----------------------------------------");
            if (found > 0)                                                                  // Om drycket är hittad
            {
                Console.WriteLine("Det finns {0} stycke(n) {1} i läskbäcken.", found, find);// Upplysa användaren hur många stycket finns
            }
            else                                                                            // Annars
            {
                Console.WriteLine("Det finns ingen {0} i läskbäcken.", find);               // Upplysa användaren att det som han söker efter inte finns i backen
            }
        }

        public void SortSoda()  // Metod för att sortera drycker i backen efter namn, pris eller typ
        {
            Console.WriteLine("----------------------------------------");
            if (AntalFlaskor == 0)
            {
                Console.WriteLine("Läskbacken är tom.");    // Om backen är tom, upplysa användaren om detta och gör inte sorteringen
            }
            else
            {
                Console.WriteLine("Välj alternativ:");      // Fråga efter sorteringskriterium
                Console.WriteLine("1. Sortera efter pris");
                Console.WriteLine("2. Sortera efter namn");
                Console.WriteLine("3. Sortera efter typ");
                Console.Write("Ange numret: ");

                int option = 0;

                do
                {
                    try                                             // Säkerställa att användaren angett ett heltal
                    {
                        option = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        option = 4;                                 // Om användaren skrivit något annat, option blir 4 och så hämtar det i default i switchen
                    }

                    switch (option)                                 // Beroende på sorteringskriterium användaren har valt motsvarande metoden anropas
                    {
                        case 1:
                            SortPrice();
                            break;
                        case 2:
                            SortName();
                            break;
                        case 3:
                            SortType();
                            break;
                        default:
                            Console.Write("Felaktig inmatning. Försök igen: "); // Om användaren skrivit ett nummer som inte finns i menyn (och därmed i switchen)
                            break;                                              // eller om användaren skrivit något som inte kan omavndlas till heltal och så blir det 4 (s. catch ovan)
                    }
                } while (option != 1 && option != 2 && option != 3);    // Inmatningen sker i en loop som körs tills användaren skrivit en av de 3 optionerna
            }
        }

        public void SortPrice() // Metod för att sortera drycker efter pris
        {
            Soda temp = new Soda("", 0.0, "");  // Skapa ett nytt objekt av klassen Soda som behövs för att byta position på två drycker i backen
            int max = AntalFlaskor - 1;

            for (int i = 0; i < max; i++)       // För att komma åt varje vektorns position som innehåller ett objekt
            {
                int nrLeft = max - i;           // Håller reda på antal positioner som inte är sorterade ännu    
                for (int j = 0; j < nrLeft; j++)
                {
                    if (mySodacrate[j].GetPrice() > mySodacrate[j + 1].GetPrice())  // Om ett objekts pris är högre än nästa objektets pris
                    {
                        temp = mySodacrate[j];                                  // Byta position på de två objekten    
                        mySodacrate[j] = mySodacrate[j + 1];
                        mySodacrate[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine("----------------------------------");
            Console.WriteLine("Läskbacken är sorterad efter pris:");
            PrintCrate();                                                   // Skriv ut backens innehåll genom att anropa PrintCrate()-metod
        }

        public void SortName()  // Metod för att sortera drycker efter namn efter bokstavsordning
        {
            Soda temp = new Soda("", 0.0, "");
            int max = AntalFlaskor - 1;
            int comp = 0;

            for (int i = 0; i < max; i++)       // För att komma åt varje vektorns position som innehåller ett objekt
            {
                int nrLeft = max - i;
                for (int j = 0; j < nrLeft; j++)
                {
                    comp = mySodacrate[j].GetName().CompareTo(mySodacrate[j + 1].GetName());    // CompareTo()-metoden jämför namn av två objekt och returnerar -1 (om t ex A står framför B), 0 (både samma) eller 1 (B står framför A)
                    if (comp == 1)                                                      // Om CompareTo() har returnerat 1
                    {
                        temp = mySodacrate[j];                                              // Byta position på de två objekten
                        mySodacrate[j] = mySodacrate[j + 1];
                        mySodacrate[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine("----------------------------------");
            Console.WriteLine("Läskbacken är sorterad efter namn:");
            PrintCrate();                                                               // Skriv ut backens innehåll genom att anropa PrintCrate()-metod                                                           
        }

        public void SortType()  // För att sortera drycker efter typ efter bokstavsordning
        {                       // Metoden funkar på samma sätt som SortName()-metoden, men jämför typer av objekter                         
            Soda temp = new Soda("", 0.0, "");
            int max = AntalFlaskor - 1;
            int comp = 0;

            for (int i = 0; i < max; i++)
            {
                int nrLeft = max - i;
                for (int j = 0; j < nrLeft; j++)
                {
                    comp = mySodacrate[j].GetTyp().CompareTo(mySodacrate[j + 1].GetTyp());
                    if (comp == 1)
                    {
                        temp = mySodacrate[j];
                        mySodacrate[j] = mySodacrate[j + 1];
                        mySodacrate[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Läskbacken är sorterad efter typ:");
            PrintCrate();
        }

        public void TakeSoda()
        {
            if (AntalFlaskor > 0)
            {
                Console.WriteLine("------------------------------------");
                Console.WriteLine("I läskbacken finns följande drycker:");
                PrintCrate();

                Console.WriteLine("------------------------------------------------------");
                Console.Write("Vilken dryck vill du ta ut? (skriv dryckens namn): ");

                bool taken = false;
                do
                {
                    string inputTake = Console.ReadLine().ToLower();
                    string take = inputTake.First().ToString().ToUpper() + inputTake.Substring(1);

                    Console.WriteLine("----------------------------------");
                    for (int i = 0; i <= mySodacrate.Length - 1; i++)
                    {
                        if (mySodacrate[i] != null)
                        {
                            if (mySodacrate[i].GetName() == take)
                            {
                                Console.WriteLine("En {0} är tagen ut", mySodacrate[i].GetName());
                                mySodacrate[i] = null;
                                AntalFlaskor--;
                                taken = true;
                                break;
                            }
                        }
                    }

                    if (!taken)
                    {
                        Console.Write("Det finns ingen {0} i läskbacken. Kolla backens innehåll och försök igen: ", take);
                    }

                } while (!taken);

                // sortiranje, da ne ostanu prazna mesta
                for (int i = 0; i <= mySodacrate.Length - 1; i++)
                {
                    if (mySodacrate[i] == null && mySodacrate[i++] != null)
                    {
                        mySodacrate[i] = mySodacrate[i++];
                        mySodacrate[i++] = null;
                    }
                }
            }
            else
            {
                Console.WriteLine("----------------------------------");
                Console.WriteLine("Läskbacken är tom.");
            }
        }
    }

    class Soda              // Klassen hanterar värden som en flaska (objekt i vektorn) har
    {
        // PROPERTIES:
        private string Name;
        private double Price;
        private string Type;

        // KONSTRUKTOR:                                         // När vi skapar ett objekt av klassen Soda skickas ett värde i konstruktorn
        public Soda(string _name, double _price, string _type)
        {
            Name = _name;
            Price = _price;
            Type = _type;
        }

        // METODER:
        public double GetPrice()    // Metod som returnerar objekts pris-värdet
        {
            return Price;
        }

        public string GetName()     // Metod som returnerar objekts namn
        {
            return Name;
        }

        public string GetTyp()      // Metod som returerar objekts typ
        {
            return Type;
        }

        public override string ToString()   // Metod som formaterar hur objektets utskrift ser ut
        {
            return string.Format("{0} \t| \"{1}\" \t| {2,7:f2} SEK", Type.First().ToString().ToUpper() + Type.Substring(1), Name.First().ToString().ToUpper() + Name.Substring(1), Price);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Sodacrate sodacrate = new Sodacrate();

            Console.WriteLine("Tryck på valfri tangent för att bekräfta");
            Console.ReadKey();
        }
    }
}



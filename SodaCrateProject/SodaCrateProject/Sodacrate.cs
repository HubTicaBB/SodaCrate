using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaCrateProject
{
    internal class Sodacrate
    {
        private Soda[] mySodacrate = new Soda[25];
        static int numberOfBottles = 0;

        public Sodacrate()
        {
            Run();
        }

        private void Run()
        {
            PrintMenu();
            int option = Select();
            SwitchSelection(option);
            Run();
        }

        private void PrintMenu()
        {
            Console.Clear();
            Console.Write("*************************************************\n" +
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
                          "[0] Avsluta programmet\n\n" +
                          "Ange numret vid onskade alternativet: ");
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
        
        public void AddSoda()
        {
            if (CrateNotFull())
            {
                string name = AskName();
                double price = 0.0;
                SodaType type = SodaType.none;

                if (UnknownSoda(name, ref price, ref type))
                {
                    price = AskPrice();
                    type = AskType();
                }

                mySodacrate[numberOfBottles] = new Soda(name, price, type);
                Console.WriteLine($"\n----------------------------------------\n" +
                                  $"En {name} á {price:c} är lagt till läskbacken.");
                numberOfBottles++;
            }
            else
            {
                Console.WriteLine("Läskbacken är full. För att tillägga en flaska, måste du först ta en annan ut.");
            }
        }

        private bool CrateNotFull()
        {
            return (numberOfBottles < 25) ? true : false;
        }

        public string AskName()
        {
            Console.Write("----------------------------------------\n" +
                          "Vilken läsk vill du lägga till?\n\n" +
                          "Namn: ");
            return StringInput();
        }

        public string StringInput()
        {
            string input = Console.ReadLine().ToLower().Trim();
            return input.First().ToString().ToUpper() + input.Substring(1);
        }        

        public double AskPrice()
        {
            return DoubleInput();
        }

        private bool UnknownSoda(string sodaName, ref double price, ref SodaType type)
        {
            for (int i = 0; i < numberOfBottles; i++)
            {
                if (mySodacrate[i].Name == sodaName)
                {
                    price = mySodacrate[i].Price;
                    type = mySodacrate[i].Type;
                    return true;
                }
            }
            return false;
        }

        public double DoubleInput()
        {
            try
            {
                return double.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.Write("\nFelaktig inmatning.\nAnge priset igen: ");
                DoubleInput();
            }
            return 0.0;
        }

        public SodaType AskType()
        {
            try
            {
                return (SodaType)Enum.Parse(typeof(SodaType), Console.ReadLine(), false);
            }
            catch (Exception)
            {
                Console.Write("Felaktig inmatnign, välj gärna mellan läsk, vatten eller lättöl: ");
                AskType();
            }
            return SodaType.none;
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
            if (numberOfBottles < 25)              // Om det finns några tomma positionen i vektorn dvs. backen
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Det finns {0} tomma platser kvar i läskbacken", 25 - numberOfBottles);  // Skriv ut hur många platser i backen är tomma
            }
        }

        public double Total()   // Metod för att beräkna det totala värdet av backen, return value se koristi u PrintTotal() i PrintAverage()
        {                       // Separat, därför att metodens returvärde används i både PrintTotal()- och PrintAverage()-metoder
            double total = 0.0;
            foreach (Soda flaska in mySodacrate)        // Loopa genom vektorn mySodacrate
            {
                if (flaska != null)                 // Om positionen i vektorn inte är tom
                {
                    total += flaska.Price;     // öka total med objektets pris, genom att anropa Price-metod av klassen Soda
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
            if (numberOfBottles > 0)                               // Om backen inte är tomm
            {
                Console.WriteLine("Medelvärdet av alla drycker i läskbacken är {0:f2} SEK", Total() / numberOfBottles);    // Skriv ut mädelvärdet (Total()-returvärde delad med antal flaskor
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
                    if (flaska.Name == find)               // Om objektets namn är lika med användarens inmatning
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
            if (numberOfBottles == 0)
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
            Soda temp = new Soda("", 0.0, SodaType.none);  // Skapa ett nytt objekt av klassen Soda som behövs för att byta position på två drycker i backen
            int max = numberOfBottles - 1;

            for (int i = 0; i < max; i++)       // För att komma åt varje vektorns position som innehåller ett objekt
            {
                int nrLeft = max - i;           // Håller reda på antal positioner som inte är sorterade ännu    
                for (int j = 0; j < nrLeft; j++)
                {
                    if (mySodacrate[j].Price > mySodacrate[j + 1].Price)  // Om ett objekts pris är högre än nästa objektets pris
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
            Soda temp = new Soda("", 0.0, SodaType.none);
            int max = numberOfBottles - 1;
            int comp = 0;

            for (int i = 0; i < max; i++)       // För att komma åt varje vektorns position som innehåller ett objekt
            {
                int nrLeft = max - i;
                for (int j = 0; j < nrLeft; j++)
                {
                    comp = mySodacrate[j].Name.CompareTo(mySodacrate[j + 1].Name);    // CompareTo()-metoden jämför namn av två objekt och returnerar -1 (om t ex A står framför B), 0 (både samma) eller 1 (B står framför A)
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
            Soda temp = new Soda("", 0.0, SodaType.none);
            int max = numberOfBottles - 1;
            int comp = 0;

            for (int i = 0; i < max; i++)
            {
                int nrLeft = max - i;
                for (int j = 0; j < nrLeft; j++)
                {
                    comp = mySodacrate[j].Type.CompareTo(mySodacrate[j + 1].Type);
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
            if (numberOfBottles > 0)
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
                            if (mySodacrate[i].Name == take)
                            {
                                Console.WriteLine("En {0} är tagen ut", mySodacrate[i].Name);
                                mySodacrate[i] = null;
                                numberOfBottles--;
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

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Podaj ścieżkę do pliku");
            string toRead = Console.ReadLine();
            string toSearch = "";

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(toRead))
                {
                    // Read the stream to a string, and write the string to the console.
                    toSearch = sr.ReadToEnd();
                    Console.WriteLine("File opened");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return;
            }

            SymbolReader SR = new SymbolReader(toSearch);
            FoundSymbol returnedSymbol;
            do
            {
                returnedSymbol = SR.GetNextSymbol();
                Console.WriteLine(string.Format("typ: {0} -> zawartość: {1}",returnedSymbol.type.Type, returnedSymbol.content));
            } while (returnedSymbol.type.Type != "błąd" && returnedSymbol.type.Type != "koniec");
           
            Console.WriteLine("kliknij aby ropocząć parsowanie");
            Console.ReadKey();
            SR = new SymbolReader(toSearch);
            returnedSymbol = SR.GetNextSymbol();
            if (returnedSymbol.type.Type != "błąd" && returnedSymbol.type.Type != "koniec")
            {
                Parser parser = new Parser(SR);
                parser.Start(returnedSymbol);
            }

            Console.WriteLine("wyrażenie poprawne");
            Console.ReadKey();
        }
    }
}

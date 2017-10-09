using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace T2
{
    public class SymbolType
    {
        public string Type;
        public string Regex;
    }
    public class FoundSymbol
    {
        public string content;
        public SymbolType type;
    }
    public class SymbolReader
    {
        public static SymbolType[] MatchTypes = new[] {
            new SymbolType {Type = "identyfikator",Regex = @"^[a-zA-Z][^\W_]*$"},
            new SymbolType {Type = "int",Regex = @"^\d+$"},
            new SymbolType {Type = "float",Regex = @"^\d+\.\d+$"},
            new SymbolType {Type = "plus",Regex = @"^\+$"},
            new SymbolType {Type = "minus",Regex = @"^-$"},
            new SymbolType {Type = "gwiazdka",Regex = @"^\*$"},
            new SymbolType {Type = "ukośnik",Regex = @"^\/$"},
            new SymbolType {Type = "nawiasLewy",Regex = @"^\($"},
            new SymbolType {Type = "nawiasPrawy",Regex = @"^\)$"},           
        };
        public static Dictionary<string, SymbolType> SpecialMatchTypes = new Dictionary<string, SymbolType> {
            {"err",new SymbolType {Type = "błąd",Regex = ""} },
            {"end",new SymbolType {Type = "koniec",Regex = ""} },
            {"ws", new SymbolType {Type = "znBiały",Regex = @"^[ \r\n]+$"} },
        };

        string baseText;
        string currentText;
        public SymbolReader(string text)
        {
            baseText = text;
            currentText = text;
        }

        public FoundSymbol GetNextSymbol()
        {
            if (currentText.Length <= 0)
            {
                return new FoundSymbol { type = SpecialMatchTypes["end"], content = "" };
            }

            int breakPosition = GetBreakIndex();
            if (breakPosition == -1)
            {
                return new FoundSymbol { type = SpecialMatchTypes["end"], content = "" };
            }
            if (breakPosition == -2)
            {
                return new FoundSymbol { type = SpecialMatchTypes["err"], content = "Break find error" };
            }

            SymbolType SymbolFound = null;
            //bool isFinished = false;
            string currentFragment = "";
            int fragmentLength = breakPosition +1;
            do
            {
                fragmentLength --;
                if (fragmentLength <= 0)
                {
                    break;
                }
                currentFragment = currentText.Substring(0, fragmentLength);

                foreach (var type in MatchTypes)
                {
                    var match = Regex.Match(currentFragment, type.Regex);
                    if (match.Success)
                    {
                        SymbolFound = type;
                        break;
                    }
                }          
            }
            while (SymbolFound == null);

            currentText = currentText.Substring(fragmentLength);
            if (SymbolFound == null)
            {
                return new FoundSymbol {type = SpecialMatchTypes["err"], content = currentText.Substring(0, breakPosition) };
            }
            return new FoundSymbol { type = SymbolFound, content = currentFragment };

        }
        int GetBreakIndex()
        {
            bool wsFound = false;
            string currentFragment = "";
            int fragmentLength = 0;

            var beginsWSMatch = Regex.Match(currentText.Substring(0,1), SpecialMatchTypes["ws"].Regex);
            if (beginsWSMatch.Success)
            {
                wsFound = false;
                currentFragment = "";
                fragmentLength = 1; // Skip first check
                do
                {
                    wsFound = false;
                    fragmentLength ++;
                    if (fragmentLength > currentText.Length)    // End check
                    {
                        return -1;
                    }
                    currentFragment = currentText.Substring(0, fragmentLength);

                    var match = Regex.Match(currentFragment, SpecialMatchTypes["ws"].Regex);
                    if (match.Success)
                    {
                        wsFound = true;
                    }
                } while (wsFound);
                currentText = currentText.Substring(fragmentLength - 1);
            }


            wsFound = false;
            currentFragment = "";
            int checkPosition = 0; // Skip first check
            do
            {
                checkPosition ++;
                if (checkPosition >= currentText.Length)
                {
                    return currentText.Length;
                }
                currentFragment = currentText.Substring(checkPosition, 1);

                var match = Regex.Match(currentFragment, SpecialMatchTypes["ws"].Regex);
                if (match.Success)
                {
                    wsFound = true;
                    return checkPosition;
                }
            } while (!wsFound);
            return -2;
        }
    }
}

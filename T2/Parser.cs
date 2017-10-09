using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2
{
    class Parser
    {
        private SymbolReader sR;
        public FoundSymbol Current { get; set; }
        public bool isEnd = false;

        public Parser(SymbolReader sR)
        {
            this.sR = sR;
        }     

        public void Start(FoundSymbol foundSymbol)
        {
            Current = foundSymbol;
            Wyr();
        }

        private void Next()
        {
            Current = sR.GetNextSymbol();
            if (Current.type.Type == "błąd")
            {
                throw new System.ArgumentException(string.Format("Błąd leksera w: {0}", Current.content));
            }
            if (Current.type.Type == "koniec")
            {
                isEnd = true;
            }
        }
        private void Accept(string expected)
        {
            if (expected == Current.type.Type)
            {
                Next();
            }
            else
            {
                throw new System.ArgumentException(string.Format("aktualny symbol różni się od oczekiwanego: {0} vs {1}", Current.type.Type, expected));
            }
        }


        private void Wyr()
        {
            Skl();
            RW();
        }

        private void Skl()
        {
            Cz();
            RS();
        }

        private void Cz()
        {
            if (Current.type.Type == "int")
            {
                Accept("int");
            }
            else if (Current.type.Type == "float")
            {
                Accept("float");
            }
            else if (Current.type.Type == "identyfikator")
            {
                Accept("identyfikator");
            }
            else
            {
                Accept("nawiasLewy");
                Wyr();
                Accept("nawiasPrawy");
            }
        }

        private void RS()
        {
            if (Current.type.Type == "gwiazdka")
            {
                Accept("gwiazdka");
                Cz();
                RS();
            }
            else if (Current.type.Type == "ukośnik")
            {
                Accept("ukośnik");
                Cz();
                RS();
            } else {}
        }

        private void RW()
        {
            if (Current.type.Type == "plus")
            {
                Accept("plus");
                Skl();
                RW();
            }
            else if (Current.type.Type == "minus")
            {
                Accept("minus");
                Skl();
                RW();
            }
            else { }
        }
    }
}

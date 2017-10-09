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
        public bool isSuccess = false;

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
                isSuccess = true;
            }
        }
        private void Accept(SymbolType expected)
        {
            if (expected == Current.type)
            {
                Next();
            }
            else
            {
                throw new System.ArgumentException(string.Format("aktualny symbol różni się od oczekiwanego: {0} vs {1}", Current.type.Type, expected.Type));
            }
        }


        private void Wyr()
        {
            throw new NotImplementedException();
        }
    }
}

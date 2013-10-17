using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeksickiGenerator
{
    [Serializable]
    public class DefLeksAnalizator
    {
        public string PocetnoStanje { get; set; }
        public List<Stanje> Stanja { get; set; }
    }

    [Serializable]
    public class Stanje
    {
        public List<Pravilo> Pravila { get; set; }
    }

    [Serializable]
    public class Pravilo
    {
        public Enka Automat { get; set; }
        public Akcije Akcije { get; set; }
    }

    [Serializable]
    public class Akcije
    {
        public string UniformniZnak { get; set; }
        public int NoviRedak { get; set; }
        public string NovoStanje { get; set; }
        public int VratiSe { get; set; }
    }
}

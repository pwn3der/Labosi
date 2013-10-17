using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeksickiAnalizator
{
	public class Enka
	{
        private const string PrazniSkupStanjaOznaka = "#";
        private const string EpsilonPrijelazOznaka = "$";

		public string pocetnoStanje { get; set; }
		public string prihStanje { get; set; }
		public List<string> prijelaziKeys { get; set; }
		public List<string> prijelaziValues { get; set; }

		public Dictionary<string, string> prijelazi;
		
		private List<string> trenutnaStanja;

		public Enka()
		{
			this.trenutnaStanja = new List<string>();

            // Build dictionary only if this was deserialized object
            if (this.prijelaziKeys != null)
            {
                this.prijelazi = new Dictionary<string, string>();
                for (int index = 0; index < this.prijelaziKeys.Count; index++)
                    this.prijelazi.Add(this.prijelaziKeys[index], this.prijelaziValues[index]);
            }
		}


		public void Resetiraj()
		{
			trenutnaStanja.Clear();
			trenutnaStanja.Add(pocetnoStanje);            
		}

		public bool SadrziPrihStanje()
		{
			if (trenutnaStanja.Contains(prihStanje)) 
				return true;
			else return false;
		}

		public void NapraviKorak(string uZnak)
		{
			List<string> novaStanja = new List<string>();
			int brStanja = trenutnaStanja.Count;

			for(int i = 0; i < brStanja; i++)
			{
                string novaStanjaRaw;
                var stanjeKey = trenutnaStanja[i] + "," + uZnak;
                if (this.prijelazi.TryGetValue(stanjeKey, out novaStanjaRaw))
                {
                    var razdvojenaStanja = novaStanjaRaw.Split(',');
                    for (int j = 0; j < razdvojenaStanja.Length; j++)
                    {
                        if (!novaStanja.Contains(razdvojenaStanja[j]) && razdvojenaStanja[j] != PrazniSkupStanjaOznaka)
                            novaStanja.Add(razdvojenaStanja[j]);
                    }
                }
			}
            if (novaStanja.Count == 0) novaStanja.Add(PrazniSkupStanjaOznaka);
			novaStanja = DodajEokolinu(novaStanja);

			this.trenutnaStanja.Clear();
			this.trenutnaStanja.AddRange(novaStanja);
		}

		List<string> DodajEokolinu(List<string> novaStanja) {
			int velicina;
			string pomocna;
			string[] polje;

			do{
				velicina = novaStanja.Count;
				for( int i = 0; i < novaStanja.Count; i++)
				{
					if (!prijelazi.ContainsKey(novaStanja[i]+"," + EpsilonPrijelazOznaka)) continue;
                    pomocna = this.prijelazi[novaStanja[i] + "," + EpsilonPrijelazOznaka];
					polje = pomocna.Split(',');
					foreach (string stanje in polje) {
						if (!(novaStanja.Contains(stanje))) 
							novaStanja.Add(stanje);
					}
				}
			}
			while(novaStanja.Count != velicina);
			return novaStanja;	
		}
	}
}

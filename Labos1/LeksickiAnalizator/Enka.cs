using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeksickiAnalizator
{
	public class Enka
	{
		public string pocetnoStanje { get; set; }
		public string prihStanje { get; set; }
		public List<string> prijelaziKeys { get; set; }
		public List<string> prijelaziValues { get; set; }

		private Dictionary<string, string> prijelazi { get; set; }
		private List<string> trenutnaStanja;


		public Enka()
		{
			this.trenutnaStanja = new List<string>();

            // Build dictionary only if this was de-serialized object
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
			string pomocna;
			string pomocna2;

			for(int i = 0; i < brStanja; i++)
			{
				pomocna2 = trenutnaStanja[i]+","+uZnak;
				if (!prijelazi.ContainsKey(pomocna2)) continue;
				pomocna = prijelazi[pomocna2];
				var polje = pomocna.Split(',');
				for (int j = 0;j < polje.Length; j++)
				{
					if( !novaStanja.Contains(polje[j]) && polje[j] !="#") 
						novaStanja.Add(polje[j]);											
				}	
			}
			if (novaStanja.Count == 0) novaStanja.Add("#");
			novaStanja = DodajEokolinu(novaStanja);

			this.trenutnaStanja.Clear();
			this.trenutnaStanja.AddRange(novaStanja);
		}

		List<string> DodajEokolinu(List<string> novaStanja)
		{
			int velicina;
			string pomocna;
			string[] polje;

			do{
				velicina = novaStanja.Count;
				for( int i = 0; i < novaStanja.Count; i++)
				{
					if (!prijelazi.ContainsKey(novaStanja[i]+",$")) continue;
					pomocna = prijelazi[novaStanja[i]+",$"];
					polje = pomocna.Split(',');
					for (int j = 0;j < polje.Length; j++){	   
						if (!(novaStanja.Contains(polje[j]))) novaStanja.Add(polje[j]);
					}
				}
			}
			while(novaStanja.Count != velicina);
			return novaStanja;	
		}
	}
}

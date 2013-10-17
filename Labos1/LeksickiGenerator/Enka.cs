using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeksickiGenerator
{
	public class Enka
	{
		public string pocetnoStanje { get; set; }
		public string prihStanje { get; set; }
		public List<string> prijelaziKeys { get; set; }
		public List<string> prijelaziValues { get; set; }

		public Dictionary<string, string> prijelazi;
		

		public void PripremiZaSerijalizaciju() {
			this.prijelaziKeys = this.prijelazi.Keys.ToList();
			this.prijelaziValues = this.prijelazi.Values.ToList();
		}
	}
}

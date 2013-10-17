using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using Engine.JSON;

namespace LeksickiGenerator
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var def = new DefLeksAnalizator()
            {
                PocetnoStanje = "S_pocetno",
                Stanja = new List<Stanje>() {
                    new Stanje() {
                        Pravila = new List<Pravilo>() {
                            new Pravilo() {
                                Akcije = new Akcije() {
                                    NoviRedak = 0,
                                    NovoStanje = "S_pocetno",
                                    UniformniZnak = "",
                                    VratiSe = 0
                                },
                                Automat = new Enka() {
                                    pocetnoStanje = "Pocetno",
                                    prihStanje = "0",
                                    prijelaziKeys = new List<string>(),
                                    prijelaziValues = new List<string>()
                                }
                            },
                            new Pravilo() {
                                Akcije = new Akcije() {
                                    NoviRedak = 0,
                                    NovoStanje = "S_komentar",
                                    UniformniZnak = "",
                                    VratiSe = 0
                                },
                                Automat = new Enka() {
                                    pocetnoStanje = "Pocetno",
                                    prihStanje = "0",
                                    prijelaziKeys = new List<string>(),
                                    prijelaziValues = new List<string>()
                                }
                            }
                        }
                    }
                }
            };

            using (var file = new StreamWriter("Definicija.json"))
            {
                try
                {
                    var serialised = Serialiser.WriteClass(def);
                    file.WriteLine(serialised);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Couldn't serialize definition: {0}", ex.ToString());
                }
                finally
                {
                    file.Flush();
                    file.Close();
                }
            }


            Console.WriteLine("Sve OK");
            Console.ReadLine();
        }
    }
}

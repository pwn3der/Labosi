using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Engine.JSON;

namespace LeksickiAnalizator
{
    public static class Program
    {
        static void Main(string[] args)
        {
            using (var stream = new StreamReader("Definicija.json"))
            {
                var formatter = new BinaryFormatter();
                try
                {
                    var fileText = stream.ReadToEnd();
                    var a = Deserialiser.ReadClass<DefLeksAnalizator>(fileText);

                    Console.WriteLine(a.PocetnoStanje);
                    Console.WriteLine(a.Stanja.Count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Couldn't deserialize definition: {0}", ex.ToString());
                }
                finally
                {
                    stream.Close();
                }
            }


            Console.WriteLine("Sve OK");
            Console.ReadLine();
        }
    }
}

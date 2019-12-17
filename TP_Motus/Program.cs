using System;
using System.IO;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Motus
{
    internal class Program
    {
        public static string GenererMot(int nbLettres)
        {
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader dico = new StreamReader("H:\\TP_Motus\\dico.txt", encoding);

            int indexPotentiel = 0;
            string[] motsPotentiels = new string[336531];
            string mot = dico.ReadLine();

            while(mot != null)
            {
                
                if (mot.Length == nbLettres)
                {
                    
                    motsPotentiels[indexPotentiel] = mot;
                    indexPotentiel++;
                }
                mot = dico.ReadLine();
            }

            Random randomIndex = new Random();

            int indexChoisi = randomIndex.Next(0, indexPotentiel);
            return motsPotentiels[indexChoisi];
        }


        public static void Main(string[] args)
        {
            string motADeviner = GenererMot(7);


        }
    }
}
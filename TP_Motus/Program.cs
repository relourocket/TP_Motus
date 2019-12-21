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

        /**
         * Lit le fichier dictionnaire et met les mots comportant le bon nombre de lettres dans un tableau
         * @param nbLettre Le nombre de lettres voulu
         * @return motPotentiels Les mots du dictionnaire comportant le bon nombre de lettres
         */
        public static String[] LireFichier(int nbLettres)
        {
            // Encode le fichier en UTF-8
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader dico = new StreamReader("H:\\TP_Motus\\dico.txt", encoding);

            int indexPotentiel = 0;
            string[] motsPotentiels = new string[336531];
            string mot = dico.ReadLine();

            while(mot != null)
            {
                // Si le mot comporte le bon nombre de lettres
                if (mot.Length == nbLettres)
                {
                    
                    motsPotentiels[indexPotentiel] = mot;
                    indexPotentiel++;
                }
                mot = dico.ReadLine();
            }
    
            dico.Close();

            return motsPotentiels;
        }


        /**
         * Récupère le nombre de cases dans lesquelles se trouvent des mots
         * @param motPotentiels Le tableau contenant les mots de n lettres
         * @return ctp Le nombre de cases contenant un mot
         */
        public static int RecupererTailleTableau(String[] motsPotentiels)
        {
            int cpt = 0;
            
            for (int i = 0; i < motsPotentiels.Length; i++)
            {
                if (motsPotentiels[i] != null)
                {
                    cpt++;
                }
            }

            return cpt;
        }
        

        /**
         * Génère un mot avec le nombre de lettres voulu
         * @param motsPotentiels Le tableau contenant les mots de n lettres
         * @return motsPotentiels[indexChoisi] Le mot généré
         */
        public static string GenererMot(String [] motsPotentiels)
        {

            Random randomIndex = new Random();
            
            // Choisi au hasard l'index d'un des mots présélectionnés
            int indexChoisi = randomIndex.Next(0, RecupererTailleTableau(motsPotentiels));
            return motsPotentiels[indexChoisi];
        }


        /**
         * Vérifie si un mot saisi par le joueur est valide
         * @return bool
         */
        public static bool VerifierMot(string motSaisi, int nbLettres)
        {
            

            if(motSaisi.Length == nbLettres)
            {
                string[] dicoVerif = LireFichier(nbLettres);

                for(int index = 0; index < dicoVerif.Length; index++)
                {
                    if(dicoVerif[index] == motSaisi)
                    {
                        return true;
                    }
                }

                return false;
            }

            else
            {
                return false;
            }
           
        }


        /**
         * Initialise le jeu et sa difficulté
         * @return param Le tableau de paramètres de difficulté
         */
        static int[] InitialiserGame()
        {

            String nbLEttresS, nbTentativesS, tempsS;
            int nbLettres = 0, nbTentatives = 0, temps = 0;
            char choixChrono = ' ';
            
            int [] param = new int[3];
            
            // Règles du jeu 
            Console.WriteLine("Bienvenue dans le jeu Motus ! Vous allez devoir trouver un mot en un nombre de tentatives définies. \n " +
                              "Les lettres rouges sont bien placées, les jaunes sont présentes dans le mot mais mal placées");
            
            //
            // Définition des paramètres de difficulté
            //
            
            Console.WriteLine("Veuillez choisir les paramètres de difficulté avant de démarrer");

            do
            {
                Console.WriteLine("Combien de lettre contiendra le mot à trouver (entre 6 et 10)");
                nbLEttresS = Console.ReadLine();

                try
                {
                    nbLettres = int.Parse(nbLEttresS);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur de parsing");
                }
                
            } while (nbLettres < 6 || nbLettres > 10);


            do
            {
                Console.WriteLine("Veuillez choisir en combien de tentatives le mot pourra être trouvé (entre 1 et 15)");
                nbTentativesS = Console.ReadLine();

                try
                {
                    nbTentatives = int.Parse(nbTentativesS);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur de parsing");
                }

            } while (nbTentatives < 1 || nbTentatives > 15);


            do
            {
                Console.WriteLine("Voulez-vous un temps imparti pour trouver le mot ? (O/N)");
                choixChrono = Console.ReadLine().ToCharArray()[0];
                
            } while (choixChrono != 'O' && choixChrono != 'N');

            
            if (choixChrono == 'O')
            {
                Console.WriteLine("Combien de temps maximum désirez-vous pour répondre ? (en secondes)");
                tempsS = Console.ReadLine();

                try
                {
                    temps = int.Parse(tempsS);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur de parsing");
                }
                
            }
            else
            {
                temps = -1;
            }
            
            // Enregistrement des paramètres de difficulté dans un tableau
            param[0] = nbLettres;
            param[1] = nbTentatives;
            param[2] = temps;

            return param;

        }
        

        public static void EnregistrerStatistiques(string mot, int nbLettres, bool success, int nbEssais)
        {
            string historiquePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Visual Studio 2019/my_projects/TP_Motus/historique.txt");

            //Création du fichier s'il n'existe pas
            if (!File.Exists(historiquePath))
            {
                File.AppendAllLines(historiquePath, 
                                    new string[] { "mot,nbLettres,success", "", "total_joue,total_success,pourcentage_success", "0,0,0" });
            }

            
            string[] historique = File.ReadAllLines(historiquePath);

            //extraction des données résumées (i.e, total_joue, total_success, etc)
            string lastLine = historique[historique.Length - 1];
            decimal totalJoue = Int32.Parse(lastLine.Split(',')[0]);
            decimal totalSuccess = Int32.Parse(lastLine.Split(',')[1]);
            decimal pourcentageSuccess;

            
            //Modification des statistiques en fonction des résultats
            totalJoue++;

            if (success)
            {
                totalSuccess++;
            }

            pourcentageSuccess = Decimal.Round((totalSuccess / totalJoue) * 100);

            
            //Création du writer pour écrire dans le fichier historique
            StreamWriter writer = new StreamWriter(historiquePath);

            for(int i = 0; i < historique.Length - 1; i++)
            {
                string line = historique[i];

                if (line == "")
                {
                    writer.WriteLine($"{mot},{nbLettres},{success},{nbEssais}");
                }

                writer.WriteLine(line);
            }

            //Ecriture ligne des statistiques
            writer.WriteLine($"{totalJoue},{totalSuccess},{pourcentageSuccess}");


            writer.Close();
        }


        public static void Main(string[] args)
        {

            // Tableau contenant les paramètres de difficulté avec : 
            // 0 : le nombre de lettres du mot à deviner
            // 1 : le nombre de tentatives pour deviner le mot
            // 2 : le temps imparti en secondes si le joueur en veut un, -1 sinon
            int[] difficulte = new int[3];
            String motADeviner;

            //difficulte = InitialiserGame();

            motADeviner = GenererMot(LireFichier(difficulte[0]));

            Console.ReadKey();
        }
    }
}
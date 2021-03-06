using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TP_Motus
{
    internal class Program
    {

        /// <summary>
        /// Lit le fichier dictionnaire et met les mots comportant le bon nombre de lettres dans un tableau.
        /// </summary>
        /// <returns>
        /// Les mots du dictionnaire comportant le bon nombre de lettres.
        /// </returns>
        /// <param name="nbLettre"> Le nombre de lettres voulu.</param>
        public static String[] LireFichier(int nbLettres)
        {
            // Encode le fichier en UTF-8
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader dico = new StreamReader("../../../Dictionnaire/dico.txt", encoding);

            //Deux expressions régulières pour éliminer les verbes conjugués 
            Regex groupe1Regex = new Regex("((e)|(es)|(ons)|(eons)|(ez)|(ent)|(ais)|(ait)|(ions)|(iez)|(aient)|(erai)|(eras)|(era)|(erons)|(erez)|(eront)|(ai)|(as)|(a)|(âmes)|(assions)|(assiez)|(assent)|(erais)|(erait)|(erions)|(eriez)|(eraient)|(ât)$)");
            Regex groupe2Regex = new Regex("((is)|(it)|(issons)|(issez)|(issent)|(issais)|(issait)|(issions)|(issiez)|(issaient)|(irai)|(iras)|(ira)|(irons)|(irez)|(iront)|(îmes)|(îtes)|(irent)|(isse)|(isses)|(issions)|(issiez)|(issent)|(ît)|(irait)|(irais)|(irions)|(iriez)|(iraient)$)");

            int indexPotentiel = 0;
            string[] motsPotentiels = new string[336531];
            string mot = dico.ReadLine();

            while (mot != null)
            {
                // Si le mot comporte le bon nombre de lettres et que ce n'est pas un verbe conjugué
                if (mot.Length == nbLettres && !groupe1Regex.IsMatch(mot) && !groupe2Regex.IsMatch(mot))
                {

                    motsPotentiels[indexPotentiel] = mot;
                    indexPotentiel++;
                }
                mot = dico.ReadLine();
            }

            dico.Close();

            return motsPotentiels;
        }


        /// <summary>
        /// Récupère le nombre de cases dans lesquelles se trouvent des mots.
        /// </summary>
        /// <returns>
        /// Le nombre de cases contenant un mot.
        /// </returns>
        /// <param name="motPotentiels"> Le tableau contenant les mots de n lettres.</param>
        public static int RecupererTailleTableau(String[] motsPotentiels)
        {
            int count = 0;

            for (int i = 0; i < motsPotentiels.Length; i++)
            {
                if (motsPotentiels[i] != null)
                {
                    count++;
                }
            }

            return count;
        }


        /// <summary>
        /// Génère un mot avec le nombre de lettres voulu.
        /// </summary>
        /// <returns>
        /// Le mot de n lettres généré.
        /// </returns>
        /// <param name="motsPotentiels"> Le tableau contenant les mots de n lettres.</param>
        public static string GenererMot(String [] motsPotentiels)
        {

            Random randomIndex = new Random();
            
            // Choisi au hasard l'index d'un des mots présélectionnés de n lettres

            int indexChoisi = randomIndex.Next(0, RecupererTailleTableau(motsPotentiels));
            return motsPotentiels[indexChoisi];
        }


        /// <summary>
        /// Vérifie si un mot saisi par le joueur est valide.
        /// </summary>
        /// <returns>
        /// True si le mot est valide, False sinon
        /// </returns>
        /// <param name="motSaisi"> Le mot saisi par l'utilisateur.</param>
        /// <param name="nbLettres"> Le nombre de lettres que doit contenir le mot saisi.</param>
        /// <param name="dicoVerif"> Tous les mots valides contenant le nombre de lettres nbLettres.</param>
        public static bool VerifierMot(string motSaisi, int nbLettres, string[] dicoVerif)
        {
            
            // Si le mot saisi contient le bon nombre de lettres
            if(motSaisi.Length == nbLettres)
            {
                for (int index = 0; index < dicoVerif.Length; index++)
                {
                    // Si le mot se trouve dans le dictionnaire
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


        /// <summary>
        /// Initialise le jeu et sa difficulté.
        /// </summary>
        /// <returns>
        /// Le tableau de difficultés avec :
        /// [0] : le nombre de lettres du mot
        /// [1] : le nombre de tentatives max
        /// [2] : le temps imparti pour trouver le mot en secondes, -1 si le joueur ne veut pas de temps imparti
        /// </returns>
        public static int[] InitialiserGame()
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

            // On redemande le nombre de lettres du mot à générer tant qu'il ne se trouve pas entre 6 et 10
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

            // On redemande le nombre de tentatives max tant qu'il n'est pas compris entre 1 et 15
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

            // On redemande de faire le choix tant que l'utilisateur n'entre pas O ou N
            do
            {
                Console.WriteLine("Voulez-vous un temps imparti pour trouver le mot ? (O/N)");
                choixChrono = Console.ReadLine().ToUpper().ToCharArray()[0];

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

        /// <summary>
        /// Affiche la grille avec les différents essais et les lettres dans les différentes couleurs.
        /// </summary>
        /// <param name="essais"> Les différents essais du joueur.</param>
        /// <param name="nbLettres"> Le nombre de lettre du mot à trouver.</param>
        /// <param name="nbTentatives"> Le nombre de tentatives max.</param>
        /// <param name="motADeviner"> Le mot à deviner.</param>
        public static void AfficherGrille(String[] essais, int nbLettres, int nbTentatives, String motADeviner)
        {
            for (int i = 0; i < nbTentatives; i++)
            {
                for (int j = 0; j < nbLettres; j++)
                {
                    // On affiche toujours la première lettre du mot à deviner
                    if (j == 0)
                    {
                        // On écrit en gris sur un fond rouge
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" {0} ", motADeviner.Substring(0, 1).ToUpper());
                    }
                    else
                    {
                        // S'il y a un essai sur la ligne courante
                        if (essais[i] != null && essais[i] != " ")
                        {
                            // On écrit en gris sur un fond noir
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write("|");

                            // On récupère la lettre de l'essai courant
                            char lettre = essais[i].Substring(j, 1).ToCharArray()[0];

                            if (EstBienPlacee(lettre, motADeviner[j]))
                            {
                                // On écrit la lettre sur fond rouge car elle est bien placée
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write(" {0} ", essais[i].Substring(j, 1).ToUpper());
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                            else if (EstMalPlacee(lettre, motADeviner))
                            {
                                // On écrit la lettre sur fond jaune car elle est mal placée
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write(" {0} ", essais[i].Substring(j, 1).ToUpper());
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                            else
                            {
                                // On laisse la lettre sur fond noir car elle n'appartient pas au mot
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write(" {0} ", essais[i].Substring(j, 1).ToUpper());
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write("|   ");
                        }
                    }
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("|");
                Console.WriteLine();

                
            }

        }

        /// <summary>
        /// Vérifie si la lettre est bien placée dans le mot.
        /// </summary>
        /// <returns>
        /// True si la lettre est bien placée, False sinon
        /// </returns>
        /// <param name="lettreMot"> La lettre du mot à vérifier.</param>
        /// <param name="lettreMotADeviner"> La lettre à la même position dans le mot à deviner.</param>
        public static bool EstBienPlacee(char lettreMot, char lettreMotADeviner)
        {
            if (lettreMot == lettreMotADeviner)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Vérifie si la lettre se trouve dans le mot à deviner.
        /// </summary>
        /// <returns>
        /// True si la lettre se trouve dans le mot à deviner, False sinon
        /// </returns>
        /// <param name="lettreMot"> La lettre du mot à vérifier.</param>
        /// <param name="motADeviner"> Le mot à deviner.</param>
        public static bool EstMalPlacee(char lettreMot, String motADeviner)
        {

            // On regarde pour chaque lettre du mot à deviner si la lettre à vérifier correspond
            foreach (var lettre in motADeviner)
            {
                if (lettreMot == lettre)
                {
                    return true;
                }
            }

            return false;

        }
        
        /// <summary>
        /// Enregistre les statistiques de jeu du joueur
        /// </summary>
        /// <param name="mot"> Le mot à deviner.</param>
        /// <param name="nbLettres"> Le nombre de lettres du mot à deviner.</param>
        /// <param name="success"> True si la partie s'est terminée après une victoire du joueur, False sinon.</param>
        /// <param name="nbEssais"> Le nombre de tentatives du joueur avant la fin de la partie.</param>
        /// <param name="tempsPartie"> Le temps qu'à duré la partie.</param>
        public static void EnregistrerStatistiques(string mot, int nbLettres, bool success, int nbEssais, decimal tempsPartie)
        {
            string historiquePath = "../../../historique.txt";

            // On crée le fichier s'il n'existe pas
            if (!File.Exists(historiquePath))
            {
                File.AppendAllLines(historiquePath, 
                                    new string[] { "mot,nbLettres,success,nbEssais,tempsPartie (en secondes)", 
                                        "", 
                                        "total_joue,total_success,pourcentage_success,tempsMoyenPartie", "0,0,0,0" });
            }


            string[] historique = File.ReadAllLines(historiquePath);

            // On extrait les données résumées (i.e, total_joue, total_success, etc)
            string lastLine = historique[historique.Length - 1];
            decimal totalJoue = Decimal.Parse(lastLine.Split(',')[0]);
            decimal totalSuccess = Decimal.Parse(lastLine.Split(',')[1]);
            decimal tempsMoyenPartie = Decimal.Parse(lastLine.Split(',')[3]);
            decimal pourcentageSuccess;


            // On modifie les statistiques en fonction des résultats
            tempsMoyenPartie = Decimal.Round((tempsMoyenPartie * totalJoue + tempsPartie) / (totalJoue + 1), 0);
            totalJoue++;

            if (success)
            {
                totalSuccess++;
            }

            pourcentageSuccess = Decimal.Round((totalSuccess / totalJoue) * 100);


            // On crée le writer pour écrire dans le fichier historique
            StreamWriter writer = new StreamWriter(historiquePath);

            for (int i = 0; i < historique.Length - 1; i++)
            {
                string line = historique[i];

                if (line == "")
                {
                    writer.WriteLine($"{mot},{nbLettres},{success},{nbEssais},{tempsPartie}");
                }

                writer.WriteLine(line);
            }

            // On écrit la ligne des statistiques
            writer.WriteLine($"{totalJoue},{totalSuccess},{pourcentageSuccess},{tempsMoyenPartie}");

            writer.Close();
        }

        /// <summary>
        /// Permet de jouer une partie.
        /// </summary>
        /// <returns>
        /// Le nombre de tentatives qu'à fait le joueur durant la partie
        /// </returns>
        /// <param name="difficulte"> Le tableau contenant les paramètres de difficulté définis.</param>
        /// <param name="motADeviner"> Le mot que le joueur doit deviner.</param>
        /// <param name="dicoVerif"> Tous les mots contenant le nombre de lettres définit par le joueur.</param>
        static int Jouer(int[] difficulte, String motADeviner, String[] dicoVerif)
        {
            String proposition;
            bool gagne = false;
            int nbEssaisJoueur = 0;

            String[] essais = new String[difficulte[1]];

            // Coordonnées x0 et y0 du curseur pour afficher la grille et pour écrire la réponse
            int x0Grille = Console.CursorTop;
            int y0Grille = Console.CursorLeft;
            int x0Rep;
            int y0Rep;


            // Objet stopwatch pour capturer le temps d'une partie
            Stopwatch timer = new Stopwatch();

            AfficherGrille(essais, difficulte[0], difficulte[1], motADeviner);
            
            x0Rep = Console.CursorTop;
            y0Rep = Console.CursorLeft;


            for (int i = 0; i < difficulte[1]; i++)
            {

                timer.Start();

                nbEssaisJoueur++;
                
                // On redemande d'entrer le mot tant qu'il n'est pas valide
                do
                {
                    Console.SetCursorPosition(y0Rep, x0Rep);
                    Console.WriteLine("Veuillez entrer votre proposition :");
                    Console.Write("                                                \r");
                    Console.WriteLine($"\n\n\n\n\n\n {motADeviner}");

                    proposition = Console.ReadLine().ToLower();

                } while (!VerifierMot(proposition, difficulte[0], dicoVerif));

                essais[i] = proposition;

                // On change les positions du curseur pour réécrire la grille par dessus l'ancienne
                Console.SetCursorPosition(y0Grille, x0Grille);
                AfficherGrille(essais, difficulte[0], difficulte[1], motADeviner);

                // Si le mot entré correspond au mot à deviner
                if (motADeviner.Equals(proposition))
                {
                    timer.Stop();

                    Console.WriteLine("                                   ");
                    Console.WriteLine("Vous avez gagné en {0} propositions et {1} secondes, bravo !", i + 1,
                        Decimal.Round((decimal)timer.Elapsed.TotalSeconds, 0));

                    gagne = true;

                    break;
                }
            }
            
            // Si la partie se termine sans que le joueur ait gagné
            if (!gagne)
            {
                Console.WriteLine("                                   ");
                Console.WriteLine("Vous avez perdu... Le mot à trouver était : {0}", motADeviner);
            }

            EnregistrerStatistiques(motADeviner, difficulte[0], gagne, nbEssaisJoueur, Decimal.Round((decimal)timer.Elapsed.TotalSeconds, 0));

            return nbEssaisJoueur;
        }

        public static int Main(string[] args)
        {
            // Tableau contenant les paramètres de difficulté avec : 
            // 0 : le nombre de lettres du mot à deviner
            // 1 : le nombre de tentatives pour deviner le mot
            // 2 : le temps imparti en secondes si le joueur en veut un, -1 sinon

            int[] difficulte = new int[3];
            String motADeviner;
            int temps;

            difficulte = InitialiserGame();

            // On convertit le temps en millisecondes
            temps = difficulte[2] * 1000;

            string[] dicoVerif = LireFichier(difficulte[0]);
            motADeviner = GenererMot(dicoVerif);

            // Si le joueur ne veut pas de temps imparti
            if (difficulte[2] == -1)
            {
                Jouer(difficulte, motADeviner, dicoVerif);
            }
            else
            {
                //
                // On sort de la fonction Jouer si elle met plus du temps imparti "temps" pour être exécutée
                //

                var task = Task.Run(() =>
                {
                    return Jouer(difficulte, motADeviner, dicoVerif);
                });

                bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(temps));

                if (isCompletedSuccessfully)
                {
                    return task.Result;
                }
                else
                {
                    Console.WriteLine($"\nTemps écoulé, vous avez perdu... Le mot a trouver était {motADeviner}");
                    EnregistrerStatistiques(motADeviner, difficulte[0], false, -1, -1);

                }
            }

            System.Threading.Thread.Sleep(3000);

            return 0;

        }
    }
}
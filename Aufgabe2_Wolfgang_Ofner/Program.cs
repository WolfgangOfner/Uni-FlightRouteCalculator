// ----------------------------------------------------------------------- 
// <copyright file="Program.cs" company="FHWN"> 
// Copyright (c) FHWN. All rights reserved. 
// </copyright> 
// <summary>This program is for calculating flight routes.</summary> 
// <author>Wolfgang Ofner</author> 
// -----------------------------------------------------------------------
namespace Aufgabe2_Wolfgang_Ofner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class which contains the main part of the program.
    /// </summary>
    /// <param></param>
    public class Program
    {
        /// <summary>
        /// Main part of the program.
        /// </summary>
        /// <param name="args">Contains -f for flight plan, -q for query.</param>
        public static void Main(string[] args)
        {
            Flightplan plan = new Flightplan();
            string flightplan = "-f";
            string query = "-q";
            string help = "help";
            string try_help;
            int status = 0;
            int before_route_calculation;
            int after_route_calculation;
            bool empty;
            bool syntax_error = false;                                               // needed later for checking if the flight list and query input was correct

            if (args.GetLength(0) > 0)
            {                                                                       // if args contains something
                if (args[0].Equals(help))
                {                                                                   // if args contains help   
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nWith this program you can insert and calculate flight plan lists. \n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("The command line argument -f starts the flight plan list. \nFor example programm.exe -f paris-london london-mailand.\n");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("The command line argument -q starts the query list. If you use -q you must \nhave already insert at least one flight route. \nFor example programm.exe -f paris-london -q paris-london\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("If you use only -f and insert your flight list, you start the half-interactive \nmode. In this mode you will be asked to insert your query. \nFor example -f paris-london\n");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("If you start the program without a command line parameter, you start the \ninteractive mode. In this mode you will be asked to insert the flight plan and \nthe query.\n\n");
                    Console.ResetColor();
                    Environment.Exit(0);
                }
            }

            for (int i = 0; i < args.GetLength(0); i++)
            {
                if (args[i].Equals(flightplan) || args[i].Equals(query))
                {                                                                   // counts how often -f and -q is in args to know to start which modus
                    status++;
                }
            }

            switch (status)
            {
                case 0:                                                             // interactive mode
                    if (args.GetLength(0) > 0)
                    {                                                               // if args doesn't contain -f or -q and contains something
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nMissing parameter -f or -q!\n");
                        Environment.Exit(1);
                    }

                    Console.WriteLine();                                            // for space between comandline start and first flight plan request
                    do
                    {                                                                // flight plan input with the format paris-london                             
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nPlease enter your flight plan, for example paris-london (for help enter help):");
                        Console.ResetColor();
                        string input = Console.ReadLine();
                        empty = string.IsNullOrWhiteSpace(input);
                        if (empty == true)
                        {                                                            // if there was no input break inputrequest and jump to the query input
                            break;
                        }

                        try_help = Flightplan.Delete_spaces(input);                  // goes to the Delete_spaces method to delete all spaces befor the first letter and after the last one
                        if (try_help.Equals(help))
                        {                                                            // if user typs in help   
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nYou are in the interactive mode. At first enter your flight routes like \nparis-london. You can enter flight routes as long as press enter without an \nflight route input.\n");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("After you have pressed enter the route query starts. Here you must insert your \ndeparture and arrival city. The programm will calculate and print all routes.\n");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("You can insert routes until you press the enter key without inputting a text. \nThis will end the program.\n\n");
                            Console.ResetColor();
                            continue;
                        }

                        plan.Add_interactive(input);                                 // add insert route to the list plan   
                    }
                    while (empty == false);

                    do
                    {                                                               // query request
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\nPlease enter query (for help enter help):");
                        Console.ResetColor();
                        string input = Console.ReadLine();
                        Console.WriteLine();
                        empty = string.IsNullOrWhiteSpace(input);
                        if (empty == true)
                        {                                                           // if there was no input break inputreqest and terminate the program
                            break;
                        }

                        try_help = Flightplan.Delete_spaces(input);                 // goes to the Delete_spaces method to delete all spaces befor the first letter and after the last one
                        if (try_help.Equals(help))
                        {                                                           // if user typs in help   
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Here you must insert your departure and arrival city like paris-london. The \nprogramm will calculate and print all routes\n");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("You can insert routes until you press the enter key without inputting a text. \nThis will end the program\n\n");
                            Console.ResetColor();
                            continue;
                        }

                        before_route_calculation = plan.Route_printed;
                        plan.Check_number_of_cities(input, ref syntax_error);
                        after_route_calculation = plan.Route_printed;

                        if (before_route_calculation == after_route_calculation && syntax_error == false)
                        {                                                                               // if destination count == 0 (at the end) and no route was printed --> no route was found
                            string[] split_query = input.Split(new char[] { '-' });
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No route found from {0} to {1}!\n", split_query[0], split_query[1]);
                            Console.ResetColor();
                            continue;
                        }
                    }
                    while (empty == false);

                    break;                                                          // break switch and so the program will be terminated

                case 1:
                    Console.WriteLine();
                    if (args.GetLength(0) < 2)
                    {                                                               // if args contains -f or -q and nothing else -> error
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nEmpty lists are not allowed!\n");
                        Console.ResetColor();
                        Environment.Exit(1);
                    }

                    if (args[0].Equals(flightplan))
                    {                                                               // if command line argument starts with -f add all elements after -f to the list
                        for (int i = 1; i < args.GetLength(0); i++)
                        {
                            plan.Add(args[i]);
                        }
                    }
                    else
                    {                                                               // jump to the error message
                        plan.Flightplan_empty();
                    }

                    do
                    {                                                               // query input    
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Please enter query (for help enter help):");
                        Console.ResetColor();
                        string input = Console.ReadLine();
                        Console.WriteLine();
                        empty = string.IsNullOrWhiteSpace(input);
                        if (string.IsNullOrWhiteSpace(input) == true)
                        {                                                           // if there was no input, break and terminate the program
                            break;
                        }

                        try_help = Flightplan.Delete_spaces(input);                 // goes to the Delete_spaces method to delete all spaces befor the first letter and after the last one
                        if (try_help.Equals(help))
                        {                                                           // if user inserts help
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Here you must insert your departure and arrival city like paris-london. The \nprogramm will calculate and print all routes\n");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("You can insert routes until you press the enter key without inputting a text. \nThis will end the program\n\n");
                            Console.ResetColor();
                            continue;
                        }

                        before_route_calculation = plan.Route_printed;
                        plan.Check_number_of_cities(input, ref syntax_error);
                        after_route_calculation = plan.Route_printed;

                        if (before_route_calculation == after_route_calculation && syntax_error == false)
                        {                                                                               // if destination count == 0 (at the end) and no route was printed --> no route was found
                            string[] split_query = input.Split(new char[] { '-' });
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No route found from {0} to {1}!\n", split_query[0], split_query[1]);
                            Console.ResetColor();
                            continue;
                        }
                    }
                    while (empty == false);

                    break;                                                           // break switch and so the program will be terminated   

                case 2:
                    Console.WriteLine();
                    for (int i = 0; i < args.GetLength(0); i++)
                    {                                                                // if -q is the last element of args  
                        if (args.GetLength(0) == i + 1 && args[i].Equals(query))
                        {                                                           // go to the method with the error message for empty query lists
                            plan.Query_empty();
                        }

                        if (args[i].Equals(query) && args[i + 1].Equals(flightplan))
                        {                                                            // if args[i] contains -q and the next element of args contains -f --> query list is empty
                            plan.Query_empty();                                      // go to the method with the error message for empty query lists
                        }

                        if (args[i].Equals(query) | args[i].Equals(flightplan) && args.GetLength(0) == i)
                        {                                                           // if -q or -f is the last element of args --> there is a empty list
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nEmpty lists are not allowed!\n");
                            Console.ResetColor();
                            Environment.Exit(1);
                        }
                    }

                    if (args[0].Equals(flightplan))
                    {                                                               // if args starts with the flight list
                        for (int i = 1; i < args.GetLength(0); i++)
                        {                                                           // starts at the first element after -f
                            if (args[i].Equals(query))
                            {                                                       // until args[i] contains -q
                                for (int j = i + 1; j < args.GetLength(0); j++)
                                {                                                   // starts at the first element after -q
                                    string[] split_query = args[j].Split(new char[] { '-' });
                                    if (split_query.Length == 1 || string.IsNullOrWhiteSpace(split_query[0]) || string.IsNullOrWhiteSpace(split_query[1]))
                                    {                                               // checks if there was no input of two citiies in the format paris-london
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("\nSyntax error in your query list!\n");
                                        Console.ResetColor();
                                        Environment.Exit(1);
                                    }

                                    plan.Check_query(split_query);                      // go to the method to check the insert query
                                }
                                //// second for loop to check query list before calculating routes
                                for (int j = i + 1; j < args.GetLength(0); j++)
                                {                                                       // split query into start and arrival city
                                    before_route_calculation = plan.Route_printed;
                                    string[] split_query = args[j].Split(new char[] { '-' });
                                    plan.Find_route(split_query[0], split_query[1]);    // go to method to calculate the route, split_query[0] == start city split_query[1] == arrival city
                                    after_route_calculation = plan.Route_printed;

                                    if (before_route_calculation == after_route_calculation && syntax_error == false)
                                    {                                                                               // if destination count == 0 (at the end) and no route was printed --> no route was found
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("No route found from {0} to {1}!\n", split_query[0], split_query[1]);
                                        Console.ResetColor();
                                        continue;
                                    }
                                }

                                break;                                                  // ends the loop after -q was found and the route was calculated
                            }
                            else
                            {                                                           // if args[i] haven't containt -q add the insert route to plan
                                plan.Add(args[i]);
                            }
                        }
                    }
                    //// if args starts with -q
                    if (args[0].Equals(query))
                    {                                                                   // start at the first element after -q
                        for (int i = 1; i < args.GetLength(0); i++)
                        {                                                               // until args[i] contains -f
                            if (args[i].Equals(flightplan))
                            {                                                           // starts at the first element after -f
                                for (int j = i + 1; j < args.GetLength(0); j++)
                                {                                                       // adds all elements to the list plan
                                    plan.Add(args[j]);
                                }
                            }
                        }
                        //// starts at the first element after -q
                        for (int i = 1; i < args.GetLength(0); i++)
                        {                                                               // if args[i] contains -f go to the next element
                            if (args[i].Equals(flightplan))
                            {
                                continue;
                            }

                            string[] split_query = args[i].Split(new char[] { '-' });   // splits the elements in args[i]
                            if (string.IsNullOrWhiteSpace(split_query[0]) || string.IsNullOrWhiteSpace(split_query[1]))
                            {                                                           // if there was a error
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nSyntax error in your query list!");
                                Console.ResetColor();
                                Environment.Exit(1);
                            }

                            plan.Check_query(split_query);                              // go to the method to check the insert query
                        }

                        for (int j = 1; j < args.GetLength(0); j++)
                        {                                                               // starts at the first element after -q
                            if (args[j].Equals(flightplan))
                            {                                                           // if args[j] contains -f --> end of query list and break
                                break;
                            }

                            string[] split_query = args[j].Split(new char[] { '-' });   // split the query
                            before_route_calculation = plan.Route_printed;
                            plan.Find_route(split_query[0], split_query[1]);    // go to method to calculate the route, split_query[0] == start city split_query[1] == arrival city
                            after_route_calculation = plan.Route_printed;

                            if (before_route_calculation == after_route_calculation && syntax_error == false)
                            {                                                                               // if destination count == 0 (at the end) and no route was printed --> no route was found
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("No route found from {0} to {1}!\n", split_query[0], split_query[1]);
                                Console.ResetColor();
                                continue;
                            }
                        }
                    }

                    break;

                default:                                                                // if there were more than one -f or -f
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nYou have insert too many command line arguments (-f or -q)!\n");
                    Console.ResetColor();
                    Environment.Exit(1);
                    break;
            }

            Environment.Exit(0);
        }
    }
}
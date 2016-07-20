// ----------------------------------------------------------------------- 
// <copyright file="Flightplan.cs" company="FHWN"> 
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
    /// Contains the class for creating and calculating the flight routes.
    /// </summary>
    public class Flightplan
    {
        /// <summary>
        /// Creates a new list which will contain the flight plans.
        /// </summary>
        private List<string> plan = new List<string>();

        /// <summary>
        /// List which will contain already visited cities as protection against circles.
        /// </summary>
        private List<string> alreadyThere = new List<string>();

        /// <summary>
        /// Gets or sets an integer to compare if there is a difference before calculating the routes and after.
        /// </summary>
        /// /// <value>Contains a different or the same value as before the route calculation.</value>
        public int Route_printed { get; set; }              // Get for checking if Route_printed is different than befor the calculation, Set to increase the count if there was a route found

        /// <summary>
        /// Method deletes all spaces before the first letter of the input and after the last letter.
        /// </summary>
        /// <param name="input">String contains the input from the user.</param>
        /// <returns>String with the input from the user without spaces before the first letter and after the last one.</returns>
        public static string Delete_spaces(string input)
        {
            int length = input.Length;
            bool empty_test;

            for (int i = 0; i < length; i++)
            {                                                                           // loop for deleting all spaces after the last letter
                empty_test = input.Substring(input.Length - 1, 1).Equals(" ");          // checks if the last char of the string is a space
                if (empty_test == true)
                {                                                                       // if a space was found
                    input = input.Remove(input.Length - 1);                             // delete the last char
                }
                else
                {                                                                       // if no space was found --> all spaces were removed (or the string contained non)
                    length = input.Length;                                              // input has a new length after removing the spaces
                    break;                                                              // breaks the loop
                }
            }

            for (int i = 0; i < length; i++)
            {                                                                           // loop for deleting all spaces before the first letter
                empty_test = input.Substring(0, 1).Equals(" ");                         // checks if the first char of the string is a space
                if (empty_test == true)
                {                                                                       // if a space was found
                    input = input.Remove(0, 1);                                         // deletes the first char
                }
                else
                {                                                                       // if no space was found --> all spaces were removed (or the string contained non)
                    break;                                                              // breaks the loop
                }
            }

            return input;                                                               // returns the input without the spaces
        }

        /// <summary>
        /// Method to create the flight plan list. Also checks the correct input format like city-city.
        /// </summary>
        /// <param name="input">Input from user from flight plan list.</param>
        public void Add(string input)
        {
            bool already_in_flightlist = false;
            string[] split_flightplan = input.Split(new char[] { '-' });                                                // split input at -

            if (split_flightplan.Length != 2 || string.IsNullOrWhiteSpace(split_flightplan[0]) || string.IsNullOrWhiteSpace(split_flightplan[1]))
            {                                                                                                           // error if there where to many cities or to less
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSyntax error in your flight plan list. You have insert too many or too little cities!\n");
                Console.ResetColor();
                Environment.Exit(1);
            }

            if (split_flightplan[0].Equals(split_flightplan[1]))
            {                                                                                                           // if start city == end city
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSyntax error in your flight plan list. \nThe starting city can't be de destination city!\n");
                Console.ResetColor();
                Environment.Exit(1);
            }

            for (int i = 0; i < this.plan.Count; i++)
            {
                if (input.Equals(this.plan[i]))
                {                                                                                                       // checks if the route is already in the list
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nNote: \nYou have entered at least two equal flight routes. The duplicate was deleted. \nThe program will now go on normally.\n");
                    Console.ResetColor();
                    already_in_flightlist = true;                                                                       // if error bool = true to restart the request
                }
            }

            if (already_in_flightlist == false)
            {                                                                                                           // if no error --> add route to list
                this.plan.Add(input);
            }
        }

        /// <summary>
        /// Method to create the flight plan list for the interactive mode. Also checks the correct input format like city-city.
        /// </summary>
        /// <param name="input">Input from user from flight plan list.</param>
        public void Add_interactive(string input)
        {
            bool error = false;

            input = Delete_spaces(input);                                                                               // method for deleting        

            string[] split_flightplan = input.Split(new char[] { ' ', '-' });                                           // split input at -           

            if (split_flightplan.Length != 2 || string.IsNullOrWhiteSpace(split_flightplan[0]) || string.IsNullOrWhiteSpace(split_flightplan[1]))
            {                                                                                                           // error if there where to many cities or to less
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSyntax error in your query list. You have insert too many or too little cities! The correct format is city-city.\n");
                Console.ResetColor();
                error = true;
            }

            if (error == false && split_flightplan[0].Equals(split_flightplan[1]))
            {                                                                              // if the starting city == end city
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSyntax error in your flight plan list. \nThe starting city can't be de destination city!\n");
                Console.ResetColor();
                error = true;
            }

            if (!input.Contains("-") && error == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSyntax error in your query list. The cities must divieded by a \"-\"! The correct format is city-city.\n");
                Console.ResetColor();
                error = true;
            }

            for (int i = 0; i < this.plan.Count; i++)
            {
                if (input.Equals(this.plan[i]))
                {                                                                                                       // checks if the route is already in the list
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nYou have already entered this flight routes. Duplicates are not allowed.\n");
                    Console.ResetColor();
                    error = true;                                                                       // if error bool = true to restart the request
                }
            }

            if (error != true)
            {                                                                               // if there was no error
                input = input.Replace(" ", string.Empty);                                   // deletes spaces in the input befor adding to the list
                this.plan.Add(input);
                error = false;
            }
        }

        /// <summary>
        /// Method prints an error message if flight plan list is empty.
        /// </summary>
        public void Flightplan_empty()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nQuery without a flightplan is not possible!\n");
            Console.ResetColor();
            Environment.Exit(1);
        }

        /// <summary>
        /// Method prints an error message if the query list is empty.
        /// </summary>
        public void Query_empty()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nUsing -q without an insert query list is invalid!\n");
            Console.ResetColor();
            Environment.Exit(1);
        }

        /// <summary>
        /// Method to check if the query cities are in the flight plan list.
        /// </summary>
        /// <param name="split_query">Array contains the divided query list.</param>
        public void Check_query(string[] split_query)
        {
            bool contains_city;
            int number_of_cities = 0;                                                       // number will be increased if the wanted city is found. If both cities was found the number is 2. If not there was an error
            string start_city = split_query[0];
            string arrival_city = split_query[1];

            for (int i = 0; i < this.plan.Count; i++)
            {                                                                                // takes everey element of the list   
                string[] split_flightplan = this.plan[i].Split(new char[] { '-' });         // splits the found element
                for (int ii = 0; ii < split_flightplan.Length; ii++)
                {                                                                            // searches the divided elements for the starting city
                    contains_city = split_flightplan[ii].Equals(start_city);
                    if (contains_city == true)
                    {                                                                       // if the starting city was found increas number_of_cities and break
                        number_of_cities++;
                        break;
                    }
                }

                if (number_of_cities == 1)
                {                                                                           // if the starting city was found break the loop
                    break;
                }
            }

            if (number_of_cities == 1)
            {                                                                               // searching for arrival city is only necessery if the starting city was found
                for (int j = 0; j < this.plan.Count; j++)
                {                                                                               // takes everey element of the list 
                    string[] split_flightplan = this.plan[j].Split(new char[] { '-' });         // splits the found element
                    for (int jj = 0; jj < split_flightplan.Length; jj++)
                    {                                                                           // searches the divided elements for the arrival city
                        contains_city = split_flightplan[jj].Equals(arrival_city);
                        if (contains_city == true)
                        {                                                                       // if the arrival city was found increas number_of_cities and break
                            number_of_cities++;
                            break;
                        }
                    }

                    if (number_of_cities == 2)
                    {                                                                           // if the arrival city was found break the loop
                        break;
                    }
                }
            }

            if (number_of_cities != 2)
            {                                                                               // if both cities were not found                             
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nUnknown city in your query list!\n");
                Console.ResetColor();
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Method checks the number of cities which were insert.
        /// </summary>
        /// <param name="input">String contains the insert cities.</param>
        /// <param name="error">Signals if the input was correct or not.</param>
        public void Check_number_of_cities(string input, ref bool error)
        {
            input = Delete_spaces(input);
            error = false;
            string[] split_query = input.Split(new char[] { ' ', '-' });                                     // splits the input at the -
            if (split_query.Length != 2 || string.IsNullOrWhiteSpace(split_query[0]) || string.IsNullOrWhiteSpace(split_query[1]))
            {                                                                                           // if there aren't two elements (if input was only london) or one of the two elements is empty (if input was london- or -london)
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSyntax error in your query list. You have insert too many or too little cities!\n");
                Console.ResetColor();
                error = true;                                                                           // signals the programm that here was an error and restart the request
            }

            if (!input.Contains("-"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSyntax error in your query list. The cities must divieded by a \"-\"! The correct format is city-city.\n");
                Console.ResetColor();
                error = true;
            }

            if (error == false)
            {                                                                                           // if there was no error go to the Check_query_mode method to check if the insert cities are in the flight plan list
                this.Check_query_mode(split_query, ref error);
            }
        }

        /// <summary>
        /// Method creates a list with the already visited cities (including the current city).
        /// </summary>
        /// <param name="start_city">String which contains the departure city.</param>
        /// <param name="arrival_city">String which contains the arrival city.</param>
        public void Find_route(string start_city, string arrival_city)
        {
            List<string> alreadyThere = new List<string>();
            if (this.alreadyThere.Contains(start_city))
            {                                                                                   // if the starting city is already in the alreadyThere list, to avoid routes like london-london-paris
                this.Find_route(start_city, this.alreadyThere, arrival_city);                   // goes to the method Find_route
            }
            else
            {                                                                                   // if the starting city isn't already in the alreadyThere list
                this.alreadyThere.Clear();                                                      // removes all elements from older searches
                this.alreadyThere.Add(start_city);                                              // adds the starting city to the list
                this.Find_route(start_city, this.alreadyThere, arrival_city);                   // goes to the method Find_route
            }
        }

        /// <summary>
        /// Method to find the a route to the arrival city.
        /// </summary>
        /// <param name="start_city">String which contains the current city.</param>
        /// <param name="already_there">List which contains all already visited cities as protection for circles.</param>
        /// <param name="arrival_city">String which contains the arrival city.</param>
        private void Find_route(string start_city, List<string> already_there, string arrival_city)
        {
            List<string> destination = new List<string>();
            this.Find_arrival_city(start_city, ref destination);                            // finds all avaible cities where the programm can flies from the starting cities and ref them in destination

            for (int i = 0; i < destination.Count; i++)
            {                                                                               // takes every element from destination
                bool already_visited = already_there.Contains(destination[i]);
                if (already_visited == false)
                {                                                                           // if the taken city is not in the already_there list
                    if (destination[i] == arrival_city)
                    {                                                                       // if the taken city is the arrival city
                        this.Print(already_there, arrival_city);                            // print the route
                        this.Route_printed++;                                               // increases found route to signal that there was a route printed
                    }
                    else
                    {
                        already_there.Add(destination[i]);                                  // add the taken city to the already_there list
                        this.Find_route(destination[i], already_there, arrival_city);       // start the search agaon (recursion)
                    }
                }
                else
                {                                                                           // if the element was already in the list go to the next element
                    continue;
                }

                already_there.Remove(destination[i]);                                       // removes visited elements
            }
        }

        /// <summary>
        /// Method to check if there is a direct flight from the departure city to the arrival city. 
        /// </summary>
        /// <param name="start_city">String which contains the departure city.</param>
        /// <param name="arrival_city">List which contains all reachable cities from the starting city.</param>
        private void Find_arrival_city(string start_city, ref List<string> arrival_city)
        {
            for (int i = 0; i < this.plan.Count; i++)
            {                                                           // takes everey element from the plan list
                string[] split_query;
                split_query = this.plan[i].Split(new char[] { '-' });   // splits the element at the -

                if (start_city == split_query[0])
                {                                                       // take the element if there is a direct way
                    arrival_city.Add(split_query[1]);
                }
            }
        }

        /// <summary>
        /// Method to check if the query cities are in the flight plan list.
        /// </summary>
        /// <param name="split_query">Array contains the divided query list.</param>
        /// <param name="error">Signals if the input was correct or not.</param>
        private void Check_query_mode(string[] split_query, ref bool error)
        {
            bool contains_city;
            int number_of_cities = 0;                                                                   // int to know at the end how many cities were found. if number_of_cities != 2 --> error
            string start_city = split_query[0];
            string arrival_city = split_query[1];

            for (int i = 0; i < this.plan.Count; i++)
            {                                                                                           // takes everey element from the list plan
                string[] split_flightplan = this.plan[i].Split(new char[] { '-' });                      // splits every element at the -           
                for (int ii = 0; ii < split_flightplan.Length; ii++)
                {
                    contains_city = split_flightplan[ii].Equals(start_city);
                    if (contains_city == true)
                    {                                                                                   // if the element contains the wanted city
                        number_of_cities++;                                                             // increas number_of_cities
                        break;                                                                          // break because the starting city is wanted only once
                    }
                }

                if (number_of_cities == 1)
                {                                                                                       // breakes the loop if the starting city was found
                    break;
                }
            }

            if (number_of_cities == 1)
            {                                                                                           // searching for arrival city is only necessery if the starting city was found
                for (int j = 0; j < this.plan.Count; j++)
                {                                                                                           // takes everey element from the list plan
                    string[] split_flightplan = this.plan[j].Split(new char[] { '-' });                     // splits every element at the - 
                    for (int jj = 0; jj < split_flightplan.Length; jj++)
                    {
                        contains_city = split_flightplan[jj].Equals(arrival_city);
                        if (contains_city == true)
                        {                                                                                   // if the element contains the wanted city
                            number_of_cities++;                                                             // increases the number of cities
                            break;                                                                          // break because the starting city is wanted only once
                        }
                    }

                    if (number_of_cities == 2)
                    {                                                                                       // breaks the loop if booth cities were found
                        break;
                    }
                }
            }

            if (number_of_cities != 2)
            {                                                                                           // if both cities weren't found
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nUnknown city in your query list!\n");
                Console.ResetColor();
                error = true;                                                                           // signals that here was an error and the programm has to go back to the input request
            }
            else
            {                                                                                           // if not go to the Find_route mehtod
                error = false;
                this.Find_route(start_city, arrival_city);
            }
        }

        /// <summary>
        /// Method for printing the found route.
        /// </summary>
        /// <param name="already_there">List which contains the found route.</param>
        /// <param name="arrival_city">String which contains the arrival city.</param>
        private void Print(List<string> already_there, string arrival_city)
        {
            string print = string.Empty;                        // empty string
            for (int i = 0; i < already_there.Count; i++)
            {                                                   // takes everey element from the already_there list
                string dart = " -> ";
                print = print + already_there[i];               // adds the taken city from already_there to the print
                print = print + dart;                           // adds a dart sign between the cities
            }

            print = print + arrival_city;                       // in the end adds the arrival city (not in the loop becaus there shouldn't be a dart sign afterwards)

            Console.ForegroundColor = ConsoleColor.Yellow;      // prints the route and calls the starting and arrival city
            Console.WriteLine("Following route found from {0} to {1}:", already_there[0], arrival_city);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(print);
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
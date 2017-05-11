using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XamarinRui
{   
    public class PersonsProvider
    {
        /// <summary>
        /// Just a static list of persons
        /// </summary>
        private List<Person> _persons = new List<Person>()
        {
            new Person()
            {
                Name = "Jayson Street",
                Description = "Hacker author. Globetrotting Infosec Ranger.",
                TwitterHandle = "@jaysonstreet",
                Website = "http://v3rb0t3n.com/"
            },
            new Person()
            {
                Name = "Scott Helme",
                Description = "Information Security Consultant, blogger.",
                TwitterHandle = "@Scott_Helme",
                Website = "https://scotthelme.co.uk/"
            },
            new Person()
            {
                Name = "Dawid Golunski",
                Description = "A digital nomad and ethical hacker.",
                TwitterHandle = "@dawid_golunski",
                Website = "https://legalhackers.com/"
            },
            new Person()
            {
                Name = "Levente Fuksz",
                Description = "Coder and InfoSec Enthusiast",
                TwitterHandle = "@levifuksz",
                Website = "https://blog.iamlevi.net"
            }
        };

        /// <summary>
        /// Returns a list of persons based on an optional search term
        /// </summary>
        /// <param name="search">Search term for the person's name</param>
        public async Task<List<Person>> GetPersons(string search = "")
        {
            // Simulate some loading time
            return await Task.Delay(800)
                .ContinueWith((task) =>
                {
                    return string.IsNullOrEmpty(search) ?
                        _persons :
                        _persons.Where(p => p.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1).ToList();
                });
        }
    }
}

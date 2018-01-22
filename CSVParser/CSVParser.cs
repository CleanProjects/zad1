using System;
using System.Collections.Generic;
using System.IO;

namespace Parsers
{
    public class CSVParser
    {
        private string filepath;

        private List<MailPerson> MailingList;

        public CSVParser(string filepath)
        {
            try
            {
                int i = 0;
                StreamReader sr = new StreamReader(filepath);

                MailingList = new List<MailPerson>();
            }
            catch (Exception e)
            {
                Console.WriteLine("No file to parse");
                Console.ReadKey();
                Environment.Exit(1);
            }


            this.filepath = filepath;
        }

        public List<MailPerson> Parse()
        {
            Console.WriteLine("Parsing");
            using (StreamReader sr = new StreamReader(filepath))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    var split = line.Split(",");
                    MailingList.Add(new MailPerson(split[0], split[1], split[2]));
                }
            }
            return MailingList;
        }
    }

    public class MailPerson
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public MailPerson(string name, string surname, string email)
        {
            this.Name = name;
            this.Surname = surname;
            this.Email = email;
        }

        public void describe()
        {
            Console.WriteLine(String.Format("{0} {1}: {2}", Name, Surname, Email));
        }
    }
}

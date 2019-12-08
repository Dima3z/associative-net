using System;
using System.Collections.Generic;
using System.IO;
using AssociativeNetwork.Helpers;

namespace AssociativeNetwork.Models
{
    public class QuestionHolder
    {
        public QuestionHolder()
        {
            Questions = new List<Question>();
            Nodes = new HashSet<string>();
        }

        public IList<Question> Questions { get; set; }
        public ISet<string> Nodes { get; set; }

        public static QuestionHolder LoadFromTxt(string filename, char separator = '.', bool randomize = true)
        {
            var holder = new QuestionHolder();
            IEnumerable<string> data = null;
            try
            {
                data = File.ReadLines(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Файл с вопросами не найден, либо произошла ошибка ввода:\r\n{e}");
                Environment.Exit(1);
            }

            foreach (var line in data)
            {
                var parsed = line.Split(separator);
                if (parsed[0][0] == '#')
                    continue;
                holder.Questions.Add(
                    new Question(
                        parsed[0],
                        parsed[1],
                        parsed[2] == "+",
                        parsed[3]));
                holder.Nodes.Add(parsed[0]);
                holder.Nodes.Add(parsed[1]);
            }

            if (randomize)
                holder.Questions.Shuffle();
            return holder;
        }
    }
}
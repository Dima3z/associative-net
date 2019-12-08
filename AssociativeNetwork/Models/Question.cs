using System;

namespace AssociativeNetwork.Models
{
    public class Question
    {
        public bool Answer;
        public TimeSpan? AnswerTime;
        public double? Dispersion;
        public bool Error;
        public string FirstNode;
        public string SecondNode;

        public string Text;

        // For deserialization
        public Question()
        {
        }

        public Question(string a, string b, bool answer, string text)
        {
            FirstNode = a;
            SecondNode = b;
            Answer = answer;
            Text = text;
        }

        public Question(Question parent, TimeSpan averageTime, double dispersion)
        {
            FirstNode = parent.FirstNode;
            SecondNode = parent.SecondNode;
            Text = parent.Text;
            AnswerTime = averageTime;
            Dispersion = dispersion;
            Answer = parent.Answer;
            Error = parent.Error;
        }

        public bool? Ask()
        {
            var checkpoint = DateTime.Now;
            Console.WriteLine(Text);
            Console.WriteLine("[a/ф] - Истина \t [d/в] - Ложь \t [x/ч] - Закончить опрос");
            while (true)
                switch (Console.ReadKey(true).KeyChar)
                {
                    case 'a':
                    case 'ф':
                        AnswerTime = DateTime.Now - checkpoint;
                        Error = !Answer;
                        return Error;
                    case 'd':
                    case 'в':
                        AnswerTime = DateTime.Now - checkpoint;
                        Error = Answer;
                        return Error;
                    case 'x':
                    case 'ч':
                        return null;
                    default:
                        continue;
                }
        }

        public override string ToString()
        {
            return
                $"{nameof(Text)}: {Text},\r\n" +
                $"First item: {FirstNode},\r\n" +
                $"Second item: {SecondNode},\r\n" +
                (!AnswerTime.HasValue ? string.Empty : $" {nameof(AnswerTime)}: {AnswerTime},\r\n") +
                (!Dispersion.HasValue ? string.Empty : $" {nameof(Dispersion)}: {Dispersion},\r\n") +
                //$"{nameof(_error)}: {_error},\r\n" +
                $"{nameof(Answer)}: {Answer}";
        }
    }
}
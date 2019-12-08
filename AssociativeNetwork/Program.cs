using System;
using System.IO;
using System.Linq;
using AssociativeNetwork.Helpers;
using AssociativeNetwork.Models;

namespace AssociativeNetwork
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Выберите один из следующих вариантов:");
                Console.WriteLine("[q/й] - Начать опрос");
                //Console.WriteLine("[w/ц] - Обработать результаты");
                Console.WriteLine("[e/у] - Выход");
                switch (Console.ReadKey(true).KeyChar)
                {
                    case 'q':
                    case 'й':
                        StartTest();
                        return;
                    case 'w':
                    case 'ц':
                        ProcessData();
                        return;
                    case 'e':
                    case 'у':
                        return;
                }
            }
        }

        private static void StartTest()
        {
            Console.WriteLine("Загрузка вопросов...");
            var holder = QuestionHolder.LoadFromTxt("questions.csv");
            Console.WriteLine("Тут я объясняю правила теста, но я их уже сообщил");
            Console.WriteLine("Пожалуйста, введите своё имя:");

            var testResult = new TestResult(Console.ReadLine());

            Console.Clear();
            Console.WriteLine("Вопрос появится здесь");
            Console.WriteLine("[a/ф] - Истина \t [d/в] - Ложь \t [x/ч] - Закончить опрос");
            Console.WriteLine("Нажмите на любую кнопку, чтобы начать тест");
            Console.ReadKey(true);
            Console.Clear();
            foreach (var question in holder.Questions)
            {
                var result = question.Ask();
                if (result == null)
                    break;
                Console.WriteLine("Нажмите на любую кнопку, чтобы показать следующий вопрос");
                Console.ReadKey(true);
                Console.Clear();
            }

            Console.WriteLine("Спасибо, это всё!");
            testResult.Result = holder;
            testResult.Save();
        }

        private static void ProcessData()
        {
            Console.WriteLine("Загрузка результатов вопросов...");
            var files =
                Directory.GetFiles(
                    Directory.GetCurrentDirectory(),
                    "TR_*.json", SearchOption.TopDirectoryOnly);
            var testResults = files
                .Select(BaseHelper.LoadJson<TestResult>)
                .ToList();

            var combinedResult = new TestResult();
            foreach (var text in testResults.SelectMany(tr => tr.Result.Questions).Select(q => q.Text))
            {
                var questions = testResults
                    .SelectMany(tr => tr.Result.Questions)
                    .Where(q => q.Text == text && q.AnswerTime.HasValue)
                    .ToArray();

                var timings = questions
                    .Select(q => q.AnswerTime.Value.TotalMilliseconds)
                    .ToArray();

                var average = timings.Average();
                var sumOfSquaresOfDifferences = timings
                    .Select(val => (val - average) * (val - average))
                    .Sum();
                var sd = Math
                    .Sqrt(sumOfSquaresOfDifferences / timings.Length);
                combinedResult.AddQuestion(new Question(questions.First(), TimeSpan.FromMilliseconds(average), sd));
            }

            combinedResult.SaveJson($"PR_{DateTime.Now:yyyy-dd-M--HH-mm-ss}.json");
            var graph = WeightedGraph.FromTestResult(combinedResult);
            File.WriteAllText($"PR_{DateTime.Now:yyyy-dd-M--HH-mm-ss}.dot", graph.ToDot());
        }
    }
}
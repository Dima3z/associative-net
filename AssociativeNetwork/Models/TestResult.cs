using System;
using System.IO;
using AssociativeNetwork.Helpers;
using Newtonsoft.Json.Linq;

namespace AssociativeNetwork.Models
{
    public class TestResult
    {
        public string Name;
        public QuestionHolder Result;
        public DateTime TestDate;

        public TestResult(string subjectName)
        {
            Name = subjectName;
            TestDate = DateTime.Now;
            Result = new QuestionHolder();
        }

        public TestResult()
        {
            Result = new QuestionHolder();
        }

        public void AddQuestion(Question q)
        {
            Result.Questions.Add(q);
            Result.Nodes.Add(q.FirstNode);
            Result.Nodes.Add(q.SecondNode);
        }

        public void Save()
        {
            this.SaveJson($"TR_{Name}_{TestDate:yyyy-dd-M--HH-mm-ss}.json");
        }

        public static TestResult Load(string filename)
        {
            var jobject = JObject.Parse(File.ReadAllText(filename));
            return jobject.ToObject<TestResult>();
        }
    }
}
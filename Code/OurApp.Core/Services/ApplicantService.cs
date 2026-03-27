using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using OurApp.Core.Models;
using OurApp.Core.Repositories;
using System.Linq;

namespace OurApp.Core.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IApplicantRepository _repository;
        private const decimal PassIndividual = 5.5m;
        private const decimal PassCollective = 7.0m;

        public ApplicantService(IApplicantRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Applicant> GetApplicantsForJob(JobPosting job)
        {
            return _repository.GetApplicantsByJob(job);
        }

        public Applicant GetApplicant(int applicantId)
        {
            return _repository.GetApplicantById(applicantId);
        }

        

        public void UpdateCompanyTestGrade(int applicantId, decimal grade)
        {
            Applicant applicant = _repository.GetApplicantById(applicantId);
            if (applicant != null)
            {
                applicant.CompanyTestGrade = grade;
                EvaluateApplicantStatus(applicant);
                _repository.UpdateApplicant(applicant);
            }
        }

        public void UpdateInterviewGrade(int applicantId, decimal grade)
        {
            Applicant applicant = _repository.GetApplicantById(applicantId);
            if (applicant != null)
            {
                applicant.InterviewGrade = grade;
                EvaluateApplicantStatus(applicant);
                _repository.UpdateApplicant(applicant);
            }
        }

        private bool IsCvValid(string xmlPath)
        {
            if (File.Exists(xmlPath) == false)
            {
                return false; 
            }
            
            try
            {
                XDocument doc = XDocument.Load(xmlPath);
                
                var nameNode = doc.Descendants("Name").FirstOrDefault();
                if (nameNode == null || string.IsNullOrWhiteSpace(nameNode.Value))
                {
                    return false;
                }

                var emailNode = doc.Descendants("Email").FirstOrDefault();
                if (emailNode == null || string.IsNullOrWhiteSpace(emailNode.Value))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private List<string> TokenizeAndClean(string text)
        {
            List<string> stopWords = new List<string> { "the", "and", "is", "a", "an", "in", "to", "of", "for" };
            
            // Remove punctuation manually
            string cleanText = "";
            foreach (char c in text)
            {
                if (char.IsPunctuation(c))
                {
                    cleanText += " ";
                }
                else
                {
                    cleanText += c;
                }
            }

            // Split into words
            string[] rawWords = cleanText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> finalWords = new List<string>();

            foreach (string word in rawWords)
            {
                string lowerWord = word.ToLower();
                if (stopWords.Contains(lowerWord) == false)
                {
                    finalWords.Add(lowerWord);
                }
            }

            return finalWords;
        }

        private List<string> ApplySynonyms(List<string> words)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("csharp", "c#");
            map.Add("js", "javascript");
            map.Add("reactjs", "react");
            map.Add("dot net", ".net");
            map.Add("dotnet", ".net");
            
            List<string> newWords = new List<string>();

            foreach (string word in words)
            {
                if (map.ContainsKey(word))
                {
                    newWords.Add(map[word]);
                }
                else
                {
                    newWords.Add(word);
                }
            }

            return newWords;
        }

        private decimal CalculateTfIdfGrade(List<string> words, List<string> importantKeywords, decimal sectionWeight)
        {
            
            Dictionary<string, int> termFrequencies = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (importantKeywords.Contains(word))
                {
                    if (termFrequencies.ContainsKey(word))
                    {
                        termFrequencies[word] += 1;
                    }
                    else
                    {
                        termFrequencies[word] = 1;
                    }
                }
            }

            decimal score = 0;
            foreach (var keywordAppearance in termFrequencies)
            {
                int count = keywordAppearance.Value;
                decimal wordScore = 0;
                decimal currentPointValue = 0.5m;

                for (int i = 0; i < count; i++)
                {
                    wordScore += currentPointValue;

                    
                    currentPointValue = currentPointValue * 0.7m;
                }

                score += wordScore;
            }
                
            return score * sectionWeight;
        }

        public void ProcessCv(int applicantId)
        {
            Applicant applicant = _repository.GetApplicantById(applicantId);
            if (applicant == null)
            {
                return;
            }

            // If the file isnt valid, grade remains null
            decimal? cvGrade = ScanCvXml(applicant);
            if (cvGrade != null)
            {
                applicant.CvGrade = cvGrade;
            }

            EvaluateApplicantStatus(applicant);
            _repository.UpdateApplicant(applicant);
        }

        public void UpdateAppTestGrade(int applicantId, decimal grade)
        {
            Applicant applicant = _repository.GetApplicantById(applicantId);
            if (applicant != null)
            {
                applicant.AppTestGrade = grade;
                EvaluateApplicantStatus(applicant);
                _repository.UpdateApplicant(applicant);
            }
        }

        public decimal? ScanCvXml(Applicant applicant)
        {
            if (IsCvValid(applicant.CvFileUrl) == false)
            {
                return null;
            }

            List<string> expectedKeywords = new List<string>();
            if (applicant.Job != null && applicant.Job.RequiredSkills != null)
            {
                foreach (var js in applicant.Job.RequiredSkills)
                {
                    if (js.Skill != null && !string.IsNullOrWhiteSpace(js.Skill.SkillName))
                    {
                        expectedKeywords.Add(js.Skill.SkillName.ToLower());
                    }
                }
            }

            if (expectedKeywords.Count == 0)
            {
                expectedKeywords = new List<string> { "c#", "java", "sql", "react", "agile", "javascript", ".net", "python", "docker", "azure" };
            }

            XDocument doc = XDocument.Load(applicant.CvFileUrl);
            decimal totalGrade = 4.0m;
            
            XElement skillsNode = doc.Descendants("Skills").FirstOrDefault();
            
            if (skillsNode != null)
            {
                List<string> words = TokenizeAndClean(skillsNode.Value);
                words = ApplySynonyms(words);
                totalGrade += CalculateTfIdfGrade(words, expectedKeywords, 1.5m);
            }
            
            XElement interestsNode = doc.Descendants("Interests").FirstOrDefault();
            
            if (interestsNode != null)
            {
                List<string> words = TokenizeAndClean(interestsNode.Value);
                words = ApplySynonyms(words);
                totalGrade += CalculateTfIdfGrade(words, expectedKeywords, 0.5m);
            }

            if (totalGrade > 10.0m)
            {
                return 10.0m;
            }
            
            return totalGrade;
        }

        public void UpdateApplicant(Applicant applicant)
        {
            EvaluateApplicantStatus(applicant);
            _repository.UpdateApplicant(applicant);
        }

        public void RemoveApplicant(int applicantId)
        {
            _repository.RemoveApplicant(applicantId);
        }

        private void EvaluateApplicantStatus(Applicant applicant)
        {
            List<decimal> nonNullGrades = new List<decimal>();

            if (applicant.AppTestGrade != null) { nonNullGrades.Add(applicant.AppTestGrade.Value); }
            if (applicant.CvGrade != null) { nonNullGrades.Add(applicant.CvGrade.Value); }
            if (applicant.CompanyTestGrade != null) { nonNullGrades.Add(applicant.CompanyTestGrade.Value); }
            if (applicant.InterviewGrade != null) {nonNullGrades.Add(applicant.InterviewGrade.Value); }

            foreach (decimal grade in nonNullGrades)
            {
                if (grade < PassIndividual)
                {
                    applicant.ApplicationStatus = "Rejected";
                    return;
                }
            }

            if (nonNullGrades.Count > 0)
            {
                decimal sum = 0;
                foreach (decimal grade in nonNullGrades)
                {
                    sum += grade;
                }
                decimal average = sum / nonNullGrades.Count;
                
                if (average < PassCollective)
                {
                    applicant.ApplicationStatus = "Rejected";
                    return;
                }
            }

            if (nonNullGrades.Count == 4 && string.IsNullOrEmpty(applicant.ApplicationStatus))
            {
                applicant.ApplicationStatus = "On Hold";
            }
        }
    }
}

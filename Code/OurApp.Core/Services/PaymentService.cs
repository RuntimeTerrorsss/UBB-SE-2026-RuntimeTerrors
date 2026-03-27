using OurApp.Core.Validators;
using OurApp.Core.Repositories;
using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;

namespace OurApp.Core.Services
{
    
    public class PaymentService
    {
        public const string connectionString = "Data Source=Aron\\SQLEXPRESS;Initial Catalog=iss_project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        private readonly PaymentValidator _validator = new PaymentValidator();
        public readonly PaymentRepository _repository = new PaymentRepository(connectionString);

        public string ProcessPayment(int jobId, int amount, string name, string cardNum, string exp, string cvv)
        {
            string validationError = _validator.Validate(name, cardNum, exp, cvv);
            if (!string.IsNullOrEmpty(validationError)) return validationError;

            try
            {
                //  Save to database
                _repository.UpdateJobPayment(jobId, amount);

                //  Fetch emails to notify
                List<string> emailsToNotify = _repository.GetCompaniesToNotify(jobId, amount);

                //  Send Emails (Fire and forget)
                if (emailsToNotify.Count > 0)
                {
                    SendNotificationEmails(emailsToNotify, amount);
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return $"Database Error: {ex.Message}";
            }
        }

        private void SendNotificationEmails(List<string> emails, int newAmount)
        {
            try
            {
                // TODO: Set up and smtp from where the emails are sent
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("your_email@gmail.com", "your_app_password");
                    smtpClient.EnableSsl = true;

                    foreach (string email in emails)
                    {
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress("your_email@gmail.com", "Job Portal Admin");
                        mail.To.Add(email);
                        mail.Subject = "A competitor just outbid your job posting!";
                        mail.Body = $"Hello! Just letting you know that a competitor has placed a bid of ${newAmount} on a job that shares the same Type and Experience Level as yours. Consider increasing your budget to stay competitive!";

                        smtpClient.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

        public List<JobPaymentInfo> GetPaidJobsInfo(string jobType, string expLevel)
        {
            return _repository.GetPaidJobs(jobType, expLevel);
        }
    }
}
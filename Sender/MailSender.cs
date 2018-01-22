using Hangfire;
using Parsers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Serilog;
using Serilog.Events;

namespace Senders
{
    public class MailSender
    {
        private SmtpClient client;

        public MailSender(string smtpServer = "smtp.gmail.com", string smtpUsername = "username", string smtpPassword = "password")
        {
            client = new SmtpClient(smtpServer);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
        }

        private void Send(List<MailPerson> sendToList, ILogger logger)
        {
            foreach (var person in sendToList)
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("sender@awesome.emal");
                mailMessage.To.Add(person.Email);
                mailMessage.Body = String.Format("Hi {0} {1}\nThis is an email sent by a program using Hangfire", person.Name, person.Surname);
                mailMessage.Subject = "Hangfire email";
                logger.Information(String.Format("Send email to {0}", person.Email));
                //client.Send(mailMessage);
            }
        }

        public List<List<MailPerson>> CutList(List<MailPerson> listOfPersons)
        {
            List<List<MailPerson>> result = new List<List<MailPerson>>();
            int i = 0;
            while (i < listOfPersons.Count)
            {
                for (var j = 0; j < (listOfPersons.Count / 100) + 1; j++)
                {
                    result.Add(new List<MailPerson>());
                }

                List<MailPerson> tmp = new List<MailPerson>();
                foreach (MailPerson person in listOfPersons)
                {
                    int listNr = (i / 100);
                    result[listNr].Add(person);
                    i++;
                }
            }
            return result;
        }
        // sprawdzic czy lista nie jest pusta -> jesli nie to wyspij z pozycji 0 i wyrzuc ja, jesli czekaj

        public void SendBatch(List<List<MailPerson>> lists, ILogger logger)
        {
            if (lists.Count == 0)
            {
                Environment.Exit(2);
            }

            RecurringJob.AddOrUpdate(() => Send(lists[0], logger), Cron.MinuteInterval(1));
            lists.RemoveAt(0);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using qNotifier.DAL;
using qNotifier.Models;
using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace qNotifier.Services
{
    public interface INotifier
    {
        void Notify();
    }

    public abstract class Notifier : INotifier
    {
        private readonly IServiceProvider _services;

        public Notifier(IServiceProvider services)
        {
            _services = services;

        }

        List<(UserRecord, string)> GetRecordsToNotify()
        {
            List<(string, UserRecord)> x = new();

            using var scope = _services.CreateScope();

            var _userManager = scope.ServiceProvider.GetService<UserManager<User>>();

            //var q = _userManager.Users;

            //var user = _userManager.GetUserAsync(_httpContextAccessor?.HttpContext?.User).Result;

            //return user.Records.Where(r => EF.Functions.DateDiffSecond(DateTime.UtcNow, r.AppDateTime) < 60
            //                                                   && EF.Functions.DateDiffSecond(DateTime.UtcNow, r.AppDateTime) > 0);            

            const int timeZoneShift = -10; //Server Timezone - User Timezone
            List<(UserRecord, string)> recordsToNotify = new();

            //foreach (var item in _userManager.Users)
            //{
            //    var qRec = item.Records.Where(r => (DateTime.UtcNow - r.AppDateTime) < new TimeSpan(timeZoneShift, 0, 60)
            //                                    && (DateTime.UtcNow - r.AppDateTime) > new TimeSpan(timeZoneShift, 0, 0));
            //    foreach (var item2 in qRec)
            //    {
            //        var y = (item2, item.Email);
            //        recordsToNotify.Add(y);
            //    }                            
            //}

            var _myDb = scope.ServiceProvider.GetService<MyDbContext>();

            foreach (UserRecord item in _myDb.UserRecords.Include("AppUser"))
            {                                    
                    if ((DateTime.UtcNow - item.AppDateTime) < new TimeSpan(-item.AppUser.ClientTimeZoneOffset, 0, 60)
                                                    && (DateTime.UtcNow - item.AppDateTime) > new TimeSpan(-item.AppUser.ClientTimeZoneOffset, 0, 0))
                    {
                        recordsToNotify.Add((item, item.AppUser.Email));

                    }                            
            }

            //var d = _userManager.Users.Where(u => u.Records.Any(r => EF.Functions.DateDiffDay(DateTime.UtcNow, r.AppDateTime) == 0 
            //                                                    && EF.Functions.DateDiffSecond(DateTime.UtcNow, r.AppDateTime) < 60
            //                                                    && EF.Functions.DateDiffSecond(DateTime.UtcNow, r.AppDateTime) > 0));

            return recordsToNotify;
        }

        public void Notify()
        {            
            foreach (var item in GetRecordsToNotify())
            {
                SendNotify(item.Item1,item.Item2);
            }
        }

        public abstract void SendNotify(UserRecord my, string userEmail);
    }

    public class EmailNotifier : Notifier
    {
        public EmailNotifier(IServiceProvider services) : base(services)
        {
        }

        public override void SendNotify(UserRecord my, string userEmail)
        {            
            StringBuilder emailContent = new();
            
            emailContent.Append($"Hello!<br />We would like to remind you about your task:<br /><br />");
            emailContent.Append($"Time: {my.AppDateTime}<br />");
            emailContent.Append($"Title: {my.Title}<br />");
            emailContent.Append($"Description: {my.Description}<br />");
            emailContent.Append($"Status: {my.Status}<br /><br />");
            emailContent.Append($"Will be glad to see you soon!<br /><br />");
            emailContent.Append($"<a href='qnotifier.com'>qNotifier.com</a>");                  

            string sbj = "Your task notification - qNotifier.com";
            string body = emailContent.ToString();

            EmailSender sender = new();
            sender.SendEmail(userEmail, sbj, body);
        }
    }

    public class NotifierHostedService : IHostedService, IDisposable
    {
        int executionCount = 0;
        Timer _timer = null!;
        IServiceProvider _services;

        private readonly ILogger _logger;

        public NotifierHostedService(ILogger<NotifierHostedService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {            
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {            
            using (var scope = _services.CreateScope())
            {
                var myNotifier = scope.ServiceProvider.GetService<INotifier>();
                myNotifier.Notify();
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {           
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

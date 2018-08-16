using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternImplementation
{
    /// <summary>
    /// Interface for ConcreteStrategy to implement.
    /// </summary>
    interface IEmailStrategy
    {
        string From { get; set; }
        string To { get; set; }
        string Smtp { get; set; }
        string EmailBody { get; }
        void CreateEmailBody(); // This method is implemented differently and thus gives us option to use Strategy Pattern.
    }

    /// <summary>
    /// Email body maker class.
    /// </summary>
    interface IEmailBodyMaker
    {
        List<string> Statements { get; }
        Exception EmailException { get; }
        void SetStatements(List<string> statements);
        void SetEmailException(Exception exception);
    }

    class EmailBodyMaker: IEmailBodyMaker
    {
        public List<string> Statements { get; private set; }
        public Exception EmailException { get; private set; }

        public EmailBodyMaker()
        {
            
        }

        public void SetStatements(List<string> statements)
        {
            this.Statements = statements;
        }

        public void SetEmailException(Exception exception)
        {
            this.EmailException = exception;
        }
    }

    /// <summary>
    /// Concrete Strategy Implementation(BusinessEmail)
    /// </summary>
    class EmailBusinessNotificationStrategy : IEmailStrategy
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Smtp { get; set; }
        public string EmailBody { get; private set; }
        private IEmailBodyMaker EmailBodyMaker { get; set; }

        public EmailBusinessNotificationStrategy(IEmailBodyMaker emailBodyMaker)
        {
            this.EmailBodyMaker = emailBodyMaker;
        }        

        /// <summary>
        /// This method keeps the logic for your Algorithm implementation.
        /// </summary>
        public void CreateEmailBody()
        {            
            Console.ForegroundColor = ConsoleColor.Green;
            this.EmailBody = this.EmailBodyMaker.Statements[0];
        }        
    }

    /// <summary>
    /// Concrete Strategy Implementation(ExceptionEmail)
    /// </summary>
    class EmailExceptionNotificationStrategy : IEmailStrategy
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Smtp { get; set; }
        public string EmailBody { get; private set; }
        private IEmailBodyMaker EmailBodyMaker { get; set; }

        public EmailExceptionNotificationStrategy(IEmailBodyMaker emailBodyMaker)
        {
            this.EmailBodyMaker = emailBodyMaker;
        }

        /// <summary>
        /// This method keeps the logic for your Algorithm implementation.
        /// </summary>
        public void CreateEmailBody()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            this.EmailBody = this.EmailBodyMaker.EmailException.Message;
        }                   
    }

    /// <summary>
    /// This class is used to send email for any Concrete Email Strategy. 
    /// Depends only on interface following 'Dependency Inversion' principle from SOLID design principles.
    /// </summary>
    class EmailProcessor
    {
        public static void SendEmail(IEmailStrategy strategy)
        {            
            Console.WriteLine(strategy.EmailBody);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    /// <summary>
    /// Contaxt class used by client to choose the required Strategy.
    /// </summary>
    class Emailmanager
    {
        internal void SendEmail(EmailType business, IEmailBodyMaker emailBodyMaker)
        {
            IEmailStrategy strategy = null;
            switch (business)
            {
                case EmailType.Business:
                    strategy = new EmailBusinessNotificationStrategy(emailBodyMaker);                                        
                    break;

                case EmailType.Exception:
                    strategy = new EmailExceptionNotificationStrategy(emailBodyMaker);                   
                    break;

                default:
                    break;
            }

            //Read below values from Configuration.
            strategy.From = "from";
            strategy.To = "to";
            strategy.Smtp = "smtp";
            strategy.CreateEmailBody();

            EmailProcessor.SendEmail(strategy);
        }
    }

    enum EmailType
    {
        Business,
        Exception
    }

    /// <summary>
    /// Client
    /// </summary>
    class Program
    {
        
        static void Main(string[] args)
        {
            Emailmanager emailmanager = new Emailmanager();
            TestMethod(emailmanager);
        }

        static void TestMethod(Emailmanager emailmanager)
        {
            IEmailBodyMaker emailBodyMaker = new EmailBodyMaker();
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Executing TestMethod...");                

                //Selecting BusinessEmail type email notification.
                emailBodyMaker.SetStatements(new List<string> { "Business Email Notification." });
                emailmanager.SendEmail(EmailType.Business, emailBodyMaker);

                throw new Exception("Exception Message Notification");
            }
            catch (Exception ex)
            {
                //Selecting Exception type email notification.
                emailBodyMaker.SetEmailException(ex);
                emailmanager.SendEmail(EmailType.Exception, emailBodyMaker);
            }
        }
    }
}

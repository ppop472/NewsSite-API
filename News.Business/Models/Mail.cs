using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Business.Models
{
    public class Mail
    {

        public string smtpServer { get; set; }
        public string senderEmail { get; set; }
        public string senderPassword { get; set; }
        public int smtpPort { get; set; }
        public string Adres { get; set; }
        public string Subject { get; set; } 
        public string Body { get; set; }
        public bool? IsBodyHtml { get; set; }
    }
}

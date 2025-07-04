using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using News.Business.Interfaces;
using News.Business.Models;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using News.Repositories.Interfaces;
using News.Repositories.Models;
using System.Reflection.Metadata.Ecma335;
using System.Collections;
using System.Security.Cryptography;

namespace News.Business
{
    public class MailerService : IMailerService
    {
        private object _senderEmail = "";
        private readonly IConfiguration Configuration;
        private readonly Mail _mySettings;
        private readonly IUserRepository _userRepository;
        private readonly ISourceRepository _sourceRepository;
        private readonly INewsItemsRepository _newsItemsRepository;
        public MailerService(IOptions<Mail> mySettings, IUserRepository userRepository, ISourceRepository sourceRepository, INewsItemsRepository newsItemsRepository) 
        {
            _mySettings = mySettings.Value;
            _userRepository = userRepository;
            _sourceRepository = sourceRepository;
            _newsItemsRepository = newsItemsRepository;
        }
        public Mail Send()
        {

            var users = _userRepository.Get();

            var sources = _sourceRepository.Get();

            var notsentnieuws = _newsItemsRepository.NotSent();


            foreach (var user in users)
            {
                var allowedSourceIds = sources.Where(s => user.SourceId.ToString().Contains(s.Id.ToString()))
                                              .Select(s => s.Id)
                                              .ToList();

                string sourceNames = string.Join(", ", sources.Where(s => allowedSourceIds.Contains(s.Id)).Select(s => s.Name));

                string titels = string.Join("<br><br>", notsentnieuws.Where(s => allowedSourceIds.Contains(s.SourceId)).Select(s => s.Title));

                if (notsentnieuws.Count == 0 || titels == null || sourceNames == null || users == null)
                {
                    return null;
                }

                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string senderEmail = "projectmail472@gmail.com";
                string senderPassword = "ohsf bwoe eztr feny";
                string recipientEmail = user.Mail;
                string subject = "Jouw Nieuwsbrief!";
                
                string body = $"<!DOCTYPE html>\r\n<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" lang=\"en\">\r\n\r\n<head>\r\n\t<title></title>\r\n\t<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n\t<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><!--[if mso]>\r\n<xml><w:WordDocument xmlns:w=\"urn:schemas-microsoft-com:office:word\"><w:DontUseAdvancedTypographyReadingMail/></w:WordDocument>\r\n<o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml>\r\n<![endif]--><!--[if !mso]><!--><!--<![endif]-->\r\n\t<style>\r\n\t\t* \r\n\t\t\tbox-sizing: border-box;\r\n\t\t\r\n\r\n\t\tbody \r\n\t\t\tmargin: 0;\r\n\t\t\tpadding: 0;\r\n\t\t\r\n\r\n\t\ta[x-apple-data-detectors] \r\n\t\t\tcolor: inherit !important;\r\n\t\t\ttext-decoration: inherit !important;\r\n\t\t\r\n\r\n\t\t#MessageViewBody a \r\n\t\t\tcolor: inherit;\r\n\t\t\ttext-decoration: none;\r\n\t\t\r\n\r\n\t\tp \r\n\t\t\tline-height: inherit\r\n\t\t\r\n\r\n\t\t.desktop_hide,\r\n\t\t.desktop_hide table \r\n\t\t\tmso-hide: all;\r\n\t\t\tdisplay: none;\r\n\t\t\tmax-height: 0px;\r\n\t\t\toverflow: hidden;\r\n\t\t\r\n\r\n\t\t.image_block img+div \r\n\t\t\tdisplay: none;\r\n\t\t\r\n\r\n\t\tsup,\r\n\t\tsub \r\n\t\t\tfont-size: 75%;\r\n\t\t\tline-height: 0;\r\n\t\t\r\n\r\n\t\t@media (max-width:620px) \r\n\t\t\t.desktop_hide table.icons-inner \r\n\t\t\t\tdisplay: inline-block !important;\r\n\t\t\t\r\n\r\n\t\t\t.icons-inner \r\n\t\t\t\ttext-align: center;\r\n\t\t\t\r\n\r\n\t\t\t.icons-inner td \r\n\t\t\t\tmargin: 0 auto;\r\n\t\t\t\r\n\r\n\t\t\t.mobile_hide \r\n\t\t\t\tdisplay: none;\r\n\t\t\t\r\n\r\n\t\t\t.row-content \r\n\t\t\t\twidth: 100% !important;\r\n\t\t\t\r\n\r\n\t\t\t.stack .column \r\n\t\t\t\twidth: 100%;\r\n\t\t\t\tdisplay: block;\r\n\t\t\t\r\n\r\n\t\t\t.mobile_hide \r\n\t\t\t\tmin-height: 0;\r\n\t\t\t\tmax-height: 0;\r\n\t\t\t\tmax-width: 0;\r\n\t\t\t\toverflow: hidden;\r\n\t\t\t\tfont-size: 0px;\r\n\t\t\t\r\n\r\n\t\t\t.desktop_hide,\r\n\t\t\t.desktop_hide table \r\n\t\t\t\tdisplay: table !important;\r\n\t\t\t\tmax-height: none !important;\r\n\t\t\t\r\n\t\t\r\n\t</style><!--[if mso ]><style>sup, sub  font-size: 100% !important;  sup  mso-text-raise:10%  sub  mso-text-raise:-10% </style> <![endif]-->\r\n</head>\r\n\r\n<body class=\"body\" style=\"background-color: #ffffff; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none;\">\r\n\t<table class=\"nl-container\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff;\">\r\n\t\t<tbody>\r\n\t\t\t<tr>\r\n\t\t\t\t<td>\r\n\t\t\t\t\t<table class=\"row row-1\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 600px; margin: 0 auto;\" width=\"600\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 5px; padding-top: 5px; vertical-align: top;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"image_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\" style=\"width:100%;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"alignment\" align=\"center\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"max-width: 600px;\"><img src=\"https://hightechcampus.com/storage/929/Logo-Kembit-nieuw.jpg\" style=\"display: block; height: auto; border: 0; width: 100%;\" width=\"600\" alt title height=\"auto\"></div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"heading_block block-2\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<h1 style=\"margin: 0; color: #565451; direction: ltr; font-family: Arial, Helvetica, sans-serif; font-size: 38px; font-weight: 700; letter-spacing: normal; line-height: 1.2; text-align: center; margin-top: 0; margin-bottom: 0; mso-line-height-alt: 46px;\"><span class=\"tinyMce-placeholder\" style=\"word-break: break-word;\">Hoi, hier is jouw nieuwsbrief!</span></h1>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"spacer_block block-3\" style=\"height:60px;line-height:60px;font-size:1px;\">&#8202;</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"color:black;direction:ltr;font-family:Arial, Helvetica, sans-serif;font-size:12px;font-weight:400;letter-spacing:0px;line-height:1.2;text-align:center;mso-line-height-alt:14px;\">\r\n\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t{sourceNames}\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"spacer_block block-4\" style=\"height:60px;line-height:60px;font-size:1px;\">&#8202;</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"paragraph_block block-5\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"color:#6f0d73;direction:ltr;font-family:Arial, Helvetica, sans-serif;font-size:12px;font-weight:400;letter-spacing:0px;line-height:1.2;text-align:center;mso-line-height-alt:14px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p style=\"margin: 0;\">Dit zijn de door jouw gekozen nieuws sources. Als je deze wilt veranderen, kan dit gemakkelijk op de site. Onder description.&nbsp; &nbsp;</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"divider_block block-6\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"alignment\" align=\"center\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"divider_inner\" style=\"font-size: 1px; line-height: 1px; border-top: 1px solid #dddddd;\"><span style=\"word-break: break-word;\">&#8202;</span></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"heading_block block-7\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<h1 style=\"margin: 0; color: #565451; direction: ltr; font-family: Arial, Helvetica, sans-serif; font-size: 38px; font-weight: 700; letter-spacing: normal; line-height: 1.2; text-align: center; margin-top: 0; margin-bottom: 0; mso-line-height-alt: 46px;\"><span class=\"tinyMce-placeholder\" style=\"word-break: break-word;\">Recent Nieuws...</span></h1>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"spacer_block block-8\" style=\"height:60px;line-height:60px;font-size:1px;\">&#8202;</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"color:black;direction:ltr;font-family:Arial, Helvetica, sans-serif;font-size:12px;font-weight:400;letter-spacing:0px;line-height:1.2;text-align:center;mso-line-height-alt:14px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t{titels}\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"spacer_block block-9\" style=\"height:60px;line-height:60px;font-size:1px;\">&#8202;</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"divider_block block-10\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"alignment\" align=\"center\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"divider_inner\" style=\"font-size: 1px; line-height: 1px; border-top: 1px solid #dddddd;\"><span style=\"word-break: break-word;\">&#8202;</span></td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"row row-2\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff;\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 600px; margin: 0 auto;\" width=\"600\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 5px; padding-top: 5px; vertical-align: top;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"icons_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; text-align: center; line-height: 0;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\" style=\"vertical-align: middle; color: #1e0e4b; font-family: 'Inter', sans-serif; font-size: 15px; padding-bottom: 5px; padding-top: 5px; text-align: center;\"><!--[if vml]><table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"display:inline-block;padding-left:0px;padding-right:0px;mso-table-lspace: 0pt;mso-table-rspace: 0pt;\"><![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<!--[if !vml]><!-->\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<table class=\"icons-inner\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; display: inline-block; padding-left: 0px; padding-right: 0px;\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"><!--<![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t</tbody>\r\n\t</table><!-- End -->\r\n</body>\r\n\r\n</html>";
                
                try
                {
                    using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
                    {
                        client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        client.EnableSsl = true;

                        MailMessage mailMessage = new MailMessage
                        {
                            From = new MailAddress(senderEmail),
                            Subject = subject,
                            Body = body,    
                            IsBodyHtml = true
                        };

                        mailMessage.To.Add(recipientEmail);
                        client.Send(mailMessage);

                        Console.WriteLine($"Email sent to {user.Mail} with sources: {sourceNames}");
                    }
                    
                    foreach(var news in notsentnieuws)
                    {
                         var updatenieuws = _newsItemsRepository.Update(news);
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email to {user.Mail}: {ex.Message}");
                }
            }
            return null;
        }
    }
}

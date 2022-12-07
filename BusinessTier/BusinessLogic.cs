namespace FinalProject;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Azure;
using Azure.Communication.Email;
using Azure.Communication.Email.Models;
class BusinessLogic
{
    static async Task Main(string[] args)
    {
        bool _continue = true;
        User user;

        GuiTier appGUI = new GuiTier();
        DataTier database = new DataTier();


        // start GUI
        user = appGUI.Login();


        if (database.LoginCheck(user))
        {

            while (_continue)
            {
                int option = appGUI.Dashboard(user);
                switch (option)
                {
                    //Add Known Package
                    case 1:
                        Console.WriteLine("Please input a unit number");
                        int unit_number = Convert.ToInt32(Console.ReadLine());

                        DataTable tableResidents = database.ListUnit(user, unit_number);
                        if (tableResidents != null)
                            appGUI.DisplayUnitResidents(tableResidents);

                        Console.WriteLine("Please input index number");
                        int target_idx = Convert.ToInt16(Console.ReadLine());
                        string? full_name = tableResidents.Rows[target_idx]["full_name"].ToString();
                        string? resident_email = tableResidents.Rows[target_idx]["email"].ToString();



                        Console.WriteLine("Please input a posting service");
                        Console.WriteLine("---FedEx, USPS, UPS, Amazon---");
                        string? posting_name = Console.ReadLine();

                        Console.WriteLine("Please input delivery date (mm/dd/yyyy)");
                        string? delivery_date = Console.ReadLine();

                        database.AddPending(unit_number, full_name, posting_name);
                        database.AddPackageHistory(unit_number, full_name, posting_name, delivery_date);


                        // Code for processing and sending notification email to resident 
                        string serviceConnectionString = "endpoint=https://hskarrweek10communicationservice.communication.azure.com/;accesskey=SWJw5rSI11E3kSiwuaP8Z6ik3A+hdAp3FcLeOht0eAMd7pu2SNzmSyJ1sca8d6SuF/dnzDMlAVKIlzMIqRY8aQ==";
                        EmailClient emailClient = new EmailClient(serviceConnectionString);
                        var subject = "Package Pending for Pickup at Amarillo Apartments";
                        var emailContent = new EmailContent(subject);
                        // use Multiline String @ to design html content
                        emailContent.Html = @"
                        <html>
                            <body>
                                <h1>We have recieved a package in the office for you.</h1>
                                <h4>Due to limited storing space you will have 5 days to pick up your package.
                                If you fail to pick up your package within 5 days, it will be returned.</h4>
                                <br>
                                <p>Thank you!</p>
                            </body>
                        </html>";

                        // mailfrom domain of your email service on Azure
                        var sender = "DoNotReply@18c47f2b-b626-4f60-8704-52dfbf265510.azurecomm.net";


                        string? inputEmail = resident_email;
                        var emailRecipients = new EmailRecipients(new List<EmailAddress> {
                            new EmailAddress(inputEmail) { DisplayName = "Testing" },
                        });

                        var emailMessage = new EmailMessage(sender, emailContent, emailRecipients);

                        try
                        {
                            SendEmailResult sendEmailResult = emailClient.Send(emailMessage);

                            string messageId = sendEmailResult.MessageId;
                            if (!string.IsNullOrEmpty(messageId))
                            {
                                Console.WriteLine($"Email sent, MessageId = {messageId}");
                            }
                            else
                            {
                                Console.WriteLine($"Failed to send email.");
                                return;
                            }
                            // wait max 2 minutes to check the send status for mail.
                            var cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(2));
                            do
                            {
                                SendStatusResult sendStatus = emailClient.GetSendStatus(messageId);
                                Console.WriteLine($"Send mail status for MessageId : <{messageId}>, Status: [{sendStatus.Status}]");

                                if (sendStatus.Status != SendStatus.Queued)
                                {
                                    break;
                                }
                                await Task.Delay(TimeSpan.FromSeconds(10));

                            } while (!cancellationToken.IsCancellationRequested);

                            if (cancellationToken.IsCancellationRequested)
                            {
                                Console.WriteLine($"Looks like we timed out for email");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error in sending email, {ex}");
                        }

                        break;

                    // Add Unknown Package
                    case 2:
                        Console.WriteLine("Please input person's name");
                        string? unknown_name = Console.ReadLine();
                        Console.WriteLine("Please input address");
                        string? unknown_address = Console.ReadLine();
                        Console.WriteLine("Please input posting service");
                        string? unknown_posting = Console.ReadLine();
                        Console.WriteLine("Please input delivery date (mm/dd/yyyy)");
                        string? unknown_date = Console.ReadLine();

                        database.AddUnknown(unknown_name, unknown_address, unknown_posting, unknown_date);

                        break;

                    // Remove Package from Pending Area
                    case 3:
                        DataTable tablePendingArea = database.ListPending(user);
                        if (tablePendingArea != null)
                            appGUI.DisplayPending(tablePendingArea);

                        Console.WriteLine("Please input index number");
                        target_idx = Convert.ToInt16(Console.ReadLine());
                        unit_number = Convert.ToInt32(tablePendingArea.Rows[target_idx]["unit_number"].ToString());
                        full_name = tablePendingArea.Rows[target_idx]["full_name"].ToString();
                        posting_name = tablePendingArea.Rows[target_idx]["posting_name"].ToString();

                        database.RemovePending(unit_number, full_name, posting_name);

                        tablePendingArea = database.ListPending(user);
                        if (tablePendingArea != null)
                            appGUI.DisplayPending(tablePendingArea);
                        break;

                    // Show Package History for Resident
                    case 4:
                        Console.WriteLine("Please input unit number");
                        unit_number = Convert.ToInt32(Console.ReadLine());

                        tableResidents = database.ListUnit(user, unit_number);
                        if (tableResidents != null)
                            appGUI.DisplayUnitResidents(tableResidents);

                        Console.WriteLine("Please input index number");
                        target_idx = Convert.ToInt16(Console.ReadLine());
                        full_name = tableResidents.Rows[target_idx]["full_name"].ToString();

                        DataTable tablePackageHistory = database.ListPackageHistory(user, unit_number, full_name);
                        if (tablePackageHistory != null)
                            appGUI.DisplayPackageHistory(tablePackageHistory);

                        break;

                    // Log Out
                    case 5:
                        _continue = false;
                        Console.WriteLine("Log out, Goodbye.");
                        break;
                    // default: wrong input
                    default:
                        Console.WriteLine("Wrong Input");
                        break;
                }

            }
        }
        else
        {
            Console.WriteLine("Login Failed, Goodbye.");
        }
    }
}
using System.Numerics;
using System.Text;

namespace Parkeermeter2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            DateTime? startTime = null;//? is het mag NULL zijn
            DateTime? endTime = null;

            bool isRunning = true;

            while (isRunning)
            {
                ShowTitle();
                int menuOption = ShowMenu();

                switch (menuOption)
                {
                    case 1: //start
                        if (startTime == null)
                        {
                            startTime = StartSession();
                        }
                        else
                        {
                            ShowError("Er is al een sessie gestart");
                        }
                        break;
                    case 2: // Stop
                        if (startTime != null && endTime == null)
                        {
                            endTime = StopSession(startTime.Value);
                        }
                        else
                        {
                            ShowError("Er is geen lopende sessie om te stoppen.");
                        }
                        break;

                    case 3: // Ticket
                        if (startTime != null && endTime != null)
                        {
                            PrintTicket(startTime.Value, endTime.Value);
                            Console.ReadLine();
                        }
                        else
                        {
                            ShowError("Start- en eindtijd zijn nodig om een ticket af te drukken.");
                        }
                        break;

                    case 4: // Sluit
                        isRunning = false;
                        break;

                    default:
                        ShowError("Ongeldige optie.");
                        break;
                }
            }
            //ShowTitle();
            //ShowMenu();
            //ShowError("Beëndig eerst de lopende sessie.");
            ////ShowError("stop"); kunt ge hergebruiken met andere tekst.
            //DateTime startTime = StartSession();//in de doos steken doen we zo als het voor de methode staat geeft het iets terug en erna heb je eerst iets nodig
            //StopSession(startTime);
        }
            
        public static void ShowTitle()
        {
            Console.Clear();
            Console.WriteLine("+-------------+");
            Console.WriteLine("| P(arking)XL |");
            Console.WriteLine("+-------------+");
        }

        public static int ShowMenu()
        {
            int result;
            do
            {
                Console.WriteLine("1 - Start");
                Console.WriteLine("2 - Stop");
                Console.WriteLine("3 - Ticket");
                Console.WriteLine("4 - Sluit");

                Console.Write("Uw Keuze:");
                
            }while(!int.TryParse(Console.ReadLine(), out result) && (result >= 1) && (result <= 4 ));

            return result;
        }

        static public void ShowError(string error)//hier moet er nog geen tekst in zitten kan ook pas achteraf 
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"{error}");
            Console.ResetColor();
            Console.ReadLine();
        }

        static public DateTime StartSession()
        {
            DateTime startTime;
            do
            {
                Console.Write("Starttijd: ");

            } while (!DateTime.TryParse(Console.ReadLine(), out startTime) && startTime < DateTime.Now);

            Console.WriteLine($"Starttijd: {startTime}");

            Console.WriteLine("Sessie gestart! Druk op enter om verder te gaan...");
            Console.ReadLine();
      
            return startTime;
            
        }

        static public DateTime StopSession(DateTime startTime)//staat bovenaan
        {
            bool isValid;
            DateTime endTime;
            do
            {
                Console.Write("Eindtijd: ");
                isValid = DateTime.TryParse(Console.ReadLine(), out endTime) && endTime > startTime;//andere versie voor ! (niet) !isValid

            } while (!isValid);

            Console.WriteLine($"Eindtijd: {endTime}");
            Console.WriteLine("Sessie gestopt! Druk op enter om verder te gaan...");
            Console.ReadLine();
            return endTime;

        }

        static public decimal CalculatePrice(double minutes)
        {
            int freeTime = 15;
            int halfHour = 30;
            decimal price = 0;
            decimal priceHalfHour = 0.6m;
            decimal maxPrice = 8;

            if (minutes <= freeTime )
            {
                return price;//geeft 0.00 euro terug
            }

            price = (int)Math.Ceiling(minutes / halfHour);//maken er een int van
            price = price * priceHalfHour;//prijs word overschreven

            if(price <= maxPrice)//prijs kleiner dan 8
            {
                return price;//de prijs terug geven
            }
            else//anders is die altijd groter dan 8
            {
                return maxPrice;//max prijs
            }
         
        }
        public static void PrintTicket(DateTime startTime, DateTime endTime)
        {
            double minutes = (endTime - startTime).TotalMinutes;
            decimal price = CalculatePrice(minutes);

            StringBuilder ticketFormatting = new StringBuilder();
            int labelWidth = 5;//wit ruimte
            int valueWidth = 10;//wit ruimte
            int totalWidth = valueWidth + labelWidth;
            
            ShowTitle();

            ticketFormatting.AppendLine("Starttijd:");//appendLine is nieuwe lijn
            ticketFormatting.AppendLine($"{new string(' ', labelWidth)}{startTime:dd/MM/yyyy}".PadLeft(labelWidth + valueWidth));//Pedleft is voor uitlijning en opvullen witruimte
            ticketFormatting.AppendLine($"{new string(' ', labelWidth)}{startTime:HH:mm}".PadLeft(labelWidth + valueWidth));
            ticketFormatting.AppendLine();

            ticketFormatting.AppendLine("Eindtijd:");
            ticketFormatting.AppendLine($"{new string(' ', labelWidth)}{endTime:dd/MM/yyyy}".PadLeft(labelWidth + valueWidth));
            ticketFormatting.AppendLine($"{new string(' ', labelWidth)}{endTime:HH:mm}".PadLeft(labelWidth + valueWidth));
            ticketFormatting.AppendLine();

            ticketFormatting.AppendLine("Duur:");
            ticketFormatting.AppendLine($"{new string(' ', labelWidth)}{minutes} minuten".PadLeft(labelWidth + valueWidth));
            ticketFormatting.AppendLine();

            ticketFormatting.AppendLine("Prijs:");
            ticketFormatting.AppendLine($"{new string(' ', labelWidth)}€ {price:0.00}".PadLeft(labelWidth + valueWidth));

            string separatorLine = new string('-', totalWidth);
            ticketFormatting.AppendLine(separatorLine);

            Console.WriteLine(ticketFormatting);
        }

    }
}

using System;
//QUESTION 3 - EDUV10439556 - Mvelo Nhlapo
class Ticket
{
    public string TicketID { get; private set; } //getters and setters
    public string CustomerName { get; private set; }
    public List<double> RepairCosts { get; private set; }
    public static int TotalTickets { get; private set; } = 0;
    static Ticket()//static constructor
    {
        TotalTickets = 0;
    }
    //Task 2
    public Ticket(string ticketID, string customerName, List<double> Costs)//constructor
    {
        //Task 3
        if (!IsValidTicket(ticketID))
        {
            throw new ArgumentException("Invalid ticket ID format. Ticket ID must be Q followed by exactly 5 digits.");
        }
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new ArgumentException("Customer name cannot be empty.");
        }
        if (Costs == null)
        {
            throw new ArgumentException("Repair cost cannot be empty");
        }
        if (Costs.Any(cost => cost < 0))
        {
            throw new ArgumentException("Repair costs cannot be negative numbers.");
        }
        //Assign values to properties
        TicketID = ticketID;
        CustomerName = customerName;
        RepairCosts = new List<double>(Costs);
        TotalTickets++;//increment total tickets
    }
    public static bool IsValidTicket(string ticketID)
    {
        if (string.IsNullOrWhiteSpace(ticketID))
        {
            return false;
        }
        if (ticketID.Length != 6 || ticketID[0] != 'Q')
        {
            return false;
        }
        for (int i = 1; i < ticketID.Length; i++)
        {
            if (!char.IsDigit(ticketID[i]))//check if the rest of the characters on the ticket are digits
            {
                return false;
            }
        }
        return true;
    }
    public double CalcTotalCost()
    {
        return RepairCosts.Sum();
    }
    public double CalcAverageCost()
    {
        if (RepairCosts.Count == 0)
        {
            return 0;
        }
        return RepairCosts.Average();
    }
    public override string ToString()//override method
    {
        return $"Ticket ID: {TicketID}, Customer Name: {CustomerName}, Total Cost R: {CalcTotalCost():C}, Average Cost R: {CalcAverageCost():C}";
    }
    ~Ticket()//destructor
    {
        Console.WriteLine($"Ticket object {TicketID} destroyed.");
    }
}
class Program
{
    private static readonly List<Ticket> LisTickets = new List<Ticket>();//list of tickets instantiated
    static void Main(string[] args)
    {
        //Created objects
        LisTickets.Add(new Ticket("Q10006", "Mvelo Nhlapo", new List<double> { 350.00 }));
        LisTickets.Add(new Ticket("Q10007", "Pinkie Pie", new List<double> { 120.00, 54.00 }));

        bool programOn = true;
        while (programOn)
        {
            MainMenu();
            Console.Write("Choose an option:");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AddTicket();
                    break;
                case "2":
                    RemoveTicket();
                    break;
                case "3":
                    DisplayTickets();
                    break;
                case"4":
                    SearchTicket();
                    break;
                case"5":
                    programOn = false;
                    Console.WriteLine("Exiting program...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
    public static void MainMenu()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("     QuickFix Service Ticket System Menu");
        Console.WriteLine("==============================================");
        Console.WriteLine("1. Add Ticket");
        Console.WriteLine("2. Remove Ticket");
        Console.WriteLine("3. Display All Tickets");
        Console.WriteLine("4. Search for Ticket");
        Console.WriteLine("5. Exit");
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine();
    }
    public static void AddTicket()
    {
        try
        {
            Console.Write("Enter Ticket ID (format: Q10001): ");
            string ticketID = Console.ReadLine().Trim();

            if (!Ticket.IsValidTicket(ticketID))
            {
                throw new InvalidOperationException("Invalid Ticket ID format. Expected: Q followed by 5 digits.");
            }
            if (LisTickets.Any(t => t.TicketID.Equals(ticketID, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A ticket with this ID already exists");
            }
            Console.Write("Enter Customer Name: ");
            string customerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(customerName))
            {
                throw new InvalidOperationException("Customer name cannot be empty.");
            }
            int numOfCosts = ReadPositiveInteger("How many repair cost line items? ");
            List<double> repairCosts = new List<double>();

            for (int i = 0; i < numOfCosts; i++)
            {
                double cost = ReadPositiveDouble($"Enter cost {i + 1}: ");
                repairCosts.Add(cost);
            }
            Ticket ticket = new Ticket(ticketID, customerName, repairCosts);
            LisTickets.Add(ticket);
            Console.WriteLine("Service Ticket Initialised.");
            Console.WriteLine("Ticket added successfully!");
        }
        
        catch (FormatException ex)
        {
            Console.WriteLine("Input format error: "+ex);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Unable to add ticket "+ex);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("Ticket error: "+ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: "+ex);
        }
    }
    private static void RemoveTicket()
    {
        Console.Write("Enter the Ticket ID to remove: ");
        string ticketID = Console.ReadLine().Trim();

        if (!Ticket.IsValidTicket(ticketID))
        {
            Console.WriteLine("Invalid Ticket ID. Use Q followed by exactly 5 digits.");
            return;
        }
        //search for the ticket in the list
        Ticket ticket = LisTickets.FirstOrDefault(t => t.TicketID.Equals(ticketID, StringComparison.OrdinalIgnoreCase));
        if (ticket == null)
        {
            Console.WriteLine("No ticket was found with that ID.");
            return;
        }
        LisTickets.Remove(ticket);
        Console.WriteLine($"Ticket removed successfully");

    }
    private static void DisplayTickets()
    {
        if (LisTickets.Count == 0)
        {
            Console.WriteLine("No tickets to display.");
            return;
        }
        Console.WriteLine(" ");
        Console.WriteLine("QUICKFIX SERVICE TICKETS");
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"{"Ticket ID "}{"Customer Name "}{"Items "}{"Total Cost "}{"Average Cost "}");
        Console.WriteLine("--------------------------------------------------");

        foreach (Ticket ticket in LisTickets)
        {
            Console.WriteLine(ticket);
        }
        Console.WriteLine("--------------------------------------------------");
    }
    private static void SearchTicket()
    {
        Console.Write("Enter the Ticket ID to search for: ");
        string ticketID = Console.ReadLine().Trim();//Trims whitespace and converts alphanumeric values to uppercase
        ;
        if (!Ticket.IsValidTicket(ticketID))
        {
            Console.WriteLine("Invalid Ticket ID. Use Q followed by exactly 5 digits.");
            return;
        }
        //Will search for the ticket in the list
        Ticket ticket = LisTickets.FirstOrDefault(t => t.TicketID.Equals(ticketID, StringComparison.OrdinalIgnoreCase));
        if (ticket == null)
        {
            Console.WriteLine("No ticket was found with that ID.");
            return;
        }
        Console.WriteLine();
        Console.WriteLine("Ticket found:");
        Console.WriteLine($"ID : {ticket.TicketID} | Customer: {ticket.CustomerName}    | Avg Cost: R{ticket.CalcAverageCost():C}");
    }
    private static int ReadPositiveInteger(string prompt)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out value) && value >= 0)
            {
                return value;
            }
            Console.WriteLine("Please enter a positive integer or 0.");
        }
    }
    private static double ReadPositiveDouble(string prompt)
    {
        double value;
        while (true)
        {
            Console.Write(prompt);
            if (double.TryParse(Console.ReadLine(), out value) && value >= 0)
            {
                return value;
            }
            Console.WriteLine("Please enter a non-negative number.");
        }
    }
}

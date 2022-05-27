// TYPE A

// Part 1 - New Ticket Types
// No longer a such thing as just a "Ticket" - making it abstract
// Removing TicketType
// Having specific types of Ticket inherit from Ticket with their own properties

// Part 2 - Calculating SLA Deadlines
// Add properties:
// ResponseDeadline (when the ticket needs to be assigned)
// BreachDeadline (when the ticket should be resolved by)


///////////////////////////////////

Ticket BugTicket1 = new BugReportTicket()
{ 
    Id = 1, 
    Title = "Bug Ticket 1",
    Description = "The notes file crashed Microsoft Teams",
    CreatedDate = DateTime.Now,
    ticketStatus = "open",
    ticketPriority = "High",
    ErrorCode = "007",
    ErrorLog = "E/Log/007/1",
};

BugTicket1 = new ResponseDeadline_Priority(BugTicket1);

Ticket ServiceTicket1 = new ServiceRequestTicket()
{
    Id = 1,
    Title = "Service Ticket 1",
    Description = "Plz fix my pc, the notes file crashed it",
    CreatedDate = DateTime.Now,
    ticketStatus = "open",
    ticketPriority = "Medium",
    //ResponseDeadLine = 0,
    //BreachDeadline = 0,
};

ServiceTicket1 = new AllDeadline_Priority(ServiceTicket1);

///////////////////////////////////

Console.WriteLine($"Title: {BugTicket1.Title}");
Console.WriteLine($"Description: {BugTicket1.Description}");
Console.WriteLine($"ticketPriority: {BugTicket1.ticketPriority}");
Console.WriteLine($"Response Deadline: {BugTicket1.ResponseDeadLine}");
Console.WriteLine($"Response Deadline: {BugTicket1.ResponseTotal()}");
Console.WriteLine($"Response Deadline: {BugTicket1.ResponseDeadLine}");
Console.WriteLine($"Breach Deadline: {BugTicket1.BreachTotal()}");
Console.WriteLine();

Console.WriteLine($"Title: {ServiceTicket1.Title}");
Console.WriteLine($"Description: {ServiceTicket1.Description}");
Console.WriteLine($"ticketPriority: {ServiceTicket1.ticketPriority}");
Console.WriteLine($"Response Deadline: {ServiceTicket1.ResponseTotal()}");
Console.WriteLine($"Breach Deadline: {ServiceTicket1.BreachTotal()}");
Console.WriteLine();

Console.ReadLine();

///////////////////////////////////


public abstract class Ticket
{
    // properties
    public virtual int  Id { get; set; }
    public virtual string Title { get; set; }
    public virtual string Description { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual DateTime? UpdatedDate { get; set; }
    public virtual string ticketStatus { get; set; }
    public virtual string ticketPriority { get; set; }

    public virtual double ResponseDeadLine { get; set; }
    public virtual double ResponseTotal()
    {
        return ResponseDeadLine;
    }

    public virtual double BreachDeadline { get; set; }
    public virtual double BreachTotal()
    {
        return BreachDeadline;
    }
}

public class BugReportTicket : Ticket
{
    public string ErrorCode { get; set; }
    public string ErrorLog { get; set; }

    public BugReportTicket(int id, string title, string description, DateTime createdDate, string TicketStatus, string TicketPriority, string errorCode, string errorLog)
    {
        Id = id;
        Title = title;
        Description = description;
        CreatedDate = createdDate;
        ticketStatus = TicketStatus;
        ticketPriority = TicketPriority;
        ErrorCode = errorCode;
        ErrorLog = errorLog;
        ResponseDeadLine = this.ResponseTotal();
        BreachDeadline = this.BreachTotal();
    }

    //public BugReportTicket()
    //{
    //    ResponseDeadLine = this.ResponseTotal();
    //    BreachDeadline = this.BreachTotal();
    //}
}

public class ServiceRequestTicket : Ticket
{
    public ServiceType serviceType { get; set; }
}

public enum ServiceType
{
    IncidentRequest,
    GeneralRequest,
    InformationRequest
}


public abstract class TicketDecorator : Ticket
{
    public Ticket Ticket { get; set; }

    public override int Id
    {
        get { return Ticket.Id; }
        set { Ticket.Id = value; }
    }
    public override string Title
    {
        get { return Ticket.Title; }
        set { Ticket.Title = value; }
    }

    public override string Description
    {
        get { return Ticket.Description; }
        set { Ticket.Description = value; }
    }
    public override DateTime CreatedDate
    {
        get { return Ticket.CreatedDate; }
        set { Ticket.CreatedDate = value; }
    }
    public override DateTime? UpdatedDate
    {
        get { return Ticket.UpdatedDate; }
        set { Ticket.UpdatedDate = value; }
    }
    public override string ticketStatus
    {
        get { return Ticket.ticketStatus; }
        set { Ticket.ticketStatus = value; }
    }
    public override string ticketPriority
    {
        get { return Ticket.ticketPriority; }
        set { Ticket.ticketPriority = value; }
    }

    //public override double ResponseDeadLine
    //{
    //    get { return Ticket.ResponseDeadLine; }
    //    set { Ticket.ResponseDeadLine = value; }
    //}
    public abstract override double ResponseTotal();


    //public virtual double BreachDeadline { get; set; }
    public abstract override double BreachTotal();
}


public class ResponseDeadline_Priority : TicketDecorator
{
    public ResponseDeadline_Priority(Ticket ticket)
    {
        Ticket = ticket;
    }


    public override double ResponseTotal()
    {
        double TimeToAdd;

        if(Ticket.ticketPriority == "High")
        {
             TimeToAdd = 24;

        }
        else if (Ticket.ticketPriority == "Medium")
        {
             TimeToAdd = 72;
        }
        else if (Ticket.ticketPriority == "Low")
        {
             TimeToAdd = 144;
        }
        else
        {
             TimeToAdd = 0;
        }

        //Ticket.ResponseDeadLine = Ticket.ResponseTotal() + TimeToAdd;
        //return Ticket.ResponseDeadLine;

        return Ticket.ResponseTotal() + TimeToAdd;
    }

    public override double BreachTotal()
    {
        return Ticket.BreachTotal();
    }
}




public class BreachDeadline_Priority : TicketDecorator
{
    public BreachDeadline_Priority(Ticket ticket)
    {
        Ticket = ticket;
    }

    public override double ResponseTotal()
    {
        return Ticket.ResponseTotal();
    }

    public override double BreachTotal()
    {
        double TimeToAdd = 0;

        if (Ticket.ticketPriority == "High")
            TimeToAdd = 24;

        else if (Ticket.ticketPriority == "Medium")
            TimeToAdd = 72;

        else if (Ticket.ticketPriority == "Medium")
            TimeToAdd = 144;

        return Ticket.BreachTotal() + TimeToAdd;
    }
}




public class AllDeadline_Priority : TicketDecorator
{
        public AllDeadline_Priority(Ticket ticket)
        {
            Ticket = ticket;
        }

        public override double ResponseTotal()
        {
            double TimeToAdd;

            if (Ticket.ticketPriority == "High")
            {
                TimeToAdd = 24;

            }
            else if (Ticket.ticketPriority == "Medium")
            {
                TimeToAdd = 72;
            }
            else if (Ticket.ticketPriority == "Low")
            {
                TimeToAdd = 144;
            }
            else
            {
                TimeToAdd = 0;
            }

            return Ticket.ResponseTotal() + TimeToAdd;
        }

        public override double BreachTotal()
        {
            double TimeToAdd = 0;

            if (Ticket.ticketPriority == "High")
                TimeToAdd = 24;

            else if (Ticket.ticketPriority == "Medium")
                TimeToAdd = 72;

            else if (Ticket.ticketPriority == "Low")
                TimeToAdd = 144;

            return Ticket.BreachTotal() + TimeToAdd;
        }
}

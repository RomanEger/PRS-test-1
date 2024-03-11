namespace Entities.Models;

public partial class GameTransaction
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public int RecieverId { get; set; }

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public virtual User Reciever { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}

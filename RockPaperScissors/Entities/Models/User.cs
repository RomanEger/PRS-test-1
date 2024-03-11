namespace Entities.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public decimal Balance { get; set; }

    public virtual ICollection<GameTransaction> GameTransactionRecievers { get; set; } = new List<GameTransaction>();

    public virtual ICollection<GameTransaction> GameTransactionSenders { get; set; } = new List<GameTransaction>();

    public virtual ICollection<MatchHistory> MatchHistoryFirstUsers { get; set; } = new List<MatchHistory>();

    public virtual ICollection<MatchHistory> MatchHistorySecondUsers { get; set; } = new List<MatchHistory>();
}

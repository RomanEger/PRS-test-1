namespace Entities.Models;

public partial class MatchResult
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public string ResultOptionId { get; set; } = null!;

    public virtual MatchHistory Match { get; set; } = null!;

    public virtual ResultOption ResultOption { get; set; } = null!;
}

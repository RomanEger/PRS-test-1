using System;
using System.Collections.Generic;

namespace RockPaperScissors;

public partial class MatchHistory
{
    public int Id { get; set; }

    public int FirstUserId { get; set; }

    public int SecondUserId { get; set; }

    public decimal Bid { get; set; }

    public DateTime MatchDate { get; set; }

    public virtual User FirstUser { get; set; } = null!;

    public virtual ICollection<MatchResult> MatchResults { get; set; } = new List<MatchResult>();

    public virtual User SecondUser { get; set; } = null!;
}

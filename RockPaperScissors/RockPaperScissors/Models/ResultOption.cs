using System;
using System.Collections.Generic;

namespace RockPaperScissors;

public partial class ResultOption
{
    public string Id { get; set; } = null!;

    public virtual ICollection<MatchResult> MatchResults { get; set; } = new List<MatchResult>();
}

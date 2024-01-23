using System;
using System.Collections.Generic;

namespace Connect2Gether_API.Models;

public partial class Rank
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int MinPont { get; set; }

    public int MaxPont { get; set; }

    public string Description { get; set; } = null!;
}

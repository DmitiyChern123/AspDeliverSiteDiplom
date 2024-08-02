using System;
using System.Collections.Generic;

namespace WebApplication1.Entities2;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public int? BonusPoints { get; set; }

    public virtual ICollection<Korzina> Korzinas { get; set; } = new List<Korzina>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

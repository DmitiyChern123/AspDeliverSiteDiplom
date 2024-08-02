using System;
using System.Collections.Generic;

namespace WebApplication1.Entities2;

public partial class Courier
{
    public int Id { get; set; }

    public string Fio { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

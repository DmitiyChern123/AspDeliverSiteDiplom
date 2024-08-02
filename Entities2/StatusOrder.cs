﻿using System;
using System.Collections.Generic;

namespace WebApplication1.Entities2;

public partial class StatusOrder
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

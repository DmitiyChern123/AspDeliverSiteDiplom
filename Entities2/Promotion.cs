using System;
using System.Collections.Generic;

namespace WebApplication1.Entities2;

public partial class Promotion
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Opis { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string ImageText { get; set; } = null!;

    public int TypeId { get; set; }

    public int Price { get; set; }

    public virtual ProductType Type { get; set; } = null!;
}

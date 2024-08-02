using System;
using System.Collections.Generic;

namespace WebApplication1.Entities2;

public partial class KorzinaInOrder
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public int Count { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ProductType Product { get; set; } = null!;
}

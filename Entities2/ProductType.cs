using System;
using System.Collections.Generic;

namespace WebApplication1.Entities2;

public partial class ProductType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Price { get; set; }

    public int ProductId { get; set; }
    public bool Is_delated { get; set; }

    public virtual ICollection<KorzinaInOrder> KorzinaInOrders { get; set; } = new List<KorzinaInOrder>();

    public virtual ICollection<Korzina> Korzinas { get; set; } = new List<Korzina>();

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}

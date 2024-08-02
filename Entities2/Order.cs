using System;
using System.Collections.Generic;

namespace WebApplication1.Entities2;

public partial class Order
{
    public int Id { get; set; }

    public string Adress { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string PayType { get; set; } = null!;

    public int UserId { get; set; }

    public int StatusId { get; set; }

    public bool Is_need_devices { get; set; }
    public int? CourierId { get; set; }
    public DateTime Date { get; set; }
    public int Sum { get; set; }
    public virtual Courier? Courier { get; set; }

    public virtual ICollection<KorzinaInOrder> KorzinaInOrders { get; set; } = new List<KorzinaInOrder>();

    public virtual StatusOrder Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

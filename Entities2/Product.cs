using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplication1.Entities2;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Opis { get; set; }

    public int? Price { get; set; }

    public string? Grams { get; set; }
    public string? Calories { get; set; }

    public int? IdCategory { get; set; }

    public string? Img { get; set; }
    public bool Is_hidden { get; set; }


    public virtual Category? IdCategoryNavigation { get; set; }
    
    public virtual ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();
}

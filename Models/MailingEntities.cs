namespace dapper_module2.Models;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CountryId { get; set; }
}

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int CityId { get; set; }
}

public class Section
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class PromotionalItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal DiscountPercent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SectionId { get; set; }
}
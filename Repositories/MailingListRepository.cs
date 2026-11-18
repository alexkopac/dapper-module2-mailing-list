using Dapper;
using Microsoft.Data.SqlClient;
using dapper_module2.Models;

namespace dapper_module2.Repositories;

public class MailingListRepository
{
    private readonly string _connectionString;

    public MailingListRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

    // =========================================================================
    // ЗАВДАННЯ 1: Вставка (INSERT)
    // =========================================================================

    public int InsertCustomer(Customer customer)
    {
        const string sql = """
            INSERT INTO Customers (FullName, BirthDate, Gender, Email, CityId)
            OUTPUT INSERTED.Id
            VALUES (@FullName, @BirthDate, @Gender, @Email, @CityId);
            """;
        using var db = CreateConnection();
        return db.ExecuteScalar<int>(sql, customer);
    }

    public int InsertCountry(Country country)
    {
        const string sql = """
            INSERT INTO Countries (Name)
            OUTPUT INSERTED.Id
            VALUES (@Name);
            """;
        using var db = CreateConnection();
        return db.ExecuteScalar<int>(sql, country);
    }

    public int InsertCity(City city)
    {
        const string sql = """
            INSERT INTO Cities (Name, CountryId)
            OUTPUT INSERTED.Id
            VALUES (@Name, @CountryId);
            """;
        using var db = CreateConnection();
        return db.ExecuteScalar<int>(sql, city);
    }

    public int InsertSection(Section section)
    {
        const string sql = """
            INSERT INTO Sections (Name)
            OUTPUT INSERTED.Id
            VALUES (@Name);
            """;
        using var db = CreateConnection();
        return db.ExecuteScalar<int>(sql, section);
    }

    public int InsertPromotionalItem(PromotionalItem item)
    {
        const string sql = """
            INSERT INTO PromotionalItems (Name, DiscountPercent, StartDate, EndDate, SectionId)
            OUTPUT INSERTED.Id
            VALUES (@Name, @DiscountPercent, @StartDate, @EndDate, @SectionId);
            """;
        using var db = CreateConnection();
        return db.ExecuteScalar<int>(sql, item);
    }

    public void SubscribeCustomerToSection(int customerId, int sectionId)
    {
        const string sql = """
            IF NOT EXISTS (SELECT 1 FROM CustomerSections WHERE CustomerId = @CustomerId AND SectionId = @SectionId)
                INSERT INTO CustomerSections (CustomerId, SectionId) VALUES (@CustomerId, @SectionId);
            """;
        using var db = CreateConnection();
        db.Execute(sql, new { CustomerId = customerId, SectionId = sectionId });
    }

    // =========================================================================
    // ЗАВДАННЯ 2: Оновлення (UPDATE)
    // =========================================================================

    public bool UpdateCustomer(Customer customer)
    {
        const string sql = """
            UPDATE Customers
            SET FullName = @FullName,
                BirthDate = @BirthDate,
                Gender = @Gender,
                Email = @Email,
                CityId = @CityId
            WHERE Id = @Id;
            """;
        using var db = CreateConnection();
        return db.Execute(sql, customer) > 0;
    }

    public bool UpdateCountry(Country country)
    {
        const string sql = "UPDATE Countries SET Name = @Name WHERE Id = @Id;";
        using var db = CreateConnection();
        return db.Execute(sql, country) > 0;
    }

    public bool UpdateCity(City city)
    {
        const string sql = "UPDATE Cities SET Name = @Name, CountryId = @CountryId WHERE Id = @Id;";
        using var db = CreateConnection();
        return db.Execute(sql, city) > 0;
    }

    public bool UpdateSection(Section section)
    {
        const string sql = "UPDATE Sections SET Name = @Name WHERE Id = @Id;";
        using var db = CreateConnection();
        return db.Execute(sql, section) > 0;
    }

    public bool UpdatePromotionalItem(PromotionalItem item)
    {
        const string sql = """
            UPDATE PromotionalItems
            SET Name = @Name,
                DiscountPercent = @DiscountPercent,
                StartDate = @StartDate,
                EndDate = @EndDate,
                SectionId = @SectionId
            WHERE Id = @Id;
            """;
        using var db = CreateConnection();
        return db.Execute(sql, item) > 0;
    }

    // =========================================================================
    // ЗАВДАННЯ 3: Видалення (DELETE)
    // =========================================================================

    public bool DeleteCustomer(int id)
    {
        using var db = CreateConnection();
        return db.Execute("DELETE FROM Customers WHERE Id = @Id;", new { Id = id }) > 0;
    }

    public bool DeleteCountry(int id)
    {
        using var db = CreateConnection();
        return db.Execute("DELETE FROM Countries WHERE Id = @Id;", new { Id = id }) > 0;
    }

    public bool DeleteCity(int id)
    {
        using var db = CreateConnection();
        return db.Execute("DELETE FROM Cities WHERE Id = @Id;", new { Id = id }) > 0;
    }

    public bool DeleteSection(int id)
    {
        using var db = CreateConnection();
        return db.Execute("DELETE FROM Sections WHERE Id = @Id;", new { Id = id }) > 0;
    }

    public bool DeletePromotionalItem(int id)
    {
        using var db = CreateConnection();
        return db.Execute("DELETE FROM PromotionalItems WHERE Id = @Id;", new { Id = id }) > 0;
    }

    // =========================================================================
    // ЗАВДАННЯ 4: Спеціалізовані вибірки (SELECT з фільтрацією)
    // =========================================================================

    public IEnumerable<City> GetCitiesByCountry(int countryId)
    {
        const string sql = "SELECT Id, Name, CountryId FROM Cities WHERE CountryId = @CountryId;";
        using var db = CreateConnection();
        return db.Query<City>(sql, new { CountryId = countryId });
    }

    public IEnumerable<Section> GetSectionsByCustomer(int customerId)
    {
        const string sql = """
            SELECT s.Id, s.Name
            FROM Sections s
            INNER JOIN CustomerSections cs ON s.Id = cs.SectionId
            WHERE cs.CustomerId = @CustomerId;
            """;
        using var db = CreateConnection();
        return db.Query<Section>(sql, new { CustomerId = customerId });
    }

    public IEnumerable<PromotionalItem> GetPromotionalItemsBySection(int sectionId)
    {
        const string sql = """
            SELECT Id, Name, DiscountPercent, StartDate, EndDate, SectionId
            FROM PromotionalItems
            WHERE SectionId = @SectionId;
            """;
        using var db = CreateConnection();
        return db.Query<PromotionalItem>(sql, new { SectionId = sectionId });
    }

    // Допоміжні методи перегляду загальних списків для меню
    public IEnumerable<Country> GetAllCountries()
    {
        using var db = CreateConnection();
        return db.Query<Country>("SELECT Id, Name FROM Countries;");
    }

    public IEnumerable<City> GetAllCities()
    {
        using var db = CreateConnection();
        return db.Query<City>("SELECT Id, Name, CountryId FROM Cities;");
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        using var db = CreateConnection();
        return db.Query<Customer>("SELECT Id, FullName, BirthDate, Gender, Email, CityId FROM Customers;");
    }

    public IEnumerable<Section> GetAllSections()
    {
        using var db = CreateConnection();
        return db.Query<Section>("SELECT Id, Name FROM Sections;");
    }

    public IEnumerable<PromotionalItem> GetAllPromotionalItems()
    {
        using var db = CreateConnection();
        return db.Query<PromotionalItem>("SELECT Id, Name, DiscountPercent, StartDate, EndDate, SectionId FROM PromotionalItems;");
    }
}
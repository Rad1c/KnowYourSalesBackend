namespace MODEL.QueryModels.User;

public class UserQueryModel
{
    public Guid Id { get; set; }
    public string FisrtName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Sex { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}


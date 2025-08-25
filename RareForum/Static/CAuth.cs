using RareForum.Models;

namespace RareForum.Static;

public class CAuth
{
    public User? User { get; set; }

    public bool Autherized => User != null;
}
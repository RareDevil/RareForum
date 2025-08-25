using System.ComponentModel.DataAnnotations;

namespace RareForum.Models;

public class CheckBirthday : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (DateTime.TryParse(value?.ToString(), out DateTime date))
        {
            if (date.Year < 1900)
            {
                ErrorMessage = "Are you really born before 1900?";
                return false;
            }
        }
        
        return true;
    }
}
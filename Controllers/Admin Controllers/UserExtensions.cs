// UserExtensions.cs
public static class UserExtensions
{
    public static string GetRole(this User user)
    {
        if (user.Admin != null) return "Admin";
        if (user.Doctor != null) return "Doctor";
        if (user.Nurse != null) return "Nurse";
        if (user.Pharmacist != null) return "Pharmacist";
        return "Patient";
    }
}
using System;
using System.Collections.Generic;

namespace EmkhontweniCounselling.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }
}

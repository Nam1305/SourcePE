using System;
using System.Collections.Generic;

namespace Q2.Models;

public partial class EmployeeProject
{
    public int EmployeeId { get; set; }

    public int ProjectId { get; set; }

    public string? Role { get; set; }

    public DateOnly? JoinDate { get; set; }

    public DateOnly? LeaveDate { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Q2.Models;

public partial class Instructor
{
    public int InstructorId { get; set; }

    public string? Fullname { get; set; }

    public DateOnly? ContractDate { get; set; }

    public bool? IsFulltime { get; set; }

    public int? Department { get; set; }

    public virtual Department? DepartmentNavigation { get; set; }
}

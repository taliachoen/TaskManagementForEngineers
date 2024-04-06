using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class TaskScheduleDays
{
    public int TaskId { get; set; }
    public string? DependencyId { get; set; }

    public string? TaskName { get; set; }
    public int DaysFromProjectStart { get; set; }

    public int TaskDays { get; set; }
    public int DaysToProjectEnd { get; set; }
}

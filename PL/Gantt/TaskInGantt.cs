using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.Gantt;

public class TaskInGantt
{
    public int TaskId { get; set; }
    public string? TaskName { get; set; }
    public int? EngineerId { get; set; }
    public string? EngineerName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BO.Status Status { get; set; }
    public List<int>? DependentTasks { get; set; }

    public TaskInGantt(BO.Task task)
    {
        TaskId = task.Id;
        TaskName = task.Description;
        EngineerId = task.Engineer!.Id;
        EngineerName = task.Engineer.Name;
        StartDate = task.StartDate!.Value;
        EndDate = task.CompleteDate!.Value;
        Status = (BO.Status)task.Status!;
        DependentTasks = task.Dependencies?.Select(dep => dep.Id).ToList();
    }
}
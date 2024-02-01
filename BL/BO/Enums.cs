using System.Diagnostics.Contracts;

namespace BO;


public enum Status {
  Unscheduled,
  Scheduled,
  OnTrack,
  InJeopardy,
  Done
};
public enum EngineerExperience {
  Beginner,
  AdvancedBeginner,
  Intermediate,
  Advanced,
  Expert
};
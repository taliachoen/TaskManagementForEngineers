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

/*מצב הרשימה הציבורי {
    לא מתוכנן,
    מתוזמן,
    במעקב,
    בסכנה,
    בוצע
};
public enum EngineerExperience {
    מתחילים,
    מתקדם מתחיל,
    בינוני,
    מִתקַדֵם,
    מוּמחֶה
};
 */
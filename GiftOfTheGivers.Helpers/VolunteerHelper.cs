using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftOfTheGivers.Helpers
{
    public static class VolunteerHelper
    {
        // Turns a list of (StartDate, EndDate) availability periods into a
        // human-readable summary, e.g. "01 Oct - 05 Oct, 12 Oct - 14 Oct"
        public static string SummarizeAvailability(IEnumerable<(DateTime Start, DateTime End)> periods)
        {
            if (periods == null || !periods.Any())
                return "No availability provided";

            return string.Join(", ", periods.Select(p =>
                $"{p.Start:dd MMM} - {p.End:dd MMM}"));
        }

        // Counts how many total days a volunteer is available across all their periods
        public static int TotalAvailableDays(IEnumerable<(DateTime Start, DateTime End)> periods)
        {
            if (periods == null) return 0;

            int total = 0;
            foreach (var p in periods)
            {
                total += (p.End.Date - p.Start.Date).Days + 1;
            }
            return total;
        }

        // Formats a volunteer's selected skills into a display-friendly, comma-separated string
        public static string FormatSkillList(IEnumerable<string> skillNames)
        {
            if (skillNames == null || !skillNames.Any())
                return "No skills selected";

            return string.Join(", ", skillNames.OrderBy(s => s));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace TownOfUs.CrewmateRoles.MedicMod
{
    public enum ShieldOptions
    {
        Self = 0,
        Medic = 1,
        SelfAndMedic = 2,
        Everyone = 3
    }

    public enum NotificationOptions
    {
        Medic = 0,
        Shielded = 1,
        Everyone = 2,
        Nobody = 3
    }
}

﻿// LICENSE PLACEHOLDER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCBS.CoreDomain.Clients
{
    [Serializable]
    public class PovertyLevelIndicators
    {
        public int ChildrenEducation { get; set; }
        public int EconomicEducation { get; set; }
        public int SocialParticipation { get; set; }
        public int HealthSituation { get; set; }
    }
}

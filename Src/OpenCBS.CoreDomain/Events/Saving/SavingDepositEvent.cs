﻿// LICENSE PLACEHOLDER

using System;
using OpenCBS.Enums;

namespace OpenCBS.CoreDomain.Events.Saving
{
    [Serializable]
    public class SavingDepositEvent : SavingPositiveEvent
    {
        public override string Code
        {
            get { return OSavingEvents.Deposit; }
        }

        public override string Description { get; set; }
    }
}

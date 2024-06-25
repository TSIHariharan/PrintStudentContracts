using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStudentContracts
{
    public class Constants
    {
        public static class StudyStatusIDs
        {
            public const int NotStarted = 0;
            public const int InProgress = 1;
            public const int Pass = 2;
            public const int Honours = 3;
            public const int HonoursAll = 4;
            public const int Fail = 5;
            public const int Cheated = 6;
            public const int Marking = 7;
            public const int History = 8;
            public const int Exemption = 9;
            public const int Complete = 10;
            public const int Abandoned = 100;
            public const int DroppedOut = 101;
            public const int Dismissed = 102;
            public const int OnHold = 103;
            public const int Cancelled = 104;
            public const int Finished = 105;
            public const int Withdrawn = 106;
            public const int Void = 107;
            public const int Transferred = 108;
            public const int Incomplete = 109;
            public const int CPL = 888;
            public const int Deleted = 999;
        }

        public const int ContractStatusCPL = 888;
    }
}

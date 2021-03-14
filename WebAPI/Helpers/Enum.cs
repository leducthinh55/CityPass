using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public enum ProcessStatus
    {
        Failed,
        Success,
        Duplicate,
        Invalid,
        Permission
    }
    public enum PriceType { 
        Adult,
        Children
    }
}

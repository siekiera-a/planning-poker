using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.Server
{
    public enum OrganizerActions
    {
        Next,
        Assign,
        Rewind
    }

    public class Payload<T>
    {
        public T Value { get; set; }
    }

}

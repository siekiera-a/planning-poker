﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.Server
{
    public class OrganizerResponse : ClientResponse
    {
        public List<Client> Clients { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeamLab.PeopleDirectory.Models
{
    public class TokenTenant
    {
        public int ID { get; set; }

        public string TenantId { get; set; }

        public string Token { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}
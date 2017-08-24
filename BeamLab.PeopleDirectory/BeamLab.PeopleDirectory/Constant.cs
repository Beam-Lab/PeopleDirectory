using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeamLab.PeopleDirectory
{
    public static class Constant
    {
        public const string graphBasePath = "https://graph.microsoft.com/v1.0";
        public const string graphBasePathBeta = "https://graph.microsoft.com/beta";

        public const string authorityFormat = "https://login.microsoftonline.com/{0}/v2.0";
        public const string msGraphScope = "https://graph.microsoft.com/.default";

    }
}
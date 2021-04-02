using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Client.Models
{
    public class ResponseMessage
    {
        public bool success { get; set; }
        public bool hasError { get; set; }
        public string ErrorMessage { get; set; }

        //Specific to UpdateCharacter
        public bool showSpawnSelection { get; set; }
    }
}

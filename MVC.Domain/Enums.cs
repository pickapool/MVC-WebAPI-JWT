using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Domain
{
    public class Enums
    {
        public enum QRCodeType
        {
            Medicine,
            Room,
            Bed
        }

        public enum RequestType
        {
            GET,
            POST,
            DELETE,
            PUT
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MVC.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static object WrapModel(this object model, string key)
        {
            return new Dictionary<string, object>
            {
                [key] = model
            };
        }
    }
}

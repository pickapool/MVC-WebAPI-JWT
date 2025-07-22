using MVC.Domain.Models;
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
        public static PatientModel MapToPatient<T>(this PatientModel model, T map)
        {
            if(model is null || map is null)
                throw new ArgumentNullException("Object s null");
            model.PatientName = map.GetType().GetProperty("patientName")?.GetValue(map, null)?.ToString();
            model.RoomName = map.GetType().GetProperty("roomName")?.GetValue(map, null)?.ToString();
            model.BedNumber = map.GetType().GetProperty("bedNumber")?.GetValue(map, null)?.ToString();
            return model;
        }
    }
}

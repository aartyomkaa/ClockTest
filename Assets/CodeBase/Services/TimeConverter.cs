using System;
using UnityEngine;

namespace CodeBase.Services
{
    public class TimeConverter
    {
        public void Convert(string json, Time time)
        {
            UnconvertedTime unconvertedTime = JsonUtility.FromJson<UnconvertedTime>(json);
            
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unconvertedTime.time).UtcDateTime;
            
            time.Synchronize(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }
    }
}
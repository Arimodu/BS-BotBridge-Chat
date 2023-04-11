using IPA.Config.Data;
using IPA.Config.Stores;
using System;
using UnityEngine;

namespace BS_BotBridge_Chat.Converters
{
    internal class Vector3Converter : ValueConverter<Vector3>
    {
        public override Vector3 FromValue(Value value, object parent)
        {
            try
            {
                Map valueMap = value as Map;
                return new Vector3(
                    (float)(valueMap["x"] as FloatingPoint).Value, 
                    (float)(valueMap["y"] as FloatingPoint).Value, 
                    (float)(valueMap["z"] as FloatingPoint).Value);
            }
            catch (Exception)
            {
                throw;
                // Rethrow as ArgumentException
                throw new ArgumentException("Failed to parse value as Map of Vector3", nameof(value));
            }
        }

        public override Value ToValue(Vector3 vector, object parent)
        {
            Map valueMap = Value.Map();
            valueMap.Add("x", Value.Float((decimal)vector.x));
            valueMap.Add("y", Value.Float((decimal)vector.y));
            valueMap.Add("z", Value.Float((decimal)vector.z));
            return valueMap;
        }
    }
}

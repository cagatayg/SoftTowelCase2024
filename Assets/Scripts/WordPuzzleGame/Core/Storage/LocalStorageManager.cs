using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace STGames
{
    public static class LocalStorageManager
    {
        public static void Set<T>(string key, T value)
        {
            switch (value)
            {
                case decimal dec:
                case double dou:
                case float f:
                    SetFloating(key, value);
                    break;
                case string s:
                    SetString(key, s);
                    break;
                case DateTime d:
                    SetDatetime(key, d);
                    break;
                case bool b:
                    SetBoolean(key, b);
                    break;
                case long l:
                    SetLong(key, l);
                    break;
                case int i:
                    SetInt(key, i);
                    break;
                default:
                    if (typeof(T).IsEnum)
                    {
                        SetInt(key, Convert.ToInt32(value));
                    }
                    else
                    {
                        SetObject(key, value);
                    }

                    break;
            }
        }

        public static T Get<T>(string key, T defaultValue = default)
        {
            var type = typeof(T);
            var typeCode = type.IsEnum ? TypeCode.Empty : Type.GetTypeCode(type); // enum typecode is int32, conflicts with real int32
            switch (typeCode)
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return GetFloating(key, defaultValue);
                case TypeCode.String:
                    return GetString(key, defaultValue);
                case TypeCode.DateTime:
                    return GetDatetime(key, defaultValue);
                case TypeCode.Boolean:
                    return GetBoolean(key, defaultValue);
                case TypeCode.Int64:
                    return GetLong(key, defaultValue);
                case TypeCode.Int32:
                    return GetInt(key, defaultValue);
                default:
                    if (type.IsEnum) return GetEnum(key, defaultValue);
                    return GetObject(key, defaultValue);
            }
        }

        private static T GetEnum<T>(string key, T defaultValue)
        {
            Enum enumValue = Enum.Parse(typeof(T), defaultValue.ToString()) as Enum;
            int integerValue = Convert.ToInt32(enumValue); // x is the integer value of enum
            return (T)Enum.ToObject(typeof(T), GetInt(key, integerValue));
        }

        private static T GetObject<T>(string key, T defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                var value = PlayerPrefs.GetString(key, "");
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<T>(value);
                    }
                    catch
                    {
                    }
                }
            }

            return defaultValue;
        }

        private static T GetLong<T>(string key, T defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                var value = PlayerPrefs.GetString(key);
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        return (T)Convert.ChangeType(Convert.ToInt64(value), typeof(T));
                    }
                    catch
                    {
                    }
                }
            }

            return defaultValue;
        }

        private static T GetBoolean<T>(string key, T defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                var value = PlayerPrefs.GetInt(key);
                return (T)Convert.ChangeType(value == 1, typeof(T));
            }

            return defaultValue;
        }

        private static T GetDatetime<T>(string key, T defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                var ticks = PlayerPrefs.GetString(key);
                if (!string.IsNullOrEmpty(ticks))
                {
                    try
                    {
                        var ticksValue = Convert.ToInt64(ticks);
                        return (T)Convert.ChangeType(new DateTime(ticksValue), typeof(T));
                    }
                    catch
                    {
                    }
                }
            }

            return defaultValue;
        }

        private static T GetString<T>(string key, T defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return (T)Convert.ChangeType(PlayerPrefs.GetString(key), typeof(T));
            }

            return defaultValue;
        }

        private static T GetInt<T>(string key, T defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return (T)Convert.ChangeType(PlayerPrefs.GetInt(key), typeof(T));
            }

            if (defaultValue == null) return (T)Convert.ChangeType(0, typeof(T));
            ;
            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }

        private static T GetFloating<T>(string key, T defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return (T)Convert.ChangeType(PlayerPrefs.GetFloat(key), typeof(T));
            }

            return defaultValue;
        }

        private static void SetObject<T>(string key, T value, bool saveToMemory = false)
        {
            var result = value == null ? "" : JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString(key, result);
            PlayerPrefs.Save();
        }

        private static void SetLong(string key, long value, bool saveToMemory = false)
        {
            var result = value.ToString();
            PlayerPrefs.SetString(key, result);
            PlayerPrefs.Save();
        }

        private static void SetBoolean(string key, bool value, bool saveToMemory = false)
        {
            var result = value ? 1 : 0;
            PlayerPrefs.SetInt(key, result);
            PlayerPrefs.Save();
        }

        private static void SetDatetime(string key, DateTime value, bool saveToMemory = false)
        {
            var result = value.Ticks.ToString();
            PlayerPrefs.SetString(key, result);
            PlayerPrefs.Save();
        }

        private static void SetFloating<T>(string key, T value, bool saveToMemory = false)
        {
            PlayerPrefs.SetFloat(key, Convert.ToSingle(value));
            PlayerPrefs.Save();
        }

        private static void SetString(string key, string value, bool saveToMemory = false)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        private static void SetInt(string key, int value, bool saveToMemory = false)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        public static List<T> GetList<T>(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetString(key).Split('|').Select(s => (T)(Convert.ChangeType(s, typeof(T)))).ToList();
            }

            return new List<T>();
        }

        public static void AddToList<T>(string key, T value)
        {
            var current = GetList<T>(key);
            current.Add(value);
            SaveList<T>(key, current);
        }

        public static void ExtendList<T>(string key, IEnumerable<T> value)
        {
            var current = GetList<T>(key);
            current.AddRange(value);
            SaveList<T>(key, current);
        }

        public static void SaveList<T>(string key, List<T> current)
        {
            if (current == null || current.Count == 0)
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
                return;
            }

            if (current.Count == 1)
            {
                PlayerPrefs.SetString(key, current[0].ToString());
            }
            else
            {
                PlayerPrefs.SetString(key, string.Join("|", current.Select(s => s.ToString())));
            }

            PlayerPrefs.Save();
        }
    }
}
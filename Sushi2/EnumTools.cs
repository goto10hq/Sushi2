﻿using System;
using System.ComponentModel;
using System.Reflection;

namespace Sushi2
{
    public sealed class EnumTools
    {
        EnumTools()
        {
        }

        /// <summary>
        /// Get a human-readable string from the value of enum type.
        /// First it tries to get a text field defined in Description.
        /// If it's not set then it returns value of enum itself.
        /// If the value is not defined, exception is being thrown.
        /// </summary>
        /// <param name="field">Enum field.</param>
        /// <returns>Description (never null).</returns>
        public static string GetEnumFieldDescription(Enum field)
        {
            return GetEnumFieldDescription(field, null);
        }

        /// <summary>
        /// Get a human-readable string from the value of enum type.
        /// First it tries to get a text field defined in Description.
        /// If it's not set then it returns value of enum itself.
        /// If nondefined value has code 0 and <paramref name="zeroValueName"/> is not null - exception is not being thrown and
        /// instead of it returns <paramref name="zeroValueName"/>.
        /// </summary>
        /// <param name="field">Enum field.</param>
        /// <param name="zeroValueName">Zero value (used when non defined field is specified).</param>
        /// <returns>Description of the enum field. Never null.</returns>
        public static string GetEnumFieldDescription(Enum field, string zeroValueName)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            Type t = field.GetType();
            string fieldName = Enum.GetName(t, field);

            if (fieldName == null)
            {
                if ((int)(object)field == 0 && zeroValueName != null)
                    return zeroValueName;

                throw new InvalidEnumArgumentException("field", (int)(object)field, t);
            }

            FieldInfo fieldInfo = t.GetField(fieldName);

            object[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.GetLength(0) >= 1)
            {
                DescriptionAttribute da = (DescriptionAttribute)attributes[0];
                return da.Description;
            }

            return field.ToString(); // no constant caption is defined - return the original constant name
        }

        /// <summary>
        /// Parse enum.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="value">Value.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Enum value.</returns>
        public static T Parse<T>(object value, T defaultValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            if (value == null)
                return defaultValue;

            T ret;

            if (Enum.TryParse(value.ToString(), true, out ret))
            {
                if (!Enum.IsDefined(typeof(T), ret))
                    return defaultValue;

                return ret;
            }

            return defaultValue;
        }

        /// <summary>
        /// Parse enum.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="value">Value.</param>
        /// <returns>Enum value.</returns>
        public static T? Parse<T>(object value) where T : struct
        {
            if (!typeof(T).IsEnum)
                new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            if (value == null)
                return null;

            T ret;

            if (Enum.TryParse(value.ToString(), true, out ret))
            {
                if (!Enum.IsDefined(typeof(T), ret))
                    return null;

                return ret;
            }

            return null;
        }

        /// <summary>
        /// Get next item in enum.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="value">Value.</param>
        /// <return>Next enum value or first if there's no next item.</return>
        public static T GetNext<T>(T value) where T : struct
        {
            if (!typeof(T).IsEnum) throw
                new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] array = (T[])Enum.GetValues(value.GetType());
            var j = Array.IndexOf<T>(array, value) + 1;
            return (array.Length == j) ? array[0] : array[j];
        }

        /// <summary>
        /// Get previous item in enum.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="value">Value.</param>
        /// <return>Previous enum value or last if there's no previous item.</return>
        public static T GetPrevious<T>(T value) where T : struct
        {
            if (!typeof(T).IsEnum) throw
                new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] array = (T[])Enum.GetValues(value.GetType());
            var j = Array.IndexOf<T>(array, value) - 1;
            return (j < 0) ? array[array.Length - 1] : array[j];
        }
    }
}
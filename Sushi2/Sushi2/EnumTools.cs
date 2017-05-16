namespace Sushi2
{
  //  public sealed class EnumTools
  //  {
  //      private EnumTools()
  //      {
  //      }

  //      /// <summary>
  //      /// Get a human-readable string from the value of enum type.
  //      /// First it tries to get a text field defined in Description.
  //      /// If it's not set then it returns value of enum itself.
  //      /// If the value is not defined, exception is being thrown.
  //      /// </summary>
  //      /// <param name="field">Enum field.</param>
  //      /// <returns>Description (never null).</returns>
  //      /// <exception cref="InvalidEnumArgumentException"/>
  //      public static string GetEnumFieldDescription(Enum field)
  //      {
  //          return GetEnumFieldDescription(field, null);
  //      }

  //      /// <summary>
  //      /// Get a human-readable string from the value of enum type.
  //      /// First it tries to get a text field defined in Description.
  //      /// If it's not set then it returns value of enum itself.
  //      /// If nondefined value has code 0 and <paramref name="zeroValueName"/> is not null - exception is not being thrown and
  //      /// insed of it returns <paramref name="zeroValueName"/>.
  //      /// </summary>
  //      /// <param name="field">Enum field.</param>
  //      /// <param name="zeroValueName">Zero value (used when non defined field is specified).</param>
  //      /// <returns>Description of the enum field. Never null.</returns>
		///// <exception cref="InvalidEnumArgumentException"/>
  //      public static string GetEnumFieldDescription(Enum field, string zeroValueName)
  //      {
		//	if (field == null)
		//		throw new ArgumentNullException("field");

  //          Type t = field.GetType();
  //          string fieldName = Enum.GetName(t, field);

  //          if (fieldName == null)
  //          {
  //          	if ((int)(object)field == 0 && zeroValueName != null)
  //                  return zeroValueName;

  //          	throw new InvalidEnumArgumentException("field", (int)(object)field, t);
  //          }

  //      	FieldInfo fieldInfo = t.GetField(fieldName);

  //          object[] attributies = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

  //          if (attributies.GetLength(0) >= 1)
  //          {
  //              DescriptionAttribute da = (DescriptionAttribute)attributies[0];
  //              return da.Description;
  //          }

  //      	return field.ToString(); // no constant caption is defined - return the original constant name
  //      }

		///// <summary>
		///// Make a collection of list items from the given enum type.
		///// </summary>
		///// <param name="enumType">Enum type.</param>		
		///// <returns>A list of list items.</returns>
		//public static List<ListItem> GetEnumFields(Type enumType)
		//{
		//	return GetEnumFields(enumType, null);
		//}

		///// <summary>
		///// Make a collection of list items from the given enum type.
		///// </summary>
		///// <param name="enumType">Enum type.</param>
		///// <param name="selectedValue">Selected value.</param>
		///// <returns>A list of list items.</returns>
		//public static List<ListItem> GetEnumFields(Type enumType, string selectedValue)
		//{
		//	List<ListItem> results = new List<ListItem>();

		//	Array ar = Enum.GetValues(enumType);

		//	foreach (Enum val in ar)
		//		results.Add(new ListItem(GetEnumFieldDescription(val), 
		//			((int)((object)val)).ToString()));

		//	if (selectedValue != null)
		//	{
		//		foreach (ListItem li in results)
		//		{
		//			if (li.Value == selectedValue)
		//			{
		//				li.Selected = true;
		//				break;
		//			}
		//		}
		//	}

		//	return results;
		//}

		///// <summary>
		///// Parse enum.
		///// </summary>
		///// <typeparam name="T">Type.</typeparam>
		///// <param name="value">Value.</param>
		///// <param name="defaultValue">Default value.</param>
		///// <returns>Enum value.</returns>
		//public static T Parse<T>(object value, T defaultValue) where T : struct, IConvertible
		//{
		//	if (!typeof(T).IsEnum)			
		//		throw new ArgumentException("T must be an enumerated type.");

		//	if (value == null)
		//		return defaultValue;			

		//	T ret;
			
		//	if (Enum.TryParse(value.ToString(), true, out ret))
		//		return ret;				
		//	else
		//		return defaultValue;
		//}

		///// <summary>
		///// Parse enum.
		///// </summary>
		///// <typeparam name="T">Type.</typeparam>
		///// <param name="value">Value.</param>		
		///// <returns>Enum value.</returns>
		//public static T? Parse<T>(object value) where T : struct
		//{
		//	if (!typeof(T).IsEnum)
		//		throw new ArgumentException("T must be an enumerated type.");

		//	if (value == null)
		//		return null;

		//	T ret;

		//	if (Enum.TryParse(value.ToString(), true, out ret))
		//		return ret;
		//	else
		//		return null;
		//}
  //  }
}

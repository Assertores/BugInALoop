using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BIAL.Runtime
{
	public enum TimeType
	{
		ScaledDeltaTime = 1,
		UnscaledDeltaTime = 2,
		FixedDeltaTime = 3,
	}

	public static class GeneralUtils
	{
		public static T GetOrAddComponent<T>(this GameObject targetObject) where T : Component
		{
			T component = targetObject.GetComponent<T>();

			return component == null ? targetObject.AddComponent<T>() : component;
		}

		public static bool ShiftBetweenLists<T>(T target, List<T> lhd, List<T> rhd)
		{
			if (lhd.Remove(target))
			{
				rhd.Add(target);

				return true;
			}
			else if (rhd.Remove(target))
			{
				lhd.Add(target);

				return true;
			}

			return false;
		}

		public static string ReplaceBetween(this string input, char firstMarking, char secondMarking, string replaceWith, bool removeMarkings = true)
		{
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] != firstMarking)
				{
					continue;
				}

				for (int j = i + 1; j < input.Length; j++)
				{
					if (input[j] != secondMarking)
					{
						continue;
					}

					int substringStart = i        + (removeMarkings ? 0 : 1);
					int substringLength = (j - i) + (removeMarkings ? 1 : -1);
					input = input.Replace(input.Substring(substringStart, substringLength), replaceWith);
					i = j + (replaceWith.Length - substringLength);

					break;
				}
			}

			return input;
		}

		public static float GetTimeValue(this TimeType timeType)
		{
			switch (timeType)
			{
				case TimeType.ScaledDeltaTime:
				{
					return Time.deltaTime;
				}
				case TimeType.FixedDeltaTime:
				{
					return Time.fixedDeltaTime;
				}
				case TimeType.UnscaledDeltaTime:
				{
					return Time.unscaledDeltaTime;
				}
				default:
				{
#if EMBER_DEBUG
					throw new SystemException($"Invalid {nameof(TimeType)}: {timeType}.");
#else
					return Time.unscaledDeltaTime;
#endif
				}
			}
		}

		public static string ToRichTextColorTag(this Color color)
		{
			return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">";
		}

		public static string EnumerableString(this IEnumerable enumerable, string separator = "")
		{
			StringBuilder totalString = enumerable is ICollection collection ? new StringBuilder(collection.Count) : new StringBuilder();
			int count = 0;
			foreach (object o in enumerable)
			{
				if (count > 0)
				{
					totalString.Append(separator);
				}
				totalString.Append(o);
				count++;
			}

			return totalString.ToString();
		}
	}
}
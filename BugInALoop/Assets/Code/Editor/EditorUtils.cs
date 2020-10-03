using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BIAL.Editor
{
	public enum ArrayElementWrap
	{
		None = 0,
		Foldout = 1,
		Header = 2,
	}
	public static class EditorUtils
	{
		#region Fields

		private const int LINE_HEIGHT = 2;
		private const float STANDARD_OFFSET = 15;
		private static readonly Color StandardLineColor = new Color(0.25f, 0.25f, 0.65f, 1);

		private const string ARRAY_SHIFT_UP = "▲";
		private const string ARRAY_SHIFT_DOWN = "▼";
		private const string ARRAY_DELETE = "✖";
		private const float ARRAY_CONTROL_WIDTH = 25;

		public static int Indent { get; private set; }
		public static bool IsDirty { get; private set; }
		private static event Action FinishedDrawingElement;

		#endregion

		#region Standard Drawing

		public static bool FloatField(ref float curValue, string label, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			float newValue = EditorGUILayout.FloatField(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool FloatSliderField(ref float curValue, string label, float minValue = 0, float maxValue = 1, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			float newValue = EditorGUILayout.Slider(label, curValue, minValue, maxValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool IntField(ref int curValue, string label, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			int newValue = EditorGUILayout.IntField(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool IntSliderField(ref int curValue, string label, int minValue, int maxValue, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			int newValue = EditorGUILayout.IntSlider(label, curValue, minValue, maxValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool DelayedIntField(ref int curValue, string label, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			int newValue = EditorGUILayout.DelayedIntField(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool DoubleField(ref double curValue, string label, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			double newValue = EditorGUILayout.DoubleField(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool LongField(ref long curValue, string label, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			long newValue = EditorGUILayout.LongField(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool StringField(ref string curValue, string label, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			string newValue = EditorGUILayout.TextField(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool BoolField(ref bool curValue, string label, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			bool newValue = EditorGUILayout.Toggle(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool Vector2Field(ref Vector2 curValue, string label, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			Vector2 newValue = EditorGUILayout.Vector2Field(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool Vector3Field(ref Vector3 curValue, string label, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			Vector3 newValue = EditorGUILayout.Vector3Field(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool ColorField(ref Color curValue, string label, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			Color newValue = EditorGUILayout.ColorField(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool GradientField(ref Gradient curValue, string label, params GUILayoutOption[] guiLayout)
		{
			Gradient newValue = new Gradient
			{
				colorKeys = curValue.colorKeys,
				alphaKeys = curValue.alphaKeys,
			};

			BeginIndentSpaces();
			newValue = EditorGUILayout.GradientField(label, newValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (!newValue.Equals(curValue))
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool AnimationCurveField(ref AnimationCurve curValue, string label, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			AnimationCurve newValue = new AnimationCurve(curValue.keys);
			newValue = EditorGUILayout.CurveField(label, newValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (!newValue.Equals(curValue))
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool EnumField<T>(ref T curValue, string label, params GUILayoutOption[] guiLayout) where T : Enum
		{
			BeginIndentSpaces();
			T newValue = (T) EditorGUILayout.EnumPopup(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue.GetHashCode() != curValue.GetHashCode())
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool PopupField(ref int curValue, string label, string[] options, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			int newValue = EditorGUILayout.Popup(label, curValue, options, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool PopupField(ref int curValue, string label, string[] options, int[] optionValues, params GUILayoutOption[] guiLayout)
		{
			GUIContent[] displayedOptions = new GUIContent[options.Length];
			for (int i = 0; i < options.Length; i++)
			{
				displayedOptions[i] = new GUIContent(options[i]);
			}

			BeginIndentSpaces();
			int newValue = EditorGUILayout.IntPopup(new GUIContent(label), curValue, displayedOptions, optionValues, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool PopupMaskField(ref int curValue, string label, string[] options, params GUILayoutOption[] guiLayout)
		{
			BeginIndentSpaces();
			int newValue = EditorGUILayout.MaskField(label, curValue, options, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool EnumFlagField<T>(ref T curValue, string label, params GUILayoutOption[] guiLayout) where T : Enum
		{
			BeginIndentSpaces();
			T newValue = (T) EditorGUILayout.EnumFlagsField(label, curValue, guiLayout);
			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue.GetHashCode() != curValue.GetHashCode())
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool UnityObjectField<T>(ref T curValue, string label, params GUILayoutOption[] guiLayout) where T : Object
		{
			BeginIndentSpaces();
			T newValue = (T) EditorGUILayout.ObjectField(label, curValue, typeof(T), false, guiLayout);
			if (typeof(T).IsSubclassOf(typeof(ScriptableObject)))
			{
				if (GUILayout.Button("✚", GUILayout.MaxWidth(ARRAY_CONTROL_WIDTH)))
				{
					ObjectCreatorWindow.InitialiseWindow(typeof(T));
				}
			}

			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		#endregion

		#region Array Drawing

		public static void DrawArray<T>(ref T[] curValue,
										string label,
										SerializedProperty arrayProperty,
										ArrayDrawStyle arrayDrawStyle,
										Action<T, string, SerializedProperty> drawerFunction,
										string elementLabel = null) where T : class
		{
			DrawArray<T>(ref curValue,
						label,
						arrayProperty,
						arrayDrawStyle,
						drawerFunction,
						elementLabel != null ? new Func<int, string>(index => elementLabel) : null);
		}

		public static void DrawArray<T>(ref T[] curValue,
										string label,
										SerializedProperty arrayProperty,
										ArrayDrawStyle arrayDrawStyle,
										Action<T, string, SerializedProperty> drawerFunction,
										Func<int, string> fieldLabelGetter) where T : class
		{
			if (curValue == null)
			{
				curValue = new T[0];
				ShouldBeDirty();
			}

			Foldout(arrayProperty, label, arrayDrawStyle.AsHeader);
			if (arrayProperty.isExpanded)
			{
				IncreaseIndent();
				int newSize = curValue.Length;
				bool hasDrawnElement = false;
				EditorGUILayout.BeginHorizontal();
				DelayedIntField(ref newSize, "Size");
				string failedGetterLabel = ObjectNames.NicifyVariableName(typeof(T).Name);
				failedGetterLabel = $". {failedGetterLabel}";

				//Expand / Collapse All
				if (newSize > 0)
				{
					if (GUILayout.Button("Expand All"))
					{
						SetExpandState(true);
					}

					if (GUILayout.Button("Collapse All"))
					{
						SetExpandState(false);
					}
				}

				EditorGUILayout.EndHorizontal();
				int? controlTargetIndex = null;
				int? shiftDirection = null;
				for (int i = 0; i < curValue.Length; i++)
				{
					if (curValue[i] == null)
					{
						continue;
					}

					string elementLabel = $"{i.ToString()}{fieldLabelGetter?.Invoke(i) ?? failedGetterLabel}";
					SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(i);
					GUILayout.BeginHorizontal();
					switch (arrayDrawStyle.ArrayElementWrap)
					{
						case ArrayElementWrap.None:
							FinishedDrawingElement += ElementWasDrawn;
							drawerFunction?.Invoke(curValue[i], elementLabel, elementProperty);
							if (!hasDrawnElement)
							{
								GUILayout.EndHorizontal();
								FinishedDrawingElement -= ElementWasDrawn;
							}

							continue;
						case ArrayElementWrap.Foldout:
							Foldout(elementProperty, elementLabel, false);

							break;
						case ArrayElementWrap.Header:
							Header(elementLabel);

							break;
					}

					GUILayout.EndHorizontal();
					if ((arrayDrawStyle.ArrayElementWrap != ArrayElementWrap.Foldout) || elementProperty.isExpanded)
					{
						IncreaseIndent();
						drawerFunction?.Invoke(curValue[i], elementLabel, elementProperty);
						DecreaseIndent();
					}

					void ElementWasDrawn()
					{
						hasDrawnElement = true;
						FinishedDrawingElement -= ElementWasDrawn;
						AttachArrayControl();
						GUILayout.EndHorizontal();
					}

					void AttachArrayControl()
					{
						if (arrayDrawStyle.AllowElementShift)
						{
							EditorGUI.BeginDisabledGroup(i == 0);
							if (GUILayout.Button(ARRAY_SHIFT_UP, GUILayout.MaxWidth(ARRAY_CONTROL_WIDTH)))
							{
								controlTargetIndex = i;
								shiftDirection = -1;
							}

							EditorGUI.EndDisabledGroup();
							EditorGUI.BeginDisabledGroup(i == (newSize - 1));
							if (GUILayout.Button(ARRAY_SHIFT_DOWN, GUILayout.MaxWidth(ARRAY_CONTROL_WIDTH)))
							{
								controlTargetIndex = i;
								shiftDirection = 1;
							}

							EditorGUI.EndDisabledGroup();
						}

						if (arrayDrawStyle.AllowElementDelete)
						{
							if (GUILayout.Button(ARRAY_DELETE, GUILayout.MaxWidth(ARRAY_CONTROL_WIDTH)))
							{
								controlTargetIndex = i;
							}
						}
					}
				}

				if (ControlArray(ref curValue, controlTargetIndex, shiftDirection))
				{
					ShouldBeDirty();
					if (!shiftDirection.HasValue)
					{
						newSize--;
					}
				}

				int arraySizeChange = newSize - curValue.Length;
				if (arraySizeChange != 0)
				{
					ChangeArraySize(ref curValue, arraySizeChange);
					arrayProperty.arraySize = newSize;
					ShouldBeDirty();
				}

				DecreaseIndent();
			}

			if (arrayDrawStyle.AsHeader)
			{
				EndFoldoutHeader();
			}

			void SetExpandState(bool newState)
			{
				for (int i = 0; i < arrayProperty.arraySize; i++)
				{
					arrayProperty.GetArrayElementAtIndex(i).isExpanded = newState;
				}
			}
		}

		public static void DrawArray<T>(ref T[] curValue,
										string label,
										SerializedProperty arrayProperty,
										ArrayDrawStyle arrayDrawStyle,
										Func<T, string, SerializedProperty, T> drawerFunction,
										string elementLabel = null) where T : struct
		{
			DrawArray<T>(ref curValue,
						label,
						arrayProperty,
						arrayDrawStyle,
						drawerFunction,
						elementLabel != null ? new Func<int, string>(index => elementLabel) : null);
		}

		public static void DrawArray<T>(ref T[] curValue,
										string label,
										SerializedProperty arrayProperty,
										ArrayDrawStyle arrayDrawStyle,
										Func<T, string, SerializedProperty, T> drawerFunction,
										Func<int, string> fieldLabelGetter) where T : struct
		{
			if (curValue == null)
			{
				curValue = new T[0];
				ShouldBeDirty();
			}

			Foldout(arrayProperty, label, arrayDrawStyle.AsHeader);
			if (arrayProperty.isExpanded)
			{
				IncreaseIndent();
				int newSize = curValue.Length;
				EditorGUILayout.BeginHorizontal();
				DelayedIntField(ref newSize, "Size");
				string failedGetterLabel = ObjectNames.NicifyVariableName(typeof(T).Name);
				failedGetterLabel = $". {failedGetterLabel}";

				//Expand / Collapse All
				if (newSize > 0)
				{
					if (GUILayout.Button("Expand All"))
					{
						SetExpandState(true);
					}

					if (GUILayout.Button("Collapse All"))
					{
						SetExpandState(false);
					}
				}

				EditorGUILayout.EndHorizontal();
				int? controlTargetIndex = null;
				int? shiftDirection = null;
				for (int i = 0; i < curValue.Length; i++)
				{
					bool hasDrawnElement = false;
					string elementLabel = $"{i.ToString()}{fieldLabelGetter?.Invoke(i) ?? failedGetterLabel}";
					SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(i);
					GUILayout.BeginHorizontal();
					switch (arrayDrawStyle.ArrayElementWrap)
					{
						case ArrayElementWrap.None:
							FinishedDrawingElement += ElementWasDrawn;
							curValue[i] = drawerFunction?.Invoke(curValue[i], elementLabel, elementProperty) ?? default;
							if (!hasDrawnElement)
							{
								GUILayout.EndHorizontal();
								FinishedDrawingElement -= ElementWasDrawn;
							}

							continue;
						case ArrayElementWrap.Foldout:
							Foldout(elementProperty, elementLabel, false);

							break;
						case ArrayElementWrap.Header:
							Header(elementLabel);

							break;
					}

					GUILayout.EndHorizontal();
					if ((arrayDrawStyle.ArrayElementWrap != ArrayElementWrap.Foldout) || elementProperty.isExpanded)
					{
						IncreaseIndent();
						curValue[i] = drawerFunction?.Invoke(curValue[i], elementLabel, elementProperty) ?? default;
						DecreaseIndent();
					}

					void ElementWasDrawn()
					{
						hasDrawnElement = true;
						FinishedDrawingElement -= ElementWasDrawn;
						AttachArrayControl();
						GUILayout.EndHorizontal();
					}

					void AttachArrayControl()
					{
						if (arrayDrawStyle.AllowElementShift)
						{
							EditorGUI.BeginDisabledGroup(i == 0);
							if (GUILayout.Button(ARRAY_SHIFT_UP, GUILayout.MaxWidth(ARRAY_CONTROL_WIDTH)))
							{
								controlTargetIndex = i;
								shiftDirection = -1;
							}

							EditorGUI.EndDisabledGroup();
							EditorGUI.BeginDisabledGroup(i == (newSize - 1));
							if (GUILayout.Button(ARRAY_SHIFT_DOWN, GUILayout.MaxWidth(ARRAY_CONTROL_WIDTH)))
							{
								controlTargetIndex = i;
								shiftDirection = 1;
							}

							EditorGUI.EndDisabledGroup();
						}

						if (arrayDrawStyle.AllowElementDelete)
						{
							if (GUILayout.Button(ARRAY_DELETE, GUILayout.MaxWidth(ARRAY_CONTROL_WIDTH)))
							{
								controlTargetIndex = i;
							}
						}
					}
				}

				if (ControlArray(ref curValue, controlTargetIndex, shiftDirection))
				{
					ShouldBeDirty();
					if (!shiftDirection.HasValue)
					{
						newSize--;
					}
				}

				int arraySizeChange = newSize - curValue.Length;
				if (arraySizeChange != 0)
				{
					ChangeArraySize(ref curValue, arraySizeChange);
					arrayProperty.arraySize = newSize;
					ShouldBeDirty();
				}

				DecreaseIndent();
			}

			if (arrayDrawStyle.AsHeader)
			{
				EndFoldoutHeader();
			}

			void SetExpandState(bool newState)
			{
				for (int i = 0; i < arrayProperty.arraySize; i++)
				{
					arrayProperty.GetArrayElementAtIndex(i).isExpanded = newState;
				}
			}
		}

		public static void DrawUnityObjectArray<T>(ref T[] curValue,
													string label,
													SerializedProperty arrayProperty,
													ArrayDrawStyle arrayDrawStyle,
													string elementLabel = null) where T : Object
		{
			DrawUnityObjectArray<T>(ref curValue,
									label,
									arrayProperty,
									arrayDrawStyle,
									elementLabel != null ? new Func<int, string>(index => elementLabel) : null);
		}

		public static void DrawUnityObjectArray<T>(ref T[] curValue,
													string label,
													SerializedProperty arrayProperty,
													ArrayDrawStyle arrayDrawStyle,
													Func<int, string> fieldLabelGetter) where T : Object
		{
			if (curValue == null)
			{
				curValue = new T[0];
				ShouldBeDirty();
			}

			Foldout(arrayProperty, label, arrayDrawStyle.AsHeader);
			if (arrayProperty.isExpanded)
			{
				IncreaseIndent();
				int newSize = curValue.Length;
				DelayedIntField(ref newSize, "Size");
				string failedGetterLabel = ObjectNames.NicifyVariableName(typeof(T).Name);
				failedGetterLabel = $". {failedGetterLabel}";
				int? controlTargetIndex = null;
				int? shiftDirection = null;
				for (int i = 0; i < curValue.Length; i++)
				{
					string elementLabel = $"{i.ToString()}{fieldLabelGetter?.Invoke(i) ?? failedGetterLabel}";
					GUILayout.BeginHorizontal();
					UnityObjectField<T>(ref curValue[i], elementLabel);
					if (arrayDrawStyle.AllowElementShift)
					{
						EditorGUI.BeginDisabledGroup(i == 0);
						if (GUILayout.Button(ARRAY_SHIFT_UP, GUILayout.MaxWidth(ARRAY_CONTROL_WIDTH)))
						{
							controlTargetIndex = i;
							shiftDirection = -1;
						}

						EditorGUI.EndDisabledGroup();
						EditorGUI.BeginDisabledGroup(i == (curValue.Length - 1));
						if (GUILayout.Button(ARRAY_SHIFT_DOWN, GUILayout.MaxWidth(ARRAY_CONTROL_WIDTH)))
						{
							controlTargetIndex = i;
							shiftDirection = 1;
						}

						EditorGUI.EndDisabledGroup();
					}

					if (arrayDrawStyle.AllowElementDelete)
					{
						if (GUILayout.Button(ARRAY_DELETE, GUILayout.MaxWidth(ARRAY_CONTROL_WIDTH)))
						{
							controlTargetIndex = i;
						}
					}

					EndIndentSpaces();
				}

				if (ControlArray(ref curValue, controlTargetIndex, shiftDirection))
				{
					ShouldBeDirty();
					if (!shiftDirection.HasValue)
					{
						newSize--;
					}
				}

				int arraySizeChange = newSize - curValue.Length;
				if (arraySizeChange != 0)
				{
					ChangeArraySize(ref curValue, arraySizeChange);
					arrayProperty.arraySize = newSize;
					ShouldBeDirty();
				}

				DecreaseIndent();
			}

			if (arrayDrawStyle.AsHeader)
			{
				EndFoldoutHeader();
			}
		}

		#region Array Utils

		private static bool ControlArray<T>(ref T[] targetArray, int? controlTargetIndex, int? shiftDirection)
		{
			if (controlTargetIndex.HasValue)
			{
				//We shift the element
				if (shiftDirection.HasValue)
				{
					T targetElement = targetArray[controlTargetIndex.Value];
					targetArray[controlTargetIndex.Value] = targetArray[controlTargetIndex.Value + shiftDirection.Value];
					targetArray[controlTargetIndex.Value                                         + shiftDirection.Value] = targetElement;
				}

				//We delete the element
				else
				{
					DeleteArrayElement(ref targetArray, controlTargetIndex.Value);
				}

				return true;
			}

			return false;
		}

		private static void DeleteArrayElement<T>(ref T[] targetArray, int elementIndex)
		{
			T[] newArray = new T[targetArray.Length - 1];
			int insertedElements = 0;
			for (int i = 0; i < newArray.Length; i++)
			{
				if (i != elementIndex)
				{
					newArray[insertedElements] = targetArray[i];
					insertedElements++;
				}
			}

			targetArray = newArray;
		}

		private static void ChangeArraySize<T>(ref T[] targetArray, int sizeChange)
		{
			int newSize = targetArray.Length + sizeChange;
			T[] newArray = new T[newSize];
			for (int i = 0; i < newSize; i++)
			{
				if (i < targetArray.Length)
				{
					newArray[i] = targetArray[i];
				}
				else
				{
					break;
				}
			}

			targetArray = newArray;
		}

		#endregion

		#endregion

		#region Layout Drawer

		public static void BeginIndentSpaces()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(Indent * STANDARD_OFFSET);
		}

		public static void EndIndentSpaces()
		{
			EditorGUILayout.EndHorizontal();
		}

		public static void Header(string label, bool spaces = true, bool bold = true, params GUILayoutOption[] guiLayout)
		{
			if (spaces)
			{
				GUILayout.Space(8);
			}

			BeginIndentSpaces();
			if (bold)
			{
				GUILayout.Label(label, EditorStyles.boldLabel, guiLayout);
			}
			else
			{
				GUILayout.Label(label, guiLayout);
			}

			EndIndentSpaces();
			FinishedElementDrawingCall();
			if (spaces)
			{
				GUILayout.Space(4);
			}
		}

		public static void LineBreak(bool spaces = true, Color? lineColor = null)
		{
			if (spaces)
			{
				GUILayout.Space(3);
			}

			Rect rect = EditorGUILayout.GetControlRect(false, LINE_HEIGHT);
			rect.height = LINE_HEIGHT;
			rect.x /= 2;
			EditorGUI.DrawRect(rect, lineColor ?? StandardLineColor);
			FinishedElementDrawingCall();
			if (spaces)
			{
				GUILayout.Space(3);
			}
		}

		public static bool Foldout(SerializedProperty property, string label, bool asHeader = false)
		{
			bool curOpen = property.isExpanded;
			if (Foldout(ref curOpen, label, asHeader))
			{
				property.isExpanded = curOpen;

				return true;
			}

			return false;
		}

		public static bool Foldout(ref bool curValue, string label, bool asHeader = false)
		{
			bool newValue;
			if (asHeader)
			{
				newValue = EditorGUILayout.BeginFoldoutHeaderGroup(curValue, label);
			}
			else
			{
				BeginIndentSpaces();
				newValue = EditorGUILayout.Foldout(curValue, label, true);
				EndIndentSpaces();
			}

			FinishedElementDrawingCall();
			if (newValue != curValue)
			{
				ShouldBeDirty();
				curValue = newValue;

				return true;
			}

			return false;
		}

		public static bool DrawInFoldout(SerializedProperty property, string label, Action drawFunction, bool asHeader = false)
		{
			bool curOpen = property.isExpanded;
			if (DrawInFoldout(ref curOpen, label, drawFunction, asHeader))
			{
				property.isExpanded = curOpen;

				return true;
			}

			return false;
		}

		public static bool DrawInFoldout(ref bool curValue, string label, Action drawFunction, bool asHeader = false)
		{
			bool changed = Foldout(ref curValue, label, asHeader);
			if (curValue)
			{
				IncreaseIndent();
				drawFunction?.Invoke();
				DecreaseIndent();
			}

			EndFoldoutHeader();

			return changed;
		}

		public static void EndFoldoutHeader()
		{
			EditorGUILayout.EndFoldoutHeaderGroup();
		}

		#endregion

		#region Utils

		public static void IncreaseIndent(int amount = 1)
		{
			Indent += amount;
		}

		public static void DecreaseIndent(int amount = 1)
		{
			Indent -= amount;
		}

		public static void ShouldBeDirty(bool state = true)
		{
			IsDirty = state;
		}

		public static void FinishedElementDrawingCall()
		{
			FinishedDrawingElement?.Invoke();
		}

		#endregion
	}

	public class ArrayDrawStyle
	{
		public static readonly ArrayDrawStyle Default = new ArrayDrawStyle(false, ArrayElementWrap.None, true, true);
		public static readonly ArrayDrawStyle DefaultHeader = new ArrayDrawStyle(true, ArrayElementWrap.None, true, true);

		public bool AsHeader;
		public ArrayElementWrap ArrayElementWrap;
		public bool AllowElementDelete;
		public bool AllowElementShift;

		public ArrayDrawStyle(bool asHeader, ArrayElementWrap arrayElementWrap, bool allowElementDelete, bool allowElementShift)
		{
			AsHeader = asHeader;
			ArrayElementWrap = arrayElementWrap;
			AllowElementDelete = allowElementDelete;
			AllowElementShift = allowElementShift;
		}

		public ArrayDrawStyle Copy()
		{
			return new ArrayDrawStyle(AsHeader, ArrayElementWrap, AllowElementDelete, AllowElementShift);
		}
	}
}
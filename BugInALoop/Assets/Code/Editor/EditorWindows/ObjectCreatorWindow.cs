using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BIAL.Runtime.DataStorage;
using Embernight.Editor;
using UnityEditor;
using UnityEngine;
using static BIAL.Editor.EditorUtils;


namespace BIAL.Editor
{
	public class ObjectCreatorWindow : EditorWindow
	{
		private static readonly Type ScriptableObjectBaseType = typeof(ScriptableObject);
		private static readonly Assembly TargetAssembly = typeof(ApplicationSettings).Assembly;

		private const string ASSET_FOLDER = "Assets";
		private const string SETTINGS_FOLDER = "Settings";

		private const string RETURN_SYMBOL = "⏎";

		private static readonly Dictionary<Type, string> DefinedPathLookup = new Dictionary<Type, string>
		{
		};

		private Type baseType;
		private bool InBaseTypeSelection => baseType == ScriptableObjectBaseType;
		private TypeGroup baseTypeGroup;
		private Vector2 scrollPos;
		private string currentTargetName = "";
		private string currentTargetPath = "";

		[MenuItem("Tools/Object Creator Window")]
		public static void CreateObjectCreateWindowUnSeeded()
		{
			InitialiseWindow(ScriptableObjectBaseType);
		}

		private void Initialise(Type baseType)
		{
			this.baseType = baseType;
			baseTypeGroup = new TypeGroup(baseType, TargetAssembly.GetTypes().Where(type => type.IsSubclassOf(baseType) || (type == baseType)).ToArray());
			currentTargetName = $"New{this.baseType.Name}";
			if (!TryGetPredictedPath())
			{
				currentTargetPath = $"{Application.dataPath}/{SETTINGS_FOLDER}";
			}
		}

		private bool TryGetPredictedPath()
		{
			Type inspectedType = baseType;
			while (inspectedType != null)
			{
				if (DefinedPathLookup.TryGetValue(inspectedType, out string retrievedPath))
				{
					currentTargetPath = $"{Application.dataPath}/{retrievedPath}";

					return true;
				}

				inspectedType = inspectedType.BaseType;
			}

			return false;
		}

		private void OnGUI()
		{
			if (baseTypeGroup == null)
			{
				Initialise(baseType ?? ScriptableObjectBaseType);
			}

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			SeededOnGUI();
			EditorGUILayout.EndScrollView();
		}

		private void SeededOnGUI()
		{
			DrawObjectCreationInput();
			DrawTypeButtons();
			DrawBackToTypeSelectionButton();
		}

		private void DrawObjectCreationInput()
		{
			if (InBaseTypeSelection)
			{
				return;
			}

			GUILayout.Space(10);
			StringField(ref currentTargetName, "Name");
			GUILayout.BeginHorizontal();
			EditorGUI.BeginDisabledGroup(true);
			BeginIndentSpaces();
			EditorGUILayout.TextField("Path", currentTargetPath.Substring(Math.Max(0, currentTargetPath.LastIndexOf(ASSET_FOLDER))));
			EndIndentSpaces();
			EditorGUI.EndDisabledGroup();
			if (GUILayout.Button("...", GUILayout.Width(25)))
			{
				string newPath;
				if (Directory.Exists(currentTargetPath))
				{
					newPath = EditorUtility.OpenFolderPanel("Asset Save Folder", currentTargetPath.Substring(Math.Max(0, currentTargetPath.LastIndexOf(ASSET_FOLDER))), "");
				}
				else
				{
					newPath = EditorUtility.OpenFolderPanel("Asset Save Folder", $"{ASSET_FOLDER}/{SETTINGS_FOLDER}", "");
				}

				if (!string.IsNullOrEmpty(newPath))
				{
					currentTargetPath = newPath;
				}
			}

			GUILayout.EndHorizontal();
		}

		private void DrawTypeButtons()
		{
			DrawTypeButton(baseTypeGroup.TargetType);
			IncreaseIndent();
			DrawTypeGroupChildren(baseTypeGroup);
			DecreaseIndent();
		}

		private void DrawBackToTypeSelectionButton()
		{
			if (InBaseTypeSelection)
			{
				return;
			}

			const float LINE_SIZE = 30;
			GUILayout.Space(LINE_SIZE / 2);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Return to Type Selection", GUILayout.Height(LINE_SIZE));
			if (GUILayout.Button(RETURN_SYMBOL, GUILayout.MaxWidth(LINE_SIZE), GUILayout.Height(LINE_SIZE)))
			{
				Initialise(ScriptableObjectBaseType);
			}

			EditorGUILayout.EndHorizontal();
		}

		private void DrawTypeGroupChildren(TypeGroup targetGroup)
		{
			foreach (TypeGroup child in targetGroup.Children)
			{
				bool mainGroup = (child.Children.Length > 0) && (child.TargetType.BaseType == baseType);
				if (mainGroup)
				{
					GUILayout.Space(6);
					LineBreak(false);
				}

				DrawTypeButton(child.TargetType);
				IncreaseIndent();
				DrawTypeGroupChildren(child);
				DecreaseIndent();
				if (mainGroup)
				{
					LineBreak(false);
					GUILayout.Space(6);
				}
			}
		}

		private void DrawTypeButton(Type targetType)
		{
			bool isAbstract = targetType.IsAbstract;
			BeginIndentSpaces();
			EditorGUI.BeginDisabledGroup(isAbstract && !InBaseTypeSelection);
			if (GUILayout.Button(ObjectNames.NicifyVariableName(GetSimpleTypeName(targetType))))
			{
				if (InBaseTypeSelection)
				{
					Initialise(targetType);
				}
				else
				{
					TryCreateObject(targetType);
				}
			}

			EditorGUI.EndDisabledGroup();
			EndIndentSpaces();
		}

		private void TryCreateObject(Type targetType)
		{
			//Check if the current path or name is invalid
			bool invalidName = string.IsNullOrEmpty(currentTargetName);
			bool invalidPath = !currentTargetPath.Contains(Application.dataPath);
			if (invalidName)
			{
				Debug.LogError("Cannot create an asset without a given name.");
				currentTargetName = "INVALID NAME";
			}

			if (invalidPath)
			{
				Debug.LogError("Invalid path given.");
				currentTargetPath = "INVALID PATH";
			}

			//Create the asset
			if (!invalidName && !invalidPath)
			{
				AssetCreator.CreateAsset(targetType, currentTargetName, currentTargetPath);
			}
		}

		private static string GetSimpleTypeName(Type t)
		{
			string name = t.Name;
			if (!t.IsGenericType)
			{
				return name;
			}
			else
			{
				//Generic types will have a "`{int}" after the basic name, we dont want it in this case so we remove it
				int index = name.IndexOf('`');

				return index == -1 ? name : name.Substring(0, index);
			}
		}

		public static void InitialiseWindow(Type baseType)
		{
			if (!baseType.IsSubclassOf(ScriptableObjectBaseType) && baseType != ScriptableObjectBaseType)
			{
				throw new ArgumentException($"Cannot initiate Object Creation window with a type not inheriting from {nameof(ScriptableObject)}. (Given: {baseType.Name})");
			}

			ObjectCreatorWindow creatorWindow = GetWindow<ObjectCreatorWindow>("Object Creation", true, typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow"));
			creatorWindow.titleContent.image = EditorGUIUtility.IconContent("Toolbar Plus").image;
			creatorWindow.Initialise(baseType);
			creatorWindow.Show();
		}

		private class TypeGroup
		{
			public readonly Type TargetType;
			public readonly TypeGroup[] Children;

			public TypeGroup(Type targetType, Type[] allTypes)
			{
				TargetType = targetType;
				Children = allTypes.Where(t => (t.BaseType.IsGenericType ? t.BaseType.GetGenericTypeDefinition() : t.BaseType) == targetType)
									.Select(t => new TypeGroup(t, allTypes))
									.ToArray();
			}
		}
	}
}
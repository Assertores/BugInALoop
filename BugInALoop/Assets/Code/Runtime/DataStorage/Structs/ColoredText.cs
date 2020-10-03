using UnityEngine;

namespace BIAL.Runtime.DataStorage
{
	[System.Serializable]
	public struct ColoredText
	{
		public const string COLOR_CLOSER = "</color>";

		public static readonly ColoredText Empty = new ColoredText("");

		public string Text;
		public Color TextColor;
		public bool OverwriteColor;

		public override string ToString()
		{
			if (OverwriteColor)
			{
				return $"{TextColor.ToRichTextColorTag()}{Text}{COLOR_CLOSER}";
			}
			else
			{
				return Text;
			}
		}

		public ColoredText(string text, Color textColor, bool overwriteColor)
		{
			Text = text;
			TextColor = textColor;
			OverwriteColor = overwriteColor;
		}

		public ColoredText(string text)
		{
			Text = text;
			TextColor = Color.white;
			OverwriteColor = false;
		}
	}
}
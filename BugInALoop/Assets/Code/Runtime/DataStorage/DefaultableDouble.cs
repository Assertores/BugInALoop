namespace BIAL.Runtime.DataStorage
{
	[System.Serializable]
	public struct DefaultableDouble
	{
		public double NonDefaultValue;
		public bool UseDefault;
		public double DefaultOffset;
		public System.Func<double> DefaultGetter;

		public DefaultableDouble(double nonDefaultValue, bool useDefault, double defaultOffset, System.Func<double> defaultGetter)
		{
			NonDefaultValue = nonDefaultValue;
			UseDefault = useDefault;
			DefaultOffset = defaultOffset;
			DefaultGetter = defaultGetter;
		}

		public DefaultableDouble(System.Func<double> defaultGetter)
		{
			NonDefaultValue = default;
			UseDefault = true;
			DefaultOffset = default;
			DefaultGetter = defaultGetter;
		}

		private double GetDefaultedValue()
		{
			return UseDefault ? DefaultGetter.Invoke() + DefaultOffset : NonDefaultValue;
		}

		public static implicit operator double(DefaultableDouble target)
		{
			return target.GetDefaultedValue();
		}
	}
}
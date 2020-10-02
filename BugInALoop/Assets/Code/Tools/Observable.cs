using System;

public class Observable<T> {
	public Action OnValueChange;
	public Action<Observable<T>> OnValueChangeWithState;
	private T backedValue;

	public T value {
		get => backedValue;
		set {
			if(value.Equals(backedValue)) {
				return;
			}

			backedValue = value;
			OnValueChange?.Invoke();
			OnValueChangeWithState?.Invoke(this);
		}
	}

	public Observable() {
		backedValue = default;
	}

	public Observable(T value) {
		backedValue = value;
	}

	public static Observable<T> operator +(Observable<T> self, Action action) {
		self.OnValueChange += action;

		return self;
	}

	public static Observable<T> operator +(Observable<T> self, Action<Observable<T>> action) {
		self.OnValueChangeWithState += action;

		return self;
	}

	public static Observable<T> operator -(Observable<T> self, Action action) {
		self.OnValueChange -= action;

		return self;
	}

	public static Observable<T> operator -(Observable<T> self, Action<Observable<T>> action) {
		self.OnValueChangeWithState -= action;

		return self;
	}
}


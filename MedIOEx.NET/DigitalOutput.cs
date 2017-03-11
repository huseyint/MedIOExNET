namespace MedIOExNET
{
	/// <summary>
	/// Represents digital output.
	/// </summary>
	public enum DigitalOutput : byte
	{
		/// <summary>
		/// 24VDC transistor output or buzzer Use SW2 for switching.
		/// </summary>
		Gpio_J4_4 = 0,

		/// <summary>
		/// 24VDC transistor output.
		/// </summary>
		Gpio_J4_3,

		/// <summary>
		/// 24VDC transistor output.
		/// </summary>
		Gpio_J4_2,

		/// <summary>
		/// 24VDC transistor output.
		/// </summary>
		Gpio_J4_1,

		/// <summary>
		/// Relay output ch1, up to 250V.
		/// </summary>
		Gpio_J3_4,

		/// <summary>
		/// Relay output ch2, up to 250V.
		/// </summary>
		Gpio_J3_3,

		/// <summary>
		/// Relay output ch3, up to 250V.
		/// </summary>
		Gpio_J3_2,

		/// <summary>
		/// Relay output ch3, up to 250V.
		/// </summary>
		Gpio_J3_1,

		/// <summary>
		/// 24VDC transistor output.
		/// </summary>
		Gpio_J6_12,

		/// <summary>
		/// 24VDC transistor output.
		/// </summary>
		Gpio_J6_11,

		/// <summary>
		/// 24VDC transistor output.
		/// </summary>
		Gpio_J6_10,

		/// <summary>
		/// 24VDC transistor output.
		/// </summary>
		Gpio_J6_9,

		/// <summary>
		/// 24VDC transistor output.
		/// </summary>
		Gpio_J5_8,

		/// <summary>
		/// 24VDC transistor output.
		/// </summary>
		Gpio_J5_7,

		/// <summary>
		/// 24VDC transistor output.
		/// </summary>
		Gpio_J5_6,

		/// <summary>
		/// 24VDC transistor output.
		/// </summary>
		Gpio_J5_5,
	}
}
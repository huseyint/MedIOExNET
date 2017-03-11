namespace MedIOExNET
{
	/// <summary>
	/// Represents analog input.
	/// </summary>
	public enum AnalogInput
	{
		/// <summary>
		/// 0 - 10V DC analog input ch0, no need resistor or external supply.
		/// </summary>
		Gpio_J13_1 = 0, //  

		/// <summary>
		/// 0 - 10V DC analog input ch1.
		/// </summary>
		Gpio_J13_2,

		/// <summary>
		/// 0 - 10V DC analog input ch2.
		/// </summary>
		Gpio_J13_3,

		/// <summary>
		/// 0 - 10V DC analog input ch3, use (24V sense -> SW2) switch to meas. power supply voltage.
		/// </summary>
		Gpio_J13_4,
	}
}
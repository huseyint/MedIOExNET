namespace MedIOExNET
{
	/// <summary>
	/// Represents analog output.
	/// </summary>
	public enum AnalogOutput
	{
		/// <summary>
		/// None.
		/// </summary>
		None = -1,

		/// <summary>
		/// 0 - 10V DC analog output ch0, no need resistor or external supply.
		/// </summary>
		Gpio_J1_1 = 0,

		/// <summary>
		/// 0 - 10V DC analog output ch1.
		/// </summary>
		Gpio_J1_2 = 1,
		
		/// <summary>
		/// 0 - 10V DC analog output ch2.
		/// </summary>
		Gpio_J1_3 = 2,
		
		/// <summary>
		/// 0 - 10V DC analog output ch3.
		/// </summary>
		Gpio_J1_4  =3,
	}
}
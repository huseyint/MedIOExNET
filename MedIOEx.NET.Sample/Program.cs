using System;
using System.Threading;

namespace MedIOExNET.Sample
{
	public static class Program
	{
		private static MedIOEx _medIoEx;

		public static void Main(string[] args)
		{
			var cancellationTokenSource = new CancellationTokenSource();

			var choices = new ParameterizedThreadStart[]
			{
				TestDigitalOutput,
				TestDigitalInput,
				TestAnalogOutput,
				TestAnalogInput,
			};

			ParameterizedThreadStart threadStart;

			while (true)
			{
				Console.Clear();
				Console.WriteLine("1. Digital Output Test");
				Console.WriteLine("2. Digital Input Test");
				Console.WriteLine("3. Analog Output Test");
				Console.WriteLine("4. Analog Input Test");
				Console.Write("Please select (1-4): ");

				var key = Console.Read();

				Console.WriteLine();

				var choice = key - 49;

				if (choice > -1 && choice < choices.Length)
				{
					threadStart = choices[choice];
					break;
				}
			}

			using (_medIoEx = new MedIOEx())
			{
				var thread = new Thread(threadStart);

				thread.Start(cancellationTokenSource.Token);

				Console.WriteLine("Press <ENTER> to close.");
				Console.ReadLine();

				cancellationTokenSource.Cancel();
			}
		}

		private static void TestDigitalOutput(object o)
		{
			Console.WriteLine("Running Digital Output Test");
			var cancellationToken = (CancellationToken)o;

			_medIoEx.InitializeDigitalInputAndOutput();

			while (true)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J3_1);
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J3_2);
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J3_3);
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J3_4);

				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J4_1); //sw2 for buzzer 
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J4_2);
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J4_3);
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J4_4);

				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J5_5);
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J5_6);
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J5_7);
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J5_8);

				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J6_9);
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J6_10);
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J6_11);
				_medIoEx.SetDigitalOutputHigh((byte)DigitalOutput.Gpio_J6_12);

				_medIoEx.Delay(1000);

				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J3_1);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J3_2);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J3_3);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J3_4);

				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J4_1);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J4_2);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J4_3);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J4_4);

				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J5_5);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J5_6);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J5_7);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J5_8);

				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J6_9);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J6_10);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J6_11);
				_medIoEx.SetDigitalOutputLow((byte)DigitalOutput.Gpio_J6_12);

				_medIoEx.Delay(1000);
			}
		}

		private static void TestDigitalInput(object o)
		{
			var cancellationToken = (CancellationToken)o;

			_medIoEx.InitializeDigitalInputAndOutput();

			while (true)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				Console.Clear();
				Console.WriteLine("Running Digital Input Test");
				Console.WriteLine("Press <ENTER> to close.");

				Console.WriteLine("J17_1 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J17_1));
				Console.WriteLine("J17_2 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J17_2));
				Console.WriteLine("J17_3 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J17_3));
				Console.WriteLine("J17_4 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J17_4));
				Console.WriteLine("J16_5 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J16_5));
				Console.WriteLine("J16_6 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J16_6));
				Console.WriteLine("J16_7 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J16_7));
				Console.WriteLine("J16_8 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J16_8));
				Console.WriteLine("J15_9 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J15_9));
				Console.WriteLine("J15_10 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J15_10));
				Console.WriteLine("J15_11 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J15_11));
				Console.WriteLine("J15_12 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J15_12));
				Console.WriteLine("J14_13 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J14_13));
				Console.WriteLine("J14_14 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J14_14));
				Console.WriteLine("J14_15 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J14_15));
				Console.WriteLine("J14_16 {0}", _medIoEx.GetDigitalInputValue((byte)DigitalInput.Gpio_J14_16));

				_medIoEx.Delay(100);
			}
		}

		private static void TestAnalogOutput(object o)
		{
			Console.WriteLine("Running Analog Output Test");
			var cancellationToken = (CancellationToken)o;

			_medIoEx.InitializeAnalogOutput(); //initialize Analog output
			var i = 0;

			Console.WriteLine("Analog Output test program starting");
			Console.WriteLine();

			while (true)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				_medIoEx.SetAnalogOutputValue(AnalogOutput.Gpio_J1_1, i); //every cycle pin voltage will be increased until 10V. 
				_medIoEx.SetAnalogOutputValue(AnalogOutput.Gpio_J1_2, i);
				_medIoEx.SetAnalogOutputValue(AnalogOutput.Gpio_J1_3, i);
				_medIoEx.SetAnalogOutputValue(AnalogOutput.Gpio_J1_4, i);

				i += 100;
				if (i > 4095)
				{
					i = 0;
				}

				_medIoEx.Delay(1000);
			}
		}

		private static void TestAnalogInput(object o)
		{
			var cancellationToken = (CancellationToken)o;

			_medIoEx.InitializeAnalogInput();

			while (true)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				Console.Clear();
				Console.WriteLine("Running Analog Input Test");
				Console.WriteLine("Press <ENTER> to close.");

				Console.WriteLine("ch0__J13_1 : {0}", _medIoEx.GetAnalogInputValue(AnalogInput.Gpio_J13_1));
				_medIoEx.Delay(100);
				Console.WriteLine("ch0__J13_2 : {0}", _medIoEx.GetAnalogInputValue(AnalogInput.Gpio_J13_2));
				_medIoEx.Delay(100);
				Console.WriteLine("ch0__J13_3 : {0}", _medIoEx.GetAnalogInputValue(AnalogInput.Gpio_J13_3));
				_medIoEx.Delay(100);
				Console.WriteLine("ch0__J13_4 : {0}", _medIoEx.GetAnalogInputValue(AnalogInput.Gpio_J13_4)); //24v sense,use SW2

				_medIoEx.Delay(1000);
				Console.WriteLine();
				Console.WriteLine();
			}
		}
	}
}
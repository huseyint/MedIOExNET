using System;
using LibBcm2835.Net;

namespace MedIOExNET
{
	public class MedIOEx : IDisposable
	{
		private static readonly byte Gpio_AI_CS3 = 25;

		private static byte pe2a_mcp23s17_adr = 0x40;
		private static byte setGPIOAorB;
		private static byte pe2a_mcp23s17_GPIOA = 0x12;
		private static byte pe2a_mcp23s17_GPIOB = 0x13;
		private static byte set_DO_apin;
		private static byte set_DO_bpin;

		// ANALOG OUTPUT dac124s085 variable declaration
		private static readonly byte[] AO_tbuf1 = new byte[2];
		private static readonly byte[] AO_rbuf1 = new byte[2];

		private readonly Bcm2835 _bcm2835;

		public MedIOEx()
		{
			// Will extract (from embedded resource) and compile the library 
			// if it doesn't exist in the same directory as LibBcm2835.dll.
			Bcm2835.ExtractAndCompileLibraryIfNotExists();

			_bcm2835 = Bcm2835.Instance;
		}

		#region Initialization

		public void InitializeDigitalInputAndOutput()
		{
			if (_bcm2835.bcm2835_init() == 0)
			{
				throw new InvalidOperationException("bcm2835_init failed. Are you running as root?");
			}

			InitializeSpi();

			// Clear MCP23S17 GPIOs
			ClearDigitalOutputBits();

			///*DIGITAL INPUT init decleration*/
			var Gpio_Arr_in = new[]
			{
				(byte)DigitalInput.Gpio_J14_16,
				(byte)DigitalInput.Gpio_J14_15,
				(byte)DigitalInput.Gpio_J14_14,
				(byte)DigitalInput.Gpio_J14_13,
				(byte)DigitalInput.Gpio_J15_12,
				(byte)DigitalInput.Gpio_J15_11,
				(byte)DigitalInput.Gpio_J15_10,
				(byte)DigitalInput.Gpio_J15_9,
				(byte)DigitalInput.Gpio_J16_8,
				(byte)DigitalInput.Gpio_J16_7,
				(byte)DigitalInput.Gpio_J16_6,
				(byte)DigitalInput.Gpio_J16_5,
				(byte)DigitalInput.Gpio_J17_4,
				(byte)DigitalInput.Gpio_J17_3,
				(byte)DigitalInput.Gpio_J17_2,
				(byte)DigitalInput.Gpio_J17_1,
			};

			for (var i = 0; i < Gpio_Arr_in.Length; ++i)
			{
				_bcm2835.bcm2835_gpio_fsel(Gpio_Arr_in[i], (byte)Bcm2835.bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT);
				_bcm2835.bcm2835_gpio_set_pud(Gpio_Arr_in[i], (byte)Bcm2835.bcm2835PUDControl.BCM2835_GPIO_PUD_UP);
			}
		}

		public void InitializeAnalogOutput()
		{
			if (_bcm2835.bcm2835_init() == 0)
			{
				throw new InvalidOperationException("bcm2835_init failed. Are you running as root?");
			}

			InitializeSpi();

			//it would be 'op1 1 && op2 0' for no voltage 
			SetAnalogOutputValue(AnalogOutput.None, 0); //all register would be 0 as starting position (init), -1 can be ignored
			pe2a_AO_OP1OP2_choosing(2, ref AO_tbuf1[0]); //mode 2: write all registers and update outputs  

			//transferring to dac124s085 by TI
			_bcm2835.bcm2835_spi_transfernb(AO_tbuf1, AO_rbuf1, (uint)AO_tbuf1.Length);
		}

		public void InitializeAnalogInput()
		{
			/*Analog input max11627*/
			if (_bcm2835.bcm2835_init() == 0)
			{
				throw new InvalidOperationException("bcm2835_init failed. Are you running as root?");
			}

			InitializeSpi();
			_bcm2835.bcm2835_gpio_fsel(Gpio_AI_CS3, (byte)Bcm2835.bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
		}

		private void InitializeSpi()
		{
			_bcm2835.bcm2835_spi_begin();
			_bcm2835.bcm2835_spi_setBitOrder((byte)Bcm2835.bcm2835SPIBitOrder.BCM2835_SPI_BIT_ORDER_MSBFIRST);
			_bcm2835.bcm2835_spi_setDataMode((byte)Bcm2835.bcm2835SPIMode.BCM2835_SPI_MODE0);
			_bcm2835.bcm2835_spi_setClockDivider((ushort)Bcm2835.bcm2835SPIClockDivider.BCM2835_SPI_CLOCK_DIVIDER_1024);
		}

		#endregion

		#region Digital Output/Input Set/Get

		public void SetDigitalOutputHigh(byte device)
		{
			if (device <= (byte)DigitalOutput.Gpio_J3_1)
			{
				setGPIOAorB = pe2a_mcp23s17_GPIOB;
				pe2a_DO_getHigh(ref set_DO_bpin, device);
				pe2a_mcp23s17_tr(pe2a_mcp23s17_adr, set_DO_bpin);
			}
			else if (device >= (byte)DigitalOutput.Gpio_J6_12)
			{
				setGPIOAorB = pe2a_mcp23s17_GPIOA;
				pe2a_DO_getHigh(ref set_DO_apin, (byte)(device - (DigitalOutput.Gpio_J3_1 + 1)));
				pe2a_mcp23s17_tr(pe2a_mcp23s17_adr, set_DO_apin);
			}
		}

		public void SetDigitalOutputLow(byte device)
		{
			//DO pin should be 0
			if (device <= (byte)DigitalOutput.Gpio_J3_1)
			{
				setGPIOAorB = pe2a_mcp23s17_GPIOB;
				pe2a_DO_getLow(ref set_DO_bpin, device);
				pe2a_mcp23s17_tr(pe2a_mcp23s17_adr, set_DO_bpin);
			}
			else if (device >= (byte)DigitalOutput.Gpio_J6_12)
			{
				setGPIOAorB = pe2a_mcp23s17_GPIOA;
				pe2a_DO_getLow(ref set_DO_apin, (byte)(device - (DigitalOutput.Gpio_J3_1 + 1)));
				pe2a_mcp23s17_tr(pe2a_mcp23s17_adr, set_DO_apin);
			}
		}

		public void ClearDigitalOutputBits()
		{
			var arrClearBits = new byte[] { 0x01, 0x00, 0x12, 0x13 };

			foreach (byte t in arrClearBits)
			{
				setGPIOAorB = t;
				pe2a_mcp23s17_tr(pe2a_mcp23s17_adr, 0);
			}
		}

		private static void pe2a_DO_getHigh(ref byte ptr, byte size)
		{
			// controlling DO bit as 1
			ptr |= (byte)(1 << size);
		}

		private static void pe2a_DO_getLow(ref byte ptr, byte size)
		{
			// controlling DO bit as 0 
			ptr &= (byte)~(1 << size);
		}

		public bool GetDigitalInputValue(byte pin)
		{
			return (_bcm2835.bcm2835_gpio_lev(pin) & 1) == 0;
		}

		private void pe2a_mcp23s17_tr(byte device, byte txdata)
		{
			_bcm2835.bcm2835_gpio_write(Gpio_AI_CS3, Bcm2835.HIGH);
			_bcm2835.bcm2835_spi_chipSelect((byte)Bcm2835.bcm2835SPIChipSelect.BCM2835_SPI_CS1);
			_bcm2835.bcm2835_spi_setChipSelectPolarity((byte)Bcm2835.bcm2835SPIChipSelect.BCM2835_SPI_CS1, Bcm2835.LOW);

			var do_tbuf = new byte[3];

			device = (byte)(device & 0xfe); // Clear last bit for a write
			do_tbuf[0] = device;
			do_tbuf[1] = setGPIOAorB; //from coming global val. 
			do_tbuf[2] = txdata;

			_bcm2835.bcm2835_spi_transfern(do_tbuf, (uint)do_tbuf.Length);
		}

		#endregion

		#region Analog Output/Input Set/Get

		public void SetAnalogOutputValue(AnalogOutput pin, int getVal)
		{
			//0: 0V, 4095 : 10V
			//in normal operation, you should use delay function at least <!> " < = 100ms " <!>

			//when you work with ao or do, you can provide ai cs High cause of reading/writing properly 
			_bcm2835.bcm2835_gpio_write(Gpio_AI_CS3, Bcm2835.HIGH); //analog input chip select high, so ai disabled 

			_bcm2835.bcm2835_spi_chipSelect((byte)Bcm2835.bcm2835SPIChipSelect.BCM2835_SPI_CS0);      //analog output cs               

			_bcm2835.bcm2835_spi_setChipSelectPolarity((byte)Bcm2835.bcm2835SPIChipSelect.BCM2835_SPI_CS0, Bcm2835.LOW);   //analog output cs active

			AO_tbuf1[0] = pe2a_AO_wrBits_firstArr(getVal);
			AO_tbuf1[1] = pe2a_AO_wrBits_secArr(getVal);

			//choosing channel number just setting tbuf1[0]
			pe2a_AO_ch_choosing(pin, ref AO_tbuf1[0]);

			//default value 01 and it can be changed according to needed
			pe2a_AO_OP1OP2_choosing(1, ref AO_tbuf1[0]);

			//transferring to dac124s085 by TI
			_bcm2835.bcm2835_spi_transfernb(AO_tbuf1, AO_rbuf1, (uint)AO_tbuf1.Length);
		}

		private static byte pe2a_AO_wrBits_firstArr(int getVal)
		{
			//transfer function for AO first array 
			/*
			have two array of AO. c1 = 0b00000000 , c2 = 0b00000000
			11,10,9,8 bit setting = pe2a_AO_wrBits_firstArr()
			c2 shall be transferred directly but not c1.
			setting AO channels and the other stuff over c1 array
			*/

			var i = 11; //12bit
			byte c1 = 0;


			for (; i >= 8; --i)
			{
				if ((getVal >> i & 1) != 0)
				{
					pe2a_AO_writeHigh_int(ref c1, (byte)(i - 8));
				}
			}

			return c1;
		}

		private static byte pe2a_AO_wrBits_secArr(int getVal)
		{
			//transfer function for AO second array
			/*
			have two array of AO. c1 = 0b00000000 , c2 = 0b00000000
			7,6...0 bit setting = pe2a_AO_wrBits_secArr()
			c2 shall be transferred directly but not c1. 
			setting AO channels and the other stuff over c1 array 
			*/
			int i = 7; //12bit 
			byte c2 = 0;

			for (; i >= 0; --i)
			{
				if ((getVal >> i & 1) != 0)
				{
					pe2a_AO_writeHigh_int(ref c2, (byte)i);
				}
			}

			return c2;
		}

		private static void pe2a_AO_ch_choosing(AnalogOutput setVal, ref byte ptr)
		{
			switch (setVal)
			{
				case AnalogOutput.Gpio_J1_1:
					//daca 00
					pe2a_AO_wrLow(ref ptr, 7);
					pe2a_AO_wrLow(ref ptr, 6);

					break;
				case AnalogOutput.Gpio_J1_2:
					//dacb	01
					pe2a_AO_wrLow(ref ptr, 7);
					pe2a_AO_wrHigh(ref ptr, 6);

					break;
				case AnalogOutput.Gpio_J1_3:
					//dacc 10
					pe2a_AO_wrLow(ref ptr, 6);
					pe2a_AO_wrHigh(ref ptr, 7);

					break;
				case AnalogOutput.Gpio_J1_4:
					//dacd 11
					pe2a_AO_wrHigh(ref ptr, 7);
					pe2a_AO_wrHigh(ref ptr, 6);

					break;
			}
		}

		private static void pe2a_AO_OP1OP2_choosing(byte val, ref byte ptr)
		{
			//detailed information TI dac124s085 datasheet
			//OP1 & OP2
			// 00 : write specified register but do not update outputs
			if (val == 0)
			{
				pe2a_AO_wrLow(ref ptr, 5);
				pe2a_AO_wrLow(ref ptr, 4);
			}

			//01 : write to specified register and update outputs
			else if (val == 1)
			{
				pe2a_AO_wrLow(ref ptr, 5);
				pe2a_AO_wrHigh(ref ptr, 4);
			}

			//10 : write all registers and update outputs
			else if (val == 2)
			{
				pe2a_AO_wrLow(ref ptr, 4);
				pe2a_AO_wrHigh(ref ptr, 5);
			}

			//11 : power down outputs
			else if (val == 3)
			{
				pe2a_AO_wrHigh(ref ptr, 5);
				pe2a_AO_wrHigh(ref ptr, 4);
			}
		}

		private static void pe2a_AO_wrLow(ref byte ptr, byte bitNumber)
		{
			// right bit shifting for setting AO produces between 0-4095 
			ptr &= (byte)~(1 << bitNumber);
		}

		private static void pe2a_AO_wrHigh(ref byte ptr, byte bitNumber)
		{
			// left bit shifting for setting AO produces between 0-4095 
			ptr |= (byte)(1 << bitNumber);
		}

		private static void pe2a_AO_writeHigh_int(ref byte ptr, byte bitNumber)
		{
			ptr |= (byte)(1 << bitNumber);
		}

		public int GetAnalogInputValue(AnalogInput pin)
		{
			//return AI val as chosen channel 
			var aiTbuf = new byte[3];
			var aiRbuf = new byte[3];

			_bcm2835.bcm2835_spi_setChipSelectPolarity((byte)Bcm2835.bcm2835SPIChipSelect.BCM2835_SPI_CS0, Bcm2835.HIGH);   //analog output cs disabled
			_bcm2835.bcm2835_spi_setChipSelectPolarity((byte)Bcm2835.bcm2835SPIChipSelect.BCM2835_SPI_CS1, Bcm2835.HIGH);   //digital output cs disabled

			_bcm2835.bcm2835_gpio_write(Gpio_AI_CS3, Bcm2835.LOW); //analog input chip select active

			if (pe2a_AI_getVal_cnv_choosing(pin, ref aiTbuf) < 0)
			{
				return -1;
			}

			_bcm2835.bcm2835_spi_transfernb(aiTbuf, aiRbuf, (uint)aiTbuf.Length);

			return pe2a_AI_getVal_cond1(ref aiRbuf);
		}

		private static int pe2a_AI_getVal_cnv_choosing(AnalogInput pin, ref byte[] ptr)
		{
			switch (pin)
			{
				case AnalogInput.Gpio_J13_1:
					ptr[0] = 0x86; //conversion ch0 
					break;
				case AnalogInput.Gpio_J13_2:
					ptr[0] = 0x8e; //conversion ch1
					break;
				case AnalogInput.Gpio_J13_3:
					ptr[0] = 0x96; //conversion ch2
					break;
				case AnalogInput.Gpio_J13_4:
					ptr[0] = 0x9e; //conversion ch3
					break;
				default:
					return -1;
			}

			ptr[1] = 0x60; //setup
			ptr[2] = 0x3c; //ave

			return 0;
		}

		private static int pe2a_AI_getVal_cond1(ref byte[] ptr)
		{
			//AI bit shifting
			if (ptr[0] < 16)
			{
				return ptr[0] * (1 << 8) + ptr[1];
			}

			return ptr[1] * (1 << 8) + ptr[0];
		}

		#endregion

		#region Public Helpers

		public void Delay(uint milliseconds)
		{
			_bcm2835.bcm2835_delay(milliseconds);
		}

		public void Close()
		{
			_bcm2835.bcm2835_close();
		}

		#endregion

		public void Dispose()
		{
			Close();
		}
	}
}
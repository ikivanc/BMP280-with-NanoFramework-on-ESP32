using System;
using System.Device.I2c;
using System.Diagnostics;
using System.Threading;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Iot.Device.Common;
using UnitsNet;

namespace BMP280Sensor
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello Bmp280!");

            // bus id on the ESP32
            const int busId = 1;

            I2cConnectionSettings i2cSettings = new(busId, Bmp280.DefaultI2cAddress);
            I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
            using var i2CBmp280 = new Bmp280(i2cDevice);

            while (true)
            {
                // set higher sampling
                i2CBmp280.TemperatureSampling = Sampling.LowPower;
                i2CBmp280.PressureSampling = Sampling.UltraHighResolution;

                // Perform a synchronous measurement
                var readResult = i2CBmp280.Read();
                i2CBmp280.TryReadAltitude(out var altValue);

                // Print out the measured data
                Debug.WriteLine("-----------------------------------------");
                Debug.WriteLine($"Temperature: {readResult.Temperature.DegreesCelsius}\u00B0C");
                Debug.WriteLine($"Pressure: {readResult.Pressure.Hectopascals}hPa");
                Debug.WriteLine($"Altitude: {altValue.Meters}m");

                Thread.Sleep(2000);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Y.ASIS.Common.Communication
{
    public class ModbusTcpClient : TcpClient
    {
        private ushort sn = 0;

        private readonly object locker = new object();
        private readonly Dictionary<ushort, byte[]> received;
        private readonly List<byte> buffer;

        public int ReadWriteTimeout { get; set; } = 1 * 1000;

        public byte StationCode { get; set; } = 0x01;

        public ModbusTcpClient(string ip, int port = 502)
            : base(ip, port)
        {
            received = new Dictionary<ushort, byte[]>();
            buffer = new List<byte>();
            ReceivedData += OnReceivedData;
            Connected += OnConnected;
        }

        private void OnConnected(object sender, SocketEventArgs e)
        {
            lock (locker)
            {
                sn = 0;
            }
            received.Clear();
            buffer.Clear();
        }

        private void OnReceivedData(object sender, SocketReceivedDataEventArgs e)
        {
            buffer.AddRange(e.Data);
            if (buffer.Count < 8)
            {
                return;
            }
            byte[] data = buffer.Take(8).ToArray();
            ushort sn = BitConverter.ToUInt16(data, 0);
            ushort length = BitConverter.ToUInt16(data.Skip(4).Take(2).Reverse().ToArray(), 0);
            length -= 2;
            if (buffer.Count < 8 + length)
            {
                return;
            }
            data = buffer.Skip(8).Take(length).ToArray();
            buffer.RemoveRange(0, 8 + length);
            if (received.ContainsKey(sn))
            {
                received[sn] = data;
            }
        }

        public bool ReadHoldingRegister(int startingAddress, out short value)
        {
            value = 0;
            bool success = SendRead(startingAddress, 1, 0x03, out ushort sn);
            if (!success)
            {
                return false;
            }
            DateTime timeout = DateTime.Now.AddMilliseconds(ReadWriteTimeout);
            while (DateTime.Now < timeout)
            {
                if (received.TryGetValue(sn, out byte[] data) && data != null)
                {
                    value = BitConverter.ToInt16(data.Reverse().ToArray(), 0);
                    received.Remove(sn);
                    return true;
                }
                Thread.Sleep(1);
            }
            received.Remove(sn);
            return false;
        }

        public bool ReadHoldingRegisters(int startingAddress, int quantity, out ushort[] values)
        {
            values = null;
            bool success = SendRead(startingAddress, quantity, 0x03, out ushort sn);
            if (!success)
            {
                return false;
            }
            DateTime timeout = DateTime.Now.AddMilliseconds(ReadWriteTimeout);
            while (DateTime.Now < timeout)
            {
                if (received.TryGetValue(sn, out byte[] data) && data != null)
                {
                    values = new ushort[(data.Length - 1) / 2];

                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = BitConverter.ToUInt16(data.Skip(2 * i + 1).Take(2).Reverse().ToArray(), 0);
                    }
                    received.Remove(sn);
                    return true;
                }
                Thread.Sleep(1);
            }
            received.Remove(sn);
            return false;
        }

        public bool WriteSingleRegister(int startingAddress, short value)
        {
            byte[] array = BitConverter.GetBytes(value).ToArray();

            byte[] data = new byte[12];
            ushort sn = GetSerialNumber();

            // 0~1
            Array.Copy(BitConverter.GetBytes(sn), data, 2);

            // 2~3 modbus tcp
            data[2] = 0x00;
            data[3] = 0x00;

            // 4~5 then length
            data[4] = 0x00;
            data[5] = 0x06;

            // 6 station address
            data[6] = StationCode;

            // 7 function code
            data[7] = 0x06;

            if (startingAddress < 0
                || startingAddress > 65535)
            {
                throw new ArgumentException("Starting address must be 0 - 65535");
            }
            // 8~9 starting address
            byte[] address = BitConverter.GetBytes(startingAddress).Reverse().ToArray();
            Array.Copy(address, 2, data, 8, 2);

            // then write data
            array.Reverse().ToArray().CopyTo(data, 10);

            if (!received.ContainsKey(sn))
            {
                received.Add(sn, null);
            }
            bool success = Send(data);
            if (!success)
            {
                received.Remove(sn);
                return false;
            }

            DateTime timeout = DateTime.Now.AddMilliseconds(ReadWriteTimeout);
            while (DateTime.Now < timeout)
            {
                if (received.TryGetValue(sn, out _))
                {
                    received.Remove(sn);
                    return true;
                }
                Thread.Sleep(1);
            }
            received.Remove(sn);
            return false;
        }

        public bool WriteMultipleRegisters(int startingAddress, short[] values)
        {
            byte[] array = new byte[values.Length * 2];

            for (int i = 0; i < values.Length; i++)
            {
                byte[] temp = BitConverter.GetBytes(values[i]).Reverse().ToArray();
                temp.CopyTo(array, 2 * i);
            }

            byte[] data = new byte[13 + array.Length];
            ushort sn = GetSerialNumber();

            // 0~1
            Array.Copy(BitConverter.GetBytes(sn), data, 2);

            // 2~3 modbus tcp
            data[2] = 0x00;
            data[3] = 0x00;

            // 4~5 then length
            int length = 7 + array.Length;
            byte[] lengthArray = BitConverter.GetBytes(length).Reverse().ToArray();
            Array.Copy(lengthArray, 2, data, 4, 2);

            // 6 station address
            data[6] = StationCode;

            // 7 function code
            data[7] = 0x10;

            if (startingAddress < 0
                || startingAddress > 65535)
            {
                throw new ArgumentException("Starting address must be 0 - 65535");
            }
            // 8~9 starting address
            byte[] address = BitConverter.GetBytes(startingAddress).Reverse().ToArray();
            Array.Copy(address, 2, data, 8, 2);

            // 10~11 quantity
            data[10] = (byte)(array.Length / 2 / 256);
            data[11] = (byte)(array.Length / 2 % 256);

            // 12 byte length
            data[12] = (byte)(array.Length);

            // then write data
            array.CopyTo(data, 13);

            if (!received.ContainsKey(sn))
            {
                received.Add(sn, null);
            }
            bool success = Send(data);
            if (!success)
            {
                received.Remove(sn);
                return false;
            }

            DateTime timeout = DateTime.Now.AddMilliseconds(ReadWriteTimeout);
            while (DateTime.Now < timeout)
            {
                if (received.TryGetValue(sn, out _))
                {
                    received.Remove(sn);
                    return true;
                }
                Thread.Sleep(1);
            }
            received.Remove(sn);
            return false;
        }

        private bool SendRead(int startingAddress, int quantity, byte functionCode, out ushort sn)
        {
            byte[] data = new byte[12];
            sn = GetSerialNumber();
            // 0~1
            Array.Copy(BitConverter.GetBytes(sn), data, 2);

            // 2~3 modbus tcp
            data[2] = 0x00;
            data[3] = 0x00;

            // 4~5 then length
            data[4] = 0x00;
            data[5] = 0x06;

            // 6 station address
            data[6] = StationCode;

            // 7 function code
            data[7] = functionCode;

            if (startingAddress < 0
                || startingAddress > 65535
                || quantity < 0
                || quantity > 125)
            {
                throw new ArgumentException("Starting address must be 0 - 65535; quantity must be 0 - 125");
            }

            // 8~9 starting address
            byte[] address = BitConverter.GetBytes(startingAddress).Reverse().ToArray();
            Array.Copy(address, 2, data, 8, 2);

            // 10~11 quantity
            byte[] quantities = BitConverter.GetBytes(quantity).Reverse().ToArray();
            Array.Copy(quantities, 2, data, 10, 2);

            if (!received.ContainsKey(sn))
            {
                received.Add(sn, null);
            }
            bool success = Send(data);
            if (!success)
            {
                received.Remove(sn);
            }
            return success;
        }

        private ushort GetSerialNumber()
        {
            lock (locker)
            {
                if (sn == ushort.MaxValue)
                {
                    sn = ushort.MinValue;
                }
                return sn++;
            }
        }
    }
}

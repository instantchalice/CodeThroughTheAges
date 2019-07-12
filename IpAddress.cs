using Newtonsoft.Json;
using System.Net;

namespace Utilities
{
    public sealed class IpAddress
    {
        private byte? _partA;
        private byte? _partB;
        private byte? _partC;
        private byte? _partD;

        [JsonConstructor]
        public IpAddress() {}

        public IpAddress(byte? partA, byte? partB, byte? partC, byte? partD)
        {
            this.PartA = partA;
            this.PartB = partB;
            this.PartC = partC;
            this.PartD = partD;
        }

        public IpAddress(string ipAddress)
        {
            var ip = IPAddress.Parse(ipAddress).GetAddressBytes();
            this.PartA = ip[0];
            this.PartB = ip[1];
            this.PartC = ip[2];
            this.PartD = ip[3];
        }

        [Exportable]
        public byte? PartA
        {
            get => _partA;
            set
            {
                _partA = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IpFullString));
            }
        }

        [Exportable]
        public byte? PartB
        {
            get => _partB;
            set
            {
                _partB = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IpFullString));
            }
        }

        [Exportable]
        public byte? PartC
        {
            get => _partC;
            set
            {
                _partC = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IpFullString));
            }
        }

        [Exportable]
        public byte? PartD
        {
            get => _partD;
            set
            {
                _partD = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IpFullString));
            }
        }

        public string IpFullString => this.CompletePart(PartA) + "." +
                                      this.CompletePart(PartB) + "." +
                                      this.CompletePart(PartC) + "." +
                                      this.CompletePart(PartD);

        public override bool IsValid => PartAIsValid && PartBIsValid && PartCIsValid && PartDIsValid;

        public bool PartAIsValid => PartA.HasValue && PartA.Value > 0 && PartA.Value < 255;
        public bool PartBIsValid => PartB.HasValue && PartB.Value <= 255;
        public bool PartCIsValid => PartC.HasValue && PartC.Value <= 255;
        public bool PartDIsValid => PartD.HasValue && PartD.Value <= 255;

        public override string ToString()
        {
            return this.IpFullString;
        }

        public IPAddress ToSystemIpAddress()
        {
            return IpFullString == null || IpFullString == "..." ? null : IPAddress.Parse(IpFullString);
        }

        private string CompletePart(byte? part)
        {
            if (!part.HasValue)
            {
                return string.Empty;
            }

            var value = part.ToString();
            if (value.Length == 3)
            {
                return value;
            }

            if (value.Length == 2)
            {
                return "0" + value;
            }

            if (value.Length == 1)
            {
                return "00" + value;
            }

            return string.Empty;
        }

        public IpAddress Clone()
        {
            var address = new IpAddress(this.IpFullString);
            return address;
        }
    }
}
using System;

namespace Utilities
{
    public sealed class GeoLocation
    {
        private double _degrees;
        private double _minutes;
        private double _seconds;
        private decimal? _coordinate;
        private Direction _direction;

        public GeoLocation() { }

        public GeoLocation(decimal? coordinate)
        {
            Coordinate = coordinate;
        }

        public GeoLocation(decimal? coordinate, CoordinateUnit coordinateUnit)
        {
            Coordinate = coordinate;
            GetDirection(coordinateUnit);
        }

        private void GetDirection(CoordinateUnit coordinateUnit)
        {
            if ( _coordinate == null )
            {
                return;
            }

            if ( _coordinate > 0)
            {
                _direction = coordinateUnit == CoordinateUnit.Latitude ? Direction.North : Direction.East;
            }
            else
            {
                _direction = coordinateUnit == CoordinateUnit.Latitude ? Direction.South : Direction.West;
            }
        }

        public double Degrees
        {
            get => _degrees;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _degrees = value;

                OnPropertyChanged();
                SetCoordinateValue();
            }
        }

        public double Minutes
        {
            get => _minutes;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _minutes = value;
                OnPropertyChanged();
                SetCoordinateValue();
            }
        }

        public double Seconds
        {
            get => _seconds;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _seconds = value;
                OnPropertyChanged();
                SetCoordinateValue();
            }
        }

        public Direction Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                OnPropertyChanged();
                SetCoordinateValue();
            }
        }

        public decimal? Coordinate
        {
            get => _coordinate;
            set
            {
                _coordinate = value;
                OnPropertyChanged();
                SetComponentValues();
            }
        }

        #region Overrides of Object

        public override string ToString()
        {
            return $"{Math.Round(Degrees, 2)}\u00b0 {Minutes}' {Seconds}'' {Direction.GetDescription()}";
        }

        #endregion

        private void SetComponentValues()
        {
            if (!_coordinate.HasValue)
            {
                _seconds = 0;
                _minutes = 0;
                _degrees = 0;
            }
            else
            {
                _seconds = (double)Math.Round(_coordinate.Value * 3600);
                _degrees = Math.Abs(_seconds / 3600);
                _seconds = Math.Abs(_seconds % 3600);
                _minutes = _seconds / 60;
                _seconds %= 60;
            }
        }

        private void SetCoordinateValue()
        {
            if ((Degrees + Minutes + Seconds).Equals(0))
            {
                _coordinate = null;
                return;
            }

            var directionValue = _direction == Direction.South || _direction == Direction.West ? -1 : 1;
            _coordinate = (decimal)(Degrees + Minutes / 60 + Seconds / 3600) * directionValue;
            ValidateCoordinate();
        }

        private void ValidateCoordinate()
        {
            var isLongitude = _direction == Direction.West || _direction == Direction.East;
            var isLatitude = _direction == Direction.North || _direction == Direction.South;

            if (_coordinate > 180 && isLongitude)
            {
                _coordinate = 180;
            }

            if (_coordinate < -180 && isLongitude)
            {
                _coordinate = -180;
            }

            if (_coordinate > 90 && isLatitude)
            {
                _coordinate = 90;
            }

            if (_coordinate < -90 && isLatitude)
            {
                _coordinate = -90;
            }
        }
    }
}

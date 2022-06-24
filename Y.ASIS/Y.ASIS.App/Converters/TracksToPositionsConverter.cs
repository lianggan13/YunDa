using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Y.ASIS.App.Models;
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.App.Converters
{
    class TracksToPositionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<Track> tracks = (IEnumerable<Track>)value;
            List<Position> positions = new List<Position>();
            if (tracks != null)
            {
                tracks.ForEach(i =>
                {
                    i.Positions.ForEach(j =>
                    {
                        positions.Add(j);
                    });
                });
            }
            return positions;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Y.ASIS.App.Models;
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.App.Converters
{
    class PlatformsToWorkersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Worker> workers = new List<Worker>();
            if (value is IEnumerable<Platform> platforms)
            {
                platforms.ForEach(i =>
                {
                    i.Workers.ForEach(j =>
                    {
                        workers.Add(j);
                    });
                });
            }
            return workers;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

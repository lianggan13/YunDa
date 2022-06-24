using System;
using System.Windows;
using System.Windows.Controls;
using Y.ASIS.App.Models;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.Selector
{
    class PositionDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TPTPositionDataTemplate { get; set; }

        public DataTemplate TPTTPositionDataTemplate { get; set; }

        public DataTemplate TPPTTPositionDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Track track)
            {
                switch (track.Type)
                {
                    case TrackType.TPT:
                        return TPTPositionDataTemplate;
                    case TrackType.TPTT:
                        return TPTTPositionDataTemplate; // for test
                    case TrackType.TPPTT:
                        return TPPTTPositionDataTemplate;
                    default:
                        throw new Exception("unknown track type: " + track.Type);
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}

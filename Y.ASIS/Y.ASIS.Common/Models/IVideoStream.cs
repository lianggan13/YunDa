using Y.ASIS.Models.Enums;

namespace Y.ASIS.Common.Models
{
    public interface IVideoStream
    {
        int ID { get; set; }

        string Name { get; set; }

        VideoType Type { get; set; }

        string Url { get; set; }

        int Channel { get; set; }

        string Ip { get; set; }

        int Port { get; set; }

        string Password { get; set; }

        string UserName { get; set; }

        string Model { get; set; }

        string Extension { get; set; }
    }
}
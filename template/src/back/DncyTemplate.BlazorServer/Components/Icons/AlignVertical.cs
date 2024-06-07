using Microsoft.FluentUI.AspNetCore.Components;

namespace DncyTemplate.BlazorServer.Components.Icons
{
    public static class CustomIcons
    {
        public static Icon AlignVertical => new AlignVertical();
    }


    public class AlignVertical : Icon
    {
        private const string svg = "<svg t=\"1715347759336\" class=\"icon\" viewBox=\"0 0 1024 1024\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" p-id=\"7516\" width=\"32\" height=\"32\"><path d=\"M219.428571 765.830095v73.142857H121.904762v-73.142857h97.523809z m682.666667 0v73.142857H268.190476v-73.142857h633.904762zM219.428571 490.496v73.142857H121.904762v-73.142857h97.523809z m682.666667 0v73.142857H268.190476v-73.142857h633.904762zM219.428571 215.137524v73.142857H121.904762v-73.142857h97.523809z m682.666667 0v73.142857H268.190476v-73.142857h633.904762z\" p-id=\"7517\" fill=\"#ffffff\"></path></svg>";

        public AlignVertical() : base("AlignVertical", IconVariant.Regular, IconSize.Size32, svg)
        {
        }
    }

}

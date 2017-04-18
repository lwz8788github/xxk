using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace xxkUI.GMap
{
    public class BaiduMapProvider : BaiduMapProviderBase
    {
        public override Guid Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);

            return GetTileImageUsingHttp(url);
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            zoom = zoom - 1;
            var offsetX = Math.Pow(2, zoom);
            var offsetY = offsetX - 1;

            var numX = pos.X - offsetX;
            var numY = -pos.Y + offsetY;

            zoom = zoom + 1;
            var num = (pos.X + pos.Y) % 8 + 1;
            var x = numX.ToString().Replace("-", "M");
            var y = numY.ToString().Replace("-", "M");

            //http://q3.baidu.com/it/u=x=721;y=209;z=12;v=014;type=web&fm=44
            string url = string.Format(UrlFormat, num, x, y, zoom, "014", "web", "44");
            Console.WriteLine("url:" + url);
            return url;
        }

        static readonly string UrlFormat = "http://q{0}.baidu.com/it/u=x={1};y={2};z={3};v={4};type={5}&fm={6}";


    }
}

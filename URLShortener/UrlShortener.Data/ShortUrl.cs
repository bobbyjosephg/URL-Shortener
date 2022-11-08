using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Data
{
    public sealed record ShortUrl(string? Destination, string? Path);
    //internal class ShortUrl
    //{
    //}
}

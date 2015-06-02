using System;
using System.Globalization;
using System.IO;
using System.Xml;

/*
 * Niestety format Rssow z TVN24 okazal sie nie byc do końca zgodny ze standardami
 * przyjetymi domyslnie w klasie XmlTextReader trzeba go było trochę przerobić. 
 * Głównie chodziło o format datyi wyrzucenie tagu html z opisu.
 */
namespace NewsReader
{
    public class MyXmlReader : XmlTextReader
    {
        private bool _readingDate;
        const string CustomUtcDateTimeFormat = "ddd, dd MMM yy HH:mm:ss K"; //Mon, 01 Jun 15 20:12:10 +0200

        public MyXmlReader(Stream s) : base(s) { }

        public MyXmlReader(string inputUri) : base(inputUri) { }

        // Sprawdzamy czy to co czytamy jest datą i ustawiamy flagę
        public override void ReadStartElement()
        {
            if (string.Equals(NamespaceURI, string.Empty, StringComparison.InvariantCultureIgnoreCase) &&
                (string.Equals(LocalName, "lastBuildDate", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(LocalName, "pubDate", StringComparison.InvariantCultureIgnoreCase)))
            {
                _readingDate = true;
            }
            base.ReadStartElement();
        }

        public override void ReadEndElement()
        {
            if (_readingDate)
            {
                _readingDate = false;
            }
            base.ReadEndElement();
        }

        public override string ReadString()
        {
            if (_readingDate) // Jezeli data
            {
                string dateString = base.ReadString();
                DateTime dt;
                if (!DateTime.TryParse(dateString, out dt))
                    dt = DateTime.ParseExact(dateString, CustomUtcDateTimeFormat, CultureInfo.InvariantCulture);
                return dt.ToUniversalTime().ToString("R", CultureInfo.InvariantCulture);
            } else { // W przeciwnym wypadku sprawdzamy na obecność znaku ">" który wystepuje bo w kontencie jest niekiedy htmlowy <img/>
                var textString = base.ReadString();
                var indexer = textString.IndexOf(">");
                if (indexer > 0)
                {
                    var length = textString.Length;
                    return textString.Substring(indexer+1, length - indexer-1).Trim();
                }
                else {
                    return textString.Trim().Replace(Environment.NewLine, ""); // Content czasami czyta sie z randomowo dodanymi newlineami?!
                }
            }
        }
    }
}

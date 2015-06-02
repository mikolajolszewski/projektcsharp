using NewsReader;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;


namespace NewsReaderTests
{
    [TestClass]
    public class MyXmlReaderTests
    {
        [TestMethod]
        public void MyXmlReaderInheritsFromXmlTextReader()
        {
            var initializedMyXmlReader = new MyXmlReader("http://www.tvn24.pl/najwazniejsze.xml");
            Assert.IsInstanceOfType(initializedMyXmlReader, typeof(XmlTextReader), "MyXmlReader should iherit from XmlTextReader");
        }
    }
}

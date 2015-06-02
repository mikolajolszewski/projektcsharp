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
    public class MainWindowTests
    {
        [TestMethod]
        public void MainWindowInheritsFromWindow()
        {
            var initializedWindow = new MainWindow();
            Assert.IsInstanceOfType(initializedWindow, typeof(Window), "Main window should iherit from Window");
        }
        [TestMethod]
        public void InitializedWindowShouldHave6Tabs()
        {
            var initializedWindow = new MainWindow();
            Assert.AreEqual(initializedWindow.TabItems.Count, 6, "Initialized collection of tab items should contain 6 tabs");
        }
    }
}

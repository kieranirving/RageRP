using RAGE;
using RAGE.Ui;
using System;
using System.Collections.Generic;
using System.Linq;

///<summary>
///Original Browser.cs From - https://github.com/xabier1989/WiredPlayers-RP
///Edited for RageRP by Kieran
///</summary>
namespace RageRP.Client.Helpers
{
    class BrowserHelper : Events.Script
    {
        private static List<windowEventObject> windowEvents = null;
        public static List<HtmlWindow> customBrowser = null;
        public class windowEventObject
        {
            public int index { get; set; }
            public string function { get; set; }
            public object[] parameters { get; set; }
        }
        public class windowObject
        {
            public string url { get; set; }
            public string function { get; set; }
            public object[] parameters { get; set; }
        }

        public BrowserHelper()
        {
            //Create our list of browser
            customBrowser = new List<HtmlWindow>();
            windowEvents = new List<windowEventObject>();
            Events.OnBrowserCreated += OnBrowserCreatedEvent;
        }

        /// <summary>
        /// Creates a new HtmlWindow and returns its index
        /// </summary>
        /// <param name="_window"></param>
        /// <returns></returns>
        public static int CreateBrowser(windowObject _window)
        {
            //Create the html window as a new element so we can get the index later
            var window = new HtmlWindow(_window.url);
            //Create the browser
            customBrowser.Add(window);
            //Get the last index of the customBrowser list
            int index = customBrowser.IndexOf(window);

            //Save the Parameters for later along with the index of the HtmlWindow
            windowEvents.Add(new windowEventObject()
            {
                index = index,
                function = _window.function,
                parameters = _window.parameters
            });
            return index;
        }

        /// <summary>
        /// Executes a function with parameters on a specific html window
        /// </summary>
        /// <param name="_window"></param>
        public static void ExecuteFunction(windowEventObject _window)
        {
            //Check for the parameters
            string input = string.Empty;

            //Loop around all the paramters
            foreach (object arg in _window.parameters)
            {
                //Append all the arguments
                input += input.Length > 0 ? (", '" + arg.ToString() + "'") : ("'" + arg.ToString() + "'");
            }

            //Call the function with the parameters
            customBrowser[_window.index].ExecuteJs(_window.function + "(" + input + ");");
        }

        /// <summary>
        /// Destroys a specific html window and removes it from the list
        /// </summary>
        /// <param name="browserId"></param>
        public static void DestroyBrowser(int browserId)
        {
            //Destroy the browser
            customBrowser[browserId].Destroy();
            customBrowser[browserId] = null;

            //Disable the cursor
            Cursor.Visible = false;
        }

        /// <summary>
        /// The OnBrowserCreated event, Fires when a browser is created?
        /// </summary>
        /// <param name="window"></param>
        public static void OnBrowserCreatedEvent(HtmlWindow window)
        {
            //Find the current browser
            var browser = customBrowser.Where(x => x != null && x.Id == window.Id).FirstOrDefault();
            //Get the index of the current browser
            var browserIndex = customBrowser.IndexOf(browser);
            if (browser != null)
            {
                //Get the relevant parameters by index
                var windowEvent = windowEvents.Where(x => x.index == browserIndex).FirstOrDefault();

                //Stop the cursor from showing up when starting browsers in the back ground
                if(windowEvent.function != BrowserFunctions.PHONE_INIT && 
                   windowEvent.function != BrowserFunctions.VOICE_INIT &&
                   windowEvent.function != BrowserFunctions.MONEY_INIT)
                {
                    //Enable the cursor
                    Cursor.Visible = true;
                }

                if (windowEvent.parameters.Length > 0)
                {
                    var args = windowEvent.parameters;
                    //Call the function passed as parameter
                    ExecuteFunction(new windowEventObject() {
                        index = windowEvent.index,
                        function = windowEvent.function,
                        parameters = windowEvent.parameters
                    });
                }
            }
        }
    }
}
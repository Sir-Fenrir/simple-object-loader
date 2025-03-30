using ExtremelySimpleLogger;
using MLEM.Data;
using MLEM.Data.Content;
using System;
using System.Collections.Generic;
using TinyLife.Mods;

namespace SimpleObjectLoader.Utils
{

    /// <summary>
    /// Class to store global state.
    /// To make it slightly less of an anti pattern, 
    /// it has to be initialized with the global variables using the static Init method.
    /// </summary>
    internal class SOL
    {

        private SOL() { }

        public static void Init(Logger logger, RawContentManager content, RuntimeTexturePacker texturePacker)
        {
            if (INITIALIZED) { return; }
            Logger = logger;
            Content = content;
            TexturePacker = texturePacker;
            INITIALIZED = true;
        }

        private static bool INITIALIZED = false;

        /// <summary>
        /// For logging in the mod.
        /// </summary>
        public static Logger Logger { get; private set; }

        /// <summary>
        /// Received from the TinyLife API, used for loading assets.
        /// </summary>
        public static RawContentManager Content { get; private set; }

        /// <summary>
        /// Received from the TinyLife API, used for loading textures.
        /// </summary>
        public static RuntimeTexturePacker TexturePacker { get; private set; }

        /// <summary>
        /// Any errors that happen during the loading of found mods are reported and listed here.
        /// </summary>
        public static List<string> Errors { get; private set; } = [];

        /// <summary>
        /// Report a loading failure.
        /// </summary>
        /// <param name="message">Any specific message to give</param>
        /// <param name="ex">The exception that caused the failure.</param>
        public static void Report(string message, Exception ex)
        {
            Errors.Add($"{message}, with error message: {UnWrap(ex).Message}");
            Logger.Error(ex);
        }

        /// <summary>
        /// Helper method to get an inner exception.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>The inner exception, if any. Otherwise return the given Exception</returns>
        private static Exception UnWrap(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return ex.InnerException;
            }
            return ex;
        }

        /// <summary>
        /// Helper method to wrap a try/catch with a default error handling flow.
        /// </summary>
        /// <param name="toTry">The lambda to execute.</param>
        /// <param name="errormessage">The error message to use in case of failure.</param>
        public static void Try(Action toTry, string errormessage)
        {
            try
            {
                toTry();
            }
            catch (Exception ex)
            {
                SOL.Report(errormessage, ex);
            }

        }
    }
}

#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

#endregion

namespace WinPos
{
    /// <summary>
    ///     Main program class
    /// </summary>
    public static class Program
    {
        #region Constants

        /// <summary>
        ///     Program name
        /// </summary>
        public static readonly String ProgramName = Assembly.GetEntryAssembly()
                                                            .GetName()
                                                            .Name;

        /// <summary>
        ///     Program version
        /// </summary>
        public static readonly String ProgramVersion = $"{ProgramName} Version {Assembly.GetEntryAssembly() .GetName() .Version}";

        #endregion

        /// <summary>
        ///     Main program entry
        /// </summary>
        /// <param name="args">String <see cref="Array" /> with arguments</param>
        public static void Main( String[] args )
        {
            // Show program details
            var versionInfo = FileVersionInfo.GetVersionInfo( Assembly.GetEntryAssembly()
                                                                      .Location );
            Console.Title = ProgramVersion;
            Console.WriteLine( "" );
            Console.WriteLine( ProgramVersion );
            Console.WriteLine( versionInfo.LegalCopyright );
            Console.WriteLine( "" );

            // Get JSON Directory and file
            var jsonDirectory = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), ProgramName );
            var jsonFile = Path.Combine( jsonDirectory, $"{ProgramName}.json" );

            // Get mode for operation
            var mode = "help";
            if ( args.Length == 1 )
                mode = args[0].ToLower();

            switch ( mode )
            {
                case "save":
                    Save( jsonDirectory, jsonFile );
                    break;

                case "restore":
                    Restore( jsonFile );
                    break;

                default:
                    Usage( jsonFile );
                    break;
            }

            Console.WriteLine( "" );
        }

        /// <summary>
        ///     Restore window position from JSON file
        /// </summary>
        /// <param name="jsonFile">Full path to JSON file</param>
        private static void Restore( String jsonFile )
        {
            // Check if JSON file exists
            if ( !File.Exists( jsonFile ) )
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write( "FAILED " );
                Console.ResetColor();
                Console.WriteLine( "File does not exists:" );
                Console.WriteLine( jsonFile );
                return;
            }

            // Read window details from JSON file
            var jsonSerializer = new JsonSerializer();
            var jsonData = jsonSerializer.Deserialize<List<WindowInfo>>( new JsonTextReader( new StreamReader( File.OpenRead( jsonFile ) ) ) );
            Console.WriteLine( $"Read {jsonFile}" );
            Console.WriteLine( "" );

            // Set each window according to details
            foreach ( var windowInfo in jsonData )
            {
                if ( SetWindowPlacementHelper.SetPlacement( windowInfo ) )
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write( "SUCCESS" );
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write( "FAILED " );
                }
                Console.ResetColor();
                Console.WriteLine( $" {windowInfo.Handle} {windowInfo.Title}" );
            }

            // Console output
            Console.WriteLine( "Restore complete" );
        }

        /// <summary>
        ///     Save all window details to JSON file
        /// </summary>
        /// <param name="jsonDirectory">Directory of JSON file</param>
        /// <param name="jsonFile">Full path to JSON file</param>
        private static void Save( String jsonDirectory, String jsonFile )
        {
            var jsonSerializer = new JsonSerializer();

            // Get all open window details
            var result = OpenWindowGetter.GetOpenWindows()
                                         .Select( window => new WindowInfo
                                         {
                                             Handle = window.Key,
                                             Title = window.Value,
                                             WindowPlacement = GetWindowPlacementHelper.GetPlacement( window.Key )
                                         } )
                                         .ToList();

            // Make sure the directory exists
            Directory.CreateDirectory( jsonDirectory );

            // Write JSON file
            using ( var jsonTextWriter = new JsonTextWriter( new StreamWriter( File.Open( jsonFile, FileMode.Create ) ) ) )
            {
                jsonTextWriter.Formatting = Formatting.Indented;
                jsonSerializer.Serialize( jsonTextWriter, result );
            }

            // Console output
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write( "SUCCESS" );
            Console.ResetColor();
            Console.WriteLine( $" Written {jsonFile}" );
        }

        /// <summary>
        ///     Display usage help to console output
        /// </summary>
        /// <param name="jsonFile">Full path to JSON file</param>
        private static void Usage( String jsonFile )
        {
            Console.WriteLine( "Usage: WinPos [save] [restore]" );
            Console.WriteLine( "" );
            Console.WriteLine( "save:    Save current window positions" );
            Console.WriteLine( "restore: Restore previously saved window positions" );
            Console.WriteLine( "" );
            Console.WriteLine( "The file for save/restore is:" );
            Console.WriteLine( jsonFile );
        }
    }
}
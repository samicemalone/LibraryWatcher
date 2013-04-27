/*
 * Copyright (c) 2013, Sam Malone. All rights reserved.
 * 
 * Redistribution and use of this software in source and binary forms, with or
 * without modification, are permitted provided that the following conditions
 * are met:
 * 
 *  - Redistributions of source code must retain the above copyright notice,
 *    this list of conditions and the following disclaimer.
 *  - Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 *  - Neither the name of Sam Malone nor the names of its contributors may be
 *    used to endorse or promote products derived from this software without
 *    specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryWatcher {
    class Args {

        public Library Library { get; set; }

        public string Destination { get; set; }

        private Args() {

        }

        private static Args newInstance() {
            return new Args();
        }

        /// <summary>
        /// Attempts to parse the input data from the arguments given
        /// </summary>
        /// <param name="args">Array of arguments</param>
        /// <returns>Parsed Arguments</returns>
        public static Args parse(string[] args) {
            Args arguments = newInstance();
            bool isArg = false;
            for(int i = 0; i < args.Length; i++) {
                if (isArg) {
                    isArg = false;
                    continue;
                }
                if (args[i].Equals("-h") || args[i].Equals("--help")) {
                    return null;
                }
                if(args[i].Equals("--dest")) {
                    arguments.Destination = args[i+1];
                    isArg = true;
                    continue;
                }
                arguments.Library = new Library(args[i], LibraryManager.getLibraryFolders(args[i]));
            }
            return arguments;
        }

        /// <summary>
        /// Attempts to validate the given arguments object. The app config settings will be 
        /// used as default values (and validated) if the program arguments haven't been set 
        /// </summary>
        /// <param name="args">Parsed Arguments</param>
        /// <returns>Same arguments object</returns>
        public static Args validate(Args args) {
            if (args.Library == null) {
                if (Config.isValidSettingValue("LIBRARY_NAME")) {
                    string name = Properties.Settings.Default.LIBRARY_NAME;
                    args.Library = new Library(name, LibraryManager.getLibraryFolders(name));
                } else {
                    throw new Exception("The library name was not specified as an argument or in the config file");
                }
            }
            if (!args.Library.isValid()) {
                throw new Exception("Unable to find library, or the library is empty");
            }
            if (args.Destination == null) {
                if (Config.isValidSettingValue("DEST_FILE")) {
                    args.Destination = Properties.Settings.Default.DEST_FILE;
                } else {
                    throw new Exception("Destination file is not set");
                }
            }
            return args;
        }

        /// <summary>
        /// Get the help/usage message
        /// </summary>
        /// <returns>Help/Usage message</returns>
        public static string getHelpMessage() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Usage: LibraryWatcher LIBRARY_NAME --dest DEST [-h]");
            sb.AppendLine();
            sb.AppendLine("   --dest DEST  Destination file to store watched contents");
            sb.AppendLine("   -h, --help   Prints this message");
            return sb.ToString();
        }
    }
}

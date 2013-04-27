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
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace LibraryWatcher {
    static class Program {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args) {
            if (Environment.UserInteractive) {
                CLI(args);
            } else {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { 
				    new LibraryWatcherService() 
			    };
                ServiceBase.Run(ServicesToRun);
            }
        }

        /// <summary>
        /// The entry point for the CLI application
        /// </summary>
        /// <param name="args"></param>
        static void CLI(string[] args) {
            Args arguments = Args.parse(args);
            if (arguments == null) {
                Console.WriteLine(Args.getHelpMessage());
                Environment.Exit(0);
            }
            if (arguments.Library == null) {
                if (isSettingSet("LIBRARY_NAME")) {
                    string name = Properties.Settings.Default.LIBRARY_NAME;
                    arguments.Library = new Library(name, LibraryManager.getLibraryFolders(name));
                } else {
                    Console.WriteLine(Args.getHelpMessage());
                    Environment.Exit(0);
                }
            }
            if (!arguments.Library.isValid()) {
                Console.WriteLine("Unable to find library, or the library is empty");
                Environment.Exit(1);
            }
            if (arguments.Destination == null) {
                if (isSettingSet("DEST_FILE")) {
                    arguments.Destination = Properties.Settings.Default.DEST_FILE;
                } else {
                    Console.WriteLine("Destination file is not set");
                    Environment.Exit(1);
                }
            }
            LibraryFile db = new LibraryFile(arguments.Library, arguments.Destination);
            db.init();
            LibraryWatcher watcher = new LibraryWatcher(arguments.Library, db);
            watcher.watch();
            Console.WriteLine("Written " + arguments.Library.Name + " library contents to " + arguments.Destination);
            watcher.close();
            Environment.Exit(0);
        }

        /// <summary>
        /// Checks if the setting with the given key exists in the configuration file
        /// </summary>
        /// <param name="key">Setting Key</param>
        /// <returns>true if key is set, false otherwise</returns>
        static bool isSettingSet(string key) {
            foreach (SettingsProperty curProperty in Properties.Settings.Default.Properties) {
                if(curProperty.Name.Equals(key)) {
                    return true;
                }
            }
            return false;
        }
    }
}

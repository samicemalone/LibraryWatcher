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
using Microsoft.WindowsAPICodePack.Shell;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace LibraryWatcher {
    public partial class LibraryWatcherService : ServiceBase {

        private LibraryWatcher watcher;

        public LibraryWatcherService() {
            InitializeComponent();
            if (!Config.isValidSettingValue("LIBRARY_NAME")) {
                string message = "The library name is not set in the configuration file";
                eventLog.WriteEntry(message, EventLogEntryType.Error);
                throw new Exception(message);
            }
            if (!Config.isValidSettingValue("DEST_FILE")) {
                string message = "The destination file is not set in the configuration file";
                eventLog.WriteEntry(message, EventLogEntryType.Error);
                throw new Exception(message);
            }
            string name = Properties.Settings.Default.LIBRARY_NAME;
            string destinationFile = Properties.Settings.Default.DEST_FILE;
            Library library = new Library(name, LibraryManager.getLibraryFolders(name));
            if (!library.isValid()) {
                string message = "Could not load the library: " + library.Name;
                eventLog.WriteEntry(message, EventLogEntryType.Error);
                throw new Exception(message);
            }
            LibraryFile db = new LibraryFile(library, destinationFile);
            db.init();
            watcher = new LibraryWatcher(library, db);
        }

        protected override void OnStart(string[] args) {
            watcher.watch();
        }

        protected override void OnShutdown() {
            watcher.close();
        }

        protected override void OnStop() {
            watcher.close();
        }

        protected override void OnPause() {
            watcher.disable();
        }

        protected override void OnContinue() {
            watcher.enable();
        }


    }
}

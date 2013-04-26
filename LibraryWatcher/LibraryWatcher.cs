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
using System.IO;
using System.Text;
using System.Threading;
using System.Security.Permissions;

namespace LibraryWatcher {
    class LibraryWatcher {
        private Library library;
        private List<FileSystemWatcher> watchers;
        private static LibraryFile libraryFile;
        private static bool isUpdating = false;

        public LibraryWatcher(Library library, LibraryFile file) {
            this.library = library;
            libraryFile = file;
            watchers = new List<FileSystemWatcher>();
        }

        /// <summary>
        /// Watches each folder in the Library
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void watch() {
            FileSystemWatcher fsw;
            foreach (string folder in library.Folders) {
                fsw = new FileSystemWatcher(folder);
                fsw.IncludeSubdirectories = false;
                fsw.NotifyFilter = NotifyFilters.DirectoryName;
                fsw.Changed += new FileSystemEventHandler(OnChanged);
                fsw.Created += new FileSystemEventHandler(OnChanged);
                fsw.Deleted += new FileSystemEventHandler(OnChanged);
                fsw.Renamed += new RenamedEventHandler(OnRenamed);
                fsw.EnableRaisingEvents = true;
                watchers.Add(fsw);
            }
        }

        /// <summary>
        /// Disables the watcher and releases any resources used.
        /// </summary>
        public void close() {
            foreach (FileSystemWatcher watcher in watchers) {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            reset();
        }

        /// <summary>
        /// Disables the watcher
        /// </summary>
        public void disable() {
            foreach (FileSystemWatcher watcher in watchers) {
                watcher.EnableRaisingEvents = false;
            }
        }

        /// <summary>
        /// Enables the watcher
        /// </summary>
        public void enable() {
            foreach (FileSystemWatcher watcher in watchers) {
                watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Resets the list of watchers
        /// </summary>
        public void reset() {
            watchers = new List<FileSystemWatcher>();
        }

        private static void OnChanged(object source, FileSystemEventArgs e) {
            update();
        }

        private static void OnRenamed(object source, RenamedEventArgs e) {
            update();
        }
        
        /// <summary>
        /// Updates the library file. If an update is already in progress, this method
        /// will block until it can complete
        /// </summary>
        private static void update() {
            try {
                while (isUpdating) {
                    Thread.Sleep(5000);
                }
                isUpdating = true;
                libraryFile.update();
                isUpdating = false;
            } catch (Exception) {
                isUpdating = false;
            }
        }
        
    }
}

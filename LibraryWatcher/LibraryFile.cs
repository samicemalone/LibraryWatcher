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

namespace LibraryWatcher {
    class LibraryFile {
        private static string FILE;
        private LibraryContents contents;
        private Library library;

        public LibraryFile(Library lib, string file) {
            library = lib;
            FILE = file;
            contents = new LibraryContents();
        }

        /// <summary>
        /// Initialises the library contents
        /// </summary>
        public void init() {
            if (File.Exists(FILE)) {
                read();
                update();
            } else {
                create();
            }
        }

        /// <summary>
        /// Creates the library file if the library contents isn't empty
        /// </summary>
        public void create() {
            contents = LibraryContents.create(library);
            if (!contents.isEmpty()) {
                write();
            }
        }

        /// <summary>
        /// Writes the contents of the library to the Library File
        /// </summary>
        public void write() {
            if (File.Exists(FILE)) {
                File.Delete(FILE);
            }
            using (FileStream stream = new FileStream(FILE, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    foreach (string show in contents.getLibraryContents()) {
                        writer.WriteLine(show);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the library file if the library contents has changed
        /// </summary>
        public void update() {
            LibraryContents c = LibraryContents.create(library);
            if (!contents.isEqual(c)) {
                contents = c;
                write();
            }
        }

        /// <summary>
        /// Reads the library file into a LibraryContents object
        /// </summary>
        public void read() {
            if (!File.Exists(FILE)) {
                return;
            }
            string line;
            using(StreamReader r = new StreamReader(FILE)) {
                while (r.Peek() >= 0) {
                    line = r.ReadLine();
                    if (line == null || line.Length == 0) {
                        continue;
                    }
                    contents.addFolderName(r.ReadLine());
                }
            }
            contents.sort();
        }
    }
}

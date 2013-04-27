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
    class LibraryContents {
        private List<string> folderList;

        public LibraryContents() {
            folderList = new List<string>();
        }

        /// <summary>
        /// Creates a LibraryContents instance from the given library lib
        /// </summary>
        /// <param name="lib">Library</param>
        /// <returns>LibraryContents instance</returns>
        public static LibraryContents create(Library lib) {
            LibraryContents c = new LibraryContents();
            foreach (string s in lib.Folders) {
                c.addSubFolders(s);
            }
            c.sort();
            return c;
        }

        /// <summary>
        /// Adds a folder name to the library contents
        /// </summary>
        /// <param name="folder">Folder Name</param>
        public void addFolderName(string folder) {
            folderList.Add(folder);
        }

        /// <summary>
        /// Adds the sub folders from the given folder into the library contents
        /// </summary>
        /// <param name="folder">Folder containing sub directories to add</param>
        public void addSubFolders(string folder) {
            DirectoryInfo[] subDirs = new DirectoryInfo(folder).GetDirectories();
            foreach (DirectoryInfo dir in subDirs) {
                folderList.Add(dir.Name);
            }
        }

        /// <summary>
        /// Get Library Contents
        /// </summary>
        /// <returns>List of folders contained in the library contents</returns>
        public List<string> getLibraryContents() {
            return folderList;
        }

        /// <summary>
        /// Sorts the folders into alphabetical order
        /// </summary>
        public void sort() {
            folderList.Sort();
        }

        /// <summary>
        /// Checks if the library contents is empty
        /// </summary>
        /// <returns>true if empty, false otherwise</returns>
        public bool isEmpty() {
            return folderList.Count == 0;
        }

        /// <summary>
        /// Compares the current instance of LibraryContents with the given contents
        /// parameter.
        /// </summary>
        /// <param name="contents">LibraryContents to compare to</param>
        /// <returns>true if the contents are the same, false otherwise</returns>
        public bool isEqual(LibraryContents contents) {
            if (contents == null || contents.getLibraryContents() == null) {
                return false;
            }
            List<string> toCompare = contents.getLibraryContents();
            if (folderList.Count != toCompare.Count) {
                return false;
            }
            for (int i = 0; i < folderList.Count; i++) {
                if(!folderList[i].Equals(toCompare[i])) {
                    return false;
                }
            }
            return true;
        }
    }
}

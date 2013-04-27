NAME
   LibraryWatcher - Watches each folder in a Windows Library and writes the
	                  contents to a file.

SYNOPSIS
   LibraryWatcher.exe LIBRARY_NAME --dest DEST [-h]

DESCRIPTION
   LibraryWatcher is a utility that will monitor each folder in a Windows
	 Library in order to keep track of each folders subdirectories. These
	 subdirectories will be written to the DEST file on start up. When a change
	 event (new folder, delete folder, rename folder) occurs, the changes will be
	 written to DEST.
	 
	 The program can run as a console program or as a Windows Service. The Windows
	 Service will monitor folder changes as described above. When run as a console
	 application, the program will not continually monitor the library. It will 
   write the contents of the library to the destination file and then exit.

   When running as a console program, if LIBRARY_NAME or DEST is not specified,
   the configuration file values will be read instead.

OPTIONS
   The LIBRARY_NAME option is the Windows Library Name e.g. Videos, TV
	 
	 --dest DEST
	    Sets the output destination file path. 
			
   -h, --help
      The help message will be output and the program will exit.
			
FILES
   The following files are used by LibraryWatcher:
      LibraryWatcher.exe.config
         This is the user configuration file used to set the Library Name and
				 destination file.
			
CONFIG
   The configuration file is required for use as a Windows Service. It is
   optional when running the program as a console application.

   Editing the config file (LibraryWatcher.exe.config) requires	replacing the
	 <value /> XML element with your value for the Library Name and Destination
	 file. The example excerpt below shows how to set the Library Name:
	 
      <setting name="LIBRARY_NAME" serializeAs="String">
			   <value>Television</value>
			</setting>

EXAMPLES
   LibraryWatcher.exe Television --dest D:\Backup\ShowList.txt
	 
	 Library "Television" structure       | D:\Backup\ShowList.txt
	 C:\TV\                               | ----------------------
	    Scrubs\                           | 30 Rock
	 D:\Archive\TV\                       | Friends
	    30 Rock\                          | Homeland
			Friends\                          | Scrubs
   E:\More\TV\                          |
	    Homeland\                         |
			video.mp4                         |
			
COPYRIGHT
   Copyright (c) 2013, Sam Malone. All rights reserved.

LICENSING
   The tv source code, binaries and man page are licensed under a BSD License.
   See LICENSE for details.

AUTHOR
   Sam Malone
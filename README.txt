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
# FileWatcherService c# 
Windows Service that moves files in specified folder to desired subfolders (by formats specified for each sub-folder).
The app also has GUI app by which user can change  folder to watch, subfolders names and formats to include to each subfolders. 

# Logic is the following: Hourly it reads all files names. and looks for for each formats (specified for subfolders)
# And moves to destined subfolders. The Service inspects specified folder hourly.


The app has Windows Service, Desktop app, and shared library to read\write ini file for parameters. 

FolderWatcher.ini file structure:

[GlobalSettings]
DownloadsFolders = D:\Chrome Downloads
[Folder1]
Formats = .jpg, .jpeg, .JPG, .JPEG,  GIF, gif, PNG, png, bmp
FolderName = Images
[Folder2]
Formats = mp4, 3gp, avi, mpeg, mpeg4, mkv, 
FolderName = Videos
[Folder3]
Formats = mp3, wap
FolderName = Music
[Folder4]
Formats = exe, msi
FolderName = Applications
[Folder5]
Formats = ppt, xls, xlsx, docx, doc, pdf, msg, txt
FolderName = Office Files



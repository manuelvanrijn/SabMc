SABnzbd Automatic TV Renamer
===========================================================================

Thank you for downloading the SABnzbd Automatic TV Renamer
by Digital Detritus.
http://digitaldetritus.tumblr.com/tvrenamer


What this program does:
===========================================================================
1 ) Parses SABnzbd job name to determine show title, season number,
    and episode number or show title, and air date, depending on the file.
2 ) Uses this information to query TVRage for the ShowID (number) by title
    (closest match)
3 ) Uses the ShowID from TVRage to query TVRage again for episode listings
4 ) Looks for a matching title
5 ) If found, looks for a matching season number and episode number,
    or looks for a matching air date
6 ) If found, creates a new folder/file name for the job based on user
    specified patterns (detailed below)
7 ) Determines the largest file in the job output directory and assumes
    it to be the video file.
8 ) Renames any files named the same way as the video file to the name
    created in step 6. Example: show.name.s06e01.xvid.blah.mkv and
    show.name.s06e01.xvid.blah.nfo become Show Name - 6x01 - Ep Title.mkv
    and Show Name - 6x01 - Ep Title.nfo.
9 ) Deletes all of the temporary download directories if they are empty.
10) Updates last update date/time of folders in the path of the final 
    download directory, unless the user has specifically turned that 
    feature off.

Note: If TVRage is offline, SABnzbd Automatic TV Renamer will attempt to
   guess the show title by (Proper Casing the Title) like that. It will
   report "Unknown Title" as the episode title.



Upgrade Instructions:
===========================================================================
Be sure not to overwrite TvRenamer.exe.config when upgrading. If you do 
this, you will have to set all the settings below again.



What you need to use this program:
===========================================================================
The instructions below are for Windows. If you are using Linux, please 
use appropriate directories, such as /home/username/downloads/complete/TV 
etc.

1) SABnzbd ( http://sabnzbd.org/ )
2) A USENET provider
3) This script placed in the scripts folder for SABnzbd
   a) Go to the directory configuration
      (e.g. http://127.0.0.1:8080/sabnzbd/config/directories/ )
   b) Place TVRenamer.exe and TVRenamer.exe.config in the directory named
      "Post-Processing Scripts Folder:"
4) Custom category created in SABnzbd
   a) Go to the categories configuration page
      (e.g. http://127.0.0.1:8080/sabnzbd/config/categories/ )
   b) Create a new category called tv-autorename
      1) Name: tv-autorename
      2) Script: TvRenamer.exe (be sure it is +x if you are using Linux)
      3) Folder/Path: Any download path you want. I suggest you make
         this a path outside your normal TV download path. Example:
         If your normal TV download path is C:\Download\TV, I suggest
         this path be C:\Download\TVTemp.
5) A properly configured TVRenamer.exe.config file
   a) In the scripts folder, open TVRenamer.exe.config in notepad.
   b) Each setting has a <value></value> set of nodes and is named
      appropriately
   c) Definition of each setting:
      1) NumberedRenamePattern - pattern defined below for episode naming
         when the episode is a numbered episode. e.g. S14E05, 14x05
      2) DatedRenamePattern - pattern defined below for episode naming when
         the episode is a dated episode. e.g. 2010-01-01
      3) TempDownloadRoot - Temporary directory defined in custom category
         above (section 4.b.3.).
      4) FinalDownloadRoot - The root where you want the TV show to end
         up. Example: C:\Download\TV (your normal TV download path described
         in section 4.b.3., but not the TempDownloadRoot. These should be
         different).
      5) ReplaceInvalidCharactersWith - String to replace any invalid
         characters with, like colons, slashes, etc. Most people will
         leave this empty (remove invalid characters) or use a period (.),
         underscore (_) or space ( ).
      6) TVRageTries - How may times to attempt a TVRage query, as TVRage
         is sometimes overloaded and unreliable. I recommend 5.
      7) TVRageTimeoutSeconds - How long to wait for a TVRage query to
         complete. Remember, there are 2 queries, so the real timeout
         could be almost double this long, in case you seem to be waiting
         longer than this timeout. I recommend 40 seconds as a default.
      8) UpdateFolderLastWriteDate - Whether or not to update the "last write"
         time on parent folders of the destination folder. Useful for sorting
         by last updated when looking at your TV folders. You should leave
         this set to true unless you have a good reason not to.
6) .Net 3.5 SP1 ( http://www.microsoft.com/net/download.aspx )
   or Mono (Linux/Mac) ( http://www.mono-project.com/ )



Optional Configuration:
===========================================================================
SABnzbd Automatic TV Renamer now includes a powerful search and replace 
engine. This is necessary because some scene show titles do not match 
the top result on TVRage. You may use this functionality to replace what 
TVRenamer is searching for on TVRage.

For example, the scene name for 
what TVRage calls "Food Revolution" is "Jamie.Olivers.Food.Revolution". 
Searching for "Jamie Olivers Food Revolution" on TVRage, however, returns 
a different show as the first result.

For normal users, there is a simple string search and replace,
which also converts the output string to lower case.

For power users, a regular expression search is available. See the 
.Net specific examples at http://www.regular-expressions.info/named.html 
for more information.

In the TVRenamer.exe.config, look for the titleReplacements section 
under the showSettings section. An example should be present already. 
You may add more nodes that conform to this standard.

Note: Periods (.) will be turned into spaces, because the TVRenamer
engine does this internally. Please use spaces in your search patterns
instead of dots. e.g. "food revolution" rather than "food.revolution"

Example 1 (Regex):
XML:    <add search="jamie oliver\'?s " replace="" isRegex="True" />
Input:  Jamie Olivers Food Revolution
        or
        Jamie Oliver's Food Revolution
Output: Food Revolution

Example 2 (Non-regex):
XML:    <add search="jamie olivers " replace="" isRegex="False" />
Input:  Jamie Olivers Food Revolution
Output: food revolution
Input:  Jamie Oliver's Food Revolution
Output: Jamie Oliver's Food Revolution
		(note it was not replaced because of the apostrophe)

Use this to test .Net's regular expression engine:
http://derekslager.com/blog/posts/2007/09/a-better-dotnet-regular-expression-tester.ashx



Suggested usage:
===========================================================================
1)  Visit http://www.nzbs.org/
2)  Add shows to saved searches with criteria like categories
3)  Click RSS at the bottom
4)  Copy the my saved searches RSS feed address.
5)  Go to the SABnzbd RSS configuration page.
    e.g. http://127.0.0.1:8080/sabnzbd/config/rss/
6)  Add the feed collected in step 5
7)  Set the new feed to use the tv-autorename category created in the
    "What you need to use this program:" section above (section 4.b.).
8)  Preview the feed (only new items will show up after you create the feed.
    Past items will not appear).
9)  Check the feed to enable it and make sure the above settings were retained.
    If not, reset the settings again (SABnzbd is particular in the order you
    click things). If there are items in the feed to download, click Force
    Download.
10) RSS download frequency can be set in the General configuration page under
    tuning. e.g. http://127.0.0.1:8080/sabnzbd/config/general/

Note: These steps should work with any RSS provider using scene release titles


What to do if you have problems, comments, or suggestions:
===========================================================================
1) Visit http://digitaldetritus.tumblr.com/tvrenamer to see if there is
   an updated version or known issue.
2) Click the link on the page to report a problem or feature suggestion
   if it is not already known.
   a) If you have a problem, be sure to include the output of the TVRenamer
      program. This can be found by clicking (more) in the history beside
      the script log in SABnzbd on the main page.
      e.g. http://127.0.0.1:8080/sabnzbd/
   b) A link to a problematic nzb file would be helpful as well
3) Regular comments may be left on any blog entry.



SABnzbd Folder Sorting Pattern Syntax:
===========================================================================
This follows most of SABnzbd's conventions from:
http://wiki.sabnzbd.org/folder-sorting

Additionally, I have added the ability to rename based on dates as well.
See below.

Meaning				Pattern	Result
Show Name:			%sn		Show Name
					%s.n	Show.Name
					%s_n	Show_Name
Season Number:		%s		1
					%0s		01
Episode Number:		%e		5
					%0e		05
Year:				%yyyy	2010
Month:				%m		3
					%0m		03
Day:				%d		2
					%0d		02
Episode Name:		%en		Episode Name
					%e.n	Episode.Name
					%e_n	Episode_Name
File Extension:		%ext	avi
Original Filename:	%fn		file
Lower Case:			{TEXT}	text (Not supported yet. Will support if there is a demand)

Examples:

1x01 Season Folder:
%sn\Season %s\%sn - %sx%0e - %en.%ext
Example: Show Name\Season 1\Show Name - 1x05 - Episode Name.avi

S01E01 Season Folder:
%sn\Season %s\%sn - S%0sE%0e - %en.%ext
Example: Show Name\Season 1\Show Name - S01E05 - Episode Name.avi

1x01 Individual Episode Folder:
%sn\%sx%0e - %en\%sn - %sx%0e - %en.%ext
Example: Show Name\1x05 - Episode Name\Show Name - 1x05 - Episode Name.avi

S01E01 Individual Episode Folder:
%sn\S%0sE%0e - %en\%sn - S%0sE%0e - %en.%ext
Example: Show Name\S01E05 - Episode Name\Show Name - S01E05 - Episode Name.avi

2010-01-01 Dated Episode In Show Folder:
%sn\%sn - %yyyy-%0m-%0d - %en.%ext
Example: Show Name\Show Name - 2010-01-01 - Episode Name.avi

2010-01-01 Dated Episode In Show Folder:
%sn\%sn - %yyyy-%m-%d - %en.%ext
Example: Show Name\Show Name - 2010-1-1 - Episode Name.avi
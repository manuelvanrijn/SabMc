#SabNzbd XBMC processing
##What?

Three SabNzbd processing scripts:

- SabMC.TvShow - Processes downloaded tvshows
- SabMC.Movie - Processes downloaded movies
- SabMC.Notifo - Just sends a notification when the download is finished

##Prerequisite/Requirements

###theRenamer *(required)*

The SabMC scripts uses the program theRenamer. You need to download, install and configure this to your needs. For example I like my TvShow file not to be renamed, so I can easly download the subtitles for it.

theRenamer: <http://www.therenamer.com>

###Notifo Notification Service *(optional)*

You can use Notifo to send you a notification when SabMC finished the job.

- Register an account
- Get your API key from <http://notifo.com/user/settings>
- Put this information into the config.xml file

Notifo: <http://notifo.com/>

###XBMC Update Library *(optional)*

If you use XBMC you can auto update your library after the SabMC script finished. You have to enable __"Allow control of XBMC via HTTP"__ in the __"Network Settings"__ menu to be able to use this functionality.

##Get Started with SabMC

- Step 1 - Run build.bat
- Step 2 - Copy the files from the /release/ to the SabNzbd user script folder
- Step 3 - Run on of the executables to create te config.
- Step 4 - Edit the just created config.xml
- Step 5 - Setup SabNZBD to point to the right executable.

##Found a bug?

Submit a bug report above or here: 

<https://github.com/manuelvanrijn/SabMc/issues>
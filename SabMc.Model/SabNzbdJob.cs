namespace SabMc.Model
{
	using System;
	using System.IO;
	using Enums;

	/// <summary>
	/// Model for a SabNZBD Job
	/// </summary>
	public class SabNzbdJob
	{
		private readonly DirectoryInfo directory;
		private FileInfo fileInfo;
		private DirectoryInfo dirInfo;
		private MediaType mediaType = MediaType.Other;
		private SabNzbdStatus status;
		private bool handleMovieFolder;

		/// <summary>
		/// Constructs a SabNZBD object from the passed arguments of SabNZBD
		/// </summary>
		/// <param name="args">SabNZBD arguments</param>
		public SabNzbdJob(string[] args)
		{
			// Set Directory and File
			string path = args[0];
			directory = new DirectoryInfo(path);

			// Check Status
			status = GetStatusFromArgs(args);
			Console.WriteLine(string.Format("== Initial SabNzbd status code: {0} ==", status));
		}

		/// <summary>
		/// Process the Job for a specific media type
		/// </summary>
		/// <param name="type">Mediatype of the job</param>
		public void Process(MediaType type)
		{
			mediaType = type;
			if (status != SabNzbdStatus.Ok)
				return;

			if (directory.Exists)
			{
				switch (type)
				{
					case MediaType.TvShow:
						// find the largest file in the target folder
						HandleGetLargestFile(true);
						status = SabNzbdStatus.Ok;
						break;
					case MediaType.Movie:
						DirectoryInfo[] directoryList = directory.GetDirectories("VIDEO_TS", SearchOption.AllDirectories);
						if (directoryList.Length == 1)
						{
							handleMovieFolder = true;
							dirInfo = directoryList[0].Parent;	// 1 up
						}
						else
						{
							handleMovieFolder = false;
							// handle largest file
							HandleGetLargestFile(false);
							// larger than 300 mb ?
							if (GetFileLength(fileInfo) < (1024 * 1024 * 300))
							{
								// weird movie if it's smaller than 300 mb and hasn't got a VIDEO_TS folder?
								status = SabNzbdStatus.FailedVerification;
								break;
							}
						}
						status = SabNzbdStatus.Ok;
						break;
					default:
						status = SabNzbdStatus.FailedUnpacking;
						break;
				}
			}
			else
			{
				status = SabNzbdStatus.FailedUnpacking;
			}
			Console.WriteLine(string.Format("== After process status code: {0} ==", status));
		}

		/// <summary>
		/// Remove old/unused files and folder of the job
		/// </summary>
		public void CleanUp()
		{
			if (status != SabNzbdStatus.Ok || mediaType == MediaType.TvShow)
				return;
			
			directory.Delete(true);
		}

		/// <summary>
		/// Move's the Movie to it's destination
		/// </summary>
		/// <param name="di">The Movie folder to move</param>
		/// <returns>if all went OK</returns>
		public bool MoveMovie(DirectoryInfo di)
		{
			try
			{
				if (mediaType != MediaType.Movie)
					return true;

				// handle a folder?
				if (handleMovieFolder)
				{
					// Move Folders
					DirectoryInfo[] folders = dirInfo.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
					foreach (DirectoryInfo folder in folders)
					{
						folder.MoveTo(Path.Combine(di.FullName, folder.Name));
					}
					// Move files (larger than 300 mb)
					FileInfo[] files = dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly);
					foreach (FileInfo file in files)
					{
						if (GetFileLength(file) >= (1024 * 1024 * 300))
						{
							file.MoveTo(Path.Combine(di.FullName, file.Name));
						}
					}
				}
				else
				{
					// just move the file
					fileInfo.MoveTo(Path.Combine(di.FullName, fileInfo.Name));
				}
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Get the largest file from a folder (recursive)
		/// </summary>
		/// <param name="delete">delete other files than are smaller?</param>
		private void HandleGetLargestFile(bool delete)
		{
			FileInfo[] fileList = directory.GetFiles("*.*", SearchOption.AllDirectories);	// recursive
			foreach (FileInfo file in fileList)
			{
				if (GetFileLength(file) > GetFileLength(fileInfo))
				{
					// delete smaller fileInfo
					if (fileInfo != null && delete)
						fileInfo.Delete();

					fileInfo = file;
				}
				else
				{
					// smaller, so delete
					if (delete)
						file.Delete();
				}
			}
		}

		/// <summary>
		/// Strips the status from the arguments
		/// </summary>
		/// <param name="args">SabNZBD arguments</param>
		/// <returns>The SabNzbdStatus</returns>
		private static SabNzbdStatus GetStatusFromArgs(string[] args)
		{
			int lastIndex = args.Length - 1;
			string lastValue = args[lastIndex];
			switch (lastValue)
			{
				case "1":
					return SabNzbdStatus.FailedVerification;
				case "2":
					return SabNzbdStatus.FailedUnpacking;
				default:
					return SabNzbdStatus.Ok;
			}
		}

		/// <summary>
		/// Safe get file lenght method 
		/// </summary>
		/// <param name="fi">FileInfo file</param>
		/// <returns>the length</returns>
		private static long GetFileLength(FileInfo fi)
		{
			long retval;
			if (fi == null)
				return 0;

			try
			{
				retval = fi.Length;
			}
			catch (FileNotFoundException)
			{
				retval = 0;
			}
			return retval;
		}

		#region Properties

		/// <summary>
		/// The Status
		/// </summary>
		public SabNzbdStatus Status
		{
			get { return status; }
			set { status = value; }
		}

		/// <summary>
		/// Fullname of the directory where the files are located
		/// </summary>
		public string FullFolderName
		{
			get { return directory.FullName; }
		}

		/// <summary>
		/// The foldername of the job
		/// </summary>
		public string FolderName
		{
			get { return directory.Name; }
		}

		/// <summary>
		/// The name of the job (filename or directory name)
		/// </summary>
		public string Name
		{
			get
			{
				if (handleMovieFolder)
					return dirInfo.Name;
				return fileInfo.Name;
			}
		}

		/// <summary>
		/// The mediatype of the job
		/// </summary>
		public MediaType MediaType
		{
			get { return mediaType; }
		}

		#endregion
	}
}
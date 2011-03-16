namespace SabMc.Model
{
	using System;
	using System.IO;
	using Enums;

	public class SabNzbdJob
	{
		private readonly DirectoryInfo directory;
		private FileInfo fileInfo;
		private DirectoryInfo dirInfo;
		private MediaType mediaType = MediaType.Other;
		private SabNzbdStatus status;
		private bool handleMovieFolder;

		public SabNzbdJob(string[] args)
		{
			// Set Directory and File
			string path = args[0];
			directory = new DirectoryInfo(path);

			// Check Status
			status = GetStatusFromArgs(args);
			Console.WriteLine(string.Format("== Initial SabNzbd status code: {0} ==", status));
		}
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
//							if (GetFileLength(fileInfo) < (1024 * 1024 * 300))
//							{
//								// weird movie if it's smaller than 300 mb and hasn't got a VIDEO_TS folder?
//								status = SabNzbdStatus.FailedVerification;
//								break;
//							}
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

		public void CleanUp()
		{
			if (status != SabNzbdStatus.Ok)
				return;
			
			directory.Delete(true);
		}

		public bool MoveMovie(DirectoryInfo di)
		{
			try
			{
				if (mediaType != Enums.MediaType.Movie)
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
					// Move files
					FileInfo[] files = dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly);
					foreach (FileInfo file in files)
					{
						file.MoveTo(Path.Combine(di.FullName, file.Name));
					}
				}
				else
				{
					// just move the file
					fileInfo.MoveTo(Path.Combine(di.FullName, fileInfo.Name));
				}
				return true;
			}
			catch(Exception e)
			{
				return false;
			}
		}

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
		public SabNzbdStatus Status
		{
			get { return status; }
			set { status = value; }
		}
		public string FullFolderName
		{
			get { return directory.FullName; }
		}
		public string FolderName
		{
			get { return directory.Name; }
		}
		public string FileName
		{
			get
			{
				if (handleMovieFolder)
					return dirInfo.Name;
				return fileInfo.Name;
			}
		}
		public MediaType MediaType
		{
			get { return mediaType; }
		}
		#endregion
	}
}
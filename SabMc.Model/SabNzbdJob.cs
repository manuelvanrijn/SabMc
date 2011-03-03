namespace SabMc.Model
{
	using System;
	using System.IO;
	using Enums;

	public class SabNzbdJob
	{
		private readonly DirectoryInfo directory;
		private FileInfo fileInfo;
		private MediaType mediaType = MediaType.Other;
		private SabNzbdStatus status;

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
						FileInfo[] fileList = directory.GetFiles("*.*", SearchOption.AllDirectories);	// recursive
						foreach (FileInfo file in fileList)
						{
							if (GetFileLength(file) > GetFileLength(fileInfo))
								fileInfo = file;
						}
						status = SabNzbdStatus.Ok;
						break;
					case MediaType.Movie:

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
			get { return fileInfo.Name; }
		}
		public MediaType MediaType
		{
			get { return mediaType; }
		}
	}
}
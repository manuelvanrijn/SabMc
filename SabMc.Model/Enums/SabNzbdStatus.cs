namespace SabMc.Model.Enums
{
	/// <summary>
	/// Status of the SabNZBD Job
	/// </summary>
	public enum SabNzbdStatus
	{
		/// <summary>
		/// Ok
		/// </summary>
		Ok = 0,
		/// <summary>
		/// Verification Error occured
		/// </summary>
		FailedVerification = 1,
		/// <summary>
		/// Error while unpacking
		/// </summary>
		FailedUnpacking = 2,
		/// <summary>
		/// Error while running the TV Rename progress
		/// </summary>
		FailedTvRenamer = 3,
		/// <summary>
		/// Error while moving the Movie to it's destination folder
		/// </summary>
		FailedMoveMovie = 4
	}
}
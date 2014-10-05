namespace CoursesAPI.Models
{
	/// <summary>
	/// 
	/// </summary>
	public class CourseInstanceDTO
	{
        /// <summary>
        /// Id of the courseInstance
        /// </summary>
		public int    CourseInstanceID { get; set; }

        /// <summary>
        /// Id of the course
        /// </summary>
		public string CourseID         { get; set; }

        /// <summary>
        /// Name of the course
        /// </summary>
		public string Name             { get; set; }

        /// <summary>
        /// Name of main teacher
        /// </summary>
		public string MainTeacher      { get; set; }
	}
}

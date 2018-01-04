namespace HelloHome.Central.Domain.Entities
{
	public class EnvironmentDataHistory : CommunicationHistory
	{
	    public EnvironmentDataHistory()
	    {
	        Type = "E";
	    }
		public virtual float? Temperature { get; set; }
		public virtual float? Humidity { get; set; }
		public virtual int? Pressure { get; set; }
	}
}


namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
	public class CommentReport : Report
	{
		public string Comment { get; private set; }

		public CommentReport (string comment)
		{
			Comment = comment;
		}

		public override string ToString ()
		{
			return $"[CommentReport: Comment={Comment}]";
		}
	}
}


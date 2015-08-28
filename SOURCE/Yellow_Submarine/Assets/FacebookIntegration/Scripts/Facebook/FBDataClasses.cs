public class FBPageData
{
	public FBPageData(){}

	public string pageName;
	public string pageID;
	public string pageCategory;
	public int pageLikeCount;

	public override string ToString ()
	{
		return string.Format ("Page Name: {0} \nCategory: {1} \nLikes: {2} \nID: {3}", pageName, pageCategory,
			pageLikeCount, pageID);
	}
}

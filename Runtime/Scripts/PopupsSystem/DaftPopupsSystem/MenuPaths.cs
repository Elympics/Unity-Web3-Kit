public static class MenuPaths
{
	public const string CreateAssetPrefix = "DaftPopups/";
	public const string CreateStyleAssetPrefix = "Styles/";
	public const int FirstOrder = 0;

	public static string CreatePopupDataPath(string dataName) => CreateAssetPrefix + dataName;
	public static string CreatePopupStylePath(string styleName) => CreateAssetPrefix + CreateStyleAssetPrefix + styleName;
}

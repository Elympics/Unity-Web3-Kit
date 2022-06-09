namespace DaftPopups
{
	public abstract class PopupWithData<TData> : Popup where TData : BasePopupData
	{
		public abstract void ApplyData(TData data);
	}
}
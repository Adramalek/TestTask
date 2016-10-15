namespace TestTask.Net
{
	public delegate T StoreAction<T>(byte[] data, int bytesRecieve, T update) where T : class;
	public delegate T ActionWithData<T>(T data);
	public delegate bool StopCondition<T>(T data) where T : class;
	public delegate void StartEvent();
	public delegate void StopEvent();
	public delegate void ActionEvent();

	public delegate byte[] GetBytes<T>(T data);
	public delegate T AccessToData<T>();
}
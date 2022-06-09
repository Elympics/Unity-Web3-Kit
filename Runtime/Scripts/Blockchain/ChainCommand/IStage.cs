using System.Collections;

public interface IStage
{
	IEnumerator Start();
	bool Success { get; }
}
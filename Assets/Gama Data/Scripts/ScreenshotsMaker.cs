using UnityEngine;

public class ScreenshotsMaker : MonoBehaviour
{
	[SerializeField] private int superSize = 2;
	private int _shotIndex = 0;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			ScreenCapture.CaptureScreenshot($"Screenshot{_shotIndex}.png", superSize);
			_shotIndex++;

			Debug.Log("Screenshot is done");
		}
	}
}

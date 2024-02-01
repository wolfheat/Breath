using UnityEngine;

public enum GameStates{Running,Paused}

public class GameState : MonoBehaviour
{

	public static GameState Instance { get; private set; }
	public GameStates state;

	private void Start()
	{
		if (Instance == null)
			Instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}
	}

}

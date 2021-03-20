using UnityEngine;
using UnityEngine.SceneManagement;
using VarelaAloisio.UpdateManagement.Runtime;

namespace Managers
{
	[CreateAssetMenu(menuName = "Managers/GameManager", fileName = "GameManager", order = 0)]
	public class GameManagerSO : ScriptableObject
	{
		[Header("Level Indexes")] [SerializeField]
		private int[] levelsToLoadAtStart;

		[SerializeField] private int mainMenu;
		[SerializeField] private int credits;

		public void Initialize()
		{
			Application.targetFrameRate = 60;
			foreach (int level in levelsToLoadAtStart)
			{
				SceneManager.LoadScene(level, LoadSceneMode.Additive);
			}
		}

		public void LoadMenu()
		{
			SceneManager.LoadScene(mainMenu, LoadSceneMode.Additive);
		}

		public void UnloadMenu()
		{
			SceneManager.UnloadSceneAsync(mainMenu);
		}

		public void UnloadLevel(int id)
		{
			if(!SceneManager.GetSceneByBuildIndex(id).isLoaded)
				return;
			UpdateManager.FlushScene(id);
			SceneManager.UnloadSceneAsync(id);
		}
		public void LoadLevel(int id)
		{
			SceneManager.LoadScene(id, LoadSceneMode.Additive);
		}

		public void LoadCredits()
		{
			SceneManager.LoadScene(credits, LoadSceneMode.Additive);
		}

		public void UnloadCredits()
		{
			SceneManager.UnloadSceneAsync(credits);
		}

		public void Exit()
		{
			Application.Quit();
		}

		public void ResetUpdates()
		{
			UpdateManager.ResetUpdateDelegates();
		}
	}
}
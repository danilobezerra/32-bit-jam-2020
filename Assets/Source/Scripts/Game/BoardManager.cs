using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace AncientTech.Game
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private GameObject healthPrefab;

        [SerializeField] private Transform[] maps;
        [SerializeField] private GameObject[] tiles;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f); // O.o

            var currentLevel = GameManager.Instance.CurrentLevel;
            var path = Path.Combine(Application.streamingAssetsPath, $"Levels/{currentLevel:D2}.txt");

            string input = null;

#if UNITY_WEBGL
            var www = UnityWebRequest.Get(path);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.LogError(www.error);
                GameManager.Instance.GameOverByContent();
            } else {
                input = www.downloadHandler.text;
            }
#else
            try {
                var file = new FileInfo(path);
                using (var reader = file.OpenText()) {
                    input = reader.ReadToEnd();
                }
            } catch (FileNotFoundException e) {
                Debug.LogError(e);
                GameManager.Instance.GameOverByContent();
            }

            yield return new WaitForEndOfFrame();
#endif
            if (input != null) {
                Init(input);
            }
        }

        private void Init(string input)
        {
            var rows = input.Split('\n');
            var board = new int[rows.Length, rows.Length];

            for (int i = 0; i < rows.Length; i++) {
                var columns = Regex.Split(rows[i], @"\s+")
                    .Where(str => str != string.Empty)
                    .ToArray();

                for (int j = 0; j < columns.Length; j++) {
                    board[i, j] = int.Parse(columns[j]);
                }
            }

            BuildTilemap(board);
        }

        private void BuildTilemap(int[,] board)
        {
            for (var i = 0; i < board.GetLength(0); i++) {
                for (var j = 0; j < board.GetLength(1); j++) {
                    BuildTile(board, i, j);
                }
            }
        }
        
        private void BuildTile(int[,] board, int i, int j)
        {
            var lastIndexOfRows = board.GetLength(0) - 1;
            var value = board[i, j];
            Debug.Log(value);

            if ((value & 0x01) != 0) { // Floor
                InstantiateGameObject(j, lastIndexOfRows - i, 0);
            }
            
            if ((value & 0x02) != 0) { // Wall
                InstantiateGameObject(j, lastIndexOfRows - i, 1);
            }
            
            if ((value & 0x04) != 0) { // Coin
                InstantiateGameObject(j, lastIndexOfRows - i, 2);
            }
            
            if ((value & 0x08) != 0) { // Health
                InstantiateGameObject(j, lastIndexOfRows - i, 3);
            }
            
            if ((value & 0x10) != 0) { // Goals
                InstantiateGameObject(j, lastIndexOfRows - i, 4);
            }
            
            if ((value & 0x20) != 0) { // Player
                var player = InstantiateGameObject(j, lastIndexOfRows - i, 5);
                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = player.transform;
            }
        }

        private GameObject InstantiateGameObject(float x, float z, int index)
        {
            var position = new Vector3(x, maps[index].position.y, z);
            var instance = Instantiate(tiles[index], position, Quaternion.identity);
            instance.transform.parent = maps[index];

            return instance;
        }
    }
}

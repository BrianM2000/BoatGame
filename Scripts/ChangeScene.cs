using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void LoadScene(int id){
        StartCoroutine(LoadAsync(id));
    }

    IEnumerator LoadAsync(int id){

        AsyncOperation op = SceneManager.LoadSceneAsync(id);

        while(!op.isDone){
            Debug.Log("Loading scene " + " [][] Progress: " + op.progress);
            yield return null;
        }

    }

}

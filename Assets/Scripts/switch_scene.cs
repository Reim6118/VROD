using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switch_scene : MonoBehaviour
{
  public void Sceneswitch()
  {
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
  }
}

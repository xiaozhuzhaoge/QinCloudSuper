using UnityEngine;
using System.Collections;
using System;

public interface UImodeInterface {

    void InitWindow();
    void OpenWindow(Action<GameObject> callback = null);
    void CloseWindow(Action<GameObject> callback = null);
    void PreDestoryWindow();
    void DestoryWindow();
    void DirectlyCloseWindow();
    void DirectlyOpenWindow();
    void OpenPreviousWindow();
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance { get; private set; }

    private Stack<BaseWindow> windowStack = new Stack<BaseWindow>();
    
    [Header("UI Menu")]
    [SerializeField] private MenuController currentOpenMenuContainer;

    public MenuController CurrentOpenMenuContainer
    {
        get { return currentOpenMenuContainer; }
        set { currentOpenMenuContainer = value; }
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SelectFirst()
    {
        if(currentOpenMenuContainer != null) currentOpenMenuContainer.SelectFirstBtnMenu();
    }

    public void UpdateCurrentMenu(MenuController window)
    {
        currentOpenMenuContainer = window;
    }

    public void OpenWindow(BaseWindow window)
    {
        if (windowStack.Count > 0)
        {
            windowStack.Peek().SetFocus(false);
        }

        windowStack.Push(window);
        window.Open();

       // InputManager.Instance.SetActionMap("UI");
    }

    public void CloseTopWindow()
    {
        if (windowStack.Count == 0) return;

        BaseWindow topWindow = windowStack.Pop();
        topWindow.Close();

        if (windowStack.Count > 0)
        {
            BaseWindow previousWindow = windowStack.Peek();
            previousWindow.SetFocus(true);
            
            if (InputManager.Instance.IsGamepad && previousWindow.FirstSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(previousWindow.FirstSelected);
            }
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
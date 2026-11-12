using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class KeyRebinder : MonoBehaviour
{
    public static KeyRebinder Instance;
    public Dictionary<string, KeyCode> currentKeys = new Dictionary<string, KeyCode>();

    public event System.Action<string> OnKeyRebound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Inicializar teclas básicas
            InitializeDefaultKeys();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDefaultKeys()
    {
        // Configuración predeterminada
        AddOrUpdateKey("Jump", KeyCode.Space);
        AddOrUpdateKey("Fire1", KeyCode.LeftShift);
        AddOrUpdateKey("Fire2", KeyCode.Q);
        AddOrUpdateKey("Fire3", KeyCode.E);
        AddOrUpdateKey("Fire4", KeyCode.W);
        // Agrega más según sea necesario
    }

    private void AddOrUpdateKey(string inputName, KeyCode defaultKey)
    {
        if (!currentKeys.ContainsKey(inputName))
        {
            LoadKey(inputName, defaultKey);
        }
    }

    void LoadKey(string inputName, KeyCode defaultKey)
    {
        if (PlayerPrefs.HasKey(inputName))
        {
            string keyString = PlayerPrefs.GetString(inputName);
            if (System.Enum.IsDefined(typeof(KeyCode), keyString))
            {
                currentKeys[inputName] = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyString);
                return;
            }
        }
        currentKeys[inputName] = defaultKey;
    }

    void SaveKey(string inputName)
    {
        PlayerPrefs.SetString(inputName, currentKeys[inputName].ToString());
    }

    public IEnumerator RebindKey(string inputName)
    {
        yield return null;

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                currentKeys[inputName] = key;
                SaveKey(inputName);
                OnKeyRebound?.Invoke(inputName);
                break;
            }
        }
    }

    public KeyCode GetKey(string actionName)
    {
        if (currentKeys.ContainsKey(actionName))
            return currentKeys[actionName];

        Debug.LogError($"Acción '{actionName}' no configurada!");
        return KeyCode.None;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var key in currentKeys.Keys)
        {
            OnKeyRebound?.Invoke(key);
        }
    }

    public void SaveAllKeys()
    {
        foreach (var key in currentKeys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveAllKeys();
    }

    private void OnDisable()
    {
        SaveAllKeys();
    }
}

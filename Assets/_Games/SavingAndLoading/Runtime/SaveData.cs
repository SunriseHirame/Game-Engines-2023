using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public float PlayerHealth;
    public float SomeRadom = 10;
    [SerializeField] private List<string> m_dataKeys;
    [SerializeField] private List<string> m_dataValues;


    public bool TryReadData (string key, out string value)
    {
        for (int i = 0; i < m_dataKeys.Count; i++)
        {
            if (key.Equals(m_dataKeys[i], System.StringComparison.Ordinal))
            {
                value = m_dataValues[i];
                return true;
            }
        }

        value = "";
        return false;
    }

    public bool TryReadData<T>(string key, out T value)
    {
        for (int i = 0; i < m_dataKeys.Count; i++)
        {
            if (key.Equals(m_dataKeys[i], System.StringComparison.Ordinal))
            {
                value = JsonUtility.FromJson<T> (m_dataValues[i]);
                return true;
            }
        }

        value = default (T);
        return false;
    }

    public void WriteData (string key, string data)
    {
        for (int i = 0; i < m_dataKeys.Count; i++)
        {
            if (key.Equals(m_dataKeys[i], System.StringComparison.Ordinal))
            {
                m_dataValues[i] = data;
            }
        }

        m_dataKeys.Add(key);
        m_dataValues.Add(data);
    }
}

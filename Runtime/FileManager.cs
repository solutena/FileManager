using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class FileManager
{
	public static readonly string PersistentDataPath = Application.persistentDataPath + "/Data/";
	public static readonly string ResourcePath = Application.dataPath + "/Resources/Data/";
	public static readonly string StreamingAssetsPath = Application.streamingAssetsPath + "/";

	public static void ExportJson<T>(string directory, T data)
	{
		if (data == null)
			throw new ArgumentNullException(nameof(data));
		
		string path = Path.Combine(directory, $"{typeof(T).Name}.json");
		try
		{
			Directory.CreateDirectory(directory);
			string json = JsonUtility.ToJson(data, true);
			File.WriteAllText(path, json, Encoding.UTF8);
			Debug.Log($"Exported JSON for {typeof(T).Name} to: {path}");
		}
		catch (Exception ex)
		{
			Debug.LogError($"Failed to export JSON for {typeof(T).Name}: {ex.Message}");
		}
	}

    public static T ImportJson<T>(string directory) where T : class
    {
		string path = Path.Combine(directory, $"{typeof(T).Name}.json");
		try
		{
			if (File.Exists(path))
            {
                string json = File.ReadAllText(path, Encoding.UTF8);
                return JsonUtility.FromJson<T>(json);
			}
			Debug.LogWarning($"JSON file not found for {typeof(T).Name}: {path}");
			return null;
        }
        catch (Exception ex)
		{
			Debug.LogError($"Failed to import JSON for {typeof(T).Name}: {ex.Message}");
			return null;
        }
    }

	public static void ExportXmlSchema<T>(string directory) where T : new()
	{
		string path = Path.Combine(directory, $"{typeof(T).Name}_Schema.xml");
		try
		{
			Directory.CreateDirectory(directory);
			var serializer = new XmlSerializer(typeof(T[]));
			using var writer = new XmlTextWriter(path, Encoding.UTF8)
			{
				Formatting = Formatting.Indented
			};
			T[] data = { new(), new() };
			serializer.Serialize(writer, data);
			Debug.Log($"Exported XML schema for {typeof(T).Name} to: {path}");
		}
		catch (Exception ex)
		{
			Debug.LogError($"Failed to export XML schema for {typeof(T).Name}: {ex.Message}");
		}
	}

	public static T[] ImportXml<T>(string directory) where T : class
	{
		string path = Path.Combine(directory, typeof(T).Name + ".xml");
		try
		{
			if (File.Exists(path))
			{
				var serializer = new XmlSerializer(typeof(T[]));
				using var reader = new StreamReader(path);
				return (T[])serializer.Deserialize(reader);
			}
			Debug.LogWarning($"XML file not found for {typeof(T).Name}: {path}");
			return Array.Empty<T>();
		}
		catch (Exception ex)
		{
			Debug.LogError($"Failed to import XML for {typeof(T).Name}: {ex.Message}");
			return Array.Empty<T>();
		}
	}
}
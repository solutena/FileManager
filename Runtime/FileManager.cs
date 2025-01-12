using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class FileManager
{
	public static void ExportJson<T>(T data, string directory) => ExportJson(data, directory, typeof(T).FullName);
	public static void ExportJson<T>(T data, string directory, string fileName)
	{
		string path = Path.Combine(directory, $"{fileName}.json");
		try
		{
			Directory.CreateDirectory(directory);
			string json = JsonUtility.ToJson(data, true);
			File.WriteAllText(path, json, Encoding.UTF8);
			Debug.Log($"Exported JSON for {fileName} to: {path}");
		}
		catch (Exception ex)
		{
			Debug.LogError($"Failed to export JSON for {fileName}: {ex.Message}");
		}
	}

	public static T ImportJson<T>(string directory) => ImportJson<T>(directory, typeof(T).FullName);
	public static T ImportJson<T>(string directory, string fileName)
	{
		string path = Path.Combine(directory, $"{fileName}.json");
		try
		{
			if (File.Exists(path))
            {
                string json = File.ReadAllText(path, Encoding.UTF8);
                return JsonUtility.FromJson<T>(json);
			}
			Debug.LogWarning($"JSON file not found for {fileName}: {path}");
			return default;
        }
        catch (Exception ex)
		{
			Debug.LogError($"Failed to import JSON for {fileName}: {ex.Message}");
			return default;
        }
    }

	public static void ExportXmlSchema<T>(string directory) where T : new() => ExportXmlSchema<T>(directory, typeof(T).FullName);
	public static void ExportXmlSchema<T>(string directory, string fileName) where T : new()
	{
		string path = Path.Combine(directory, $"{fileName}_Schema.xml");
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
			Debug.Log($"Exported XML schema for {fileName} to: {path}");
		}
		catch (Exception ex)
		{
			Debug.LogError($"Failed to export XML schema for {fileName}: {ex.Message}");
		}
	}

	public static T[] ImportXml<T>(string directory) => ImportXml<T>(directory, typeof(T).FullName);
	public static T[] ImportXml<T>(string directory, string fileName)
	{
		string path = Path.Combine(directory, $"{fileName}.xml");
		try
		{
			if (File.Exists(path))
			{
				var serializer = new XmlSerializer(typeof(T[]));
				using var reader = new StreamReader(path);
				return (T[])serializer.Deserialize(reader);
			}
			Debug.LogWarning($"XML file not found for {fileName}: {path}");
			return Array.Empty<T>();
		}
		catch (Exception ex)
		{
			Debug.LogError($"Failed to import XML for {fileName}: {ex.Message}");
			return Array.Empty<T>();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class FileManager : MonoBehaviour
{
	public static FileManager Instance { get; set; }

	public List<PathSetting> pathSettings;
	private ReadOnlyDictionary<string, string> Paths { get; set; }

	[Serializable]
	public class PathSetting
	{
		public enum PathType
		{
			dataPath,               // 주 데이터 저장 경로
			persistentDataPath,     // 지속 가능한 데이터 저장 경로
			streamingAssetsPath,    // 스트리밍 자산 경로
			temporaryCachePath,     // 임시 캐시 경로
			consoleLogPath,         // 콘솔 로그 경로
		}

		public PathType type;
		public string key;
		public string path;

		public string FullPath
		{
			get
			{
				return type switch
				{
					PathType.dataPath => Path.Combine(Application.dataPath, path),
					PathType.persistentDataPath => Path.Combine(Application.persistentDataPath, path),
					PathType.streamingAssetsPath => Path.Combine(Application.streamingAssetsPath, path),
					PathType.temporaryCachePath => Path.Combine(Application.temporaryCachePath, path),
					PathType.consoleLogPath => Path.Combine(Application.consoleLogPath, path),
					_ => throw new NotImplementedException(),
				};
			}
		}
	}

	private void Awake()
	{
		Instance = this;
		Paths = new(pathSettings.ToDictionary(setting => setting.key, setting => setting.FullPath));
	}

	private string GetPath(string pathKey, string fileName)
	{
		var directory = Paths.GetValueOrDefault(pathKey);
		if (string.IsNullOrEmpty(directory))
			throw new ArgumentException($"Path type '{pathKey}' is not valid or not defined.");
		return Path.Combine(directory, fileName);
	}

	public void ExportJson<T>(T data, string pathKey) => ExportJson(data, pathKey, typeof(T).FullName);
	public void ExportJson<T>(T data, string pathKey, string fileName)
	{
		string path = GetPath(pathKey, $"{fileName}.json");
		try
		{
			Directory.CreateDirectory(pathKey);
			string json = JsonUtility.ToJson(data, true);
			File.WriteAllText(path, json, Encoding.UTF8);
			Debug.Log($"Exported JSON for {fileName} to: {path}");
		}
		catch (Exception ex)
		{
			Debug.LogError($"Failed to export JSON for {fileName}: {ex.Message}");
		}
	}

	public T ImportJson<T>(string pathKey) => ImportJson<T>(pathKey, typeof(T).FullName);
	public T ImportJson<T>(string pathKey, string fileName)
	{
		string path = GetPath(pathKey, $"{fileName}.json");
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

	public void ExportXmlSchema<T>(string pathKey) where T : new() => ExportXmlSchema<T>(pathKey, typeof(T).FullName);
	public void ExportXmlSchema<T>(string pathKey, string fileName) where T : new()
	{
		string path = GetPath(pathKey, $"{fileName}_Schema.xml");
		try
		{
			Directory.CreateDirectory(pathKey);
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

	public T[] ImportXml<T>(string pathKey) => ImportXml<T>(pathKey, typeof(T).FullName);
	public T[] ImportXml<T>(string pathKey, string fileName)
	{
		string path = GetPath(pathKey, $"{fileName}.xml");
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
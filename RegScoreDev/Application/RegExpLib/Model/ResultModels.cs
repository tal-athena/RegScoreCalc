using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace RegExpLib.Model
{
	public static class CsvHelper
	{
		#region Static operations

		public static void Serialize<T>(StreamWriter writer, StringBuilder sb, Action<StringBuilder> action) where T : class
		{
			sb.Clear();

			if (action != null)
				action(sb);

			writer.WriteLine(sb.ToString());
		}

		public static T Deserialize<T>(StreamReader reader, int valueCount, Func<string[], T> func) where T : class
		{
			if (func == null)
				return null;

			var value = reader.ReadLine();

			if (value == null)
				return null;

			var separator = new[] { ';' };

			var split = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			if (split.Length != valueCount)
				return null;

			return func(split);
		}

		#endregion
	}

	public class RegExpProcessingResultsCollection<T> where T : RegExpProcessingResultBase, new()
	{
		#region Fields

		protected ConcurrentQueue<T> _queue;
		protected List<T> _list;

		#endregion

		#region Properties

		public IEnumerable<T> Items
		{
			get
			{
				if (_list != null)
					return _list;

				return _queue;
			}
		}

		#endregion

		#region Ctors

		public RegExpProcessingResultsCollection()
		{
			_queue = new ConcurrentQueue<T>();
		}

		public RegExpProcessingResultsCollection(IEnumerable<T> enumerable)
		{
			_list = enumerable.ToList();
		}

		public RegExpProcessingResultsCollection(List<T> list)
		{
			_list = list;
		}

		#endregion

		#region Operations

		public void Add(T value)
		{
			_queue.Enqueue(value);
		}

		public void Serialize(string filePath)
		{
			var json = JsonConvert.SerializeObject(this.Items);
			File.WriteAllText(filePath, json);
		}

		public void Deserialize(string filePath)
		{
			if (File.Exists(filePath))
			{
				var json = File.ReadAllText(filePath);

				_list = JsonConvert.DeserializeObject<List<T>>(json);
			}
			else
				_list = new List<T>();
		}

		#endregion
	}

	public class RegExpProcessingParamsBase
	{
		#region Fields

		public ProcessingOperation Operation { get; set; }
		public string RegExpDatabaseFilePath { get; set; }
		public string DocumentsDatabaseFilePath { get; set; }
		public string Password { get; set; }
		public string WorkingFolder { get; set; }

		public string LogFileName { get; set; }
		public string ProgressFileName { get; set; }

		#endregion

		#region Operations

		public string GetFullPath(string fileName)
		{
			return Path.Combine(this.WorkingFolder, fileName);
		}

		public void Serialize(string filePath)
		{
			var json = JsonConvert.SerializeObject(this);
			File.WriteAllText(filePath, json);
		}

		public static RegExpProcessingParamsBase Deserialize(string filePath)
		{
			var json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<RegExpProcessingParamsBase>(json);
		}

		public static T Deserialize<T>(string filePath) where T : RegExpProcessingParamsBase
		{
			var json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<T>(json);
		}

		#endregion
	}

	public class RegExpProcessingResultBase
	{

	}

    public class EntitiesProcessingParamBase
    {
        public string WorkingFolder { get; set; }

        public string LogFileName { get; set; }
        public string ProgressFileName { get; set; }        
        public string OutputFileName { get; set; }

        public string GetFullPath(string fileName)
        {
            return Path.Combine(this.WorkingFolder, fileName);
        }
        public void Serialize(string filePath)
        {
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, json);
        }

        public static EntitiesProcessingParamBase Deserialize(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<EntitiesProcessingParamBase>(json);
        }

        public static T Deserialize<T>(string filePath) where T : EntitiesProcessingParamBase
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

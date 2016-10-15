using System;
using System.Text;
using System.Collections.Generic;
using TestTask.Database.Entity;
using TestTask.Database;

namespace TestTask.Net.Config
{
	public class EncryptionConfig
	{
		public static readonly EncryptionConfig DEFAULT_CONFIG = new EncryptionConfig();

		public const string DEFAULT_STOP_COND_STR = "<END>";
		public const char DEFAULT_ENC_FLAG = '+';
		public const char DEFAULT_DEC_FLAG = '-';

		public string StopCondStr { get; private set; }
		public char EncFlag { get; private set; }
		public char DecFlag { get; private set; }

		public GenerationStrategy<EncryptionData> generationStrategy;

		public EncryptionConfig(string stopCondStr = DEFAULT_STOP_COND_STR,
		                        char encFlag = DEFAULT_ENC_FLAG,
		                        char decFlag = DEFAULT_DEC_FLAG,
		                        GenerationStrategy<EncryptionData> generationStrategy = null)
		{
			StopCondStr = stopCondStr;
			EncFlag = encFlag;
			DecFlag = decFlag;
			this.generationStrategy = generationStrategy == null ? DefaultGenerationStrategy : generationStrategy;
		}

		public static IList<EncryptionData> DefaultGenerationStrategy()
		{
			IList<EncryptionData> result = new List<EncryptionData>();
			StringBuilder source1 = new StringBuilder()
				.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWYZ0123456789-.,!?\"\\()<>;:'");
			StringBuilder source2 = new StringBuilder()
				.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWYZ0123456789-.,!?\"\\()<>;:'");
			Random rand = new Random();
			int id = 1;
			while (source1.Length > 0)
			{
				int i = rand.Next(0,source1.Length), j = rand.Next(0,source2.Length);
				EncryptionData e = new EncryptionData();
				e.ID = id;
				e.OldSymbol = source1[i].ToString();
				e.NewSymbol = source2[j].ToString();
				result.Add(e);
				id++;
				source1.Remove(i,1);
				source2.Remove(j,1);
			}
			return result;
		}
	}
}

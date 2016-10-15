using System.Collections.Generic;
using System.Text;
using System.Net;
using System;
using TestTask.Database.Util;
using TestTask.Database.Entity;
using TestTask.Database.DAO;
using TestTask.Net.Config;
using TestTask.Net.Server.Config;

namespace TestTask.Net.Server
{
	public class EncryptionServer : Server
	{
		private EncryptionConfig config;
		private EncyptionDataDAO service;
		private bool encrypt;

		public EncryptionServer(IPAddress addres, 
		                        int port, 
		                        ServerConfig serverConfig = null,
		                        EncryptionConfig config = null) : 
		base (addres,port,serverConfig)
		{
			if (config == null) this.config = EncryptionConfig.DEFAULT_CONFIG;
			else this.config = config;
			service = new EncyptionDataDAO(NHibernateUtil.Factory.GetCurrentSession());
			onStart += () => Console.WriteLine("Starting server...");
			onStart += DBInit;
			onStop += () => Console.WriteLine("Finishing working...");
			onStop += () => service.CloseSession();
			onStop += () => Console.WriteLine("Finished.");
			beforeDataManipulates += () => Console.WriteLine("Start "+(encrypt?"en":"de")+"crypting recieved message...");
			afterDataManipulates += () => Console.WriteLine("Finished operation.");
		}

		public override void Start()
		{
			Start<StringBuilder>((data,rec,update) => update.Append(Encoding.UTF8.GetString(data,0,rec)),
			                     Action,
			                     (data) => data.ToString().Equals(config.StopCondStr));
		}

		private EncryptionData FindInDB(string propertyName, string requiredValue)
		{
			IList <EncryptionData> searchResult = service.FindByProperty(propertyName,requiredValue);
			if (searchResult == null || searchResult.Count == 0)
			{
				return null;
			}
			return searchResult[0];
		}

		private StringBuilder Action(StringBuilder strb)
		{
			if (strb[0] == config.EncFlag)
			{
				encrypt = true;
				strb = Encrypt(strb);
			}
			else if (strb[0] == config.DecFlag)
			{
				encrypt = false;
				strb = Decrypt(strb);
			}
			return strb;
		}

		private StringBuilder Encrypt(StringBuilder strb)
		{
			EncryptionData founded;
			for (int i = 0; i < strb.Length; i++)
			{
				founded = FindInDB("oldSymbol", strb[i].ToString());
				if (founded == null)
				{
					continue;
				}
				strb[i] = founded.NewSymbol[0];
			}
			return strb;
		}

		private StringBuilder Decrypt(StringBuilder strb) {
			EncryptionData founded;
			for (int i = 1; i < strb.Length; i++)
			{
				founded = FindInDB("newSymbol", strb[i].ToString());
				if (founded == null)
				{
					continue;
				}
				strb[i] = founded.OldSymbol[0];
			}
			return strb;
		}

		private void DBInit()
		{
			TableValidator<EncryptionData, int> validator = new TableValidator<EncryptionData, int>(service);
			if (validator.EmptyTable())
			{
				validator.GenerateTableContent(config.generationStrategy);
			}
		}
	}
}

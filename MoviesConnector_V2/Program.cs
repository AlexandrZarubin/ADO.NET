using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MoviesConnector_V2
{
	internal class Program
	{
		static void Main(string[] args)
		{
			/*
			const string CONNECTION_STRING =
			"Data Source=(localdb)\\MSSQLLocalDB;" +
			"Initial Catalog=Movies_VPD_311;" +
			"Integrated Security=True;" +
			"Connect Timeout=30;" +
			"Encrypt=False;" +
			"TrustServerCertificate=False;" +
			"ApplicationIntent=ReadWrite;" +
			"MultiSubnetFailover=False";
			Connector connector = new Connector(CONNECTION_STRING);
			*/
			
			Connector connector = new Connector(ConfigurationManager.ConnectionStrings["Movies_VPD_311"].ConnectionString);
			//connector.Select("SELECT * FROM Directors");
			//connector.Select("SELECT * FROM Movies");
			//connector.Select("*", "Movies");
			connector.IsertDirector("Martin", "Scorsese");
			connector.Select("*", "Directors");
			connector.Select(
				"titel,release_date,FORMATMESSAGE(N'%s %s', first_name, last_name) AS N'Режиссер'",
				"Movies,Directors",
				"director=director_id");

            Console.WriteLine("\n-------------------------------------------\n");
			Connector connector_academy = new Connector(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);

			connector_academy.Select("*", "Disciplines");
        }
	}
}

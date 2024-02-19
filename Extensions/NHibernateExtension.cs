using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;


namespace ORM.Extensions
{
    public static class NHibernateExtensions
    {
        public static ISessionFactory AddNHibernate(
            this IServiceCollection services,
            string connectionString, Action<ISessionFactory, Configuration> mappings)
        {

            var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
            {
                db.ConnectionString = connectionString;
                db.Driver<NHibernate.Driver.MySqlConnector.MySqlConnectorDriver>();
                db.Dialect<MySQL55Dialect>();
                db.LogSqlInConsole = true;
            });

            configuration.AddAssembly(Assembly.GetExecutingAssembly());

            var schema = new SchemaExport(configuration);

            schema
            .SetOutputFile(@"db.sql")
            .Execute(false, false, false);


            var update = new SchemaUpdate(configuration);
            update.Execute(false, true);


            var sessionFactory = configuration.BuildSessionFactory();
            
            services.AddSingleton(sessionFactory);
            mappings(sessionFactory, configuration);

            return sessionFactory;

        }

       
       
    }
}
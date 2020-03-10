using System;
using System.Threading.Tasks;

namespace MongoDb
{
    class Program
    {
        static void Main(string[] args)
        {


            var mdc = new MyMongoDbClient();

            try
            {

          // mdc.Insert();

               mdc.Query();
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}

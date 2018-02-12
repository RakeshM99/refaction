using refactor_me.Interface;
using refactor_me.Repositories;

namespace refactor_me.Service
{
    //For future use when data complexity increases.
    public class ProductService
    {
        //This will be used when we have complex table with foreign key relationships.
        //If data is coming from more than 3 tables it's better to use storeprocedure and consume storeprocedure data at ProductTasks level. 
        //Utilize methods defined at ProdTasks level. Get final result here(service files) and send it to Controller.
        private IProductTask prodtask = new ProductTasks();
    }
}
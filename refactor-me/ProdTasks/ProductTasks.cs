using refactor_me.Interface;
using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace refactor_me.Repositories
{
    public class ProductTasks : IProductTask
    {
        #region Private Variables
        private ProductsDB db;
        private Products products;
        private ProductOptions options;
        #endregion

        #region Public Variables
        public ProductTasks()
        {
            db = new ProductsDB();
            products = new Products();
            options = new ProductOptions();
        }
        #endregion

        #region Product Methods
        ///<summary>
        ///Get all Products
        ///</summary>
        public async Task<Products> GetAllProducts()
        {
            return await LoadProductsAsync(null);
        }

        ///<summary>
        //// Find Product(s) as per search parameter
        ///<param>Param</param>
        ///</summary>
        public async Task<Products> GetProduct(string param)
        {
            return await LoadProductsAsync(param);
        }

        ///<summary>
        ////Common method to fetch Products available
        ///<param>Search Parameter</param>
        ///</summary>
        private async Task<Products> LoadProductsAsync(string guidId)
        {
            try
            {
                Guid guidOutput;
                IList<Product> result = new List<Product>();

                if (guidId == null)
                {
                    result = await db.Products.ToListAsync();
                }
                else if (Guid.TryParse(guidId, out guidOutput) == true)
                {
                    Guid guId = Guid.Parse(guidId);
                    result = await db.Products.Where(p => p.Id == guId).ToListAsync();
                }
                else
                {
                    result = await db.Products.Where(p => p.Name.ToLower().Contains(guidId)).ToListAsync();
                }

                foreach (var item in result)
                {
                    products.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return products;
        }

        ///<summary>
        ////Add new Product
        ///<param>Product</param>
        ///</summary>
        public async Task<Product> AddProduct(Product item)
        {
            try
            {
                item.Id = Guid.NewGuid();
                db.Products.Add(item);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException dbAddException)
            {
                if (ProductExists(item.Id))
                {
                    throw new Exception("Product with same Id already exists.");
                }
                else
                {
                    throw dbAddException;
                }
            }

            return item;
        }

        ///<summary>
        ////Update existing Product details
        ///<param>Product</param>
        ///</summary>
        public async Task<bool> UpdateProduct(Product item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception dbUpdateException)
            {
                if (!ProductExists(item.Id))
                {
                    throw new Exception("Product not found.");
                }
                else
                {
                    throw dbUpdateException;
                }
            }
            return true;
        }

        ///<summary>
        ///Checking for existing Product as per Id
        ///<param>ProductId</param>
        ///</summary>
        private bool ProductExists(Guid id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }

        ///<summary>
        ////Delete Product and related Option(s)
        ///<param>ProductId</param>
        ///</summary>
        public async Task<bool> DeleteProduct(Guid id)
        {
            try
            {
                IList<ProductOption> productOptions = await db.ProductOptions.Where(p => p.ProductId == id).ToListAsync<ProductOption>();

                if (productOptions != null && productOptions.Count > 0)
                {
                    db.ProductOptions.RemoveRange(productOptions);
                }

                Product product = await db.Products.FindAsync(id);

                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                db.Products.Remove(product);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        #endregion Product Methods

        #region ProductOption Methods
        ///<summary>
        ///Get all ProductOptions available for specific Product
        ///<param>ProductId</param>
        ///</summary>
        public async Task<ProductOptions> GetAllProductOptions(Guid productId)
        {
            return await LoadProductOptionsAsync(productId, null);
        }

        ///<summary>
        ///Get Option as per input ProductId and OptionId
        ///<param>ProductId</param>
        ///<param>OptionId</param>
        ///</summary>
        public async Task<ProductOption> GetProductOption(Guid productId, Guid id)
        {
            var result = await LoadProductOptionsAsync(productId, id);
            return result.Items.First();
        }

        ///<summary>
        ///Common method to fetch Options as per ProductId
        ///<param>ProductId</param>
        ///<param>OptionId</param>
        ///</summary>
        private async Task<ProductOptions> LoadProductOptionsAsync(Guid productId, Guid? id)
        {
            try
            {
                IList<ProductOption> productOptions;

                if (id == null)
                {
                    productOptions = await db.ProductOptions.Where(po => po.ProductId == productId).ToListAsync();
                }
                else
                {
                    productOptions = await db.ProductOptions.Where(po => po.ProductId == productId && po.Id == id).ToListAsync();
                }

                foreach (var item in productOptions)
                {
                    options.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return options;
        }

        ///<summary>
        ///Add Option for Product
        ///<param>ProductOption</param>
        ///</summary>
        public async Task<ProductOption> AddProductOption(string productId, ProductOption item)
        {
            try
            {
                Guid result;
                item.Id = Guid.NewGuid();
                if (Guid.TryParse(productId, out result))
                    item.ProductId = result;
                else
                    return null;
                db.ProductOptions.Add(item);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException dbAddException)
            {
                if (ProductOptionExists(item.Id))
                {
                    throw new Exception("ProductOption with same Id already exists.");
                }
                else
                {
                    throw dbAddException;
                }
            }

            return item;
        }

        ///<summary>
        ///Update Option
        ///<param>ProductOption</param>
        ///</summary>
        
        public async Task<bool> UpdateProductOption(ProductOption item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception dbUpdateException)
            {
                if (!ProductOptionExists(item.Id))
                {
                    throw new Exception("ProductOption not found.");
                }
                else
                {
                    throw dbUpdateException;
                }
            }
            return true;
        }

        ///<summary>
        ///Checking for existing Option as per Id
        ///<param>Id</param>
        ///</summary>
        private bool ProductOptionExists(Guid id)
        {
            return db.ProductOptions.Count(e => e.Id == id) > 0;
        }

        ///<summary>
        ///Delete ProductOption as per given Id
        ///<param>Id</param>
        ///</summary>
        
        public async Task<bool> DeleteProductOption(Guid id)
        {
            try
            {
                ProductOption option = await db.ProductOptions.FindAsync(id);

                if (option == null)
                {
                    throw new Exception("ProductOption not found");
                }

                db.ProductOptions.Remove(option);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        #endregion ProductOption Methods
    }
}

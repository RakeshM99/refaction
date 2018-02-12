using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace refactor_me.Interface
{
    public interface IProductTask 
    {
        ////<summary>
        ////Get all Products
        ////</summary>
        Task<Products> GetAllProducts();

        ////<summary>
        //// Find Product(s) by name
        ////<param>Param</param>
        ////</summary>
        Task<Products> GetProduct(string param);

        ////<summary>
        ////Add New Product
        ////<param>Product</param>
        ////</summary>
        Task<Product> AddProduct(Product item);

        ///<summary>
        ////Update Product
        ///<param>Product</param>
        ///</summary>
        Task<bool> UpdateProduct(Product item);

        ////<summary>
        ////Delete Product and related Option(s)
        ////<param>ProductId</param>
        ////</summary>
        Task<bool> DeleteProduct(Guid id);

        ////<summary>
        ////Get all Options for specific Product
        ////<param>ProductId</param>
        ////</summary>
        Task<ProductOptions> GetAllProductOptions(Guid productId);

        ////<summary>
        ////Get Option as per ProductId and OptionId
        ////<param>ProductId</param>
        ////<param>OptionId</param>
        ////</summary>
        Task<ProductOption> GetProductOption(Guid productId, Guid id);

        ////<summary>
        ////Add Option for Product
        ////<param>ProductOption</param>
        ////</summary>
        Task<ProductOption> AddProductOption(string productId, ProductOption item);

        ////<summary>
        ////Update Option
        ////<param>ProductOption</param>
        ////</summary>
        Task<bool> UpdateProductOption(ProductOption item);

        ////<summary>
        ////Delete Option as per Id
        ////<param>Id</param>
        ////</summary>
        Task<bool> DeleteProductOption(Guid id);
    }
}
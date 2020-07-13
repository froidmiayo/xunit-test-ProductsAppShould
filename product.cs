using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xunit;

namespace TestingWithxUnit {

    public class ProductsAppShould {
        
        // Add your test here
        private Products _productsSvc;
        public ProductsAppShould()
        {
            _productsSvc  = new Products();
        }

        [Fact]
        public void Products_Should_Return_ArgumentNullException_When_Product_IsNull()
        {
            //Arrange
            var product = new Product()
            {
                Name =  new Guid().ToString(),
                IsSold = false
            };

            
            //Act
            product = null; //comment to fail

            //Assert
            Assert.Throws<ArgumentNullException>(() => _productsSvc.AddNew(product));

        }
        

        [Fact]
        public void Products_Should_Return_AddedProduct()
        {
            //Arrange
            var product = new Product()
            {
                Name =  new Guid().ToString(), //comment to fail
                IsSold = false
            };

            //Act
            _productsSvc.AddNew(product);

            //Assert
            Assert.Contains(product, _productsSvc.Items);
            Assert.Contains(_productsSvc.Items, x =>x.Name == product.Name);
        }

        [Fact]
        public void Products_Should_Return_NameRequiredException_When_Product_Name_IsNull()
        {
            //Arrange
            var product = new Product()
            {
                Name =  new Guid().ToString(), 
                IsSold =  false
            };
            
            //Act
            product.Name = null; //comment to fail

            //Assert
            Assert.Throws<NameRequiredException>(() => _productsSvc.AddNew(product));

        }

    }

    internal class Products {
        private readonly List<Product> _products = new List<Product> ();

        public IEnumerable<Product> Items => _products.Where (t => !t.IsSold);

        public void AddNew (Product product) {
            product = product ??
                      throw new ArgumentNullException ();
            product.Validate ();
            _products.Add (product);
        }

        public void Sold (Product product) {
            product.IsSold = true;
        }

    }

    internal class Product {
        public bool IsSold { get; set; }
        public string Name { get; set; }

        internal void Validate () {
            Name = Name ??
                   throw new NameRequiredException ();
        }

    }

    [Serializable]
    internal class NameRequiredException : Exception {
        public NameRequiredException () { /* ... */ }

        public NameRequiredException (string message) : base (message) { /* ... */ }

        public NameRequiredException (string message, Exception innerException) : base (message, innerException) { /* ... */ }

        protected NameRequiredException (SerializationInfo info, StreamingContext context) : base (info, context) { /* ... */ }
    }
}
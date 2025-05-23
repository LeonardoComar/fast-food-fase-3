import pytest
from unittest.mock import MagicMock, patch
from app.service.product_service import ProductService
from app.domain.product_model import ProductCreate, ProductResponse, ProductListResponse, ProductDB

class TestProductService:
    def setup_method(self):
        # Create a mock repository for each test
        self.mock_repository = MagicMock()
        
        # Create the service with the mock repository
        with patch('app.service.product_service.ProductRepository', return_value=self.mock_repository):
            self.mock_db_session = MagicMock()
            self.service = ProductService(self.mock_db_session)
        
        # Sample product data
        self.sample_product_db = ProductDB(
            id=1, 
            name="Test Product", 
            category="Lanche", 
            price=10.99, 
            description="Test description"
        )
        
        self.sample_product_create = ProductCreate(
            name="New Product",
            category="Bebida",
            price=5.99,
            description="New product description"
        )
    
    def test_get_all_products(self):
        # Arrange
        self.mock_repository.get_all_products.return_value = [self.sample_product_db]
        
        # Act
        result = self.service.get_all_products()
        
        # Assert
        self.mock_repository.get_all_products.assert_called_once()
        assert isinstance(result, ProductListResponse)
        assert len(result.products) == 1
        assert result.products[0].id == 1
        assert result.products[0].name == "Test Product"
    
    def test_get_product_by_id_found(self):
        # Arrange
        self.mock_repository.get_product_by_id.return_value = self.sample_product_db
        
        # Act
        result = self.service.get_product_by_id(1)
        
        # Assert
        self.mock_repository.get_product_by_id.assert_called_once_with(1)
        assert isinstance(result, ProductResponse)
        assert result.id == 1
        assert result.name == "Test Product"
    
    def test_get_product_by_id_not_found(self):
        # Arrange
        self.mock_repository.get_product_by_id.return_value = None
        
        # Act & Assert
        with pytest.raises(ValueError, match="Product with ID 999 not found"):
            self.service.get_product_by_id(999)
    
    def test_create_product(self):
        # Arrange
        created_product = ProductDB(
            id=2, 
            name=self.sample_product_create.name,
            category=self.sample_product_create.category,
            price=self.sample_product_create.price,
            description=self.sample_product_create.description
        )
        self.mock_repository.create_product.return_value = created_product
        
        # Act
        result = self.service.create_product(self.sample_product_create)
        
        # Assert
        self.mock_repository.create_product.assert_called_once()
        # Verify the dict passed to repository has correct data
        product_dict = self.mock_repository.create_product.call_args[0][0]
        assert product_dict["name"] == "New Product"
        assert product_dict["category"] == "Bebida"
        assert product_dict["price"] == 5.99
        
        assert isinstance(result, ProductResponse)
        assert result.id == 2
        assert result.name == "New Product"
    
    def test_update_product_found(self):
        # Arrange
        self.mock_repository.get_product_by_id.return_value = self.sample_product_db
        
        updated_product = ProductDB(
            id=1, 
            name="Updated Product",
            category="Lanche",
            price=12.99,
            description="Updated description"
        )
        self.mock_repository.update_product.return_value = updated_product
        
        # Act
        update_data = ProductCreate(
            name="Updated Product",
            category="Lanche",
            price=12.99,
            description="Updated description"
        )
        result = self.service.update_product(1, update_data)
        
        # Assert
        self.mock_repository.get_product_by_id.assert_called_once_with(1)
        self.mock_repository.update_product.assert_called_once()
        assert isinstance(result, ProductResponse)
        assert result.id == 1
        assert result.name == "Updated Product"
        assert result.price == 12.99
    
    def test_update_product_not_found(self):
        # Arrange
        self.mock_repository.get_product_by_id.return_value = None
        
        # Act & Assert
        update_data = ProductCreate(
            name="Updated Product",
            category="Lanche",
            price=12.99,
            description="Updated description"
        )
        with pytest.raises(ValueError, match="Product with ID 999 not found"):
            self.service.update_product(999, update_data)

import pytest
from pydantic import ValidationError
from app.domain.product_model import ProductCreate, ProductResponse, ProductCategory, ProductDB

class TestProductModel:
    def test_product_create_valid(self):
        # Test valid product creation
        product_data = {
            "name": "Test Product",
            "category": ProductCategory.SANDWICH,
            "price": 10.99,
            "description": "Test description"
        }
        product = ProductCreate(**product_data)
        
        assert product.name == "Test Product"
        assert product.category == ProductCategory.SANDWICH
        assert product.price == 10.99
        assert product.description == "Test description"
    
    def test_product_create_no_description(self):
        # Test product creation without optional description
        product_data = {
            "name": "Test Product",
            "category": ProductCategory.SANDWICH,
            "price": 10.99
        }
        product = ProductCreate(**product_data)
        
        assert product.name == "Test Product"
        assert product.description is None
    
    def test_product_create_invalid_category(self):
        # Test product creation with invalid category
        product_data = {
            "name": "Test Product",
            "category": "Invalid Category",
            "price": 10.99
        }
        
        with pytest.raises(ValidationError):
            ProductCreate(**product_data)
    
    def test_product_create_negative_price(self):
        # Test product creation with negative price
        product_data = {
            "name": "Test Product",
            "category": ProductCategory.SANDWICH,
            "price": -10.99
        }
        
        # Note: Pydantic doesn't automatically validate that prices are positive
        # If you want this validation, you would need to add a validator to your model
        product = ProductCreate(**product_data)
        assert product.price == -10.99
    
    def test_product_response_from_db(self):
        # Test creating ProductResponse from ProductDB
        db_product = ProductDB(
            id=1,
            name="Test Product",
            category=ProductCategory.SANDWICH,
            price=10.99,
            description="Test description"
        )
        
        # Using model_validate to create from SQLAlchemy model
        response = ProductResponse.model_validate(db_product)
        
        assert response.id == 1
        assert response.name == "Test Product"
        assert response.category == ProductCategory.SANDWICH
        assert response.price == 10.99
        assert response.description == "Test description"

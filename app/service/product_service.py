from sqlalchemy.orm import Session
from typing import List
from app.repository.product_repository import ProductRepository
from app.domain.product_model import ProductResponse, ProductListResponse, ProductCreate

class ProductService:
    def __init__(self, db_session: Session):
        self.repository = ProductRepository(db_session)
    
    def get_all_products(self) -> ProductListResponse:
        """
        Get all products and return them as a response model
        """
        products = self.repository.get_all_products()
        return ProductListResponse(
            products=[ProductResponse.model_validate(product) for product in products]
        )
    
    def get_product_by_id(self, product_id: int) -> ProductResponse:
        """
        Get a product by its ID
        """
        product = self.repository.get_product_by_id(product_id)
        if not product:
            raise ValueError(f"Product with ID {product_id} not found")
        return ProductResponse.model_validate(product)
    
    def create_product(self, product_data: ProductCreate) -> ProductResponse:
        """
        Create a new product
        """
        # Convert Pydantic model to dict for the repository
        product_dict = product_data.model_dump()
        product = self.repository.create_product(product_dict)
        return ProductResponse.model_validate(product)
        
    def update_product(self, product_id: int, product_data: ProductCreate) -> ProductResponse:
        """
        Update an existing product
        """
        # Check if the product exists
        existing_product = self.repository.get_product_by_id(product_id)
        if not existing_product:
            raise ValueError(f"Product with ID {product_id} not found")
        
        # Convert Pydantic model to dict for the repository
        product_dict = product_data.model_dump()
        
        # Update the product
        updated_product = self.repository.update_product(product_id, product_dict)
        return ProductResponse.model_validate(updated_product)

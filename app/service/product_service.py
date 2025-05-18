from sqlalchemy.orm import Session
from typing import List
from app.repository.product_repository import ProductRepository
from app.domain.product_model import ProductResponse, ProductListResponse

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

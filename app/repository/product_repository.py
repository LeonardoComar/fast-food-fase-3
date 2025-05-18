from sqlalchemy.orm import Session
from typing import List, Optional
from app.domain.product_model import ProductDB

class ProductRepository:
    def __init__(self, db_session: Session):
        self.db_session = db_session
    
    def get_all_products(self) -> List[ProductDB]:
        """
        Retrieve all products from the database
        """
        return self.db_session.query(ProductDB).all()
    
    def get_product_by_id(self, product_id: int) -> Optional[ProductDB]:
        """
        Retrieve a product by its ID
        """
        return self.db_session.query(ProductDB).filter(ProductDB.id == product_id).first()
    
    def create_product(self, product_data: dict) -> ProductDB:
        """
        Create a new product in the database
        """
        new_product = ProductDB(**product_data)
        self.db_session.add(new_product)
        self.db_session.commit()
        self.db_session.refresh(new_product)
        return new_product
        
    def update_product(self, product_id: int, product_data: dict) -> ProductDB:
        """
        Update an existing product in the database
        """
        product = self.db_session.query(ProductDB).filter(ProductDB.id == product_id).first()
        if not product:
            raise ValueError(f"Product with ID {product_id} not found")
        
        # Update product attributes
        for key, value in product_data.items():
            setattr(product, key, value)
        
        # Commit changes
        self.db_session.commit()
        self.db_session.refresh(product)
        return product

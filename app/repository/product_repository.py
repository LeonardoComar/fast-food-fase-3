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

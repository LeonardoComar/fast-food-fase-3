from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.core.mysql_connection import get_db
from app.service.product_service import ProductService
from app.domain.product_model import ProductResponse, ProductListResponse

router = APIRouter(
    prefix="/products",
    tags=["products"]
)

@router.get("/", response_model=ProductListResponse)
async def get_all_products(db: Session = Depends(get_db)):
    """
    Get all products
    """
    try:
        service = ProductService(db)
        return service.get_all_products()
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=str(e)
        )

@router.get("/{product_id}", response_model=ProductResponse)
async def get_product_by_id(product_id: int, db: Session = Depends(get_db)):
    """
    Get a specific product by ID
    """
    try:
        service = ProductService(db)
        return service.get_product_by_id(product_id)
    except ValueError as e:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=str(e)
        )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=str(e)
        )

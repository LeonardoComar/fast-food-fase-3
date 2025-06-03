from typing import Optional
from fastapi import APIRouter, Depends, HTTPException, status, Query
from sqlalchemy.orm import Session
from app.core.mysql_connection import get_db
from app.service.order_service import OrderService
from app.domain.order_model import OrderResponse, OrderListResponse, OrderCreate, OrderStatusUpdate, OrderStatus
from app.core.auth import get_current_user

router = APIRouter(
    prefix="/orders",
    tags=["orders"]
)

@router.get("/", response_model=OrderListResponse)
async def get_orders(
    status: Optional[OrderStatus] = Query(None, description="Filter orders by status"),
    db: Session = Depends(get_db),
    current_user: dict = Depends(get_current_user)
):
    """
    Get all orders or filter by status if provided
    """
    try:
        service = OrderService(db)
        if status:
            return service.get_orders_by_status(status)
        return service.get_all_orders()
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=str(e)
        )

@router.get("/{order_id}", response_model=OrderResponse)
async def get_order_by_id(order_id: int, db: Session = Depends(get_db), current_user: dict = Depends(get_current_user)):
    """
    Get a specific order by ID
    """
    try:
        service = OrderService(db)
        return service.get_order_by_id(order_id)
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

@router.post("/", response_model=OrderResponse, status_code=status.HTTP_201_CREATED)
async def create_order(order: OrderCreate, db: Session = Depends(get_db), current_user: dict = Depends(get_current_user)):
    """
    Create a new order
    """
    try:
        service = OrderService(db)
        return service.create_order(order)
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=str(e)
        )

@router.patch("/{order_id}/status", response_model=OrderResponse)
async def update_order_status(order_id: int, status_update: OrderStatusUpdate, db: Session = Depends(get_db), current_user: dict = Depends(get_current_user)):
    """
    Update an order's status
    """
    try:
        service = OrderService(db)
        return service.update_order_status(order_id, status_update)
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

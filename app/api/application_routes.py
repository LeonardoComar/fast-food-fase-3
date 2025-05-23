from fastapi import APIRouter
from .product_routes import router as product_router
from .client_routes import router as client_router
from .order_routes import router as order_router

router = APIRouter()

router.include_router(product_router)
router.include_router(client_router)
router.include_router(order_router)

@router.get("/health_check")
async def health_check():
    return {"status": "ok"}
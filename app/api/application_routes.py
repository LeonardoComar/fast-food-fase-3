from fastapi import APIRouter
from .product_routes import router as product_router
from .client_routes import router as client_router
from .order_routes import router as order_router

router = APIRouter()

# Inclui todas as rotas dos módulos
router.include_router(product_router)
router.include_router(client_router)
router.include_router(order_router)

# Rota de Health Check
@router.get("/health_check")
async def health_check():
    return {"status": "ok"}
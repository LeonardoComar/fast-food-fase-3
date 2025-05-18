from fastapi import APIRouter
from .product_routes import router as product_router

router = APIRouter()

# Inclui todas as rotas dos módulos
router.include_router(product_router)

# Rota de Health Check
@router.get("/health_check")
async def health_check():
    return {"status": "ok"}
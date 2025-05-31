from fastapi import FastAPI
from contextlib import asynccontextmanager
from app.api.application_routes import router as api_router
from app.repository.db_repository import create_db_tables


@asynccontextmanager
async def lifespan(app: FastAPI):
    create_db_tables()
    yield

app = FastAPI(lifespan=lifespan)

app.include_router(api_router, prefix="/api")
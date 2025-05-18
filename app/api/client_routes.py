from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.core.mysql_connection import get_db
from app.service.client_service import ClientService
from app.domain.client_model import ClientResponse, ClientListResponse, ClientCreate, ClientUpdate

router = APIRouter(
    prefix="/clients",
    tags=["clients"]
)

@router.get("/", response_model=ClientListResponse)
async def get_all_clients(db: Session = Depends(get_db)):
    """
    Get all clients
    """
    try:
        service = ClientService(db)
        return service.get_all_clients()
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=str(e)
        )

@router.get("/{client_id}", response_model=ClientResponse)
async def get_client_by_id(client_id: int, db: Session = Depends(get_db)):
    """
    Get a specific client by ID
    """
    try:
        service = ClientService(db)
        return service.get_client_by_id(client_id)
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

@router.post("/", response_model=ClientResponse, status_code=status.HTTP_201_CREATED)
async def create_client(client: ClientCreate, db: Session = Depends(get_db)):
    """
    Create a new client
    """
    try:
        service = ClientService(db)
        return service.create_client(client)
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=str(e)
        )

@router.put("/{client_id}", response_model=ClientResponse)
async def update_client(client_id: int, client: ClientUpdate, db: Session = Depends(get_db)):
    """
    Update an existing client's name
    """
    try:
        service = ClientService(db)
        return service.update_client(client_id, client)
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

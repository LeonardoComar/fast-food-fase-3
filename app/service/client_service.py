from sqlalchemy.orm import Session
from typing import List
from app.repository.client_repository import ClientRepository
from app.domain.client_model import ClientResponse, ClientListResponse, ClientCreate, ClientUpdate

class ClientService:
    def __init__(self, db_session: Session):
        self.repository = ClientRepository(db_session)
    
    def get_all_clients(self) -> ClientListResponse:
        """
        Get all clients and return them as a response model
        """
        clients = self.repository.get_all_clients()
        return ClientListResponse(
            clients=[ClientResponse.model_validate(client) for client in clients]
        )
    
    def get_client_by_id(self, client_id: int) -> ClientResponse:
        """
        Get a client by its ID
        """
        client = self.repository.get_client_by_id(client_id)
        if not client:
            raise ValueError(f"Client with ID {client_id} not found")
        return ClientResponse.model_validate(client)
    
    def get_client_by_cpf(self, cpf: str) -> ClientResponse:
        """
        Get a client by CPF
        """
        client = self.repository.get_client_by_cpf(cpf)
        if not client:
            raise ValueError(f"Client with CPF {cpf} not found")
        return ClientResponse.model_validate(client)
    
    def create_client(self, client_data: ClientCreate) -> ClientResponse:
        """
        Create a new client
        """
        # Convert Pydantic model to dict for the repository
        client_dict = client_data.model_dump()
        client = self.repository.create_client(client_dict)
        return ClientResponse.model_validate(client)
        
    def update_client(self, client_id: int, client_data: ClientUpdate) -> ClientResponse:
        """
        Update an existing client's name
        """
        # Check if the client exists
        existing_client = self.repository.get_client_by_id(client_id)
        if not existing_client:
            raise ValueError(f"Client with ID {client_id} not found")
        
        # Convert Pydantic model to dict for the repository
        client_dict = client_data.model_dump()
        
        # Update the client
        updated_client = self.repository.update_client(client_id, client_dict)
        return ClientResponse.model_validate(updated_client)

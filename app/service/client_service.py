from sqlalchemy.orm import Session
from typing import List
from app.repository.client_repository import ClientRepository
from app.domain.client_model import ClientResponse, ClientListResponse, ClientCreate, ClientUpdate, ClientTokenResponse
from app.core.jwt import create_access_token

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
    
    def get_client_by_cpf(self, cpf: str) -> ClientTokenResponse:
        """
        Get a client by CPF and generate a JWT token
        """
        client = self.repository.get_client_by_cpf(cpf)
        if not client:
            raise ValueError(f"Client with CPF {cpf} not found")
        
        # Create token data with client information
        token_data = {
            "sub": str(client.id),
            "cpf": client.cpf,
            "name": client.name,
            "role": "client"  # Assuming default role for clients
        }
        
        # Generate JWT token
        token = create_access_token(token_data)
        
        # Create response with client data and token
        client_response = ClientResponse.model_validate(client)
        return ClientTokenResponse(
            **client_response.model_dump(),
            token=token
        )
    
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

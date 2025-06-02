from sqlalchemy.orm import Session
from typing import List, Optional
from app.domain.client_model import ClientDB

class ClientRepository:
    def __init__(self, db_session: Session):
        self.db_session = db_session
    
    def get_all_clients(self) -> List[ClientDB]:
        """
        Retrieve all clients from the database
        """
        return self.db_session.query(ClientDB).all()
    
    def get_client_by_id(self, client_id: int) -> Optional[ClientDB]:
        """
        Retrieve a client by its ID
        """
        return self.db_session.query(ClientDB).filter(ClientDB.id == client_id).first()
    
    def get_client_by_cpf(self, cpf: str) -> Optional[ClientDB]:
        """
        Retrieve a client by CPF
        """
        return self.db_session.query(ClientDB).filter(ClientDB.cpf == cpf).first()
    
    def create_client(self, client_data: dict) -> ClientDB:
        """
        Create a new client in the database
        """
        new_client = ClientDB(**client_data)
        self.db_session.add(new_client)
        self.db_session.commit()
        self.db_session.refresh(new_client)
        return new_client
        
    def update_client(self, client_id: int, client_data: dict) -> ClientDB:
        """
        Update an existing client in the database
        """
        client = self.db_session.query(ClientDB).filter(ClientDB.id == client_id).first()
        if not client:
            raise ValueError(f"Client with ID {client_id} not found")
        
        # Update client attributes
        for key, value in client_data.items():
            setattr(client, key, value)
        
        # Commit changes
        self.db_session.commit()
        self.db_session.refresh(client)
        return client

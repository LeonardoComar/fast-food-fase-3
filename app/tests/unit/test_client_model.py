import pytest
from pydantic import ValidationError
from app.domain.client_model import ClientCreate, ClientResponse, ClientDB, ClientUpdate

class TestClientModel:
    def test_client_create_valid(self):
        # Test valid client creation
        client_data = {
            "name": "John Doe",
            "cpf": "123.456.789-10"
        }
        client = ClientCreate(**client_data)
        
        assert client.name == "John Doe"
        assert client.cpf == "123.456.789-10"
    
    def test_client_create_empty_name(self):
        # Test client creation with empty name
        client_data = {
            "name": "",
            "cpf": "123.456.789-10"
        }
        
        # Pydantic's default validation doesn't prevent empty strings
        # This test shows current behavior; if you want to prevent empty names,
        # you would need to add a validator to your model
        client = ClientCreate(**client_data)
        assert client.name == ""
    
    def test_client_response_from_db(self):
        # Test creating ClientResponse from ClientDB
        db_client = ClientDB(
            id=1,
            name="John Doe",
            cpf="123.456.789-10"
        )
        
        # Using model_validate to create from SQLAlchemy model
        response = ClientResponse.model_validate(db_client)
        
        assert response.id == 1
        assert response.name == "John Doe"
        assert response.cpf == "123.456.789-10"
    
    def test_client_update_valid(self):
        # Test valid client name update
        update_data = {
            "name": "Jane Doe"
        }
        client_update = ClientUpdate(**update_data)
        
        assert client_update.name == "Jane Doe"
    
    def test_client_update_empty_name(self):
        # Test client update with empty name
        update_data = {
            "name": ""
        }
        
        # Pydantic's default validation doesn't prevent empty strings
        # This test shows current behavior; if you want to prevent empty names,
        # you would need to add a validator to your model
        client_update = ClientUpdate(**update_data)
        assert client_update.name == ""

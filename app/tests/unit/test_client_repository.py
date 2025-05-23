import pytest
from sqlalchemy.orm import Session
from unittest.mock import MagicMock, patch
from app.repository.client_repository import ClientRepository
from app.domain.client_model import ClientDB

class TestClientRepository:
    def setup_method(self):
        # Create a mock session for each test
        self.mock_session = MagicMock(spec=Session)
        self.repository = ClientRepository(self.mock_session)
        
        # Sample client data
        self.sample_client = ClientDB(
            id=1, 
            name="John Doe", 
            cpf="123.456.789-10"
        )
    
    def test_get_all_clients(self):
        # Arrange
        self.mock_session.query.return_value.all.return_value = [self.sample_client]
        
        # Act
        clients = self.repository.get_all_clients()
        
        # Assert
        self.mock_session.query.assert_called_once_with(ClientDB)
        assert len(clients) == 1
        assert clients[0].id == 1
        assert clients[0].name == "John Doe"
    
    def test_get_client_by_id_found(self):
        # Arrange
        self.mock_session.query.return_value.filter.return_value.first.return_value = self.sample_client
        
        # Act
        client = self.repository.get_client_by_id(1)
        
        # Assert
        self.mock_session.query.assert_called_once_with(ClientDB)
        self.mock_session.query.return_value.filter.assert_called_once()
        assert client.id == 1
        assert client.name == "John Doe"
    
    def test_get_client_by_id_not_found(self):
        # Arrange
        self.mock_session.query.return_value.filter.return_value.first.return_value = None
        
        # Act
        client = self.repository.get_client_by_id(999)
        
        # Assert
        assert client is None
    
    def test_create_client(self):
        # Arrange
        client_data = {
            "name": "Jane Doe",
            "cpf": "987.654.321-00"
        }
        
        # Mock add and commit methods
        self.mock_session.add = MagicMock()
        self.mock_session.commit = MagicMock()
        self.mock_session.refresh = MagicMock()
        
        # Mock the behavior of creating a new client
        with patch('app.repository.client_repository.ClientDB', return_value=ClientDB(
            id=2, **client_data
        )):
            # Act
            new_client = self.repository.create_client(client_data)
            
            # Assert
            self.mock_session.add.assert_called_once()
            self.mock_session.commit.assert_called_once()
            self.mock_session.refresh.assert_called_once()
            assert new_client.id == 2
            assert new_client.name == "Jane Doe"
            assert new_client.cpf == "987.654.321-00"
    
    def test_update_client_found(self):
        # Arrange
        client_to_update = ClientDB(
            id=1, 
            name="Original Name", 
            cpf="123.456.789-10"
        )
        
        self.mock_session.query.return_value.filter.return_value.first.return_value = client_to_update
        
        update_data = {
            "name": "Updated Name"
        }
        
        # Act
        updated_client = self.repository.update_client(1, update_data)
        
        # Assert
        self.mock_session.commit.assert_called_once()
        self.mock_session.refresh.assert_called_once()
        assert updated_client.id == 1
        assert updated_client.name == "Updated Name"
        # CPF should remain unchanged
        assert updated_client.cpf == "123.456.789-10"
    
    def test_update_client_not_found(self):
        # Arrange
        self.mock_session.query.return_value.filter.return_value.first.return_value = None
        
        update_data = {
            "name": "Updated Name"
        }
        
        # Act & Assert
        with pytest.raises(ValueError, match="Client with ID 999 not found"):
            self.repository.update_client(999, update_data)

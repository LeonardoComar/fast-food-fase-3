import pytest
from unittest.mock import MagicMock, patch
from app.service.client_service import ClientService
from app.domain.client_model import ClientCreate, ClientResponse, ClientListResponse, ClientDB, ClientUpdate

class TestClientService:
    def setup_method(self):
        # Create a mock repository for each test
        self.mock_repository = MagicMock()
        
        # Create the service with the mock repository
        with patch('app.service.client_service.ClientRepository', return_value=self.mock_repository):
            self.mock_db_session = MagicMock()
            self.service = ClientService(self.mock_db_session)
        
        # Sample client data
        self.sample_client_db = ClientDB(
            id=1, 
            name="John Doe", 
            cpf="123.456.789-10"
        )
        
        self.sample_client_create = ClientCreate(
            name="Jane Doe",
            cpf="987.654.321-00"
        )
        
        self.sample_client_update = ClientUpdate(
            name="Updated Name"
        )
    
    def test_get_all_clients(self):
        # Arrange
        self.mock_repository.get_all_clients.return_value = [self.sample_client_db]
        
        # Act
        result = self.service.get_all_clients()
        
        # Assert
        self.mock_repository.get_all_clients.assert_called_once()
        assert isinstance(result, ClientListResponse)
        assert len(result.clients) == 1
        assert result.clients[0].id == 1
        assert result.clients[0].name == "John Doe"
    
    def test_get_client_by_id_found(self):
        # Arrange
        self.mock_repository.get_client_by_id.return_value = self.sample_client_db
        
        # Act
        result = self.service.get_client_by_id(1)
        
        # Assert
        self.mock_repository.get_client_by_id.assert_called_once_with(1)
        assert isinstance(result, ClientResponse)
        assert result.id == 1
        assert result.name == "John Doe"
    
    def test_get_client_by_id_not_found(self):
        # Arrange
        self.mock_repository.get_client_by_id.return_value = None
        
        # Act & Assert
        with pytest.raises(ValueError, match="Client with ID 999 not found"):
            self.service.get_client_by_id(999)
    
    def test_create_client(self):
        # Arrange
        created_client = ClientDB(
            id=2, 
            name=self.sample_client_create.name,
            cpf=self.sample_client_create.cpf
        )
        self.mock_repository.create_client.return_value = created_client
        
        # Act
        result = self.service.create_client(self.sample_client_create)
        
        # Assert
        self.mock_repository.create_client.assert_called_once()
        # Verify the dict passed to repository has correct data
        client_dict = self.mock_repository.create_client.call_args[0][0]
        assert client_dict["name"] == "Jane Doe"
        assert client_dict["cpf"] == "987.654.321-00"
        
        assert isinstance(result, ClientResponse)
        assert result.id == 2
        assert result.name == "Jane Doe"
    
    def test_update_client_found(self):
        # Arrange
        self.mock_repository.get_client_by_id.return_value = self.sample_client_db
        
        updated_client = ClientDB(
            id=1, 
            name="Updated Name",
            cpf="123.456.789-10"
        )
        self.mock_repository.update_client.return_value = updated_client
        
        # Act
        result = self.service.update_client(1, self.sample_client_update)
        
        # Assert
        self.mock_repository.get_client_by_id.assert_called_once_with(1)
        self.mock_repository.update_client.assert_called_once()
        assert isinstance(result, ClientResponse)
        assert result.id == 1
        assert result.name == "Updated Name"
        # CPF should remain unchanged
        assert result.cpf == "123.456.789-10"
    
    def test_update_client_not_found(self):
        # Arrange
        self.mock_repository.get_client_by_id.return_value = None
        
        # Act & Assert
        with pytest.raises(ValueError, match="Client with ID 999 not found"):
            self.service.update_client(999, self.sample_client_update)

import pytest
from unittest.mock import patch, MagicMock
from fastapi import status
from fastapi.testclient import TestClient
from app.main import app
from app.domain.client_model import ClientResponse, ClientListResponse

@pytest.fixture
def client():
    return TestClient(app)

class TestClientRoutes:
    def setup_method(self):
        # Sample client data
        self.sample_client = {
            "id": 1,
            "name": "John Doe",
            "cpf": "123.456.789-10"
        }
        
        self.sample_client_response = ClientResponse(**self.sample_client)
        
        self.sample_client_create = {
            "name": "Jane Doe",
            "cpf": "987.654.321-00"
        }
        
        self.sample_client_update = {
            "name": "Updated Name"
        }
    
    @patch('app.api.client_routes.ClientService')
    def test_get_all_clients(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        
        client_list = ClientListResponse(clients=[self.sample_client_response])
        mock_service.get_all_clients.return_value = client_list
        
        # Act
        response = client.get("/api/clients/")
        
        # Assert
        assert response.status_code == status.HTTP_200_OK
        assert response.json() == {"clients": [self.sample_client]}
        mock_service.get_all_clients.assert_called_once()
    
    @patch('app.api.client_routes.ClientService')
    def test_get_client_by_id_found(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.get_client_by_id.return_value = self.sample_client_response
        
        # Act
        response = client.get("/api/clients/1")
        
        # Assert
        assert response.status_code == status.HTTP_200_OK
        assert response.json() == self.sample_client
        mock_service.get_client_by_id.assert_called_once_with(1)
    
    @patch('app.api.client_routes.ClientService')
    def test_get_client_by_id_not_found(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.get_client_by_id.side_effect = ValueError("Client with ID 999 not found")
        
        # Act
        response = client.get("/api/clients/999")
        
        # Assert
        assert response.status_code == status.HTTP_404_NOT_FOUND
        assert "not found" in response.json()["detail"]
    
    @patch('app.api.client_routes.ClientService')
    def test_create_client(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.create_client.return_value = self.sample_client_response
        
        # Act
        response = client.post("/api/clients/", json=self.sample_client_create)
        
        # Assert
        assert response.status_code == status.HTTP_201_CREATED
        assert response.json() == self.sample_client
        
        # Verify service was called with correct data
        mock_service.create_client.assert_called_once()
        
    @patch('app.api.client_routes.ClientService')
    def test_update_client_found(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        
        updated_client = ClientResponse(
            id=1,
            name="Updated Name",
            cpf="123.456.789-10"
        )
        mock_service.update_client.return_value = updated_client
        
        # Act
        response = client.put("/api/clients/1", json=self.sample_client_update)
        
        # Assert
        assert response.status_code == status.HTTP_200_OK
        assert response.json()["name"] == "Updated Name"
        mock_service.update_client.assert_called_once()
    
    @patch('app.api.client_routes.ClientService')
    def test_update_client_not_found(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.update_client.side_effect = ValueError("Client with ID 999 not found")
        
        # Act
        response = client.put("/api/clients/999", json=self.sample_client_update)
        
        # Assert
        assert response.status_code == status.HTTP_404_NOT_FOUND
        assert "not found" in response.json()["detail"]

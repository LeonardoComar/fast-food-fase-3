import pytest
from unittest.mock import patch, MagicMock
from fastapi import HTTPException
from app.core.auth import get_current_user, get_admin_user

class TestAuth:
    @patch('app.core.auth.verify_access_token')
    def test_get_current_user_valid(self, mock_verify_token):
        # Arrange
        test_token = "valid_token"
        mock_payload = {"sub": "test@example.com", "role": "user"}
        mock_verify_token.return_value = mock_payload
        
        # Act
        user = get_current_user(test_token)
        
        # Assert
        mock_verify_token.assert_called_once_with(test_token)
        assert user == mock_payload
    
    @patch('app.core.auth.verify_access_token')
    def test_get_current_user_invalid(self, mock_verify_token):
        # Arrange
        test_token = "invalid_token"
        mock_verify_token.return_value = None
        
        # Act & Assert
        with pytest.raises(HTTPException) as exc_info:
            get_current_user(test_token)
        
        assert exc_info.value.status_code == 401
        assert "Invalid token or expired" in exc_info.value.detail
    
    def test_get_admin_user_valid(self):
        # Arrange
        mock_admin = {"sub": "admin@example.com", "role": "administrator"}
        
        # Act
        user = get_admin_user(mock_admin)
        
        # Assert
        assert user == mock_admin
    
    def test_get_admin_user_invalid(self):
        # Arrange
        mock_user = {"sub": "user@example.com", "role": "user"}
        
        # Act & Assert
        with pytest.raises(HTTPException) as exc_info:
            get_admin_user(mock_user)
        
        assert exc_info.value.status_code == 403
        assert "Acesso não permitido" in exc_info.value.detail

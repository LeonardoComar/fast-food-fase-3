import pytest
from datetime import datetime, timedelta, timezone
from unittest.mock import patch
from jose import jwt
from app.core.jwt import create_access_token, verify_access_token

class TestJWT:
    @patch('app.core.jwt.SECRET_KEY', 'test_secret_key')
    @patch('app.core.jwt.ALGORITHM', 'HS256')
    @patch('app.core.jwt.ACCESS_TOKEN_EXPIRE_MINUTES', 30)
    def test_create_access_token(self):
        # Arrange
        test_data = {"sub": "test@example.com", "role": "user"}
        
        # Act
        token = create_access_token(test_data)
        
        # Assert
        assert token is not None
        # Verify the token can be decoded with our test secret
        payload = jwt.decode(token, 'test_secret_key', algorithms=['HS256'])
        assert payload["sub"] == "test@example.com"
        assert payload["role"] == "user"
        assert "exp" in payload
    
    @patch('app.core.jwt.SECRET_KEY', 'test_secret_key')
    @patch('app.core.jwt.ALGORITHM', 'HS256')
    def test_verify_access_token_valid(self):
        # Arrange
        test_data = {"sub": "test@example.com", "role": "user"}
        # Create a token that expires in 30 minutes
        expiration = datetime.now(timezone.utc) + timedelta(minutes=30)
        test_data.update({"exp": expiration})
        token = jwt.encode(test_data, 'test_secret_key', algorithm='HS256')
        
        # Act
        payload = verify_access_token(token)
        
        # Assert
        assert payload is not None
        assert payload["sub"] == "test@example.com"
        assert payload["role"] == "user"
    
    @patch('app.core.jwt.SECRET_KEY', 'test_secret_key')
    @patch('app.core.jwt.ALGORITHM', 'HS256')
    def test_verify_access_token_expired(self):
        # Arrange
        test_data = {"sub": "test@example.com"}
        # Create a token that has already expired
        expiration = datetime.now(timezone.utc) - timedelta(minutes=5)
        test_data.update({"exp": expiration})
        token = jwt.encode(test_data, 'test_secret_key', algorithm='HS256')
        
        # Act
        payload = verify_access_token(token)
        
        # Assert
        assert payload is None
    
    @patch('app.core.jwt.SECRET_KEY', 'test_secret_key')
    @patch('app.core.jwt.ALGORITHM', 'HS256')
    def test_verify_access_token_invalid(self):
        # Arrange - create token with different secret
        test_data = {"sub": "test@example.com"}
        token = jwt.encode(test_data, 'wrong_secret_key', algorithm='HS256')
        
        # Act
        payload = verify_access_token(token)
        
        # Assert
        assert payload is None

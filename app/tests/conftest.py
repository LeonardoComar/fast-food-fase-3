import pytest
from unittest.mock import patch
from fastapi.testclient import TestClient
from app.main import app
from app.core.jwt import create_access_token

@pytest.fixture
def auth_token():
    """Create a valid JWT token for testing"""
    # Create a token with admin privileges
    test_data = {"sub": "test@example.com", "role": "administrator"}
    return create_access_token(test_data)

@pytest.fixture
def authenticated_client(auth_token):
    """Returns a TestClient with auth token automatically added to headers"""
    client = TestClient(app)
    client.headers = {
        "Authorization": f"Bearer {auth_token}",
        **client.headers
    }
    return client

@pytest.fixture
def client():
    """Regular unauthenticated client for specific tests"""
    return TestClient(app)

@pytest.fixture
def mock_env_variables():
    with patch('os.getenv') as mock_getenv:
        # Set default test values for environment variables
        mock_getenv.side_effect = lambda key, default=None: {
            'SECRET_KEY': 'test_secret_key',
            'ALGORITHM': 'HS256',
            'ACCESS_TOKEN_EXPIRE_MINUTES': '30',
        }.get(key, default)
        yield mock_getenv

@pytest.fixture
def mock_token():
    """Fixture that returns a valid JWT token for testing"""
    return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0QGV4YW1wbGUuY29tIiwicm9sZSI6InVzZXIiLCJleHAiOjk5OTk5OTk5OTl9.this_is_a_test_token"

# Add a fixture to bypass authentication for route tests
@pytest.fixture
def auth_bypass():
    """Bypasses the authentication for route testing"""
    with patch('app.core.auth.get_current_user', return_value={"sub": "test@example.com", "role": "administrator"}):
        yield

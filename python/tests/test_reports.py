"""
Test scaffold for GET /get-financial-report
============================================
Your task — implement the body of each test using the AI assistant:

  1. test_admin_sees_full_data    — ADMIN gets full report including the real account_number
  2. test_analyst_sees_masked_data — ANALYST gets masked account_number and rounded figures
  3. test_user_gets_403            — USER is denied access (HTTP 403)

Helpers available:
  - get_token(role)  returns a valid Bearer JWT for the seeded user with that role
  - client           a FastAPI TestClient wired to an in-memory database (auto-seeded)

Seeded credentials (password "Password1!" for all — only needed for the /auth/login flow):
  ADMIN   → admin@corp.com
  ANALYST → analyst@corp.com
  USER    → user@corp.com
"""

import pytest
from fastapi.testclient import TestClient

from app.auth import create_access_token
from app.main import app

client = TestClient(app)

# Seeded user IDs — match insertion order in main._seed()
_USER_IDS = {"ADMIN": 1, "ANALYST": 2, "USER": 3}
_USER_EMAILS = {"ADMIN": "admin@corp.com", "ANALYST": "analyst@corp.com", "USER": "user@corp.com"}


def get_token(role: str) -> str:
    """Returns a valid Bearer token for the seeded user with the given role."""
    return create_access_token({
        "sub": str(_USER_IDS[role]),
        "role": role,
        "email": _USER_EMAILS[role],
    })


# ---------------------------------------------------------------------------
# Tests — implement the body of each test using your AI assistant
# ---------------------------------------------------------------------------

def test_admin_sees_full_data():
    """ADMIN should receive the full report including the real account_number."""
    token = get_token("ADMIN")
    # TODO: call GET /get-financial-report with the token and assert:
    #   - response status is 200
    #   - at least one report is returned
    #   - account_number contains the real value (e.g. "ACC-0012-3456")
    raise NotImplementedError("Implement this test")


def test_analyst_sees_masked_data():
    """ANALYST should receive reports with a masked account_number and rounded figures."""
    token = get_token("ANALYST")
    # TODO: call GET /get-financial-report with the token and assert:
    #   - response status is 200
    #   - account_number does NOT expose the raw value
    #     (should be masked, e.g. "****3456")
    #   - revenue, net_profit, and expenses are each rounded to the nearest 1,000
    raise NotImplementedError("Implement this test")


def test_user_gets_403():
    """USER role should be denied access with HTTP 403."""
    token = get_token("USER")
    # TODO: call GET /get-financial-report with the token and assert HTTP 403
    raise NotImplementedError("Implement this test")

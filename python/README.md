# Financial Reports API — Python (FastAPI)

## Challenge

This codebase has a working JWT-authenticated API. Your task is to add an **RBAC layer** to the existing `/get-financial-report` endpoint using an AI coding assistant.

**Time budget: 30–45 minutes**

### Requirements

| Role | Expected behaviour |
|---|---|
| `ADMIN` | Full report data — all fields returned as-is |
| `ANALYST` | Masked data — `account_number` shows only the last 4 chars (e.g. `****3456`); `revenue`, `net_profit`, `expenses` rounded to the nearest 1,000 |
| `USER` | HTTP 403 Forbidden |

### Rules

- Implement the RBAC logic inside `app/routers/reports.py`
- The authenticated user (including their role) is already available via the `current_user` parameter — **do not re-validate the JWT or add new auth middleware**
- Write unit tests covering all three roles in `tests/test_reports.py`

---

## Project structure

```
app/
├── main.py           # FastAPI app + /auth/login endpoint
├── database.py       # SQLAlchemy engine — DATABASE_URL env var
├── models.py         # User, FinancialReport ORM models
├── schemas.py        # Pydantic schemas (FullReportResponse, MaskedReportResponse)
├── auth.py           # JWT utils + get_current_user dependency  ← existing, don't change
└── routers/
    └── reports.py    # /get-financial-report endpoint           ← add RBAC here
seed.py               # One-time SQLite seeder
tests/
└── test_reports.py   # Pytest scaffold — implement the 3 test stubs
```

---

## Setup

```bash
# 1. Install dependencies
pip install -r requirements.txt

# 2. Configure environment
cp .env.example .env
# (edit .env if needed — defaults are fine for local dev)

# 3. Seed the SQLite database
python seed.py

# 4. Run the server
uvicorn app.main:app --reload
```

Swagger UI: http://127.0.0.1:8000/docs

---

## Seeded users

| Email | Password | Role |
|---|---|---|
| admin@corp.com | Password1! | ADMIN |
| analyst@corp.com | Password1! | ANALYST |
| user@corp.com | Password1! | USER |

Obtain a token:
```bash
curl -X POST http://127.0.0.1:8000/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@corp.com","password":"Password1!"}'
```

Then call the endpoint:
```bash
curl http://127.0.0.1:8000/get-financial-report \
  -H "Authorization: Bearer <token>"
```

---

## Running tests

Tests use an in-memory SQLite database (auto-seeded) — no setup required.

```bash
pytest tests/ -v
```

The three test stubs will show `NotImplementedError` until you implement them — that is expected.

---

## Evaluation notes (for interviewer)

**Key observation:** `app/auth.py` already validates the JWT and returns a `CurrentUser` object with the `role` field populated. Watch whether the candidate:

1. **Feeds context to the AI** — shares `auth.py` and `reports.py` together so the AI understands the existing dependency injection pattern
2. **Catches the design trap** — a naive AI response often proposes decoding the JWT a second time or adding a new auth middleware; the correct solution reads `current_user.role` directly
3. **Covers all three roles** in tests with realistic assertions
4. **Uses the right schemas** — `FullReportResponse` for ADMIN, a masked mapping for ANALYST

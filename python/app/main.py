from contextlib import asynccontextmanager

from fastapi import Depends, FastAPI, HTTPException, status
from sqlalchemy.orm import Session

from app.auth import create_access_token, hash_password, verify_password
from app.database import Base, SessionLocal, engine, get_db
from app.models import FinancialReport, User
from app.routers import reports
from app.schemas import LoginRequest, Token


def _seed(db: Session) -> None:
    """Seeds the database with mock users and reports. No-op if already seeded."""
    if db.query(User).count() > 0:
        return

    db.add_all([
        User(email="admin@corp.com",   username="Admin User",   password_hash=hash_password("Password1!"), role="ADMIN"),
        User(email="analyst@corp.com", username="Analyst User", password_hash=hash_password("Password1!"), role="ANALYST"),
        User(email="user@corp.com",    username="Regular User", password_hash=hash_password("Password1!"), role="USER"),
    ])
    db.add_all([
        FinancialReport(company_name="Acme Corp",   revenue=5_230_000,  net_profit=1_045_000, expenses=4_185_000, account_number="ACC-0012-3456", report_date="2024-03-31", period="Q1 2024"),
        FinancialReport(company_name="Acme Corp",   revenue=6_100_000,  net_profit=1_220_000, expenses=4_880_000, account_number="ACC-0012-3456", report_date="2024-06-30", period="Q2 2024"),
        FinancialReport(company_name="Globex Inc",  revenue=12_400_000, net_profit=2_480_000, expenses=9_920_000, account_number="ACC-7890-1234", report_date="2024-03-31", period="Q1 2024"),
        FinancialReport(company_name="Globex Inc",  revenue=11_750_000, net_profit=2_350_000, expenses=9_400_000, account_number="ACC-7890-1234", report_date="2024-06-30", period="Q2 2024"),
        FinancialReport(company_name="Initech Ltd", revenue=3_890_000,  net_profit=778_000,   expenses=3_112_000, account_number="ACC-4567-8901", report_date="2024-03-31", period="Q1 2024"),
        FinancialReport(company_name="Initech Ltd", revenue=4_210_000,  net_profit=842_000,   expenses=3_368_000, account_number="ACC-4567-8901", report_date="2024-06-30", period="Q2 2024"),
        FinancialReport(company_name="Umbrella Co", revenue=8_600_000,  net_profit=1_720_000, expenses=6_880_000, account_number="ACC-2345-6789", report_date="2024-03-31", period="Q1 2024"),
        FinancialReport(company_name="Umbrella Co", revenue=9_300_000,  net_profit=1_860_000, expenses=7_440_000, account_number="ACC-2345-6789", report_date="2024-06-30", period="Q2 2024"),
    ])
    db.commit()


@asynccontextmanager
async def lifespan(app: FastAPI):
    Base.metadata.create_all(bind=engine)
    db = SessionLocal()
    try:
        _seed(db)
    finally:
        db.close()
    yield


app = FastAPI(title="Financial Reports API", lifespan=lifespan)
app.include_router(reports.router)


@app.post("/auth/login", response_model=Token)
def login(body: LoginRequest, db: Session = Depends(get_db)):
    user = db.query(User).filter(User.email == body.email).first()
    if not user or not verify_password(body.password, user.password_hash):
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Invalid credentials",
        )
    token = create_access_token({"sub": str(user.id), "role": user.role, "email": user.email})
    return Token(access_token=token, token_type="bearer")

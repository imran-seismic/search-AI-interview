"""
Run this once to populate the SQLite database with mock users and reports.

Usage (from the python/ directory):
    python seed.py

The database file will be created at the path specified by DATABASE_URL in .env
(defaults to ./financial.db).
"""

from app.auth import hash_password
from app.database import Base, SessionLocal, engine
from app.models import FinancialReport, User


def seed() -> None:
    Base.metadata.create_all(bind=engine)
    db = SessionLocal()

    if db.query(User).count() > 0:
        print("Database already seeded — skipping.")
        db.close()
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
    db.close()
    print("Seeded 3 users and 8 financial reports.")


if __name__ == "__main__":
    seed()

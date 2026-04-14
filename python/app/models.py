from sqlalchemy import Column, Float, Integer, String

from app.database import Base


class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True, index=True)
    email = Column(String, unique=True, index=True, nullable=False)
    username = Column(String, nullable=False)
    password_hash = Column(String, nullable=False)
    role = Column(String, nullable=False)  # ADMIN | ANALYST | USER


class FinancialReport(Base):
    __tablename__ = "financial_reports"

    id = Column(Integer, primary_key=True, index=True)
    company_name = Column(String, nullable=False)
    revenue = Column(Float, nullable=False)
    net_profit = Column(Float, nullable=False)
    expenses = Column(Float, nullable=False)
    account_number = Column(String, nullable=False)
    report_date = Column(String, nullable=False)  # ISO date YYYY-MM-DD
    period = Column(String, nullable=False)        # e.g. "Q1 2024"

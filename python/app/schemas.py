from pydantic import BaseModel


class LoginRequest(BaseModel):
    email: str
    password: str


class Token(BaseModel):
    access_token: str
    token_type: str


class CurrentUser(BaseModel):
    id: int
    email: str
    username: str
    role: str  # ADMIN | ANALYST | USER


class FullReportResponse(BaseModel):
    """Full financial report — all fields visible."""
    id: int
    company_name: str
    revenue: float
    net_profit: float
    expenses: float
    account_number: str
    report_date: str
    period: str

    model_config = {"from_attributes": True}


class MaskedReportResponse(BaseModel):
    """
    Partial financial report for ANALYST role.
    account_number is masked (e.g. ACC-0012-3456 → ****3456).
    revenue, net_profit, expenses are rounded to the nearest 1,000.
    """
    id: int
    company_name: str
    revenue: float
    net_profit: float
    expenses: float
    account_number: str  # masked value
    report_date: str
    period: str

    model_config = {"from_attributes": True}

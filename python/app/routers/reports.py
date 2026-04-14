from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session

from app.auth import get_current_user
from app.database import get_db
from app.models import FinancialReport
from app.schemas import CurrentUser

router = APIRouter(tags=["reports"])


@router.get("/get-financial-report")
def get_financial_report(
    current_user: CurrentUser = Depends(get_current_user),
    db: Session = Depends(get_db),
):
    """
    Returns financial reports for the authenticated user.

    TODO: Implement the RBAC layer below using current_user.role:

      ADMIN   → return full report data as-is (use FullReportResponse schema)

      ANALYST → return masked data:
                  - account_number: mask all but the last 4 characters
                    e.g.  "ACC-0012-3456"  →  "****3456"
                  - revenue, net_profit, expenses: round each to the nearest 1,000

      USER    → raise HTTP 403 Forbidden

    HINT: current_user.role is already populated — it comes from the validated JWT.
          Do NOT re-decode the token or add new authentication middleware.
    """
    reports = db.query(FinancialReport).all()

    # Replace this with role-based logic
    return reports

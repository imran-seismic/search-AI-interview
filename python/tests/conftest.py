import os

# Must be set before importing anything from app/ so database.py picks up :memory:
os.environ.setdefault("DATABASE_URL", "sqlite:///:memory:")

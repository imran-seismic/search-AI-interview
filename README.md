# Financial Reports API — Interview Challenge

**Duration:** 30–45 minutes  
**AI assistant:** Required — use GitHub Copilot, ChatGPT, Cursor, or any AI tool of your choice

---

## Overview

You are given a partially built Financial Reports API. Authentication is already wired up and working. Your job is to extend it with a **Role-Based Access Control (RBAC)** layer using an AI coding assistant — then prove it works with unit tests.

This exercise is less about memorising APIs and more about how effectively you collaborate with an AI tool to understand existing code, implement a feature, and critically review what the AI produces.

---

## Choose your stack

Pick **one** — both repos contain identical functionality, same mock data, and the same test stubs:

| Folder | Stack |
|---|---|
| [`python/`](python/) | Python 3.10 · FastAPI · SQLAlchemy · pytest |
| [`csharp/`](csharp/) | C# · ASP.NET Core 8 · EF Core · xUnit |

Follow the `README.md` inside your chosen folder for setup and run instructions.

---

## The task

The existing `/get-financial-report` endpoint returns data to **any** authenticated user — there is no role check. Add the following behaviour:

| Role | Expected response |
|---|---|
| `ADMIN` | Full report — all fields, real values |
| `ANALYST` | Masked report — `account_number` shows only last 4 chars (e.g. `ACC-0012-3456` → `****3456`); `revenue`, `net_profit`/`NetProfit`, and `expenses` rounded to the nearest 1,000 |
| `USER` | HTTP 403 Forbidden |

The three seeded test users (password `Password1!` for all):

| Email | Role |
|---|---|
| admin@corp.com | ADMIN |
| analyst@corp.com | ANALYST |
| user@corp.com | USER |

---

## Goals

These are the **core deliverables**. All must be completed.

- [ ] Implement the RBAC logic in the reports endpoint so the three role behaviours above are enforced
- [ ] Implement all three test stubs so the test suite passes (`pytest tests/` or `dotnet test`)
- [ ] ADMIN test asserts at least one report is returned with the real, unmasked `account_number`
- [ ] ANALYST test asserts the `account_number` is masked and at least one numeric field is rounded
- [ ] USER test asserts HTTP 403

---

## Stretch goals

Complete these if time allows — they are not required but demonstrate depth.

- [ ] **Input validation** — the login endpoint currently accepts any payload shape; add validation so missing or empty `email`/`password` returns HTTP 422 with a meaningful error
- [ ] **Pagination** — add optional `?page=1&page_size=10` query parameters to `/get-financial-report`; ADMIN and ANALYST both respect pagination
- [ ] **Audit log** — log every access attempt to `/get-financial-report` (user id, role, timestamp, outcome) to a separate `audit_logs` table; expose a `GET /audit-log` endpoint accessible by ADMIN only
- [ ] **Expand the role model** — add a `MANAGER` role that sees full data for companies they are assigned to and masked data for all others; add at least one test covering this

---

## Guidelines

**You must use an AI assistant.** The interviewer will ask you to talk through how you used it.

**Read the existing code before prompting.** The codebase already handles JWT validation and sets the authenticated user on the request context. Pay attention to the comments marked `EXISTING CODE` — understand what is already there before asking the AI to add anything.

**Review what the AI generates.** AI output is not always correct. A common mistake: the AI will propose re-validating the JWT or adding new auth middleware when the user context already exists — do not accept this without questioning it.

**Do not rewrite existing auth code.** The authentication layer is already working. Your changes should be additive, not replacements.

**Tests must use the in-memory database.** The test scaffold already configures this — do not change that setup.

**Commit your work.** Make at least one commit when the core goals are done so there is a clear save point.

---

## Deliverables

When time is up, share the following:

1. **Running code** — the server starts without errors
2. **Passing tests** — all three role tests pass (`pytest tests/ -v` or `dotnet test`)
3. **A brief walk-through** (2–3 minutes) covering:
   - How you used the AI assistant (what prompts worked, what needed correcting)
   - One thing the AI got wrong or that needed adjustment, and how you fixed it
   - What you would do next if you had another 30 minutes
